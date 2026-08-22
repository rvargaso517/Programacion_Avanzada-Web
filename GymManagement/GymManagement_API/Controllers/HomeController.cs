using BCrypt.Net;
using Dapper;
using GymManagement_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IConfiguration _config) : ControllerBase
    {
        [HttpPost("RegistrarClienteAPI")]
        public IActionResult RegistrarClienteAPI(RegistroUsuarioRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            if (model.Contrasenna != model.ConfirmarContrasenna)
                return BadRequest("Las contraseñas no coinciden.");

            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parametros = new DynamicParameters();

            parametros.Add("@Nombre", model.Nombre);
            parametros.Add("@Apellido", model.Apellido);
            parametros.Add("@Cedula", model.Cedula);
            parametros.Add("@Telefono", model.Telefono);
            parametros.Add("@Correo", model.Correo);
            parametros.Add("@Direccion", model.Direccion);

            // Encriptar contraseña
            parametros.Add("@PasswordHash",
                BCrypt.Net.BCrypt.HashPassword(model.Contrasenna));

            var resultado = context.QueryFirst<dynamic>(
                "dbo.SP_RegistrarUsuario",
                parametros,
                commandType: CommandType.StoredProcedure);

            if (resultado.Resultado == 1)
                return Ok(resultado.Mensaje);

            return BadRequest(resultado.Mensaje);
        }

        [HttpPost("IniciarSesionAPI")]
        public IActionResult IniciarSesionAPI(InicioSesionRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@Correo", model.Correo);

            var usuario = context.QueryFirstOrDefault<DatosUsuarioResponseModel>(
                "dbo.SP_InicioSesionUsuario",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (usuario == null)
                return Unauthorized("Correo o contraseña incorrectos.");

            if (!BCrypt.Net.BCrypt.Verify(model.Contrasenna, usuario.PasswordHash))
                return Unauthorized("Correo o contraseña incorrectos.");

            return Ok(usuario);
         
        }

    }
}
