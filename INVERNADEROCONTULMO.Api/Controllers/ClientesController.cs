using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.Models;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    // Servicio de clientes
    private readonly IClienteService _service;
    // Constructor que inyecta dependencias
    public ClientesController(IClienteService service) => _service = service;
    // Obtiene todos los clientes activos
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
    // Obtiene un cliente por su identificador
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));
    // Crea un nuevo cliente
    [HttpPost] public async Task<IActionResult> Create([FromBody] Cliente c) => Ok(await _service.CreateAsync(c));
    // Actualiza los datos de un cliente
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, [FromBody] Cliente c) => Ok(await _service.UpdateAsync(id, c));
    // Desactiva un cliente (borrado lógico)
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}
