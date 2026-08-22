namespace Tarea1.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>Id de la fila en dbo.LogErrores donde quedó registrado el error (0 si no se pudo guardar).</summary>
        public int IdError { get; set; }

        public bool ShowIdError => IdError > 0;
    }
}
