using System.ComponentModel.DataAnnotations;

namespace SilSalon_v._1.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El proveedor es requerido")]
    public int SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public bool IsActive { get; set; } = true;

    public int VariantsCount { get; set; }
}
