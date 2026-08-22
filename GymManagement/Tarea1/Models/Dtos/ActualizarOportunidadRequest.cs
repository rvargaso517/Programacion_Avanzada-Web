using System;
using System.ComponentModel.DataAnnotations;

namespace Tarea1.Models.Dtos
{
    public class ActualizarOportunidadRequest
    {
        [Required(ErrorMessage = "El ID de la oportunidad es obligatorio.")]
        public int IdOportunidad { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        [Range(1, int.MaxValue, ErrorMessage = "El cliente seleccionado no es válido.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El título de la oportunidad es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede superar los 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El monto estimado es obligatorio.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "El monto estimado debe ser mayor o igual a 0.")]
        public decimal MontoEstimado { get; set; }

        [Required(ErrorMessage = "La etapa es obligatoria.")]
        [StringLength(50, ErrorMessage = "La etapa no puede superar los 50 caracteres.")]
        public string Etapa { get; set; } = "Nuevo";

        [DataType(DataType.Date)]
        public DateTime? FechaCierre { get; set; }

        public bool Estado { get; set; }
    }
}
