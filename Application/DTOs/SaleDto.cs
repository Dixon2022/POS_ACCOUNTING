using ERP_Software.Domain.Enums;

namespace ERP_Software.Application.DTOs;

public class SaleDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal Total { get; set; }
    
    /// <summary>
    /// Monto de abono inicial pagado por el cliente
    /// </summary>
    public decimal DepositAmount { get; set; } = 0;
    
    /// <summary>
    /// Saldo pendiente por pagar
    /// </summary>
    public decimal PendingBalance { get; set; } = 0;
    
    /// <summary>
    /// Indica si los productos fueron entregados al cliente.
    /// Si es false, no se descuenta del inventario.
    /// </summary>
    public bool ProductDelivered { get; set; } = true;
    
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;
    public string? Notes { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
    
    /// <summary>
    /// Indica si la venta tiene un abono parcial
    /// </summary>
    public bool HasDeposit => DepositAmount > 0 && DepositAmount < Total;
    
    /// <summary>
    /// Indica si la venta está pagada completamente
    /// </summary>
    public bool IsPaidInFull => PendingBalance <= 0;
}

public class SaleItemDto
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public ItemType ItemType { get; set; }
    public int? ProductVariantId { get; set; }
    public string? ProductVariantName { get; set; }
    public int? ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }

    public string ItemName => ItemType == ItemType.Product ? ProductVariantName ?? "" : ServiceName ?? "";
}
