using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Helpers;

// Helper para generación y validación de tokens JWT
public class JwtHelper
{
    // Configuración de la aplicación
    private readonly IConfiguration _config;
    // Constructor que recibe la configuración
    public JwtHelper(IConfiguration config) => _config = config;

    // Genera un token JWT para el usuario autenticado
    public string GenerateToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim("Nombre", usuario.Nombre)
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"],
            claims: claims, expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
