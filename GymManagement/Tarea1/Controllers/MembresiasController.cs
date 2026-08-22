using GymManagement_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net.Http.Json;
using Tarea1.Models.Dtos;
using Tarea1.Services;
using Tarea1.Data;
using Tarea1.Models.Entities;
using Dapper;

namespace Tarea1.Controllers
{
    public class MembresiasController(
        IHttpClientFactory _http,
        IConfiguration _config,
        EmailService _emailService,
        IDbConnectionFactory _factory) : Controller
    {
        private readonly string _apiUrl = "Membresias";

        // GET: Membresias
        public async Task<IActionResult> Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ListarMembresias";
            var response = await client.GetAsync(url);

            var membresias = new List<MembresiaDto>();
            if (response.IsSuccessStatusCode)
            {
                membresias = await response.Content.ReadFromJsonAsync<List<MembresiaDto>>() ?? new List<MembresiaDto>();
            }

            var rol = HttpContext.Session.GetInt32("Rol");
            var correo = HttpContext.Session.GetString("Correo");

            if (rol == 4 && !string.IsNullOrEmpty(correo))
            {
                using var db = _factory.CreateConnection();
                var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                    "dbo.sp_Cliente_ObtenerPorCorreo", new { Correo = correo }, commandType: CommandType.StoredProcedure);
                if (cliente != null)
                {
                    membresias = membresias.Where(m => m.IdCliente == cliente.IdCliente).ToList();
                }
                else
                {
                    membresias = new List<MembresiaDto>();
                }
            }

            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(membresias);
        }

        // GET: Membresias/Create
        public async Task<IActionResult> Create(int? idCliente = null, int? idPlan = null)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var rol = HttpContext.Session.GetInt32("Rol");
            var correo = HttpContext.Session.GetString("Correo");
            
            // If logged in user is a client (Rol == 4), enforce their client ID
            if (rol == 4 && !string.IsNullOrEmpty(correo))
            {
                using var db = _factory.CreateConnection();
                var cliente = await db.QueryFirstOrDefaultAsync<Cliente>(
                    "dbo.sp_Cliente_ObtenerPorCorreo", new { Correo = correo }, commandType: CommandType.StoredProcedure);
                if (cliente != null)
                {
                    idCliente = cliente.IdCliente;
                }
            }

            using var client = _http.CreateClient();
            await CargarClientesYPlanesDropdownAsync(client, idCliente);

            var model = new CrearMembresiaRequest
            {
                IdCliente = idCliente ?? 0,
                IdPlan = idPlan ?? 0,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddMonths(1)
            };

            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(model);
        }

        // POST: Membresias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearMembresiaRequest request)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();

            if (!ModelState.IsValid)
            {
                await CargarClientesYPlanesDropdownAsync(client, request.IdCliente);
                var rol = HttpContext.Session.GetInt32("Rol");
                ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                return View(request);
            }

            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/CrearMembresia";
            var response = await client.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al asignar la membresía.");
            await CargarClientesYPlanesDropdownAsync(client, request.IdCliente);
            var rolDb = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rolDb == 1 || rolDb == 2) ? "_LayoutAdmin" : "_Layout";
            return View(request);
        }

        // POST: Membresias/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/EliminarMembresia/{id}";
            var response = await client.DeleteAsync(url);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EnviarRecordatorio(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var db = _factory.CreateConnection();
            var info = await db.QueryFirstOrDefaultAsync<RecordatorioMembresiaDto>(
                "dbo.sp_Membresia_DatosRecordatorio",
                new { IdMembresiaCliente = id },
                commandType: CommandType.StoredProcedure);

            if (info is null || string.IsNullOrEmpty(info.Correo))
            {
                TempData["Error"] = "No fue posible enviar el recordatorio (el cliente no posee correo registrado).";
                return RedirectToAction(nameof(Index));
            }

            _emailService.EnviarRecordatorioPago(
                info.Correo, info.NombreCompleto, info.PlanNombre, info.DiasRestantes);
            TempData["Exito"] = $"Recordatorio enviado exitosamente a {info.Correo}";

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarClientesYPlanesDropdownAsync(HttpClient client, int? idClienteSeleccionado)
        {
            // Clientes activos
            var resClientes = await client.GetAsync($"{_config["Valores:UrlApi"]}Clientes/ListarClientes?estado=true");
            var clientes = new List<ClienteDto>();
            if (resClientes.IsSuccessStatusCode)
            {
                clientes = await resClientes.Content.ReadFromJsonAsync<List<ClienteDto>>() ?? new List<ClienteDto>();
            }
           
            // Planes activos
            var resPlanes = await client.GetAsync($"{_config["Valores:UrlApi"]}Planes/ListarPlanes");
            var planes = new List<PlanDto>();
            if (resPlanes.IsSuccessStatusCode)
            {
                planes = await resPlanes.Content.ReadFromJsonAsync<List<PlanDto>>() ?? new List<PlanDto>();
                planes = planes.Where(p => p.Estado).ToList(); // Filtrar activos
            }

            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NombreCompleto", idClienteSeleccionado);
            ViewBag.PlanesList = planes; // Store full list to access DuracionDias on client-side
            ViewBag.Planes = new SelectList(planes.Select(p => new { p.IdPlan, Display = $"{p.Nombre} ({p.DuracionDias} días)" }), "IdPlan", "Display", null);
        }
    }
}
