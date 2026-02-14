using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilSalon_v._1.Domain.Entities;

public class Sale
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public int? CustomerId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    // Navigation properties
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}
