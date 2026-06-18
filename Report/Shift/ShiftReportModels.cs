namespace вход_курсач
{
    public class ShiftReportData
    {
        public int Id { get; set; }
        public int ReportNumber { get; set; }
        public string ReportDate { get; set; } = string.Empty;
        public int FilialId { get; set; }
        public string ShiftType { get; set; } = string.Empty;
        public int ShiftManagerId { get; set; }
        public int? NextManagerId { get; set; }
        public double PlannedGas { get; set; }
        public double ActualGas { get; set; }
        public double Completion { get; set; }
        public string TransferNote { get; set; } = string.Empty;
    }

    public class ShiftReportListItem
    {
        public int Id { get; set; }
        public int ReportNumber { get; set; }
        public string ReportDate { get; set; } = string.Empty;
        public string ShiftType { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public double PlannedGas { get; set; }
        public double ActualGas { get; set; }
        public double Completion { get; set; }
    }
}