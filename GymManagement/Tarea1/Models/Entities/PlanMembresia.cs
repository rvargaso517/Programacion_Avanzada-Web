namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Plan de membresía disponible en el gimnasio.
    /// Corresponde a la tabla dbo.PlanesMembresia.
    /// </summary>
    public class PlanMembresia
    {
        public int IdPlan { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int DuracionDias { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
    }
}
