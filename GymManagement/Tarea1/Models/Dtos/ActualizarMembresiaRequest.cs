using System;
using System.ComponentModel.DataAnnotations;

namespace Tarea1.Models.Dtos
{
    public class ActualizarMembresiaRequest
    {
        [Required(ErrorMessage = "El identificador de la membresía es obligatorio.")]
        public int IdMembresiaCliente { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El plan es obligatorio.")]
        public int IdPlan { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; }
    }
}
