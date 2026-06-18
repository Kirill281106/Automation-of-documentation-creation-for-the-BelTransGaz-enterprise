public class QuarterlyReportListItem
{
    public int Id { get; set; }
    public int Quarter { get; set; }
    public int Year { get; set; }
    public string FilialName { get; set; }
    public string CompiledBy { get; set; }
    public string AddressedTo { get; set; }
    public string CreatedAt { get; set; }
    public double PlannedGas { get; set; }
    public double ActualGas { get; set; }
    public double Completion { get; set; }

    public double PurchasePrice { get; set; }
    public double SalePrice { get; set; }
    public double Revenue { get; set; }
    public double Cost { get; set; }
    public double Profit { get; set; }
}