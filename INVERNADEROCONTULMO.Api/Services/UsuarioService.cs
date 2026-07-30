using Microsoft.EntityFrameworkCore;
using INVERNADEROCONTULMO.Api.Data;
using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Helpers;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Implementación del servicio de usuarios
public class UsuarioService : IUsuarioService
{
    // Contexto de base de datos
    private readonly AppDbContext _db;
    // Helper para generación de tokens JWT
    private readonly JwtHelper _jwt;
    // Constructor que inyecta dependencias
    public UsuarioService(AppDbContext db, JwtHelper jwt) { _db = db; _jwt = jwt; }

    // Autentica un usuario verificando email y contraseña, devuelve token JWT
    public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
    {
        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Activo);
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas");
        return new LoginResponseDTO
        {
            Id = usuario.Id, Nombre = usuario.Nombre, Email = usuario.Email,
            Rol = usuario.Rol, Token = _jwt.GenerateToken(usuario)
        };
    }

    // Crea un nuevo usuario encriptando la contraseña
    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();
        return usuario;
    }

    // Obtiene todos los usuarios activos
    public async Task<IEnumerable<Usuario>> GetAllAsync() => await _db.Usuarios.Where(u => u.Activo).ToListAsync();
    // Obtiene un usuario por su identificador
    public async Task<Usuario> GetByIdAsync(int id) => await _db.Usuarios.FindAsync(id);

    // Actualiza los datos de un usuario
    public async Task<Usuario> UpdateAsync(int id, Usuario usuario)
    {
        var existing = await _db.Usuarios.FindAsync(id);
        if (existing == null) return null;
        existing.Nombre = usuario.Nombre;
        existing.Email = usuario.Email;
        existing.Rol = usuario.Rol;
        if (!string.IsNullOrEmpty(usuario.PasswordHash))
            existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
        await _db.SaveChangesAsync();
        return existing;
    }

    // Desactiva un usuario (borrado lógico)
    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _db.Usuarios.FindAsync(id);
        if (usuario == null) return false;
        usuario.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }
}
