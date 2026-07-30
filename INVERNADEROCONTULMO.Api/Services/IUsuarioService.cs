using INVERNADEROCONTULMO.Api.DTOs;
using INVERNADEROCONTULMO.Api.Models;

namespace INVERNADEROCONTULMO.Api.Services;

// Servicio de usuarios con autenticación y CRUD
public interface IUsuarioService
{
    // Autentica un usuario y devuelve token JWT
    Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
    // Crea un nuevo usuario con contraseña encriptada
    Task<Usuario> CreateAsync(Usuario usuario);
    // Obtiene todos los usuarios activos
    Task<IEnumerable<Usuario>> GetAllAsync();
    // Obtiene un usuario por su identificador
    Task<Usuario> GetByIdAsync(int id);
    // Actualiza los datos de un usuario
    Task<Usuario> UpdateAsync(int id, Usuario usuario);
    // Desactiva un usuario (borrado lógico)
    Task<bool> DeleteAsync(int id);
}
