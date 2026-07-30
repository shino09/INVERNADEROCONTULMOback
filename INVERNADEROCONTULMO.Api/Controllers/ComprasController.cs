using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComprasController : ControllerBase
{
    // Servicio de compras
    private readonly ICompraService _service;
    // Constructor que inyecta dependencias
    public ComprasController(ICompraService service) => _service = service;
    // Obtiene todas las compras
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
    // Obtiene una compra por su identificador
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));
    // Crea una nueva compra con actualización de stock
    [HttpPost] public async Task<IActionResult> Create([FromBody] CompraDTO dto) => Ok(await _service.CreateAsync(dto));
    // Elimina una compra por su identificador
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}
