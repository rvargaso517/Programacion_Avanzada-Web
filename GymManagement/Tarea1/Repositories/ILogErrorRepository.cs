using Tarea1.Models.Entities;

namespace Tarea1.Repositories
{
    /// <summary>Acceso a dbo.LogErrores (registro de errores no controlados).</summary>
    public interface ILogErrorRepository
    {
        /// <summary>Guarda un error y devuelve el Id generado.</summary>
        Task<int> RegistrarAsync(string mensaje, string? stackTrace, string? ruta, string? usuarioAfectado);

        /// <summary>Últimos errores registrados, del más reciente al más antiguo.</summary>
        Task<IEnumerable<LogError>> ListarAsync(int top = 200);
    }
}
