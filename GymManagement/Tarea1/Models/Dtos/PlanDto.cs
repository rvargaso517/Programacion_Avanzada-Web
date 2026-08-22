namespace Tarea1.Models.Dtos
{
    public class PlanDto
    {
        public int IdPlan { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int DuracionDias { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
    }
}
