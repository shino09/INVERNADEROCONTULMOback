using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("PROVEEDORES")]
public class Proveedor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(200)] public string Nombre { get; set; }
    [MaxLength(20)] public string Rut { get; set; }
    [MaxLength(100)] public string Email { get; set; }
    [MaxLength(20)] public string Telefono { get; set; }
    [MaxLength(500)] public string Direccion { get; set; }
    [MaxLength(100)] public string Contacto { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
