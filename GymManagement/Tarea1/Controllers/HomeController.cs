using GymManagement_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using Tarea1.Models;
using Tarea1.Services;
using Tarea1.Models.Dtos;
using Tarea1.Repositories;
using Tarea1.Security;

namespace Tarea1.Controllers
{
    public class HomeController(
        IHttpClientFactory _http,
        IConfiguration _config,
        IAuthService _authService,
        EmailService _emailService,
        IUsuarioRepository _usuarios,
        IJwtTokenGenerator _jwt) : Controller
    {
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registro(RegistroUsuarioModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using var client = _http.CreateClient();
            var urlApi = _config["Valores:UrlApi"] + "Home/RegistrarClienteAPI";
            var respuesta = client.PostAsJsonAsync(urlApi, model).Result;

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Cuenta creada correctamente.";

                return RedirectToAction("Login");
            }

            ViewBag.Mensaje = respuesta.Content.ReadAsStringAsync().Result;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using var client = _http.CreateClient();
            var urlApi = $"{_config["Valores:UrlApi"]}Planes/ListarPlanes";
            var response = await client.GetAsync(urlApi);
            var planes = new List<Tarea1.Models.Dtos.PlanDto>();

            if (response.IsSuccessStatusCode)
            {
                planes = await response.Content.ReadFromJsonAsync<List<Tarea1.Models.Dtos.PlanDto>>() ?? new List<Tarea1.Models.Dtos.PlanDto>();
                planes = planes.Where(p => p.Estado).ToList(); // Solo planes activos
            }

            return View(planes);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioModel model)
        {
            using var client = _http.CreateClient();
            var urlApi = _config["Valores:UrlApi"] + "Home/IniciarSesionAPI";
            var response = await client.PostAsJsonAsync(urlApi, model);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = await response.Content.ReadFromJsonAsync<UsuarioModel>();

                HttpContext.Session.SetString("Autenticado", "1");
                HttpContext.Session.SetInt32("IdUsuario", datos!.IdUsuario);
                HttpContext.Session.SetString("Nombre", datos.Nombre);
                HttpContext.Session.SetString("Correo", datos.Correo);
                HttpContext.Session.SetInt32("Rol", datos.IdRol);

                await EmitirTokenAsync(datos.IdUsuario);

                var rol = datos.IdRol;
                if (rol == 1 || rol == 2 || rol == 3)
                {
                    return RedirectToAction("Index", "Citas");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Mensaje = "Correo o contraseña incorrectos.";
                return View(model);
            }

            throw new Exception("Ocurrió un error al intentar iniciar sesión.");
        }

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarAcceso(string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                ViewBag.Mensaje = "Debe ingresar su correo electrónico.";
                return View();
            }

            var request = new ForgotPasswordRequest { Correo = correo };
            var result = await _authService.SolicitarRecuperacionAsync(request);

            if (result.Success && !string.IsNullOrEmpty(result.Data))
            {
                var token = result.Data;
                var enlace = Url.Action("RestablecerPassword", "Home", new { token }, Request.Scheme);

                _emailService.EnviarEnlaceRecuperacion(correo, "Usuario", enlace!);
            }

            TempData["Exito"] = "Si el correo está registrado, recibirás un enlace de recuperación en unos minutos.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult RestablecerPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Mensaje"] = "El token de recuperación no es válido.";
                return RedirectToAction("Login");
            }
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerPassword(string token, string nuevaContrasenna, string confirmarContrasenna)
        {
            ViewBag.Token = token;
            if (string.IsNullOrEmpty(nuevaContrasenna) || nuevaContrasenna != confirmarContrasenna)
            {
                ViewBag.Mensaje = "Las contraseñas no coinciden o no son válidas.";
                return View();
            }

            var request = new ResetPasswordRequest
            {
                Token = token,
                NuevaPassword = nuevaContrasenna
            };

            var result = await _authService.RestablecerPasswordAsync(request);
            if (result.Success)
            {
                TempData["Exito"] = "Tu contraseña ha sido restablecida correctamente. Ya puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            ViewBag.Mensaje = result.Error ?? "Ocurrió un error al intentar restablecer la contraseña.";
            return View();
        }

        /// <summary>
        /// Emite el JWT del usuario que acaba de entrar y lo guarda en una cookie
        /// HttpOnly. Es el token que valida `[Authorize]` en los controladores /api
        /// (ver la configuración de JwtBearer en Program.cs).
        /// </summary>
        private async Task EmitirTokenAsync(int idUsuario)
        {
            var usuario = await _usuarios.ObtenerPorIdAsync(idUsuario);
            if (usuario is null) return;

            var (token, expira) = _jwt.GenerateToken(usuario);

            HttpContext.Session.SetString("Token", token);

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = expira
            });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>Pantalla para cuando el usuario está autenticado pero su rol no alcanza.</summary>
        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View();
        }

        /// <summary>Pantalla de error a la que redirige el middleware de excepciones.</summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int idError)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                IdError = idError
            });
        }
    }
}
