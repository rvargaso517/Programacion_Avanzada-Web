using System.ComponentModel.DataAnnotations;

namespace Tarea1.Models.Dtos
{
    public class CrearPlanRequest
    {
        [Required(ErrorMessage = "El nombre del plan es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La duración en días es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser de al menos 1 día.")]
        public int DuracionDias { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal Precio { get; set; }

        public bool Estado { get; set; } = true;
    }
}
