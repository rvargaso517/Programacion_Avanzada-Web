using System;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_API.Models
{
    public class MembresiaDto
    {
        public int IdMembresiaCliente { get; set; }
        public int IdCliente { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int IdPlan { get; set; }
        public string PlanNombre { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Estado { get; set; }
    }

    public class CrearMembresiaRequest
    {
        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El plan es obligatorio.")]
        public int IdPlan { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; } = true;
    }

    public class ActualizarMembresiaRequest
    {
        [Required(ErrorMessage = "El identificador de la membresía es obligatorio.")]
        public int IdMembresiaCliente { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El plan es obligatorio.")]
        public int IdPlan { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; }
    }
}
