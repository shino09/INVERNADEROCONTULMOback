using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Servicio de contabilidad con libro diario y libro mayor
public interface IContabilidadService
{
    // Obtiene asientos del libro diario filtrados por fecha
    Task<IEnumerable<AsientoContable>> GetLibroDiarioAsync(DateTime? desde, DateTime? hasta);
    // Obtiene cuentas del libro mayor con saldos agrupados
    Task<IEnumerable<object>> GetLibroMayorAsync(DateTime? desde, DateTime? hasta);
    // Obtiene todos los asientos contables
    Task<IEnumerable<AsientoContable>> GetAllAsientosAsync();
    // Crea un nuevo asiento contable
    Task<AsientoContable> CreateAsientoAsync(AsientoContable asiento);
}

// Implementación del servicio de contabilidad
public class ContabilidadService : IContabilidadService
{
    // Contexto de base de datos
    private readonly AppDbContext _db;
    // Constructor que inyecta dependencias
    public ContabilidadService(AppDbContext db) => _db = db;

    // Obtiene asientos del libro diario ordenados por fecha, con filtro opcional
    public async Task<IEnumerable<AsientoContable>> GetLibroDiarioAsync(DateTime? desde, DateTime? hasta)
    {
        var q = _db.AsientosContables.AsQueryable();
        if (desde.HasValue) q = q.Where(a => a.FechaAsiento >= desde.Value);
        if (hasta.HasValue) q = q.Where(a => a.FechaAsiento <= hasta.Value);
        return await q.OrderBy(a => a.FechaAsiento).ThenBy(a => a.NumeroAsiento).ToListAsync();
    }

    // Obtiene cuentas del libro mayor agrupadas con saldo final
    public async Task<IEnumerable<object>> GetLibroMayorAsync(DateTime? desde, DateTime? hasta)
    {
        var q = _db.AsientosContables.AsQueryable();
        if (desde.HasValue) q = q.Where(a => a.FechaAsiento >= desde.Value);
        if (hasta.HasValue) q = q.Where(a => a.FechaAsiento <= hasta.Value);
        var asientos = await q.ToListAsync();
        return asientos.GroupBy(a => a.CuentaContable).Select(g => new
        {
            Cuenta = g.Key,
            SaldoDebe = g.Sum(a => a.Debe),
            SaldoHaber = g.Sum(a => a.Haber),
            SaldoFinal = g.Sum(a => a.Debe) - g.Sum(a => a.Haber),
            Movimientos = g.OrderBy(a => a.FechaAsiento).ToList()
        }).ToList();
    }

    // Obtiene todos los asientos contables ordenados por fecha descendente
    public async Task<IEnumerable<AsientoContable>> GetAllAsientosAsync() =>
        await _db.AsientosContables.OrderByDescending(a => a.FechaAsiento).ToListAsync();

    // Crea un nuevo asiento contable con número generado automáticamente
    public async Task<AsientoContable> CreateAsientoAsync(AsientoContable a)
    {
        a.NumeroAsiento = $"AS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        _db.AsientosContables.Add(a);
        await _db.SaveChangesAsync();
        return a;
    }
}
