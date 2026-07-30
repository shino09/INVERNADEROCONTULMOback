using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Servicio de ventas con creación, consulta y eliminación
public interface IVentaService
{
    // Obtiene todas las ventas con clientes y detalles
    Task<IEnumerable<Venta>> GetAllAsync();
    // Obtiene una venta por su identificador
    Task<Venta> GetByIdAsync(int id);
    // Crea una nueva venta con asientos contables
    Task<Venta> CreateAsync(VentaDTO dto, int usuarioId);
    // Elimina una venta por su identificador
    Task<bool> DeleteAsync(int id);
}

// Implementación del servicio de ventas
public class VentaService : IVentaService
{
    // Contexto de base de datos
    private readonly AppDbContext _db;
    // Constructor que inyecta dependencias
    public VentaService(AppDbContext db) => _db = db;

    // Obtiene todas las ventas con clientes y detalles de producto
    public async Task<IEnumerable<Venta>> GetAllAsync() =>
        await _db.Ventas.Include(v => v.Cliente).Include(v => v.Detalles).ThenInclude(d => d.Producto).ToListAsync();

    // Obtiene una venta por su identificador con relaciones incluidas
    public async Task<Venta> GetByIdAsync(int id) =>
        await _db.Ventas.Include(v => v.Cliente).Include(v => v.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);

    // Crea una nueva venta, descuenta stock y genera asientos contables
    public async Task<Venta> CreateAsync(VentaDTO dto, int usuarioId)
    {
        var venta = new Venta
        {
            NumeroFactura = $"FAC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}",
            ClienteId = dto.ClienteId, UsuarioId = usuarioId, MetodoPago = dto.MetodoPago,
            FechaVenta = DateTime.UtcNow, Detalles = new List<DetalleVenta>()
        };

        decimal subtotal = 0;
        foreach (var item in dto.Detalles)
        {
            var prod = await _db.Productos.FindAsync(item.ProductoId);
            if (prod == null) throw new InvalidOperationException("Producto no encontrado");
            if (prod.StockActual < item.Cantidad) throw new InvalidOperationException($"Stock insuficiente para {prod.Nombre}: disponible {prod.StockActual}, solicitado {item.Cantidad}");
            var det = new DetalleVenta { ProductoId = item.ProductoId, Cantidad = item.Cantidad, PrecioUnitario = prod.PrecioVenta, Subtotal = prod.PrecioVenta * item.Cantidad };
            subtotal += det.Subtotal;
            venta.Detalles.Add(det);
            prod.StockActual -= item.Cantidad;
        }

        venta.Subtotal = subtotal;
        venta.Impuesto = subtotal * 0.18m;
        venta.Total = subtotal + venta.Impuesto;
        _db.Ventas.Add(venta);
        _db.AsientosContables.Add(new AsientoContable { NumeroAsiento = $"AS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}", Descripcion = $"Venta {venta.NumeroFactura}", FechaAsiento = DateTime.UtcNow, Debe = venta.Total, Haber = 0, CuentaContable = "Caja", TipoAsiento = "Venta", ReferenciaId = venta.Id, ReferenciaTipo = "Venta" });
        _db.AsientosContables.Add(new AsientoContable { NumeroAsiento = $"AS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}", Descripcion = $"Venta {venta.NumeroFactura}", FechaAsiento = DateTime.UtcNow, Debe = 0, Haber = venta.Total, CuentaContable = "Ventas", TipoAsiento = "Venta", ReferenciaId = venta.Id, ReferenciaTipo = "Venta" });

        await _db.SaveChangesAsync();
        return await GetByIdAsync(venta.Id);
    }

    // Elimina una venta por su identificador
    public async Task<bool> DeleteAsync(int id)
    {
        var v = await _db.Ventas.FindAsync(id);
        if (v == null) return false;
        _db.Ventas.Remove(v);
        await _db.SaveChangesAsync();
        return true;
    }
}
