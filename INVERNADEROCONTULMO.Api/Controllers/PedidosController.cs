using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidosController : ControllerBase
{
    // Servicio de pedidos
    private readonly IPedidoService _service;
    // Constructor que inyecta dependencias
    public PedidosController(IPedidoService service) => _service = service;
    // Obtiene todos los pedidos
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
    // Obtiene un pedido por su identificador
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));
    // Crea un nuevo pedido
    [HttpPost] public async Task<IActionResult> Create([FromBody] PedidoDTO dto) => Ok(await _service.CreateAsync(dto));
    // Actualiza el estado de un pedido
    [HttpPut("{id}/estado")] public async Task<IActionResult> UpdateEstado(int id, [FromBody] string estado) => Ok(await _service.UpdateEstadoAsync(id, estado));
    // Elimina un pedido por su identificador
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}
