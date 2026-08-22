namespace Tarea1.Models.Dtos
{
    /// <summary>Representación de un usuario para respuestas y listados (sin contraseña).</summary>
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
