using Dapper;
using System.Data;
using Tarea1.Data;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly IDbConnectionFactory _factory;

        public RolRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Rol>> ListarAsync()
        {
            using var db = _factory.CreateConnection();
            return await db.QueryAsync<Rol>(
                "dbo.sp_Rol_Listar", commandType: CommandType.StoredProcedure);
        }
    }
}
