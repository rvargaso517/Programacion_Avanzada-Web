using System.Data;
using Microsoft.Data.SqlClient;

namespace Tarea1.Data
{
    /// <summary>
    /// Implementación de <see cref="IDbConnectionFactory"/> para SQL Server.
    /// Lee la cadena de conexión "GymManagementDB" de la configuración.
    /// </summary>
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GymManagementDB")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión 'GymManagementDB' en la configuración.");
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
