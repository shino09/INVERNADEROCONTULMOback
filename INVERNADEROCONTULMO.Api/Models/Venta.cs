using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("VENTAS")]
public class Venta
{
    // Identificador único de la venta
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // Número único de factura
    [Required, MaxLength(20)] public string NumeroFactura { get; set; }
    // Identificador del cliente
    public int ClienteId { get; set; }
    // Cliente asociado a la venta
    [ForeignKey("ClienteId")] public Cliente Cliente { get; set; }
    // Identificador del usuario que registró la venta
    public int UsuarioId { get; set; }
    // Usuario que registró la venta
    [ForeignKey("UsuarioId")] public Usuario Usuario { get; set; }
    // Subtotal de la venta
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Subtotal { get; set; }
    // Impuesto aplicado a la venta (18%)
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Impuesto { get; set; }
    // Monto total de la venta
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Total { get; set; }
    // Método de pago utilizado
    [MaxLength(20)] public string MetodoPago { get; set; }
    // Fecha de la venta
    public DateTime FechaVenta { get; set; } = DateTime.UtcNow;
    // Detalles de la venta
    public ICollection<DetalleVenta> Detalles { get; set; }
}

[Table("DETALLE_VENTAS")]
public class DetalleVenta
{
    // Identificador único del detalle
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // Identificador de la venta asociada
    public int VentaId { get; set; }
    // Venta asociada al detalle
    [ForeignKey("VentaId")] public Venta Venta { get; set; }
    // Identificador del producto
    public int ProductoId { get; set; }
    // Producto asociado al detalle
    [ForeignKey("ProductoId")] public Producto Producto { get; set; }
    // Cantidad vendida
    public int Cantidad { get; set; }
    // Precio unitario de venta
    [Column(TypeName = "DECIMAL(18,2)")] public decimal PrecioUnitario { get; set; }
    // Subtotal del detalle
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Subtotal { get; set; }
}
