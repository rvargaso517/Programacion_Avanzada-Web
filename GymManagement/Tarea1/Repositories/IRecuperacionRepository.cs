using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public interface IRecuperacionRepository
    {
        Task<int> CrearAsync(int idUsuario, string token, DateTime fechaExpira);
        Task<RecuperacionPassword?> ObtenerPorTokenAsync(string token);
        Task<int> MarcarUtilizadoAsync(int idRecuperacion);
    }
}
