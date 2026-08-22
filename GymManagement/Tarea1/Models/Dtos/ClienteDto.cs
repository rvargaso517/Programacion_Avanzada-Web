using System;

namespace Tarea1.Models.Dtos
{
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    }
}
