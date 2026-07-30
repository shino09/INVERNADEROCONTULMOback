using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("PRODUCTOS")]
public class Producto
{
    // Identificador único del producto
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Nombre del producto
    [Required, MaxLength(200)]
    public string Nombre { get; set; }

    // Descripción del producto
    [MaxLength(500)]
    public string Descripcion { get; set; }

    // Precio de compra del producto
    [Required, Column(TypeName = "DECIMAL(18,2)")]
    public decimal PrecioCompra { get; set; }

    // Precio de venta del producto
    [Required, Column(TypeName = "DECIMAL(18,2)")]
    public decimal PrecioVenta { get; set; }

    // Cantidad actual en stock
    public int StockActual { get; set; }
    // Stock mínimo permitido
    public int StockMinimo { get; set; } = 10;
    // Categoría del producto
    [MaxLength(100)] public string Categoria { get; set; }
    // Indica si el producto está activo
    public bool Activo { get; set; } = true;
    // Fecha de creación del registro
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
