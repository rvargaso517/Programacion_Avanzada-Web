using System;

namespace Tarea1.Models.Entities
{
    public class Rutina
    {
        public int IdRutina { get; set; }
        public int IdCliente { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int IdEntrenador { get; set; }
        public string EntrenadorNombre { get; set; } = string.Empty;
        public string NombreRutina { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime FechaAsignacion { get; set; }
    }
}
