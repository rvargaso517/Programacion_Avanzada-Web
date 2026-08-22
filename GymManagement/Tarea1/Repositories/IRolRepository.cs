using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> ListarAsync();
    }
}
