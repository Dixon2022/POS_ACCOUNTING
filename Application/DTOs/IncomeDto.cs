using System.ComponentModel.DataAnnotations;

namespace ERP_Software.Application.DTOs;

public class IncomeDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
}
