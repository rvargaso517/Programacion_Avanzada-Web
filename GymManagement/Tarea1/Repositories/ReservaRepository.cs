using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Tarea1.Data;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly IDbConnectionFactory _factory;

        public ReservaRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> CrearReservaAsync(ReservaEntrenador reserva)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Reserva_Crear",
                new
                {
                    IdCliente = reserva.IdCliente,
                    IdEntrenador = reserva.IdEntrenador,
                    FechaHora = reserva.FechaHora,
                    Costo = reserva.Costo
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ReservaEntrenador>> ListarPendientesPorClienteAsync(int idCliente)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryAsync<ReservaEntrenador>(
                "dbo.sp_Reserva_ListarPendientesPorCliente",
                new { IdCliente = idCliente },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ReservaEntrenador>> ListarTodasAsync()
        {
            using var db = _factory.CreateConnection();
            return await db.QueryAsync<ReservaEntrenador>(
                "dbo.sp_Reserva_ListarTodas",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> MarcarComoPagadaAsync(int idReserva)
        {
            using var db = _factory.CreateConnection();
            var result = await db.ExecuteScalarAsync<int>(
                "dbo.sp_Reserva_MarcarComoPagada",
                new { IdReserva = idReserva },
                commandType: CommandType.StoredProcedure
            );
            return result > 0;
        }
    }
}
