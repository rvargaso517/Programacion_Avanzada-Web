using System;

namespace Tarea1.Models.Dtos
{
    public class OportunidadDto
    {
        public int IdOportunidad { get; set; }
        public int IdCliente { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal MontoEstimado { get; set; }
        public string Etapa { get; set; } = "Nuevo";
        public DateTime? FechaCierre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }

        public string? ClienteNombreCompleto { get; set; }
    }
}
