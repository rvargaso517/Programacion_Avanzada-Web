using System.ComponentModel.DataAnnotations;

namespace GymManagement_WEB.Models
{
    public class RegistroUsuarioModel
    {
        [Required(ErrorMessage = "Ingrese el nombre.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el apellido.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la cédula.")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el teléfono.")]
        [RegularExpression(@"^\+?[0-9\s-]{8,15}$", ErrorMessage = "El número de teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el correo.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail|hotmail)\.[a-zA-Z0-9.-]+$", ErrorMessage = "El correo debe ser de dominio @gmail o @hotmail.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la dirección.")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la contraseña.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, un número y un carácter especial.")]
        public string Contrasenna { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme la contraseña.")]
        [DataType(DataType.Password)]
        [Compare("Contrasenna", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
