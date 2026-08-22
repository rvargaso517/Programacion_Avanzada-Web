using System;

namespace Tarea1.Models.Dtos
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
}
