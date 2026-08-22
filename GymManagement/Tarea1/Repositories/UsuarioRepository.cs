using Dapper;
using System.Data;
using Tarea1.Data;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionFactory _factory;

        public UsuarioRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Usuario>(
                "dbo.sp_Usuario_ObtenerPorCorreo",
                new { Correo = correo },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int idUsuario)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Usuario>(
                "dbo.sp_Usuario_ObtenerPorId",
                new { IdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Usuario>> ListarAsync()
        {
            using var db = _factory.CreateConnection();
            return await db.QueryAsync<Usuario>(
                "dbo.sp_Usuario_Listar",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ExisteCorreoAsync(string correo, int? idExcluir = null)
        {
            using var db = _factory.CreateConnection();
            var existe = await db.ExecuteScalarAsync<int>(
                "dbo.sp_Usuario_ExisteCorreo",
                new { Correo = correo, IdExcluir = idExcluir },
                commandType: CommandType.StoredProcedure);
            return existe == 1;
        }

        public async Task<int> CrearAsync(int idRol, string nombre, string correo, string passwordHash, bool estado)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Usuario_Crear",
                new { IdRol = idRol, Nombre = nombre, Correo = correo, PasswordHash = passwordHash, Estado = estado },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ActualizarAsync(int idUsuario, int idRol, string nombre, string correo, bool estado)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Usuario_Actualizar",
                new { IdUsuario = idUsuario, IdRol = idRol, Nombre = nombre, Correo = correo, Estado = estado },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ActualizarPasswordAsync(int idUsuario, string passwordHash)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Usuario_ActualizarPassword",
                new { IdUsuario = idUsuario, PasswordHash = passwordHash },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> EliminarAsync(int idUsuario)
        {
            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_Usuario_Eliminar",
                new { IdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure);
        }
    }
}
