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
    public class RutinasController : Controller
    {
        private readonly IRutinaRepository _rutinaRepository;
        private readonly IDbConnectionFactory _factory;

        public RutinasController(
            IRutinaRepository rutinaRepository,
            IDbConnectionFactory factory)
        {
            _rutinaRepository = rutinaRepository;
            _factory = factory;
        }

        // Listar rutinas
        [HttpGet]
        public async Task<IActionResult> Index(int? idCliente)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            var userCorreo = HttpContext.Session.GetString("Correo");

            int targetClienteId = 0;

            if (userRol == 4) // Cliente
            {
                using var db = _factory.CreateConnection();
                var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                    "dbo.sp_Cliente_ObtenerPorCorreo", new { Correo = userCorreo }, commandType: CommandType.StoredProcedure);
                if (cliente != null)
                {
                    targetClienteId = cliente.IdCliente;
                }
                else
                {
                    ViewBag.Mensaje = "No se encontró el perfil de cliente asociado a tu cuenta.";
                    ViewData["Layout"] = "_Layout";
                    return View(new List<Rutina>());
                }
            }
            else if (idCliente.HasValue)
            {
                targetClienteId = idCliente.Value;
            }
            else
            {
                // Para administradores o entrenadores, mostrar lista de todos los clientes para que seleccionen uno
                using var db = _factory.CreateConnection();
                var clientes = await db.QueryAsync<Cliente>(
                    "dbo.sp_Cliente_Listar", commandType: CommandType.StoredProcedure);
                ViewData["Layout"] = (userRol == 1 || userRol == 2) ? "_LayoutAdmin" : "_Layout";
                return View("SeleccionarCliente", clientes);
            }

            var rutinas = await _rutinaRepository.ListarPorClienteAsync(targetClienteId);
            ViewBag.IdCliente = targetClienteId;

            using (var db = _factory.CreateConnection())
            {
                var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                    "dbo.sp_Cliente_ObtenerPorId",
                    new { IdCliente = targetClienteId },
                    commandType: CommandType.StoredProcedure);
                ViewBag.ClienteNombre = cliente != null ? $"{cliente.Nombre} {cliente.Apellido}" : "Cliente";
            }

            ViewData["Layout"] = (userRol == 1 || userRol == 2) ? "_LayoutAdmin" : "_Layout";
            return View(rutinas);
        }

        // Crear Rutina (GET)
        [HttpGet]
        public async Task<IActionResult> Create(int idCliente)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            if (userRol != 1 && userRol != 3) // Solo Admin o Entrenador
            {
                return RedirectToAction("Index");
            }

            using var db = _factory.CreateConnection();
            var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                "dbo.sp_Cliente_ObtenerPorId", new { IdCliente = idCliente }, commandType: CommandType.StoredProcedure);
            if (cliente == null) return NotFound();

            ViewBag.Cliente = cliente;
            ViewData["Layout"] = (userRol == 1 || userRol == 2) ? "_LayoutAdmin" : "_Layout";
            return View();
        }

        // Crear Rutina (POST)
        [HttpPost]
        public async Task<IActionResult> Create(int idCliente, string nombreRutina, string descripcion, List<DetalleRutina> ejercicios)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            var trainerId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            if (userRol != 1 && userRol != 3) return Forbid();

            var rutina = new Rutina
            {
                IdCliente = idCliente,
                IdEntrenador = trainerId,
                NombreRutina = nombreRutina,
                Descripcion = descripcion
            };

            var idRutina = await _rutinaRepository.CrearRutinaAsync(rutina);

            if (ejercicios != null && ejercicios.Count > 0)
            {
                foreach (var ej in ejercicios)
                {
                    if (!string.IsNullOrEmpty(ej.Ejercicio))
                    {
                        ej.IdRutina = idRutina;
                        ej.DiaSemana = string.IsNullOrEmpty(ej.DiaSemana) ? "Lunes" : ej.DiaSemana;
                        ej.Repeticiones = string.IsNullOrEmpty(ej.Repeticiones) ? "10" : ej.Repeticiones;
                        await _rutinaRepository.CrearDetalleRutinaAsync(ej);
                    }
                }
            }

            return RedirectToAction("Index", new { idCliente });
        }

        // Ver Detalles
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            var rutina = await _rutinaRepository.ObtenerPorIdAsync(id);
            if (rutina == null) return NotFound();

            var detalles = await _rutinaRepository.ListarDetallesPorRutinaAsync(id);
            ViewBag.Detalles = detalles;

            ViewData["Layout"] = (userRol == 1 || userRol == 2) ? "_LayoutAdmin" : "_Layout";
            return View(rutina);
        }

        // Eliminar
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int idCliente)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            if (userRol != 1 && userRol != 3) return Forbid();

            await _rutinaRepository.EliminarRutinaAsync(id);
            return RedirectToAction("Index", new { idCliente });
        }

        // Editar Rutina (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            if (userRol != 1 && userRol != 3) return RedirectToAction("Index");

            var rutina = await _rutinaRepository.ObtenerPorIdAsync(id);
            if (rutina == null) return NotFound();

            using var db = _factory.CreateConnection();
            var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                "dbo.sp_Cliente_ObtenerPorId", new { IdCliente = rutina.IdCliente }, commandType: CommandType.StoredProcedure);
            
            ViewBag.Cliente = cliente;
            ViewBag.Detalles = await _rutinaRepository.ListarDetallesPorRutinaAsync(id);
            ViewData["Layout"] = (userRol == 1 || userRol == 2) ? "_LayoutAdmin" : "_Layout";
            return View(rutina);
        }

        // Editar Rutina (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int idRutina, int idCliente, string nombreRutina, string descripcion, List<DetalleRutina> ejercicios)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado)) return RedirectToAction("Login", "Home");

            var userRol = HttpContext.Session.GetInt32("Rol");
            if (userRol != 1 && userRol != 3) return Forbid();

            var rutina = new Rutina
            {
                IdRutina = idRutina,
                NombreRutina = nombreRutina,
                Descripcion = descripcion
            };

            await _rutinaRepository.ActualizarRutinaAsync(rutina);
            await _rutinaRepository.EliminarDetallesPorRutinaAsync(idRutina);

            if (ejercicios != null && ejercicios.Count > 0)
            {
                foreach (var ej in ejercicios)
                {
                    if (!string.IsNullOrEmpty(ej.Ejercicio))
                    {
                        ej.IdRutina = idRutina;
                        ej.DiaSemana = string.IsNullOrEmpty(ej.DiaSemana) ? "Lunes" : ej.DiaSemana;
                        ej.Repeticiones = string.IsNullOrEmpty(ej.Repeticiones) ? "10" : ej.Repeticiones;
                        await _rutinaRepository.CrearDetalleRutinaAsync(ej);
                    }
                }
            }

            return RedirectToAction("Index", new { idCliente });
        }
    }
}
