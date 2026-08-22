using System.ComponentModel.DataAnnotations;

namespace GymManagement_API.Models
{
    public class RegistroUsuarioRequestModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]    
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Cedula { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        public string Contrasenna { get; set; } = string.Empty;

        [Required]
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
