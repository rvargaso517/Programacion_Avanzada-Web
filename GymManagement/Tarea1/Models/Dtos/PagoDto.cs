using System;

namespace Tarea1.Models.Dtos
{
    public class PagoDto
    {
        public int IdPago { get; set; }
        public int IdCliente { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int? IdMembresiaCliente { get; set; }
        public string? PlanNombre { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
