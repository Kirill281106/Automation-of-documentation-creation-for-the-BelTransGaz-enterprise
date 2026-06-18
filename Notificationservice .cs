namespace вход_курсач
{
    public static class NotificationService
    {
        private static ITelegramNotifier? _notifier;

        public static void Register(ITelegramNotifier notifier)
        {
            _notifier = notifier;
        }

        // ──────────────────────────────────────────────────────────────
        //  УВЕДОМЛЕНИЯ ДЛЯ БОТА
        // ──────────────────────────────────────────────────────────────

        /// <summary>Новая заявка на регистрацию</summary>
        public static void NewRequest(string surname, string name)
            => _notifier?.NotifyNewRequest(surname, name);

        /// <summary>Новый сменный отчёт</summary>
        public static void NewShiftReport(string reportDate, string filialName, string shiftType, string managerName)
            => _notifier?.NotifyNewShiftReport(reportDate, filialName, shiftType, managerName);

        /// <summary>Создан отчёт (сменный/квартальный/годовой)</summary>
        public static void ReportCreated(string reportType, string details)
            => _notifier?.NotifyReportCreated(reportType, details);

        /// <summary>Действие с пользователем (добавлен/изменён/удалён)</summary>
        public static void UserAction(string action, string fio, string role = "")
            => _notifier?.NotifyUserAction(action, fio, role);
    }

    public interface ITelegramNotifier
    {
        void NotifyNewRequest(string surname, string name);
        void NotifyNewShiftReport(string reportDate, string filialName, string shiftType, string managerName);
        void NotifyReportCreated(string reportType, string details);
        void NotifyUserAction(string action, string fio, string role = "");
    }
}