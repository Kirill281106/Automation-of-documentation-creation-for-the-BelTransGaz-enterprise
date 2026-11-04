namespace вход_курсач
{
    // Одна запись в таблице WorkSchedule
    internal class WorkScheduleItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;  // Фамилия И.О.
        public int TeamNum { get; set; }
        public string WorkDate { get; set; } = string.Empty;      // хранится как TEXT "yyyy-MM-dd"
        public string ShiftType { get; set; } = string.Empty;     // "Дневная" / "Ночная"
        public int FilialId { get; set; }
        public string FilialName { get; set; } = string.Empty;
    }
}