namespace ERP_Software.Application.DTOs;

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
    public int? Quantity { get; set; } // Cantidad de productos (solo para Ventas y Compras)
    public decimal? UnitPrice { get; set; } // Precio unitario promedio (solo para Ventas y Compras)
    public string? ProductNames { get; set; } // Nombres de productos/servicios vendidos o comprados
}

public enum FinancialMovementType
{
    Sale,       // Venta (Ingreso)
    Purchase,   // Compra (Egreso)
    Expense,    // Gasto (Egreso)
    Income      // Otro Ingreso (Ingreso)
}
