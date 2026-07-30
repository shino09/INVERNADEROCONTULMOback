using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Servicio de pedidos con gestión de estados
public interface IPedidoService
{
    // Obtiene todos los pedidos con clientes y detalles
    Task<IEnumerable<Pedido>> GetAllAsync();
    // Obtiene un pedido por su identificador
    Task<Pedido> GetByIdAsync(int id);
    // Crea un nuevo pedido
    Task<Pedido> CreateAsync(PedidoDTO dto);
    // Actualiza el estado de un pedido
    Task<Pedido> UpdateEstadoAsync(int id, string estado);
    // Elimina un pedido por su identificador
    Task<bool> DeleteAsync(int id);
}

// Implementación del servicio de pedidos
public class PedidoService : IPedidoService
{
    // Contexto de base de datos
    private readonly AppDbContext _db;
    // Constructor que inyecta dependencias
    public PedidoService(AppDbContext db) => _db = db;

    // Obtiene todos los pedidos con cliente y detalles de producto
    public async Task<IEnumerable<Pedido>> GetAllAsync() =>
        await _db.Pedidos.Include(p => p.Cliente).Include(p => p.Detalles).ThenInclude(d => d.Producto).ToListAsync();

    // Obtiene un pedido por su identificador con relaciones incluidas
    public async Task<Pedido> GetByIdAsync(int id) =>
        await _db.Pedidos.Include(p => p.Cliente).Include(p => p.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

    // Crea un nuevo pedido con estado Pendiente
    public async Task<Pedido> CreateAsync(PedidoDTO dto)
    {
        var pedido = new Pedido
        {
            NumeroPedido = $"PED-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}",
            ClienteId = dto.ClienteId, Estado = "Pendiente", FechaPedido = DateTime.UtcNow, Detalles = new List<DetallePedido>()
        };
        decimal total = 0;
        foreach (var item in dto.Detalles)
        {
            var prod = await _db.Productos.FindAsync(item.ProductoId);
            if (prod == null) continue;
            var det = new DetallePedido { ProductoId = item.ProductoId, Cantidad = item.Cantidad, PrecioUnitario = prod.PrecioVenta, Subtotal = prod.PrecioVenta * item.Cantidad };
            total += det.Subtotal;
            pedido.Detalles.Add(det);
        }
        pedido.Total = total;
        _db.Pedidos.Add(pedido);
        await _db.SaveChangesAsync();
        return pedido;
    }

    // Actualiza el estado de un pedido y registra fecha de entrega
    public async Task<Pedido> UpdateEstadoAsync(int id, string estado)
    {
        var p = await _db.Pedidos.FindAsync(id);
        if (p == null) return null;
        p.Estado = estado;
        if (estado == "Entregado") p.FechaEntrega = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return p;
    }

    // Elimina un pedido por su identificador
    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _db.Pedidos.FindAsync(id);
        if (p == null) return false;
        _db.Pedidos.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}
