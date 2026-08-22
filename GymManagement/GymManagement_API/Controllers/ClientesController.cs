using Dapper;
using GymManagement_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ClientesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("ListarClientes")]
        public IActionResult ListarClientes(string? buscar = null, bool? estado = null)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@Buscar", buscar);
            parameters.Add("@Estado", estado);

            var datos = context.Query<ClienteDto>(
                "dbo.sp_Cliente_Listar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(datos);
        }

        [HttpGet("ObtenerCliente/{id}")]
        public IActionResult ObtenerCliente(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCliente", id);

            var cliente = context.QueryFirstOrDefault<ClienteDto>(
                "dbo.sp_Cliente_ObtenerPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (cliente == null) return NotFound();

            return Ok(cliente);
        }

        [HttpGet("ObtenerClientePorCedula/{cedula}")]
        public IActionResult ObtenerClientePorCedula(string cedula)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@Cedula", cedula);

            var cliente = context.QueryFirstOrDefault<ClienteDto>(
                "dbo.sp_Cliente_ObtenerPorCedula",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(cliente);
        }

        [HttpPost("CrearCliente")]
        public IActionResult CrearCliente(CrearClienteRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", request.Nombre);
            parameters.Add("@Apellido", request.Apellido);
            parameters.Add("@Cedula", request.Cedula);
            parameters.Add("@Telefono", request.Telefono);
            parameters.Add("@Correo", request.Correo);
            parameters.Add("@Direccion", request.Direccion);
            parameters.Add("@Estado", request.Estado);

            var id = context.QueryFirstOrDefault<int>(
                "dbo.sp_Cliente_Crear",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { IdCliente = id, Success = true });
        }

        [HttpPut("ActualizarCliente")]
        public IActionResult ActualizarCliente(ActualizarClienteRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCliente", request.IdCliente);
            parameters.Add("@Nombre", request.Nombre);
            parameters.Add("@Apellido", request.Apellido);
            parameters.Add("@Cedula", request.Cedula);
            parameters.Add("@Telefono", request.Telefono);
            parameters.Add("@Correo", request.Correo);
            parameters.Add("@Direccion", request.Direccion);
            parameters.Add("@Estado", request.Estado);

            var rows = context.Execute(
                "dbo.sp_Cliente_Actualizar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }

        [HttpDelete("EliminarCliente/{id}")]
        public IActionResult EliminarCliente(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCliente", id);

            var rows = context.Execute(
                "dbo.sp_Cliente_Eliminar",
                parameters,
                commandType: CommandType.StoredProcedure);

            return Ok(new { Afectados = rows, Success = rows > 0 });
        }
    }
}
