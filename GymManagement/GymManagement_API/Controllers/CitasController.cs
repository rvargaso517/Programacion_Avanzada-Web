using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using GymManagement_API.Models;

namespace GymManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController(IConfiguration _config) : ControllerBase
    {
        [HttpGet("ConsultarCitas")]
        public IActionResult ConsultarCitas()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);
            context.Open();

            var response = context.Query<DatosCitaResponseModel>(
                "dbo.sp_Cita_Listar",
                commandType: CommandType.StoredProcedure);

            return Ok(response);
        }

        [HttpGet("ConsultarCita/{id}")]
        public IActionResult ConsultarCita(int id)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCita", id);

            var response = context.QueryFirstOrDefault<DatosCitaResponseModel>(
                "dbo.sp_Cita_ObtenerPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("RegistrarCita")]
        public IActionResult RegistrarCita(RegistrarCitaRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCliente", model.IdCliente);
            parameters.Add("@IdUsuario", model.IdUsuario);
            parameters.Add("@Titulo", model.Titulo);
            parameters.Add("@Descripcion", model.Descripcion);
            parameters.Add("@Fecha", model.Fecha);
            parameters.Add("@HoraInicio", model.HoraInicio);
            parameters.Add("@HoraFin", model.HoraFin);

            var idCita = context.QueryFirstOrDefault<int>(
                "dbo.sp_Cita_Crear",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (idCita > 0)
                return Ok();

            return BadRequest("No fue posible registrar la cita.");
        }

        [HttpPut("ActualizarCita")]
        public IActionResult ActualizarCita(ActualizarCitaRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCita", model.IdCita);
            parameters.Add("@IdCliente", model.IdCliente);
            parameters.Add("@Titulo", model.Titulo);
            parameters.Add("@Descripcion", model.Descripcion);
            parameters.Add("@Fecha", model.Fecha);
            parameters.Add("@HoraInicio", model.HoraInicio);
            parameters.Add("@HoraFin", model.HoraFin);
            parameters.Add("@Estado", model.Estado);

            var response = context.Execute(
                "dbo.sp_Cita_Actualizar",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response > 0)
                return Ok();

            return BadRequest("No fue posible actualizar la cita.");
        }

        [HttpDelete("EliminarCita/{id}")]
        public IActionResult EliminarCita(int id)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCita", id);

            var response = context.Execute(
                "dbo.sp_Cita_Eliminar",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response > 0)
                return Ok();

            return BadRequest("No fue posible eliminar la cita.");
        }
    }
}
