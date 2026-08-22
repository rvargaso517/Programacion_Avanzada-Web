using System;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_API.Models
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

    public class CrearPagoRequest
    {
        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int IdCliente { get; set; }

        public int? IdMembresiaCliente { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [StringLength(50, ErrorMessage = "El método de pago no puede superar los 50 caracteres.")]
        public string MetodoPago { get; set; } = string.Empty;

        [StringLength(30)]
        public string Estado { get; set; } = "Pagado";
    }
}
