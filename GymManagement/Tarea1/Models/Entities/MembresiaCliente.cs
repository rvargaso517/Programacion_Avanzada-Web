namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Membresía asignada a un cliente (relación cliente <-> plan).
    /// Corresponde a la tabla dbo.MembresiaCliente.
    /// </summary>
    public class MembresiaCliente
    {
        public int IdMembresiaCliente { get; set; }
        public int IdCliente { get; set; }
        public int IdPlan { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Estado { get; set; }
    }
}
