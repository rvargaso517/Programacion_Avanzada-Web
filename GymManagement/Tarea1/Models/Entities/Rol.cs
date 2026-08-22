namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Rol del sistema (Administrador, Recepcionista, Entrenador...).
    /// Corresponde a la tabla dbo.Roles.
    /// </summary>
    public class Rol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
