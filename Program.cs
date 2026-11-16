namespace вход_курсач
{
    internal static class Program
    {
        // Строка подключения к PostgreSQL, которой пользуются все классы DB_*
        public static string ConnectionString { get; private set; } = string.Empty;

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            ApplicationConfiguration.Initialize();

            if (!ConnectToDatabase())
                return;

            Application.Run(new Login());
        }

        // Подключается к базе по настройкам из config.json.
        // Если настроек ещё нет или подключиться не удалось — показывает окно параметров
        // подключения и пробует снова. Возвращает false, если пользователь закрыл это окно.
        private static bool ConnectToDatabase()
        {
            DbSettings? settings = DbSettings.Load();
            bool enteredInForm = false;

            while (true)
            {
                if (settings != null)
                {
                    try
                    {
                        ConnectionString = settings.ConnectionString;
                        PrepareDatabase();
                        if (enteredInForm)
                            SaveSettings(settings);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось подключиться к базе данных:\n{ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                using var form = new ConnectionForm(settings ?? new DbSettings());
                if (form.ShowDialog() != DialogResult.OK)
                    return false;

                settings = form.Settings;
                enteredInForm = true;
            }
        }

        // Если сохранить настройки не удалось (например, нет прав на запись в папку программы),
        // программа всё равно работает, просто при следующем запуске параметры спросят снова.
        private static void SaveSettings(DbSettings settings)
        {
            try
            {
                settings.Save();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show($"Не удалось сохранить параметры подключения в config.json:\n{ex.Message}",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // При первом запуске создаёт базу, таблицы и заполняет их начальными данными.
        // Если всё это уже есть, ничего не меняет.
        private static void PrepareDatabase()
        {
            var create = new DB_Create();
            create.CreateDatabase();
            if (!create.TablesExist())
                create.CreateTables();

            var fill = new DB_Fill();
            if (fill.IsEmpty())
                fill.Fill();
        }
    }
}
