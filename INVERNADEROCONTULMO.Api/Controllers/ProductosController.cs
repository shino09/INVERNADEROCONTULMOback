using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.Models;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    // Servicio de productos
    private readonly IProductoService _service;
    // Constructor que inyecta dependencias
    public ProductosController(IProductoService service) => _service = service;
    // Obtiene todos los productos activos
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
    // Obtiene un producto por su identificador
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));
    // Obtiene productos filtrados por categoría
    [HttpGet("categoria/{categoria}")] public async Task<IActionResult> GetByCategoria(string categoria) => Ok(await _service.GetByCategoriaAsync(categoria));
    // Crea un nuevo producto
    [HttpPost] public async Task<IActionResult> Create([FromBody] Producto p) => Ok(await _service.CreateAsync(p));
    // Actualiza los datos de un producto
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, [FromBody] Producto p) => Ok(await _service.UpdateAsync(id, p));
    // Desactiva un producto (borrado lógico)
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}
