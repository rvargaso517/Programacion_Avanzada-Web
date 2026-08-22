using Dapper;
using GymManagement_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OportunidadesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OportunidadesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("ListarOportunidades")]
        public IActionResult ListarOportunidades(string? buscar = null, string? etapa = null, int? idCliente = null, bool? estado = null)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@Buscar", buscar);
            parameters.Add("@Etapa", etapa);
            parameters.Add("@IdCliente", idCliente);
            parameters.Add("@Estado", estado);

            var datos = context.Query<OportunidadDto>(
                "dbo.sp_Oportunidad_Listar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(datos);
        }

        [HttpGet("ObtenerOportunidad/{id}")]
        public IActionResult ObtenerOportunidad(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdOportunidad", id);

            var op = context.QueryFirstOrDefault<OportunidadDto>(
                "dbo.sp_Oportunidad_ObtenerPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (op == null) return NotFound();

            return Ok(op);
        }

        [HttpPost("CrearOportunidad")]
        public IActionResult CrearOportunidad(CrearOportunidadRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCliente", request.IdCliente);
            parameters.Add("@Titulo", request.Titulo);
            parameters.Add("@Descripcion", request.Descripcion);
            parameters.Add("@MontoEstimado", request.MontoEstimado);
            parameters.Add("@Etapa", request.Etapa);
            parameters.Add("@FechaCierre", request.FechaCierre);
            parameters.Add("@Estado", request.Estado);

            var id = context.QueryFirstOrDefault<int>(
                "dbo.sp_Oportunidad_Crear",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { IdOportunidad = id, Success = true });
        }

        [HttpPut("ActualizarOportunidad")]
        public IActionResult ActualizarOportunidad(ActualizarOportunidadRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdOportunidad", request.IdOportunidad);
            parameters.Add("@IdCliente", request.IdCliente);
            parameters.Add("@Titulo", request.Titulo);
            parameters.Add("@Descripcion", request.Descripcion);
            parameters.Add("@MontoEstimado", request.MontoEstimado);
            parameters.Add("@Etapa", request.Etapa);
            parameters.Add("@FechaCierre", request.FechaCierre);
            parameters.Add("@Estado", request.Estado);

            var rows = context.Execute(
                "dbo.sp_Oportunidad_Actualizar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }

        [HttpDelete("EliminarOportunidad/{id}")]
        public IActionResult EliminarOportunidad(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdOportunidad", id);

            var rows = context.Execute(
                "dbo.sp_Oportunidad_Eliminar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }
    }
}
