using System.Text.Json;

namespace вход_курсач
{
    public class AppConfig
    {
        public string DatabasePath { get; set; } = string.Empty;
    }

    internal static class Program
    {
        public static string ConnectionString { get; private set; } = string.Empty;
        private static string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            ApplicationConfiguration.Initialize();

            // Загружаем сохранённый путь
            string dbFile = LoadDatabasePath();

            if (string.IsNullOrEmpty(dbFile) || !File.Exists(dbFile))
            {
                dbFile = ChooseDatabaseLocation();
                if (string.IsNullOrEmpty(dbFile))
                {
                    MessageBox.Show("Не выбрано место для сохранения базы данных. Приложение будет закрыто.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SaveDatabasePath(dbFile);
            }

            ConnectionString = $"Data Source={dbFile}";

            string dbDirectory = Path.GetDirectoryName(dbFile);
            if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
                Directory.CreateDirectory(dbDirectory);

            if (!File.Exists(dbFile))
                CreateDatabase();

            var telegramBridge = new TelegramBridge(ConnectionString);
            NotificationService.Register(telegramBridge);

            Console.WriteLine("WinForms приложение запущено. Бот должен быть запущен отдельно.");

            Application.Run(new Login());
        }

        private static string LoadDatabasePath()
        {
            if (!File.Exists(ConfigFilePath))
                return string.Empty;

            string json = File.ReadAllText(ConfigFilePath);
            var config = JsonSerializer.Deserialize<AppConfig>(json);
            return config?.DatabasePath ?? string.Empty;
        }

        private static void SaveDatabasePath(string path)
        {
            var config = new AppConfig { DatabasePath = path };
            string json = JsonSerializer.Serialize(config);
            File.WriteAllText(ConfigFilePath, json);
        }

        /// <summary>
        /// Открывает диалог выбора папки для сохранения базы данных
        /// </summary>
        private static string ChooseDatabaseLocation()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Выберите папку для сохранения базы данных";
                dialog.ShowNewFolderButton = true;

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    return Path.Combine(dialog.SelectedPath, "TPPO.db");
                }
            }
            return string.Empty;
        }

        private static void CreateDatabase()
        {
            SQL_DB sql_db = new SQL_DB();
            sql_db.SQLite_DB();
        }
    }
}