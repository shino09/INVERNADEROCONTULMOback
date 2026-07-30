using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("CLIENTES")]
public class Cliente
{
    // Identificador único del cliente
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Nombre del cliente
    [Required, MaxLength(200)] public string Nombre { get; set; }
    // Número de documento del cliente
    [MaxLength(20)] public string Documento { get; set; }
    // Correo electrónico del cliente
    [MaxLength(100)] public string Email { get; set; }
    // Teléfono del cliente
    [MaxLength(20)] public string Telefono { get; set; }
    // Dirección del cliente
    [MaxLength(500)] public string Direccion { get; set; }
    // Indica si el cliente está activo
    public bool Activo { get; set; } = true;
    // Fecha de creación del registro
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
