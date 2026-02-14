using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilSalon_v._1.Domain.Entities;

public class Income
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
}
