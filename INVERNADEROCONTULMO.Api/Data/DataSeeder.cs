using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Data;

public static class DataSeeder
{
    // Siembra datos iniciales de usuarios, productos y clientes en la BD
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Usuarios.Any()) return;

        db.Categorias.AddRange(
            new Categoria { Nombre = "Hortalizas", Descripcion = "Hortalizas y verduras frescas" },
            new Categoria { Nombre = "Aromáticas", Descripcion = "Hierbas aromáticas y condimentos" },
            new Categoria { Nombre = "Frutas", Descripcion = "Frutas frescas de temporada" }
        );

        db.Usuarios.AddRange(
            new Usuario { Nombre = "Admin Principal", Email = "admin@invernadero.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Rol = "Admin" },
            new Usuario { Nombre = "Operario Juan", Email = "operario@invernadero.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("operario123"), Rol = "Operario" }
        );

        db.Productos.AddRange(
            new Producto { Nombre = "Tomate Cherry", Descripcion = "Tomate cherry orgánico", PrecioCompra = 2.5m, PrecioVenta = 5, StockActual = 100, StockMinimo = 20, Categoria = "Hortalizas" },
            new Producto { Nombre = "Lechuga Romana", Descripcion = "Lechuga romana hidropónica", PrecioCompra = 1.8m, PrecioVenta = 3.5m, StockActual = 80, StockMinimo = 15, Categoria = "Hortalizas" },
            new Producto { Nombre = "Pimiento Rojo", Descripcion = "Pimiento rojo dulce", PrecioCompra = 3, PrecioVenta = 6, StockActual = 60, StockMinimo = 10, Categoria = "Hortalizas" },
            new Producto { Nombre = "Pepino", Descripcion = "Pepino verde largo", PrecioCompra = 1.5m, PrecioVenta = 3, StockActual = 120, StockMinimo = 25, Categoria = "Hortalizas" },
            new Producto { Nombre = "Albahaca", Descripcion = "Albahaca fresca en maceta", PrecioCompra = 2, PrecioVenta = 4.5m, StockActual = 50, StockMinimo = 10, Categoria = "Aromáticas" }
        );

        db.Proveedores.AddRange(
            new Proveedor { Nombre = "Semillas del Sur Ltda.", Rut = "76.123.456-7", Email = "ventas@semillassur.cl", Telefono = "+56 9 8765 4321", Direccion = "Av. Matta 456, Santiago", Contacto = "Carlos Muñoz" },
            new Proveedor { Nombre = "Fertilizantes Norte Grande S.A.", Rut = "77.987.654-3", Email = "pedidos@fertilizantesng.cl", Telefono = "+56 9 9123 4567", Direccion = "Calle Prat 789, Antofagasta", Contacto = "María González" },
            new Proveedor { Nombre = "Invernaderos del Maule SpA", Rut = "78.456.789-1", Email = "contacto@invernaderosmaule.cl", Telefono = "+56 9 7654 3210", Direccion = "Ruta 5 Sur Km 230, Talca", Contacto = "Pedro Soto" }
        );

        db.Clientes.AddRange(
            new Cliente { Nombre = "Mercado Mayorista Lo Valledor", Documento = "76.543.210-8", Email = "compras@lovalledor.cl", Telefono = "+56 9 8877 6655", Direccion = "Av. Departamental 5100, Santiago" },
            new Cliente { Nombre = "Supermercados Unimarc", Documento = "77.321.654-0", Email = "pedidos@unimarc.cl", Telefono = "+56 9 9988 7766", Direccion = "Av. Argentina 1234, Valparaíso" },
            new Cliente { Nombre = "Restaurante Boragó", Documento = "78.654.321-9", Email = "cocina@borago.cl", Telefono = "+56 9 7766 5544", Direccion = "Nueva Costanera 3467, Santiago" }
        );

        await db.SaveChangesAsync();
    }
}
