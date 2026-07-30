using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Servicio de productos con operaciones CRUD y filtro por categoría
public interface IProductoService : ICrudService<Producto>
{
    // Obtiene productos filtrados por categoría
    Task<IEnumerable<Producto>> GetByCategoriaAsync(string categoria);
}

// Implementación del servicio de productos
public class ProductoService : IProductoService
{
    // Contexto de base de datos
    private readonly AppDbContext _db;
    // Constructor que inyecta dependencias
    public ProductoService(AppDbContext db) => _db = db;

    // Obtiene todos los productos activos
    public async Task<IEnumerable<Producto>> GetAllAsync() => await _db.Productos.Where(p => p.Activo).ToListAsync();
    // Obtiene un producto por su identificador
    public async Task<Producto> GetByIdAsync(int id) => await _db.Productos.FindAsync(id);
    // Crea un nuevo producto
    public async Task<Producto> CreateAsync(Producto p) { _db.Productos.Add(p); await _db.SaveChangesAsync(); return p; }

    // Actualiza los datos de un producto
    public async Task<Producto> UpdateAsync(int id, Producto p)
    {
        var e = await _db.Productos.FindAsync(id);
        if (e == null) return null;
        e.Nombre = p.Nombre; e.Descripcion = p.Descripcion; e.PrecioCompra = p.PrecioCompra;
        e.PrecioVenta = p.PrecioVenta; e.StockActual = p.StockActual; e.StockMinimo = p.StockMinimo; e.Categoria = p.Categoria;
        await _db.SaveChangesAsync();
        return e;
    }

    // Desactiva un producto (borrado lógico)
    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _db.Productos.FindAsync(id);
        if (p == null) return false;
        p.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }

    // Obtiene productos por categoría
    public async Task<IEnumerable<Producto>> GetByCategoriaAsync(string categoria) =>
        await _db.Productos.Where(p => p.Categoria == categoria && p.Activo).ToListAsync();
}
