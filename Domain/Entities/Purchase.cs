using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SilSalon_v._1.Domain.Enums;

namespace SilSalon_v._1.Domain.Entities;

public class Purchase
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public int SupplierId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    /// <summary>
    /// Monto de abono inicial pagado al proveedor
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DepositAmount { get; set; } = 0;

    /// <summary>
    /// Saldo pendiente por pagar (Total - DepositAmount)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal PendingBalance { get; set; } = 0;

    /// <summary>
    /// Indica si los productos fueron recibidos en el salón.
    /// Si es false, no se suma al inventario.
    /// </summary>
    public bool ProductReceived { get; set; } = true;

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;

    [StringLength(500, ErrorMessage = "Las notas no pueden exceder 500 caracteres")]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Supplier Supplier { get; set; } = null!;
    public virtual ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
}
