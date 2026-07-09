using System.ComponentModel.DataAnnotations;

namespace ERP_Software.Application.DTOs;

public class SupplierDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Phone { get; set; }

    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    [StringLength(100, ErrorMessage = "El nombre de contacto no puede exceder 100 caracteres")]
    public string? ContactPerson { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;
}
