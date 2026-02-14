using System.ComponentModel.DataAnnotations;

namespace SilSalon_v._1.Domain.Entities;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    public int SupplierId { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Supplier Supplier { get; set; } = null!;
    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}
