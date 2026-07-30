using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.Models;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsuariosController : ControllerBase
{
    // Servicio de usuarios
    private readonly IUsuarioService _service;
    // Constructor que inyecta dependencias
    public UsuariosController(IUsuarioService service) => _service = service;
    // Obtiene todos los usuarios activos
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
    // Obtiene un usuario por su identificador
    [HttpGet("{id}")] public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));
    // Crea un nuevo usuario
    [HttpPost] public async Task<IActionResult> Create([FromBody] Usuario u) => Ok(await _service.CreateAsync(u));
    // Actualiza los datos de un usuario
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, [FromBody] Usuario u) => Ok(await _service.UpdateAsync(id, u));
    // Desactiva un usuario (borrado lógico)
    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteAsync(id));
}
