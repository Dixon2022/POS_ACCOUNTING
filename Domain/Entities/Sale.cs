using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_Software.Domain.Enums;

namespace ERP_Software.Domain.Entities;

public class Sale
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public int? CustomerId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    /// <summary>
    /// Monto de abono inicial pagado por el cliente
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DepositAmount { get; set; } = 0;

    /// <summary>
    /// Saldo pendiente por pagar (Total - DepositAmount)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal PendingBalance { get; set; } = 0;

    /// <summary>
    /// Indica si los productos fueron entregados al cliente.
    /// Si es false, no se descuenta del inventario.
    /// </summary>
    public bool ProductDelivered { get; set; } = true;

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Efectivo;

    [StringLength(500, ErrorMessage = "Las notas no pueden exceder 500 caracteres")]
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}
