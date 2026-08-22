namespace GymManagement_WEB.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
