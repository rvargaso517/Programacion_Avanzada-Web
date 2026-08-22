using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Tarea1.Models.Dtos;

namespace Tarea1.Controllers
{
    public class PlanesController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
        private readonly string _apiUrl = "Planes";

        // GET: Planes
        public async Task<IActionResult> Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ListarPlanes";
            var response = await client.GetAsync(url);

            var planes = new List<PlanDto>();
            if (response.IsSuccessStatusCode)
            {
                planes = await response.Content.ReadFromJsonAsync<List<PlanDto>>() ?? new List<PlanDto>();
            }

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(planes);
        }

        // GET: Planes/Create
        public IActionResult Create()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var model = new CrearPlanRequest();

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(model);
        }

        // POST: Planes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearPlanRequest request)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
            {
                var rol = HttpContext.Session.GetInt32("Rol");
                ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                return View(request);
            }

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/CrearPlan";
            var response = await client.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al crear el plan.");
            var rolDb = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rolDb == 1 || rolDb == 2) ? "_LayoutAdmin" : "_Layout";
            return View(request);
        }

        // GET: Planes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ObtenerPlan/{id}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return NotFound();

            var plan = await response.Content.ReadFromJsonAsync<PlanDto>();
            if (plan == null) return NotFound();

            var model = new ActualizarPlanRequest
            {
                IdPlan = plan.IdPlan,
                Nombre = plan.Nombre,
                Descripcion = plan.Descripcion,
                DuracionDias = plan.DuracionDias,
                Precio = plan.Precio,
                Estado = plan.Estado
            };

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(model);
        }

        // POST: Planes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActualizarPlanRequest request)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
            {
                var rol = HttpContext.Session.GetInt32("Rol");
                ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                return View(request);
            }

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/ActualizarPlan";
            var response = await client.PutAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Error al actualizar el plan.");
            var rolDb = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rolDb == 1 || rolDb == 2) ? "_LayoutAdmin" : "_Layout";
            return View(request);
        }

        // POST: Planes/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var url = $"{_config["Valores:UrlApi"]}{_apiUrl}/EliminarPlan/{id}";
            var response = await client.DeleteAsync(url);

            return RedirectToAction(nameof(Index));
        }
    }
}
