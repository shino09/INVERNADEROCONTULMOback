using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Models;
using INVERNADEROCONTULMO.Api.Services;

namespace INVERNADEROCONTULMO.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Servicio de usuarios para autenticación
    private readonly IUsuarioService _usuarioService;
    // Constructor que inyecta dependencias
    public AuthController(IUsuarioService usuarioService) => _usuarioService = usuarioService;

    // Autentica un usuario y devuelve token JWT
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try { return Ok(await _usuarioService.LoginAsync(dto)); }
        catch (UnauthorizedAccessException) { return Unauthorized(new { message = "Credenciales inválidas" }); }
    }

    // Registra un nuevo usuario (solo Admin)
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Usuario usuario) => Ok(await _usuarioService.CreateAsync(usuario));
}
