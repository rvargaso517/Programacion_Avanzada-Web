namespace Tarea1.Models.Entities
{
    /// <summary>
    /// Registro de asistencia (entrada) de un cliente al gimnasio.
    /// Corresponde a la tabla dbo.Asistencia.
    /// </summary>
    public class Asistencia
    {
        public int IdAsistencia { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
