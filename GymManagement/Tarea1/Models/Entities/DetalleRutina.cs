namespace Tarea1.Models.Entities
{
    public class DetalleRutina
    {
        public int IdDetalle { get; set; }
        public int IdRutina { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public string Ejercicio { get; set; } = string.Empty;
        public int Series { get; set; }
        public string Repeticiones { get; set; } = string.Empty;
        public string? Descanso { get; set; }
        public string? VideoUrl { get; set; }
    }
}
