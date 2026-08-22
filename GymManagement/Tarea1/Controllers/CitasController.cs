using GymManagement_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using Tarea1.Models;

namespace Tarea1.Controllers
{
    public class CitasController(
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2 || rol == 3) ? "_LayoutAdmin" : "_Layout";

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerCitas()
        {
            using var client = _http.CreateClient();

            var urlApi = _config["Valores:UrlApi"] + "Citas/ConsultarCitas";

            var response = await client.GetAsync(urlApi);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = await response.Content.ReadFromJsonAsync<List<CitaModel>>()
                            ?? new List<CitaModel>();

                var eventos = datos.Select(x => new
                {
                    id = x.IdCita,
                    title = x.Titulo,
                    start = x.Fecha.ToString("yyyy-MM-dd") + "T" + x.HoraInicio,
                    end = x.Fecha.ToString("yyyy-MM-dd") + "T" + x.HoraFin
                });

                return Json(eventos);
            }

            return Json(new List<object>());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2 || rol == 3) ? "_LayoutAdmin" : "_Layout";

            using var client = _http.CreateClient();
            var urlApi = _config["Valores:UrlApi"] + "Clientes/ListarClientes";
            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var clientes = response.Content
                    .ReadFromJsonAsync<List<ClienteComboModel>>()
                    .Result;

                ViewBag.Clientes = new SelectList(
                    clientes,
                    "IdCliente",
                    "NombreCompleto");

                return View();
            }

            ViewBag.Clientes = new SelectList(
                new List<ClienteComboModel>(),
                "IdCliente",
                "NombreCompleto");

            return View();
        }

        [HttpPost]
        public IActionResult Create(RegistrarCitaRequestModel model)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2 || rol == 3) ? "_LayoutAdmin" : "_Layout";

            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            model.IdUsuario = idUsuario ?? 1;

            using var client = _http.CreateClient();
            var urlApi = _config["Valores:UrlApi"] + "Citas/RegistrarCita";
            var response = client.PostAsJsonAsync(urlApi, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }

            // Si hubo error, volver a cargar el combo
            var clientes = client.GetFromJsonAsync<List<ClienteComboModel>>(
                _config["Valores:UrlApi"] + "Clientes/ListarClientes").Result;

            ViewBag.Clientes = new SelectList(
                clientes,
                "IdCliente",
                "NombreCompleto");

            ViewBag.Mensaje = "No fue posible registrar la cita.";

            return View(model);
        }
    }
}
