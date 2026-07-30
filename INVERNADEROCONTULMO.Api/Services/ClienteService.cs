using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Servicio de clientes con operaciones CRUD
public interface IClienteService : ICrudService<Cliente> { }

// Implementación del servicio de clientes
public class ClienteService : IClienteService
{
    // Contexto de base de datos
    private readonly AppDbContext _db;
    // Constructor que inyecta dependencias
    public ClienteService(AppDbContext db) => _db = db;

    // Obtiene todos los clientes activos
    public async Task<IEnumerable<Cliente>> GetAllAsync() => await _db.Clientes.Where(c => c.Activo).ToListAsync();
    // Obtiene un cliente por su identificador
    public async Task<Cliente> GetByIdAsync(int id) => await _db.Clientes.FindAsync(id);
    // Crea un nuevo cliente
    public async Task<Cliente> CreateAsync(Cliente c) { _db.Clientes.Add(c); await _db.SaveChangesAsync(); return c; }

    // Actualiza los datos de un cliente
    public async Task<Cliente> UpdateAsync(int id, Cliente c)
    {
        var e = await _db.Clientes.FindAsync(id);
        if (e == null) return null;
        e.Nombre = c.Nombre; e.Documento = c.Documento; e.Email = c.Email; e.Telefono = c.Telefono; e.Direccion = c.Direccion;
        await _db.SaveChangesAsync();
        return e;
    }

    // Desactiva un cliente (borrado lógico)
    public async Task<bool> DeleteAsync(int id)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c == null) return false;
        c.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }
}
