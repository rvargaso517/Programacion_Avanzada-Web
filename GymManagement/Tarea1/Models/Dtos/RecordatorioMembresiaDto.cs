namespace Tarea1.Models.Dtos
{
    /// <summary>
    /// Datos que devuelve dbo.sp_Membresia_DatosRecordatorio para armar el correo
    /// de aviso de vencimiento de una membresía.
    /// </summary>
    public class RecordatorioMembresiaDto
    {
        public int IdMembresiaCliente { get; set; }
        public DateTime FechaFin { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Correo { get; set; }
        public string PlanNombre { get; set; } = string.Empty;

        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

        /// <summary>Días que faltan para que venza la membresía (negativo si ya venció).</summary>
        public int DiasRestantes => (FechaFin.Date - DateTime.Today).Days;
    }
}
