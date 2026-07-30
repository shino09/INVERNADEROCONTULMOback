using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Servicio de compras con actualización de stock y contabilidad
public interface ICompraService
{
    // Obtiene todas las compras con detalles
    Task<IEnumerable<Compra>> GetAllAsync();
    // Obtiene una compra por su identificador
    Task<Compra> GetByIdAsync(int id);
    // Crea una nueva compra con asientos contables
    Task<Compra> CreateAsync(CompraDTO dto);
    // Elimina una compra por su identificador
    Task<bool> DeleteAsync(int id);
}

// Implementación del servicio de compras
public class CompraService : ICompraService
{
    // Contexto de base de datos
    private readonly AppDbContext _db;
    // Constructor que inyecta dependencias
    public CompraService(AppDbContext db) => _db = db;

    // Obtiene todas las compras con detalles de producto
    public async Task<IEnumerable<Compra>> GetAllAsync() =>
        await _db.Compras.Include(c => c.Detalles).ThenInclude(d => d.Producto).ToListAsync();

    // Obtiene una compra por su identificador con relaciones incluidas
    public async Task<Compra> GetByIdAsync(int id) =>
        await _db.Compras.Include(c => c.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(c => c.Id == id);

    // Crea una nueva compra, actualiza stock y genera asientos contables
    public async Task<Compra> CreateAsync(CompraDTO dto)
    {
        var compra = new Compra
            {
                NumeroCompra = $"COM-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}",
                Proveedor = dto.Proveedor, FechaCompra = DateTime.UtcNow, Detalles = new List<DetalleCompra>()
            };
            decimal total = 0;
            foreach (var item in dto.Detalles)
            {
                var prod = await _db.Productos.FindAsync(item.ProductoId);
                if (prod == null) throw new InvalidOperationException("Producto no encontrado");
                var det = new DetalleCompra { ProductoId = item.ProductoId, Cantidad = item.Cantidad, PrecioUnitario = item.PrecioUnitario, Subtotal = item.PrecioUnitario * item.Cantidad };
                total += det.Subtotal;
                compra.Detalles.Add(det);
                prod.StockActual += item.Cantidad;
                prod.PrecioCompra = item.PrecioUnitario;
            }
            compra.Total = total;
            _db.Compras.Add(compra);
            _db.AsientosContables.Add(new AsientoContable { NumeroAsiento = $"AS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}", Descripcion = $"Compra {compra.NumeroCompra}", FechaAsiento = DateTime.UtcNow, Debe = total, Haber = 0, CuentaContable = "Inventario", TipoAsiento = "Compra", ReferenciaId = compra.Id, ReferenciaTipo = "Compra" });
            _db.AsientosContables.Add(new AsientoContable { NumeroAsiento = $"AS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}", Descripcion = $"Compra {compra.NumeroCompra}", FechaAsiento = DateTime.UtcNow, Debe = 0, Haber = total, CuentaContable = "Banco", TipoAsiento = "Compra", ReferenciaId = compra.Id, ReferenciaTipo = "Compra" });

            await _db.SaveChangesAsync();
            return compra;
    }

    // Elimina una compra por su identificador
    public async Task<bool> DeleteAsync(int id)
    {
        var c = await _db.Compras.FindAsync(id);
        if (c == null) return false;
        _db.Compras.Remove(c);
        await _db.SaveChangesAsync();
        return true;
    }
}
