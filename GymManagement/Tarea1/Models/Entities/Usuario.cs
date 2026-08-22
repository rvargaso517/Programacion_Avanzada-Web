namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Usuario del sistema con un rol asignado.
    /// Corresponde a la tabla dbo.Usuarios.
    /// </summary>
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        /// <summary>Nombre del rol (poblado por los SP que hacen JOIN con Roles).</summary>
        public string? RolNombre { get; set; }
    }
}
