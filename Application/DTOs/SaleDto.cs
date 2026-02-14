using SilSalon_v._1.Domain.Enums;

namespace SilSalon_v._1.Application.DTOs;

public class SaleDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal Total { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;
    public string? Notes { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
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
