using GymManagement_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using Tarea1.Models;
using Dapper;
using Tarea1.Data;
using Tarea1.Models.Entities;

namespace Tarea1.Controllers
{
    public class CitasController(
        IHttpClientFactory _http,
        IConfiguration _config,
        IDbConnectionFactory _factory) : Controller
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
            var userRol = HttpContext.Session.GetInt32("Rol");
            var userId = HttpContext.Session.GetInt32("IdUsuario");

            using var client = _http.CreateClient();
            var urlApi = _config["Valores:UrlApi"] + "Citas/ConsultarCitas";
            var response = await client.GetAsync(urlApi);

            var eventosList = new List<object>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = await response.Content.ReadFromJsonAsync<List<CitaModel>>()
                            ?? new List<CitaModel>();

                // Si es Entrenador (Rol == 3), filtrar citas asignadas a él
                if (userRol == 3 && userId.HasValue)
                {
                    datos = datos.Where(x => x.IdUsuario == userId.Value).ToList();
                }

                foreach (var x in datos)
                {
                    eventosList.Add(new
                    {
                        id = "cita_" + x.IdCita,
                        title = x.Titulo,
                        start = x.Fecha.ToString("yyyy-MM-dd") + "T" + x.HoraInicio,
                        end = x.Fecha.ToString("yyyy-MM-dd") + "T" + x.HoraFin,
                        backgroundColor = "#f36100", // Color del gimnasio (naranja)
                        borderColor = "#f36100",
                        extendedProps = new {
                            cliente = x.Cliente,
                            entrenador = x.Usuario,
                            descripcion = x.Descripcion,
                            estado = x.Estado,
                            tipo = "Cita"
                        }
                    });
                }
            }

            // Si es Entrenador (Rol == 3), también cargar sus reservas de entrenador directamente de la base de datos
            if (userRol == 3 && userId.HasValue)
            {
                using var db = _factory.CreateConnection();
                var reservas = await db.QueryAsync<dynamic>(
                    @"SELECT r.IdReserva, r.IdCliente, r.IdEntrenador, r.FechaHora, r.Estado, 
                             (c.Nombre + ' ' + c.Apellido) AS ClienteNombre 
                      FROM dbo.ReservasEntrenador r
                      INNER JOIN dbo.Clientes c ON r.IdCliente = c.IdCliente
                      WHERE r.IdEntrenador = @IdEntrenador",
                    new { IdEntrenador = userId.Value });

                foreach (var r in reservas)
                {
                    DateTime startDateTime = r.FechaHora;
                    DateTime endDateTime = startDateTime.AddHours(1); // Duración estimada 1 hora

                    eventosList.Add(new
                    {
                        id = "reserva_" + r.IdReserva,
                        title = "Entrenamiento: " + r.ClienteNombre,
                        start = startDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                        end = endDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                        backgroundColor = "#007bff", // Color azul para reservas de entrenador
                        borderColor = "#007bff",
                        extendedProps = new {
                            cliente = r.ClienteNombre,
                            entrenador = "Yo",
                            descripcion = "Sesión de entrenamiento personal reservada por el cliente.",
                            estado = r.Estado,
                            tipo = "Reserva"
                        }
                    });
                }
            }

            return Json(eventosList);
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
