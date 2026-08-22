using Tarea1.Models;
using Tarea1.Models.Dtos;
using Tarea1.Models.Entities;

namespace Tarea1.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> ListarAsync();
        Task<UsuarioDto?> ObtenerAsync(int idUsuario);
        Task<IEnumerable<Rol>> ListarRolesAsync();
        Task<ServiceResult<UsuarioDto>> CrearAsync(CrearUsuarioRequest request);
        Task<ServiceResult<bool>> ActualizarAsync(ActualizarUsuarioRequest request);
        Task<ServiceResult<bool>> EliminarAsync(int idUsuario);
    }
}
