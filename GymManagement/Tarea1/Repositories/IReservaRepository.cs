using System.Collections.Generic;
using System.Threading.Tasks;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public interface IReservaRepository
    {
        Task<int> CrearReservaAsync(ReservaEntrenador reserva);
        Task<IEnumerable<ReservaEntrenador>> ListarPendientesPorClienteAsync(int idCliente);
        Task<IEnumerable<ReservaEntrenador>> ListarTodasAsync();
        Task<bool> MarcarComoPagadaAsync(int idReserva);
    }
}
