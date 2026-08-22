using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tarea1.Data;
using Tarea1.Models.Entities;
using Tarea1.Repositories;
using Dapper;

namespace Tarea1.Controllers
{
    public class ReservasController : Controller
    {
        /// <summary>IdRol de los entrenadores en dbo.Roles.</summary>
        private const int RolEntrenador = 3;

        private readonly IReservaRepository _reservaRepository;
        private readonly IDbConnectionFactory _factory;

        public ReservasController(
            IReservaRepository reservaRepository,
            IDbConnectionFactory factory)
        {
            _reservaRepository = reservaRepository;
            _factory = factory;
        }

        // Listar reservas (depende del rol)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            var userCorreo = HttpContext.Session.GetString("Correo");

            IEnumerable<ReservaEntrenador> reservas;

            if (userRol == 4) // Cliente
            {
                using var db = _factory.CreateConnection();
                var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                    "dbo.sp_Cliente_ObtenerPorCorreo", new { Correo = userCorreo }, commandType: CommandType.StoredProcedure);

                if (cliente != null)
                {
                    // Para clientes, listamos todas sus reservas pasadas y pendientes
                    reservas = await db.QueryAsync<ReservaEntrenador>(
                        "dbo.sp_Reserva_ListarPorCliente",
                        new { IdCliente = cliente.IdCliente },
                        commandType: CommandType.StoredProcedure);
                }
                else
                {
                    reservas = new List<ReservaEntrenador>();
                }
            }
            else // Admin / Recepcionista / Entrenador
            {
                reservas = await _reservaRepository.ListarTodasAsync();
            }

            ViewData["Layout"] = (userRol == 1 || userRol == 2 || userRol == 3) ? "_LayoutAdmin" : "_Layout";
            return View(reservas);
        }

        // Crear Reserva (GET)
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            if (userRol != 4) return Forbid(); // Solo clientes pueden reservar

            using var db = _factory.CreateConnection();
            var entrenadores = await db.QueryAsync<Usuario>(
                "dbo.sp_Usuario_ListarPorRol", new { IdRol = RolEntrenador }, commandType: CommandType.StoredProcedure);

            ViewBag.Entrenadores = entrenadores;
            ViewData["Layout"] = "_Layout";
            return View();
        }

        // Crear Reserva (POST)
        [HttpPost]
        public async Task<IActionResult> Create(int idEntrenador, DateTime fechaHora)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            if (userRol != 4) return Forbid();

            var userCorreo = HttpContext.Session.GetString("Correo");
            using var db = _factory.CreateConnection();
            var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                "dbo.sp_Cliente_ObtenerPorCorreo", new { Correo = userCorreo }, commandType: CommandType.StoredProcedure);

            if (cliente == null)
            {
                ModelState.AddModelError("", "No se pudo encontrar tu perfil de cliente.");
                var entrenadores = await db.QueryAsync<Usuario>(
                    "dbo.sp_Usuario_ListarPorRol", new { IdRol = RolEntrenador }, commandType: CommandType.StoredProcedure);
                ViewBag.Entrenadores = entrenadores;
                ViewData["Layout"] = "_Layout";
                return View();
            }

            var reserva = new ReservaEntrenador
            {
                IdCliente = cliente.IdCliente,
                IdEntrenador = idEntrenador,
                FechaHora = fechaHora,
                Costo = 10000.00m // Costo fijo por sesión de personal trainer / clase especial
            };

            await _reservaRepository.CrearReservaAsync(reserva);
            TempData["Exito"] = "Reserva registrada con éxito. Recuerda realizar el pago en recepción.";
            return RedirectToAction(nameof(Index));
        }
    }
}
