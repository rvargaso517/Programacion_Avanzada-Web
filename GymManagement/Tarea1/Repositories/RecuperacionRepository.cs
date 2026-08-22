using Dapper;
using System.Data;
using Tarea1.Data;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public class RecuperacionRepository : IRecuperacionRepository
    {
        private readonly IDbConnectionFactory _factory;

        public RecuperacionRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> CrearAsync(int idUsuario, string token, DateTime fechaExpira)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Recuperacion_Crear",
                new { IdUsuario = idUsuario, Token = token, FechaExpira = fechaExpira },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<RecuperacionPassword?> ObtenerPorTokenAsync(string token)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<RecuperacionPassword>(
                "dbo.sp_Recuperacion_ObtenerPorToken",
                new { Token = token },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> MarcarUtilizadoAsync(int idRecuperacion)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Recuperacion_MarcarUtilizado",
                new { IdRecuperacion = idRecuperacion },
                commandType: CommandType.StoredProcedure);
        }
    }
}
