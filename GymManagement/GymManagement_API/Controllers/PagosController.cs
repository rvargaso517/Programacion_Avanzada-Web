using Dapper;
using GymManagement_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GymManagement_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Services.EmailService _emailService;

        public PagosController(IConfiguration config, Services.EmailService emailService)
        {
            _config = config;
            _emailService = emailService;
        }

        [HttpGet("ListarPagos")]
        public IActionResult ListarPagos()
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var datos = context.Query<PagoDto>(
                "dbo.sp_Pago_Listar",
                commandType: CommandType.StoredProcedure);

            return Ok(datos);
        }

        [HttpGet("ObtenerPago/{id}")]
        public IActionResult ObtenerPago(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdPago", id);

            var pago = context.QueryFirstOrDefault<PagoDto>(
                "dbo.sp_Pago_ObtenerPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (pago == null) return NotFound();

            return Ok(pago);
        }

        [HttpPost("CrearPago")]
        public IActionResult CrearPago(CrearPagoRequest request)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
            context.Open();

            var parameters = new DynamicParameters();
            parameters.Add("@IdCliente", request.IdCliente);
            parameters.Add("@IdMembresiaCliente", request.IdMembresiaCliente);
            parameters.Add("@Monto", request.Monto);
            parameters.Add("@MetodoPago", request.MetodoPago);
            parameters.Add("@Estado", request.Estado);

            var id = context.QueryFirstOrDefault<int>(
                "dbo.sp_Pago_Crear",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (id > 0)
            {
                try
                {
                    // Buscar detalles del cliente
                    var pCliente = new DynamicParameters();
                    pCliente.Add("@IdCliente", request.IdCliente);
                    var cliente = context.QueryFirstOrDefault<ClienteDto>(
                        "dbo.sp_Cliente_ObtenerPorId",
                        pCliente,
                        commandType: CommandType.StoredProcedure);

                    if (cliente != null && !string.IsNullOrEmpty(cliente.Correo))
                    {
                        string? planNombre = null;
                        if (request.IdMembresiaCliente.HasValue)
                        {
                            var pMemb = new DynamicParameters();
                            pMemb.Add("@IdMembresiaCliente", request.IdMembresiaCliente.Value);
                            var memb = context.QueryFirstOrDefault<MembresiaDto>(
                                "dbo.sp_Membresia_ObtenerPorId",
                                pMemb,
                                commandType: CommandType.StoredProcedure);
                            planNombre = memb?.PlanNombre;
                        }

                        // Enviar recibo
                        _emailService.EnviarRecibo(cliente.Correo, $"{cliente.Nombre} {cliente.Apellido}", request.Monto, request.MetodoPago, planNombre);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar preparar el correo: {ex.Message}");
                }
            }

            return Ok(new { IdPago = id, Success = id > 0 });
        }
    }
}
