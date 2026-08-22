using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);
        Task<Usuario?> ObtenerPorIdAsync(int idUsuario);
        Task<IEnumerable<Usuario>> ListarAsync();
        Task<bool> ExisteCorreoAsync(string correo, int? idExcluir = null);
        Task<int> CrearAsync(int idRol, string nombre, string correo, string passwordHash, bool estado);
        Task<int> ActualizarAsync(int idUsuario, int idRol, string nombre, string correo, bool estado);
        Task<int> ActualizarPasswordAsync(int idUsuario, string passwordHash);
        Task<int> EliminarAsync(int idUsuario);
    }
}
