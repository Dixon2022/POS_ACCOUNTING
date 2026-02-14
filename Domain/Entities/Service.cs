using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilSalon_v._1.Domain.Entities;

public class Service
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser positivo")]
    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
