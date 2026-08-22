using Dapper;
using GymManagement_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PlanesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("ListarPlanes")]
        public IActionResult ListarPlanes()
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var datos = context.Query<PlanDto>(
                "dbo.sp_Plan_Listar",
                commandType: CommandType.StoredProcedure);

            return Ok(datos);
        }

        [HttpGet("ObtenerPlan/{id}")]
        public IActionResult ObtenerPlan(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdPlan", id);

            var plan = context.QueryFirstOrDefault<PlanDto>(
                "dbo.sp_Plan_ObtenerPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (plan == null) return NotFound();

            return Ok(plan);
        }

        [HttpPost("CrearPlan")]
        public IActionResult CrearPlan(CrearPlanRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", request.Nombre);
            parameters.Add("@Descripcion", request.Descripcion);
            parameters.Add("@DuracionDias", request.DuracionDias);
            parameters.Add("@Precio", request.Precio);
            parameters.Add("@Estado", request.Estado);

            var id = context.QueryFirstOrDefault<int>(
                "dbo.sp_Plan_Crear",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { IdPlan = id, Success = true });
        }

        [HttpPut("ActualizarPlan")]
        public IActionResult ActualizarPlan(ActualizarPlanRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdPlan", request.IdPlan);
            parameters.Add("@Nombre", request.Nombre);
            parameters.Add("@Descripcion", request.Descripcion);
            parameters.Add("@DuracionDias", request.DuracionDias);
            parameters.Add("@Precio", request.Precio);
            parameters.Add("@Estado", request.Estado);

            var rows = context.Execute(
                "dbo.sp_Plan_Actualizar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }

        [HttpDelete("EliminarPlan/{id}")]
        public IActionResult EliminarPlan(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdPlan", id);

            var rows = context.Execute(
                "dbo.sp_Plan_Eliminar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }
    }
}
