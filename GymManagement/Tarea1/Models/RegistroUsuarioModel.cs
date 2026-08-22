using System.ComponentModel.DataAnnotations;

namespace GymManagement_WEB.Models
{
    public class RegistroUsuarioModel
    {
        [Required(ErrorMessage = "Ingrese el nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la cédula")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el correo")]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la contraseña")]
        [DataType(DataType.Password)]
        public string Contrasenna { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme la contraseña")]
        [DataType(DataType.Password)]
        [Compare("Contrasenna", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
