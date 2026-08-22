namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Pago realizado por un cliente.
    /// Corresponde a la tabla dbo.Pagos.
    /// </summary>
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdCliente { get; set; }
        public int? IdMembresiaCliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
