using GymManagement_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace Tarea1.Controllers
{
    public class ClientesController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
        // GET: Clientes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var urlApi = $"{_config["Valores:UrlApi"]}Clientes/ListarClientes"; // Usamos ListarClientes que soporta la API

            var response = await client.GetAsync(urlApi);
            var datos = new List<ClienteModel>();

            if (response.IsSuccessStatusCode)
            {
                datos = await response.Content.ReadFromJsonAsync<List<ClienteModel>>() ?? new List<ClienteModel>();
            }
            else
            {
                ViewBag.Mensaje = "No fue posible consultar los clientes.";
            }

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(datos);
        }

        // GET: Clientes/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();

            // Obtener cliente
            var urlApi = $"{_config["Valores:UrlApi"]}Clientes/ObtenerCliente/{id}";
            var response = await client.GetAsync(urlApi);

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var cliente = await response.Content.ReadFromJsonAsync<ClienteModel>();
            if (cliente == null) return NotFound();

            // Obtener oportunidades del cliente
            var urlOportunidades = $"{_config["Valores:UrlApi"]}Oportunidades/ListarOportunidades?idCliente={id}";
            var responseOportunidades = await client.GetAsync(urlOportunidades);
            var oportunidades = new List<OportunidadModel>();

            if (responseOportunidades.IsSuccessStatusCode)
            {
                oportunidades = await responseOportunidades.Content.ReadFromJsonAsync<List<OportunidadModel>>() ?? new List<OportunidadModel>();
            }

            ViewBag.Oportunidades = oportunidades;

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(cliente);
        }

        // GET: Clientes/Create
        [HttpGet]
        public IActionResult Create()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(new ClienteModel());
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteModel model)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
            {
                var rol = HttpContext.Session.GetInt32("Rol");
                ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                return View(model);
            }

            using var client = _http.CreateClient();
            var urlApi = $"{_config["Valores:UrlApi"]}Clientes/CrearCliente";
            var response = await client.PostAsJsonAsync(urlApi, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Cliente registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Mensaje = await response.Content.ReadAsStringAsync();
            var rolDb = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rolDb == 1 || rolDb == 2) ? "_LayoutAdmin" : "_Layout";
            return View(model);
        }

        // GET: Clientes/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var urlApi = $"{_config["Valores:UrlApi"]}Clientes/ObtenerCliente/{id}";
            var response = await client.GetAsync(urlApi);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var cliente = await response.Content.ReadFromJsonAsync<ClienteModel>();
                if (cliente != null)
                {
                    var rol = HttpContext.Session.GetInt32("Rol");
                    ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                    return View(cliente);
                }
            }

            return NotFound();
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClienteModel model)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
            {
                var rol = HttpContext.Session.GetInt32("Rol");
                ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                return View(model);
            }

            using var client = _http.CreateClient();
            var urlApi = $"{_config["Valores:UrlApi"]}Clientes/ActualizarCliente";
            var response = await client.PutAsJsonAsync(urlApi, model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Cliente actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Mensaje = await response.Content.ReadAsStringAsync();
            var rolDb = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rolDb == 1 || rolDb == 2) ? "_LayoutAdmin" : "_Layout";
            return View(model);
        }

        // GET: Clientes/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var urlApi = $"{_config["Valores:UrlApi"]}Clientes/ObtenerCliente/{id}";
            var response = await client.GetAsync(urlApi);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var cliente = await response.Content.ReadFromJsonAsync<ClienteModel>();
                if (cliente != null)
                {
                    var rol = HttpContext.Session.GetInt32("Rol");
                    ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                    return View(cliente);
                }
            }

            return NotFound();
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ClienteModel model)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            using var client = _http.CreateClient();
            var urlApi = $"{_config["Valores:UrlApi"]}Clientes/EliminarCliente/{model.IdCliente}";
            var response = await client.DeleteAsync(urlApi);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = await response.Content.ReadAsStringAsync();
                var rol = HttpContext.Session.GetInt32("Rol");
                ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";
                return View(model);
            }

            TempData["Exito"] = "Cliente desactivado/eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
