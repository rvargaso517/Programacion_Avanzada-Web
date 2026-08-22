using System.Collections.Generic;
using System.Threading.Tasks;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public interface IRutinaRepository
    {
        Task<int> CrearRutinaAsync(Rutina rutina);
        Task CrearDetalleRutinaAsync(DetalleRutina detalle);
        Task<IEnumerable<Rutina>> ListarPorClienteAsync(int idCliente);
        Task<Rutina?> ObtenerPorIdAsync(int idRutina);
        Task<IEnumerable<DetalleRutina>> ListarDetallesPorRutinaAsync(int idRutina);
        Task<bool> EliminarRutinaAsync(int idRutina);
        Task<bool> ActualizarRutinaAsync(Rutina rutina);
        Task<bool> EliminarDetallesPorRutinaAsync(int idRutina);
    }
}
