using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Data;

public class AppDbContext : DbContext
{
    // Constructor del contexto de base de datos
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Tabla de usuarios
    public DbSet<Usuario> Usuarios { get; set; }
    // Tabla de productos
    public DbSet<Producto> Productos { get; set; }
    // Tabla de clientes
    public DbSet<Cliente> Clientes { get; set; }
    // Tabla de pedidos
    public DbSet<Pedido> Pedidos { get; set; }
    // Tabla de detalle de pedidos
    public DbSet<DetallePedido> DetallePedidos { get; set; }
    // Tabla de ventas
    public DbSet<Venta> Ventas { get; set; }
    // Tabla de detalle de ventas
    public DbSet<DetalleVenta> DetalleVentas { get; set; }
    // Tabla de compras
    public DbSet<Compra> Compras { get; set; }
    // Tabla de detalle de compras
    public DbSet<DetalleCompra> DetalleCompras { get; set; }
    // Tabla de asientos contables
    public DbSet<AsientoContable> AsientosContables { get; set; }
    // Tabla de categorías
    public DbSet<Categoria> Categorias { get; set; }
    // Tabla de proveedores
    public DbSet<Proveedor> Proveedores { get; set; }

    // Configura las relaciones entre entidades
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>().HasMany(p => p.Detalles).WithOne(d => d.Pedido).HasForeignKey(d => d.PedidoId);
        modelBuilder.Entity<Venta>().HasMany(v => v.Detalles).WithOne(d => d.Venta).HasForeignKey(d => d.VentaId);
        modelBuilder.Entity<Compra>().HasMany(c => c.Detalles).WithOne(d => d.Compra).HasForeignKey(d => d.CompraId);

        // Mapea las propiedades C# a columnas Oracle en UPPER_SNAKE_CASE
        // (ej: PasswordHash -> PASSWORD_HASH, FechaCreacion -> FECHA_CREACION)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName(ToOracleColumn(property.Name));
            }
        }

        // Oracle no soporta bool nativo: se mapea como NUMBER(1) (0 = false, 1 = true)
        var entitiesWithActivo = new[]
        {
            modelBuilder.Entity<Usuario>().Metadata.Name,
            modelBuilder.Entity<Producto>().Metadata.Name,
            modelBuilder.Entity<Cliente>().Metadata.Name,
            modelBuilder.Entity<Categoria>().Metadata.Name,
            modelBuilder.Entity<Proveedor>().Metadata.Name
        };
        foreach (var entityName in entitiesWithActivo)
        {
            var entity = modelBuilder.Entity(entityName);
            entity.Property("Activo").HasConversion<decimal>();
        }
    }

    // Convierte un nombre PascalCase a UPPER_SNAKE_CASE
    private static string ToOracleColumn(string name)
    {
        var builder = new System.Text.StringBuilder();
        foreach (var c in name)
        {
            if (char.IsUpper(c) && builder.Length > 0) builder.Append('_');
            builder.Append(char.ToUpperInvariant(c));
        }
        return builder.ToString();
    }
}
