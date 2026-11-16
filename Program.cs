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

            while (true)
            {
                if (settings != null)
                {
                    try
                    {
                        ConnectionString = settings.ConnectionString;
                        PrepareDatabase();
                        settings.Save();
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
