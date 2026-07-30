using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

public interface IProveedorService : ICrudService<Proveedor> { }

public class ProveedorService : IProveedorService
{
    private readonly AppDbContext _db;
    public ProveedorService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Proveedor>> GetAllAsync() => await _db.Set<Proveedor>().Where(p => p.Activo).ToListAsync();
    public async Task<Proveedor> GetByIdAsync(int id) => await _db.Set<Proveedor>().FindAsync(id);

    public async Task<Proveedor> CreateAsync(Proveedor p)
    {
        _db.Set<Proveedor>().Add(p);
        await _db.SaveChangesAsync();
        return p;
    }

    public async Task<Proveedor> UpdateAsync(int id, Proveedor p)
    {
        var e = await _db.Set<Proveedor>().FindAsync(id);
        if (e == null) return null;
        e.Nombre = p.Nombre; e.Rut = p.Rut; e.Email = p.Email;
        e.Telefono = p.Telefono; e.Direccion = p.Direccion; e.Contacto = p.Contacto;
        await _db.SaveChangesAsync();
        return e;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _db.Set<Proveedor>().FindAsync(id);
        if (p == null) return false;
        p.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }
}
