namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Registro de errores del sistema (capturados por Middleware).
    /// Corresponde a la tabla dbo.LogErrores.
    /// </summary>
    public class LogError
    {
        public int IdError { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? Ruta { get; set; }
        public string? UsuarioAfectado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
