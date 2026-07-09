using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_Software.Domain.Enums;

namespace ERP_Software.Domain.Entities;

public class SaleItem
{
    public int Id { get; set; }

    public int SaleId { get; set; }

    public ItemType ItemType { get; set; }

    public int? ProductVariantId { get; set; }

    public int? ServiceId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    // Navigation properties
    public virtual Sale Sale { get; set; } = null!;
    public virtual ProductVariant? ProductVariant { get; set; }
    public virtual Service? Service { get; set; }
}
