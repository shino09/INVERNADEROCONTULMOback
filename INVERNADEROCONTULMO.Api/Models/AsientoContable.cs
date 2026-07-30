using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVERNADEROCONTULMO.Api.Models;

[Table("ASIENTOS_CONTABLES")]
public class AsientoContable
{
    // Identificador único del asiento
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // Número único del asiento contable
    [Required, MaxLength(20)] public string NumeroAsiento { get; set; }
    // Descripción del asiento
    [MaxLength(500)] public string Descripcion { get; set; }
    // Fecha del asiento contable
    public DateTime FechaAsiento { get; set; } = DateTime.UtcNow;
    // Monto del debe
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Debe { get; set; }
    // Monto del haber
    [Column(TypeName = "DECIMAL(18,2)")] public decimal Haber { get; set; }
    // Nombre de la cuenta contable
    [MaxLength(100)] public string CuentaContable { get; set; }
    // Tipo de asiento (Venta/Compra)
    [MaxLength(50)] public string TipoAsiento { get; set; }
    // Identificador del registro de referencia
    public int? ReferenciaId { get; set; }
    // Tipo de referencia
    [MaxLength(50)] public string ReferenciaTipo { get; set; }
}
