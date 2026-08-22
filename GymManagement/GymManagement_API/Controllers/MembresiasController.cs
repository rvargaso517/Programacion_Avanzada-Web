using Dapper;
using GymManagement_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembresiasController : ControllerBase
    {
        private readonly IConfiguration _config;

        public MembresiasController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("ListarMembresias")]
        public IActionResult ListarMembresias()
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var datos = context.Query<MembresiaDto>(
                "dbo.sp_Membresia_Listar",
                commandType: CommandType.StoredProcedure);

            return Ok(datos);
        }

        [HttpGet("ObtenerMembresia/{id}")]
        public IActionResult ObtenerMembresia(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdMembresiaCliente", id);

            var membresia = context.QueryFirstOrDefault<MembresiaDto>(
                "dbo.sp_Membresia_ObtenerPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (membresia == null) return NotFound();

            return Ok(membresia);
        }

        [HttpPost("CrearMembresia")]
        public IActionResult CrearMembresia(CrearMembresiaRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCliente", request.IdCliente);
            parameters.Add("@IdPlan", request.IdPlan);
            parameters.Add("@FechaInicio", request.FechaInicio);
            parameters.Add("@FechaFin", request.FechaFin);
            parameters.Add("@Estado", request.Estado);

            var id = context.QueryFirstOrDefault<int>(
                "dbo.sp_Membresia_Crear",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { IdMembresiaCliente = id, Success = true });
        }

        [HttpPut("ActualizarMembresia")]
        public IActionResult ActualizarMembresia(ActualizarMembresiaRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdMembresiaCliente", request.IdMembresiaCliente);
            parameters.Add("@IdCliente", request.IdCliente);
            parameters.Add("@IdPlan", request.IdPlan);
            parameters.Add("@FechaInicio", request.FechaInicio);
            parameters.Add("@FechaFin", request.FechaFin);
            parameters.Add("@Estado", request.Estado);

            var rows = context.Execute(
                "dbo.sp_Membresia_Actualizar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }

        [HttpDelete("EliminarMembresia/{id}")]
        public IActionResult EliminarMembresia(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdMembresiaCliente", id);

            var rows = context.Execute(
                "dbo.sp_Membresia_Eliminar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }
    }
}
