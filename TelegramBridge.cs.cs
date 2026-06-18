using Microsoft.Data.Sqlite;
using Dapper;

namespace вход_курсач
{
    /// <summary>
    /// Мост между WinForms приложением и ботом.
    /// Отправляет уведомления через общую БД (бот читает из БД)
    /// </summary>
    public class TelegramBridge : ITelegramNotifier
    {
        private readonly string _connectionString;

        // Таблица для уведомлений (бот будет читать из неё)
        private const string NotificationsTableSql = @"
            CREATE TABLE IF NOT EXISTS BotNotifications (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Type TEXT NOT NULL,
                Data TEXT NOT NULL,
                CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP,
                IsSent INTEGER DEFAULT 0
            )";

        public TelegramBridge(string connectionString)
        {
            _connectionString = connectionString;
            EnsureNotificationTable();
        }

        private void EnsureNotificationTable()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            conn.Execute(NotificationsTableSql);
        }

        private void AddNotification(string type, string data)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            conn.Execute(
                "INSERT INTO BotNotifications (Type, Data, IsSent) VALUES (@Type, @Data, 0)",
                new { Type = type, Data = data });
        }

        // ──────────────────────────────────────────────────────────────
        //  РЕАЛИЗАЦИЯ ITelegramNotifier
        // ──────────────────────────────────────────────────────────────

        public void NotifyNewRequest(string surname, string name)
        {
            var data = $"{surname}|{name}";
            AddNotification("NEW_REQUEST", data);
            Console.WriteLine($"[TelegramBridge] Добавлено уведомление о новой заявке: {surname} {name}");
        }

        public void NotifyNewShiftReport(string reportDate, string filialName, string shiftType, string managerName)
        {
            var data = $"{reportDate}|{filialName}|{shiftType}|{managerName}";
            AddNotification("SHIFT_REPORT", data);
            Console.WriteLine($"[TelegramBridge] Добавлено уведомление о сменном отчёте: {filialName} {shiftType}");
        }

        public void NotifyReportCreated(string reportType, string details)
        {
            var data = $"{reportType}|{details}";
            AddNotification("REPORT_CREATED", data);
            Console.WriteLine($"[TelegramBridge] Добавлено уведомление о создании отчёта: {reportType}");
        }

        public void NotifyUserAction(string action, string fio, string role = "")
        {
            var data = $"{action}|{fio}|{role}";
            AddNotification("USER_ACTION", data);
            Console.WriteLine($"[TelegramBridge] Добавлено уведомление о действии с пользователем: {action} {fio}");
        }
    }
}