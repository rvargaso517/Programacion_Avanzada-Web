using System;

namespace Tarea1.Models.Entities
{
    public class ReservaEntrenador
    {
        public int IdReserva { get; set; }
        public int IdCliente { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int IdEntrenador { get; set; }
        public string EntrenadorNombre { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public decimal Costo { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
