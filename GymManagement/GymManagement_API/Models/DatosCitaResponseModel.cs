namespace GymManagement_API.Models
{
    public class DatosCitaResponseModel
    {
        public int IdCita { get; set; }

        public int IdCliente { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public int IdUsuario { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}
