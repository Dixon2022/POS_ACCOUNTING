using System.ComponentModel.DataAnnotations;

namespace ERP_Software.Application.DTOs;

public class ProductVariantDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El producto es requerido")]
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    [Required(ErrorMessage = "El nombre de la variante es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
    public string? Code { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio de costo debe ser positivo")]
    public decimal CostPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio de venta debe ser positivo")]
    public decimal SalePrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    public bool IsActive { get; set; } = true;

    public string FullName => $"{ProductName} - {Name}";
}
