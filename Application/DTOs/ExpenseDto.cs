using System.ComponentModel.DataAnnotations;

namespace SilSalon_v._1.Application.DTOs;

public class ExpenseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres")]
    public string? Category { get; set; }
}
