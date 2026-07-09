using System.ComponentModel.DataAnnotations;

namespace ERP_Software.Application.DTOs;

public class ServiceDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser positivo")]
    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; } = true;
}
