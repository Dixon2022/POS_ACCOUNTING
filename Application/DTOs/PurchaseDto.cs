using SilSalon_v._1.Domain.Enums;

namespace SilSalon_v._1.Application.DTOs;

public class PurchaseDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public decimal Total { get; set; }
    
    /// <summary>
    /// Monto de abono inicial pagado al proveedor
    /// </summary>
    public decimal DepositAmount { get; set; } = 0;
    
    /// <summary>
    /// Saldo pendiente por pagar
    /// </summary>
    public decimal PendingBalance { get; set; } = 0;
    
    /// <summary>
    /// Indica si los productos fueron recibidos en el salón.
    /// Si es false, no se suma al inventario.
    /// </summary>
    public bool ProductReceived { get; set; } = true;
    
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;
    public string? Notes { get; set; }
    public List<PurchaseItemDto> Items { get; set; } = new();
    
    /// <summary>
    /// Indica si la compra tiene un abono parcial
    /// </summary>
    public bool HasDeposit => DepositAmount > 0 && DepositAmount < Total;
    
    /// <summary>
    /// Indica si la compra está pagada completamente
    /// </summary>
    public bool IsPaidInFull => PendingBalance <= 0;
}

public class PurchaseItemDto
{
    public int Id { get; set; }
    public int PurchaseId { get; set; }
    public int ProductVariantId { get; set; }
    public string? ProductVariantName { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitCost { get; set; }
    public decimal Total { get; set; }
}
