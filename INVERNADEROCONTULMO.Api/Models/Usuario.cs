using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("USUARIOS")]
public class Usuario
{
    // Identificador único del usuario
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Nombre completo del usuario
    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    // Correo electrónico del usuario
    [Required, MaxLength(100)]
    public string Email { get; set; }

    // Contraseña encriptada del usuario
    [Required, MaxLength(255)]
    public string PasswordHash { get; set; }

    // Rol del usuario (Admin/Operario)
    [Required, MaxLength(20)]
    public string Rol { get; set; }

    // Indica si el usuario está activo
    public bool Activo { get; set; } = true;
    // Fecha de creación del registro
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
