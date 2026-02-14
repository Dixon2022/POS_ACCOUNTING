using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SilSalon_v._1.Domain.Enums;

namespace SilSalon_v._1.Domain.Entities;

public class Purchase
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public int SupplierId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;

    [StringLength(500, ErrorMessage = "Las notas no pueden exceder 500 caracteres")]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Supplier Supplier { get; set; } = null!;
    public virtual ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
}
