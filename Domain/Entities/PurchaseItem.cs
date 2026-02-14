using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilSalon_v._1.Domain.Entities;

public class PurchaseItem
{
    public int Id { get; set; }

    public int PurchaseId { get; set; }

    public int ProductVariantId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    // Navigation properties
    public virtual Purchase Purchase { get; set; } = null!;
    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
