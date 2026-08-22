using System.ComponentModel.DataAnnotations;

namespace Tarea1.Models.Dtos
{
    /// <summary>Datos para actualizar un usuario desde el CRUD administrativo.</summary>
    public class ActualizarUsuarioRequest
    {
        [Required]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [StringLength(150)]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;
    }
}
