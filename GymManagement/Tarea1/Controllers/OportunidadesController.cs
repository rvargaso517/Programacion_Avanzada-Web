using GymManagement_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using Tarea1.Models.Dtos;

namespace Tarea1.Controllers
{
    public class OportunidadesController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
        private readonly string _apiUrl = "Oportunidades";

        // GET: Oportunidades
        public async Task<IActionResult> Index(string? buscar = null, string? etapa = null, int? idCliente = null, bool? estado = null)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ListarOportunidades?buscar={buscar}&etapa={etapa}&idCliente={idCliente}&estado={estado}";
            var response = await client.GetAsync(url);
            
            var oportunidades = new List<OportunidadDto>();
            if (response.IsSuccessStatusCode)
            {
                oportunidades = await response.Content.ReadFromJsonAsync<List<OportunidadDto>>() ?? new List<OportunidadDto>();
            }

            // Cargar clientes activos para el filtro
            var resClientes = await client.GetAsync($"{_config["Valores:UrlApi"]}Clientes/ListarClientes?estado=true");
            var clientes = new List<ClienteDto>();
            if (resClientes.IsSuccessStatusCode)
            {
                clientes = await resClientes.Content.ReadFromJsonAsync<List<ClienteDto>>() ?? new List<ClienteDto>();
            }

            ViewBag.Buscar = buscar;
            ViewBag.Etapa = etapa;
            ViewBag.IdCliente = idCliente;
            ViewBag.Estado = estado;

            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NombreCompleto", idCliente);
            ViewBag.Etapas = ObtenerEtapasList(etapa);

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(oportunidades);
        }

        // GET: Oportunidades/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ObtenerOportunidad/{id}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return NotFound();

            var op = await response.Content.ReadFromJsonAsync<OportunidadDto>();

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(op);
        }

        // GET: Oportunidades/Create
        public async Task<IActionResult> Create(int? idCliente = null)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            await CargarClientesDropdownAsync(client, idCliente);
            ViewBag.Etapas = ObtenerEtapasList("Nuevo");

            var model = new CrearOportunidadRequest
            {
                IdCliente = idCliente ?? 0,
                Etapa = "Nuevo"
            };

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(model);
        }

        // POST: Oportunidades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearOportunidadRequest request)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            if (!ModelState.IsValid)
            {
                await CargarClientesDropdownAsync(client, request.IdCliente);
                ViewBag.Etapas = ObtenerEtapasList(request.Etapa);
                return View(request);
            }

            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/CrearOportunidad";
            var response = await client.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Oportunidad comercial creada con éxito.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "No fue posible crear la oportunidad.");
            await CargarClientesDropdownAsync(client, request.IdCliente);
            ViewBag.Etapas = ObtenerEtapasList(request.Etapa);

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(request);
        }

        // GET: Oportunidades/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ObtenerOportunidad/{id}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return NotFound();

            var op = await response.Content.ReadFromJsonAsync<OportunidadDto>();
            await CargarClientesDropdownAsync(client, op!.IdCliente);
            ViewBag.Etapas = ObtenerEtapasList(op.Etapa);

            var request = new ActualizarOportunidadRequest
            {
                IdOportunidad = op.IdOportunidad,
                IdCliente = op.IdCliente,
                Titulo = op.Titulo,
                Descripcion = op.Descripcion,
                MontoEstimado = op.MontoEstimado,
                Etapa = op.Etapa,
                FechaCierre = op.FechaCierre,
                Estado = op.Estado
            };

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(request);
        }

        // POST: Oportunidades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActualizarOportunidadRequest request)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            if (!ModelState.IsValid)
            {
                await CargarClientesDropdownAsync(client, request.IdCliente);
                ViewBag.Etapas = ObtenerEtapasList(request.Etapa);
                return View(request);
            }

            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ActualizarOportunidad";
            var response = await client.PutAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Oportunidad comercial actualizada con éxito.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "No fue posible actualizar la oportunidad.");
            await CargarClientesDropdownAsync(client, request.IdCliente);
            ViewBag.Etapas = ObtenerEtapasList(request.Etapa);

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(request);
        }

        // GET: Oportunidades/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ObtenerOportunidad/{id}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return NotFound();

            var op = await response.Content.ReadFromJsonAsync<OportunidadDto>();

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(op);
        }

        // POST: Oportunidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/EliminarOportunidad/{id}";
            var response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Oportunidad comercial eliminada con éxito.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "No fue posible eliminar la oportunidad.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarClientesDropdownAsync(HttpClient client, int? seleccionado = null)
        {
            var url = $"{_config["Valores:UrlApi"]}Clientes/ListarClientes?estado=true";
            var response = await client.GetAsync(url);
            var clientes = new List<ClienteDto>();
            if (response.IsSuccessStatusCode)
            {
                clientes = await response.Content.ReadFromJsonAsync<List<ClienteDto>>() ?? new List<ClienteDto>();
            }
            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NombreCompleto", seleccionado);
        }

        private static SelectList ObtenerEtapasList(string? seleccionado)
        {
            var etapas = new[] { "Nuevo", "En Contacto", "Propuesta", "Ganada", "Perdida" };
            return new SelectList(etapas, seleccionado);
        }
    }
}
