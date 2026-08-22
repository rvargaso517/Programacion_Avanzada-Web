using System;
using System.ComponentModel.DataAnnotations;

namespace Tarea1.Models.Dtos
{
    public class CrearMembresiaRequest
    {
        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El plan es obligatorio.")]
        public int IdPlan { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; } = DateTime.Today.AddMonths(1);

        public bool Estado { get; set; } = true;
    }
}
