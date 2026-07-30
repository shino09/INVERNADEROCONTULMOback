using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.Models;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Tests;

public class ProductoServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProducto()
    {
        using var db = CreateDbContext();
        var service = new ProductoService(db);
        var producto = new Producto { Nombre = "Tomate", Descripcion = "Test", Categoria = "Test", PrecioCompra = 2, PrecioVenta = 5, StockActual = 50 };

        var result = await service.CreateAsync(producto);

        Assert.NotNull(result);
        Assert.Equal("Tomate", result.Nombre);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDelete()
    {
        using var db = CreateDbContext();
        var service = new ProductoService(db);
        var producto = new Producto { Nombre = "Lechuga", Descripcion = "Test", Categoria = "Test", PrecioCompra = 1, PrecioVenta = 3, StockActual = 20 };
        await service.CreateAsync(producto);

        var deleted = await service.DeleteAsync(1);

        Assert.True(deleted);
        var fetched = await service.GetByIdAsync(1);
        Assert.False(fetched.Activo);
    }
}
