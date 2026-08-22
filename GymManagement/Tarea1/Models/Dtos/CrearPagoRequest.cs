using System.ComponentModel.DataAnnotations;

namespace Tarea1.Models.Dtos
{
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
