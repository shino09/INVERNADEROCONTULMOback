using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("PEDIDOS")]
public class Pedido
{
    // Identificador único del pedido
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // Número único del pedido
    [Required, MaxLength(20)] public string NumeroPedido { get; set; }
    // Identificador del cliente asociado
    public int ClienteId { get; set; }
    // Cliente asociado al pedido
    [ForeignKey("ClienteId")] public Cliente Cliente { get; set; }
    // Estado del pedido (Pendiente/Entregado)
    [Required, MaxLength(20)] public string Estado { get; set; }
    // Monto total del pedido
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Total { get; set; }
    // Fecha en que se realizó el pedido
    public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
    // Fecha de entrega del pedido
    public DateTime? FechaEntrega { get; set; }
    // Detalles del pedido
    public ICollection<DetallePedido> Detalles { get; set; }
}

[Table("DETALLE_PEDIDOS")]
public class DetallePedido
{
    // Identificador único del detalle
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // Identificador del pedido asociado
    public int PedidoId { get; set; }
    // Pedido asociado al detalle
    [ForeignKey("PedidoId")] public Pedido Pedido { get; set; }
    // Identificador del producto
    public int ProductoId { get; set; }
    // Producto asociado al detalle
    [ForeignKey("ProductoId")] public Producto Producto { get; set; }
    // Cantidad del producto
    public int Cantidad { get; set; }
    // Precio unitario del producto
    [Column(TypeName = "DECIMAL(18,2)")] public decimal PrecioUnitario { get; set; }
    // Subtotal del detalle
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Subtotal { get; set; }
}
