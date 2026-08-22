using System.ComponentModel.DataAnnotations;

namespace Tarea1.Models.Dtos
{
    /// <summary>Solicitud de recuperación de contraseña.</summary>
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;
    }
}
