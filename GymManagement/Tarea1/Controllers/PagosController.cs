using GymManagement_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using Tarea1.Models.Dtos;
using Tarea1.Repositories;
using Tarea1.Models.Entities;

namespace Tarea1.Controllers
{
    public class PagosController(
        IHttpClientFactory _http,
        IConfiguration _config,
        IReservaRepository _reservaRepository) : Controller
    {
        private readonly string _apiUrl = "Pagos";

        // GET: Pagos
        public async Task<IActionResult> Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ListarPagos";
            var response = await client.GetAsync(url);

            var pagos = new List<PagoDto>();
            if (response.IsSuccessStatusCode)
            {
                pagos = await response.Content.ReadFromJsonAsync<List<PagoDto>>() ?? new List<PagoDto>();
            }

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(pagos);
        }

        // GET: Pagos/Create
        public async Task<IActionResult> Create(int? idCliente = null, int? idMembresiaCliente = null)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            await CargarDropdownsAsync(client, idCliente, idMembresiaCliente);

            // Intentar sugerir monto si se seleccionó una membresía
            decimal montoInicial = 0;
            if (idMembresiaCliente.HasValue)
            {
                var resMemb = await client.GetAsync($"{_config["Valores:UrlApi"]}Membresias/ObtenerMembresia/{idMembresiaCliente.Value}");
                if (resMemb.IsSuccessStatusCode)
                {
                    var memb = await resMemb.Content.ReadFromJsonAsync<MembresiaDto>();
                    if (memb != null)
                    {
                        var resPlan = await client.GetAsync($"{_config["Valores:UrlApi"]}Planes/ObtenerPlan/{memb.IdPlan}");
                        if (resPlan.IsSuccessStatusCode)
                        {
                            var plan = await resPlan.Content.ReadFromJsonAsync<PlanDto>();
                            if (plan != null) montoInicial = plan.Precio;
                        }
                    }
                }
            }

            var model = new CrearPagoRequest
            {
                IdCliente = idCliente ?? 0,
                IdMembresiaCliente = idMembresiaCliente,
                Monto = montoInicial,
                MetodoPago = "Efectivo",
                Estado = "Pagado"
            };

            // Buscar reservas pendientes
            if (idCliente.HasValue)
            {
                var reservas = await _reservaRepository.ListarPendientesPorClienteAsync(idCliente.Value);
                ViewBag.ReservasPendientes = reservas;
            }
            else
            {
                ViewBag.ReservasPendientes = new List<ReservaEntrenador>();
            }

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(model);
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearPagoRequest request, List<int> reservasSeleccionadas)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();

            if (!ModelState.IsValid)
            {
                await CargarDropdownsAsync(client, request.IdCliente, request.IdMembresiaCliente);
                var rol = HttpContext.Session.GetInt32("Rol");
                ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

                var res = await _reservaRepository.ListarPendientesPorClienteAsync(request.IdCliente);
                ViewBag.ReservasPendientes = res;
                return View(request);
            }

            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/CrearPago";
            var response = await client.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                if (reservasSeleccionadas != null && reservasSeleccionadas.Count > 0)
                {
                    foreach (var idReserva in reservasSeleccionadas)
                    {
                        await _reservaRepository.MarcarComoPagadaAsync(idReserva);
                    }
                }
                TempData["MostrarAlertaPago"] = "true";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al registrar el pago.");
            await CargarDropdownsAsync(client, request.IdCliente, request.IdMembresiaCliente);
            var rolDb = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rolDb == 1 || rolDb == 2) ? "_LayoutAdmin" : "_Layout";

            var resError = await _reservaRepository.ListarPendientesPorClienteAsync(request.IdCliente);
            ViewBag.ReservasPendientes = resError;
            return View(request);
        }

        private async Task CargarDropdownsAsync(HttpClient client, int? idClienteSeleccionado, int? idMembresiaSeleccionada)
        {
            // Clientes activos
            var resClientes = await client.GetAsync($"{_config["Valores:UrlApi"]}Clientes/ListarClientes?estado=true");
            var clientes = new List<ClienteDto>();
            if (resClientes.IsSuccessStatusCode)
            {
                clientes = await resClientes.Content.ReadFromJsonAsync<List<ClienteDto>>() ?? new List<ClienteDto>();
            }

            // Membresías activas
            var resMembresias = await client.GetAsync($"{_config["Valores:UrlApi"]}Membresias/ListarMembresias");
            var membresias = new List<MembresiaDto>();
            if (resMembresias.IsSuccessStatusCode)
            {
                membresias = await resMembresias.Content.ReadFromJsonAsync<List<MembresiaDto>>() ?? new List<MembresiaDto>();
                membresias = membresias.Where(m => m.Estado).ToList(); // Solo activas
            }

            // Planes para obtener precios
            var resPlanes = await client.GetAsync($"{_config["Valores:UrlApi"]}Planes/ListarPlanes");
            var planes = new List<PlanDto>();
            if (resPlanes.IsSuccessStatusCode)
            {
                planes = await resPlanes.Content.ReadFromJsonAsync<List<PlanDto>>() ?? new List<PlanDto>();
            }
            var planesDict = planes.ToDictionary(p => p.IdPlan, p => p.Precio);

            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NombreCompleto", idClienteSeleccionado);

            // Mapeo para mostrar "Cliente - Plan" en el dropdown de membresías con precios y IDs de clientes
            var listadoMembresias = membresias.Select(m => new
            {
                m.IdMembresiaCliente,
                m.IdCliente,
                Text = $"{m.ClienteNombre} - {m.PlanNombre} (Hasta {m.FechaFin:dd/MM/yyyy})",
                Precio = planesDict.TryGetValue(m.IdPlan, out var precio) ? precio : 0
            }).ToList();

            ViewBag.MembresiasList = listadoMembresias;
        }
    }
}
