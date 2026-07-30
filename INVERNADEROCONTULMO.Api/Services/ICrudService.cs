namespace INVERNADEROCONTULMO.Api.Services;

// Interfaz genérica con operaciones CRUD básicas
public interface ICrudService<T>
{
    // Obtiene todos los registros activos
    Task<IEnumerable<T>> GetAllAsync();
    // Obtiene un registro por su identificador
    Task<T> GetByIdAsync(int id);
    // Crea un nuevo registro
    Task<T> CreateAsync(T entity);
    // Actualiza un registro existente
    Task<T> UpdateAsync(int id, T entity);
    // Elimina (desactiva) un registro por su identificador
    Task<bool> DeleteAsync(int id);
}
