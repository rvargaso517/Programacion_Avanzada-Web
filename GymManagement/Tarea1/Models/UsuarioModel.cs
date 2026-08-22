using System.ComponentModel.DataAnnotations;

namespace GymManagement_WEB.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato de correo electrónico no es válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasenna { get; set; } = string.Empty;
        
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
