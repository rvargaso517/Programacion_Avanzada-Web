using Dapper;
using System.Data;
using Tarea1.Data;
using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    public class LogErrorRepository : ILogErrorRepository
    {
        /// <summary>Largo de la columna dbo.LogErrores.Mensaje.</summary>
        private const int LargoMaximoMensaje = 1000;

        /// <summary>Largo de la columna dbo.LogErrores.Ruta.</summary>
        private const int LargoMaximoRuta = 300;

        /// <summary>Largo de la columna dbo.LogErrores.UsuarioAfectado.</summary>
        private const int LargoMaximoUsuario = 150;

        private readonly IDbConnectionFactory _factory;

        public LogErrorRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> RegistrarAsync(string mensaje, string? stackTrace, string? ruta, string? usuarioAfectado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Mensaje", Recortar(mensaje, LargoMaximoMensaje));
            parametros.Add("@StackTrace", stackTrace);
            parametros.Add("@Ruta", Recortar(ruta, LargoMaximoRuta));
            parametros.Add("@UsuarioAfectado", Recortar(usuarioAfectado, LargoMaximoUsuario));

            using var db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "dbo.sp_LogError_Crear", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<LogError>> ListarAsync(int top = 200)
        {
            using var db = _factory.CreateConnection();
            return await db.QueryAsync<LogError>(
                "dbo.sp_LogError_Listar", new { Top = top }, commandType: CommandType.StoredProcedure);
        }

        /// <summary>Evita que un texto largo haga fallar el INSERT por truncamiento.</summary>
        private static string? Recortar(string? texto, int largo) =>
            string.IsNullOrEmpty(texto) || texto.Length <= largo ? texto : texto[..largo];
    }
}
