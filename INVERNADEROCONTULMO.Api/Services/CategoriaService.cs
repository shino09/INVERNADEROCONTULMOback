using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

public interface ICategoriaService : ICrudService<Categoria> { }

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _db;
    public CategoriaService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Categoria>> GetAllAsync() => await _db.Set<Categoria>().Where(c => c.Activo).ToListAsync();
    public async Task<Categoria> GetByIdAsync(int id) => await _db.Set<Categoria>().FindAsync(id);
    public async Task<Categoria> CreateAsync(Categoria c) { _db.Set<Categoria>().Add(c); await _db.SaveChangesAsync(); return c; }

    public async Task<Categoria> UpdateAsync(int id, Categoria c)
    {
        var e = await _db.Set<Categoria>().FindAsync(id);
        if (e == null) return null;
        e.Nombre = c.Nombre; e.Descripcion = c.Descripcion;
        await _db.SaveChangesAsync();
        return e;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var c = await _db.Set<Categoria>().FindAsync(id);
        if (c == null) return false;
        c.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }
}
