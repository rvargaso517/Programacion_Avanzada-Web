namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Token de recuperación de contraseña asociado a un usuario.
    /// Corresponde a la tabla dbo.RecuperacionPassword.
    /// </summary>
    public class RecuperacionPassword
    {
        public int IdRecuperacion { get; set; }
        public int IdUsuario { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime FechaExpira { get; set; }
        public bool Utilizado { get; set; }
    }
}
