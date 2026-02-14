using SilSalon_v._1.Domain.Enums;

namespace SilSalon_v._1.Application.DTOs;

public class PurchaseDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public decimal Total { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;
    public string? Notes { get; set; }
    public List<PurchaseItemDto> Items { get; set; } = new();
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
