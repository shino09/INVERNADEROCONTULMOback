using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("COMPRAS")]
public class Compra
{
    // Identificador único de la compra
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // Número único de compra
    [Required, MaxLength(20)] public string NumeroCompra { get; set; }
    // Nombre del proveedor
    [MaxLength(200)] public string Proveedor { get; set; }
    // Monto total de la compra
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Total { get; set; }
    // Fecha de la compra
    public DateTime FechaCompra { get; set; } = DateTime.UtcNow;
    // Detalles de la compra
    public ICollection<DetalleCompra> Detalles { get; set; }
}

[Table("DETALLE_COMPRAS")]
public class DetalleCompra
{
    // Identificador único del detalle
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // Identificador de la compra asociada
    public int CompraId { get; set; }
    // Compra asociada al detalle
    [ForeignKey("CompraId")] public Compra Compra { get; set; }
    // Identificador del producto
    public int ProductoId { get; set; }
    // Producto asociado al detalle
    [ForeignKey("ProductoId")] public Producto Producto { get; set; }
    // Cantidad comprada
    public int Cantidad { get; set; }
    // Precio unitario de compra
    [Column(TypeName = "DECIMAL(18,2)")] public decimal PrecioUnitario { get; set; }
    // Subtotal del detalle
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Subtotal { get; set; }
}
