namespace ERP_Software.Application.DTOs;

public class DashboardDto
{
    public decimal IncomeToday { get; set; }
    public decimal IncomeMonth { get; set; }
    public decimal ExpensesMonth { get; set; }
    public decimal PurchasesMonth { get; set; }
    public decimal NetProfit { get; set; }
    public int NewCustomersMonth { get; set; }
    public int CustomersServedMonth { get; set; }
    public string? BestSellingProduct { get; set; }
    public string? MostRequestedService { get; set; }
    public string? MainSupplier { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public List<ChartDataPoint> SalesLast7Days { get; set; } = new();
    public List<ChartDataPoint> ExpensesVsIncomeMonth { get; set; } = new();
}

public class ChartDataPoint
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
