using Microsoft.Data.Sqlite;
namespace вход_курсач
{
    internal static class Program
    {
        public static string ConnectionString { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                // Создаем папку Data рядом с приложением
                // если удалить папку всё ок если бд будет ругаться
                string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                if (!Directory.Exists(dataFolder))
                    Directory.CreateDirectory(dataFolder);

                // Путь к бд
                string dbFile = Path.Combine(dataFolder, "TPPO.db");
                ConnectionString = $"Data Source={dbFile}";

                // Если бд нет
                if (!File.Exists(dbFile))// добавь проверку на папку Data
                {
                    CreateDatabase();
                }

                Application.Run(new Login());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n" +
                    "Попробуйте запустить от имени администратора",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CreateDatabase()
        {
            SQL_DB sql_db = new SQL_DB();
            sql_db.SQLite_DB();
        }
    }
}
/*
 *                     CREATE TABLE IF NOT EXISTS ShiftReports (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        ReportDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        ShiftType TEXT NOT NULL,
                        ProductionAmount REAL NOT NULL,
                        DefectAmount REAL DEFAULT 0,
                        Comments TEXT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                    );

                    CREATE TABLE IF NOT EXISTS WorkSchedules (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Filial INTEGER NOT NULL,
                        TeamNum INTEGER NOT NULL,
                        WorkDate DATE NOT NULL,
                        ShiftType TEXT NOT NULL,
                        CreatedBy INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                    );

                    CREATE TABLE IF NOT EXISTS FinancialReports (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ReportType TEXT NOT NULL,
                        Period TEXT NOT NULL,
                        Filial INTEGER NOT NULL,
                        TotalIncome REAL NOT NULL,
                        TotalExpenses REAL NOT NULL,
                        Profit REAL NOT NULL,
                        CreatedBy INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                    );

                    CREATE TABLE IF NOT EXISTS GrowthForecasts (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Filial INTEGER NOT NULL,
                        ForecastYear INTEGER NOT NULL,
                        ExpectedGrowth REAL NOT NULL,
                        CreatedBy INTEGER NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                    );
 */