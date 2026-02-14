namespace SilSalon_v._1.Application.DTOs;

/// <summary>
/// Representa un movimiento financiero unificado (Venta, Compra, Gasto o Ingreso)
/// </summary>
public class FinancialMovementDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public FinancialMovementType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsIncome { get; set; }
    public string? Category { get; set; }
    public string? Notes { get; set; }
    public string? RelatedEntity { get; set; } // Cliente, Proveedor, etc.
}

public enum FinancialMovementType
{
    Sale,       // Venta (Ingreso)
    Purchase,   // Compra (Egreso)
    Expense,    // Gasto (Egreso)
    Income      // Otro Ingreso (Ingreso)
}
