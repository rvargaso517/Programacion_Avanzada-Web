using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Tarea1.Data;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public class RutinaRepository : IRutinaRepository
    {
        private readonly IDbConnectionFactory _factory;

        public RutinaRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> CrearRutinaAsync(Rutina rutina)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Rutina_Crear",
                new
                {
                    IdCliente = rutina.IdCliente,
                    IdEntrenador = rutina.IdEntrenador,
                    NombreRutina = rutina.NombreRutina,
                    Descripcion = rutina.Descripcion
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task CrearDetalleRutinaAsync(DetalleRutina detalle)
        {
            using var db = _factory.CreateConnection();
            await db.ExecuteAsync(
                "dbo.sp_DetalleRutina_Crear",
                new
                {
                    IdRutina = detalle.IdRutina,
                    DiaSemana = detalle.DiaSemana,
                    Ejercicio = detalle.Ejercicio,
                    Series = detalle.Series,
                    Repeticiones = detalle.Repeticiones,
                    Descanso = detalle.Descanso,
                    VideoUrl = detalle.VideoUrl
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Rutina>> ListarPorClienteAsync(int idCliente)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryAsync<Rutina>(
                "dbo.sp_Rutina_ListarPorCliente",
                new { IdCliente = idCliente },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Rutina?> ObtenerPorIdAsync(int idRutina)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Rutina>(
                "dbo.sp_Rutina_ObtenerPorId",
                new { IdRutina = idRutina },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<DetalleRutina>> ListarDetallesPorRutinaAsync(int idRutina)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryAsync<DetalleRutina>(
                "dbo.sp_DetalleRutina_ListarPorRutina",
                new { IdRutina = idRutina },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> EliminarRutinaAsync(int idRutina)
        {
            using var db = _factory.CreateConnection();
            var result = await db.ExecuteScalarAsync<int>(
                "dbo.sp_Rutina_Eliminar",
                new { IdRutina = idRutina },
                commandType: CommandType.StoredProcedure
            );
            return result > 0;
        }

        public async Task<bool> ActualizarRutinaAsync(Rutina rutina)
        {
            using var db = _factory.CreateConnection();
            var affected = await db.ExecuteAsync(
                "dbo.sp_Rutina_Actualizar",
                new
                {
                    IdRutina = rutina.IdRutina,
                    NombreRutina = rutina.NombreRutina,
                    Descripcion = rutina.Descripcion
                },
                commandType: CommandType.StoredProcedure
            );
            return affected > 0;
        }

        public async Task<bool> EliminarDetallesPorRutinaAsync(int idRutina)
        {
            using var db = _factory.CreateConnection();
            var affected = await db.ExecuteAsync(
                "dbo.sp_DetalleRutina_EliminarPorRutina",
                new { IdRutina = idRutina },
                commandType: CommandType.StoredProcedure
            );
            return affected > 0;
        }
    }
}
