namespace GymManagement_WEB.Models
{
    public class RegistrarCitaRequestModel
    {
        public int IdCliente { get; set; }

        public int IdUsuario { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }
    }
}
