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
        private readonly Services.EmailService _emailService;

        public ClientesController(IConfiguration config, Services.EmailService emailService)
        {
            _config = config;
            _emailService = emailService;
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

            // Validar si la cédula ya existe
            var cedulaExiste = context.QueryFirstOrDefault<int>(
                "SELECT COUNT(1) FROM dbo.Clientes WHERE Cedula = @Cedula",
                new { Cedula = request.Cedula });
            if (cedulaExiste > 0)
            {
                return BadRequest("La cédula ya está registrada.");
            }

            // Validar si el correo ya existe en Usuarios
            if (!string.IsNullOrEmpty(request.Correo))
            {
                var correoExiste = context.QueryFirstOrDefault<int>(
                    "SELECT COUNT(1) FROM dbo.Usuarios WHERE Correo = @Correo",
                    new { Correo = request.Correo });
                if (correoExiste > 0)
                {
                    return BadRequest("El correo ya está registrado.");
                }
            }

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

            if (id > 0 && !string.IsNullOrEmpty(request.Correo))
            {
                try
                {
                    // Generar contraseña temporal
                    var random = new Random();
                    var passwordTemporal = $"Gym{random.Next(1000, 9999)}*";
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordTemporal);

                    // Crear usuario en la tabla Usuarios usando sp_Usuario_Crear
                    var userParams = new DynamicParameters();
                    userParams.Add("@IdRol", 4); // Rol 4 = Cliente
                    userParams.Add("@Nombre", $"{request.Nombre} {request.Apellido}");
                    userParams.Add("@Correo", request.Correo);
                    userParams.Add("@PasswordHash", passwordHash);
                    userParams.Add("@Estado", true);

                    context.Execute(
                        "dbo.sp_Usuario_Crear",
                        userParams,
                        commandType: CommandType.StoredProcedure);

                    // Enviar correo con la contraseña temporal
                    _emailService.EnviarPasswordTemporal(request.Correo, $"{request.Nombre} {request.Apellido}", passwordTemporal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear el usuario o enviar el correo: {ex.Message}");
                }
            }

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
