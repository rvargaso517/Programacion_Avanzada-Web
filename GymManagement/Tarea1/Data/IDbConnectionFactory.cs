using System.Data;

namespace Tarea1.Data
{
    /// <summary>
    /// Abstracción para crear conexiones a la base de datos.
    /// Permite inyectar la creación de conexiones y facilita el uso con Dapper.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Crea y devuelve una nueva conexión abierta a GymManagementDB.
        /// </summary>
        IDbConnection CreateConnection();
    }
}
