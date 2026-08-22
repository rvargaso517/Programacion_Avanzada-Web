using Microsoft.AspNetCore.Mvc;
using Tarea1.Services;
using Tarea1.Repositories;
using Tarea1.Security;
using Tarea1.Models.Dtos;

namespace Tarea1.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _hasher;

        public UsuarioController(IUsuarioService usuarioService, IUsuarioRepository usuarioRepository, IPasswordHasher hasher)
        {
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
            _hasher = hasher;
        }

        // GET: Usuario/Perfil
        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var idUsuarioVal = HttpContext.Session.GetInt32("IdUsuario");
            if (!idUsuarioVal.HasValue) return NotFound();

            var usuario = await _usuarioService.ObtenerAsync(idUsuarioVal.Value);
            if (usuario == null) return NotFound();

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            return View(usuario);
        }

        // POST: Usuario/Perfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Perfil(string nombre, string correo, string? nuevoPassword, string? confirmarPassword)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var idUsuarioVal = HttpContext.Session.GetInt32("IdUsuario");
            if (!idUsuarioVal.HasValue) return NotFound();

            var usuario = await _usuarioService.ObtenerAsync(idUsuarioVal.Value);
            if (usuario == null) return NotFound();

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewData["Layout"] = (rol == 1 || rol == 2) ? "_LayoutAdmin" : "_Layout";

            // Validaciones básicas
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(correo))
            {
                ViewBag.Error = "El nombre y el correo son obligatorios.";
                return View(usuario);
            }

            // Cambiar contraseña si se ingresó
            if (!string.IsNullOrEmpty(nuevoPassword))
            {
                if (nuevoPassword != confirmarPassword)
                {
                    ViewBag.Error = "La nueva contraseña y su confirmación no coinciden.";
                    return View(usuario);
                }

                // Generar hash y guardar contraseña
                var hash = _hasher.Hash(nuevoPassword);
                await _usuarioRepository.ActualizarPasswordAsync(idUsuarioVal.Value, hash);
            }

            // Actualizar datos personales (nombre, correo)
            var request = new ActualizarUsuarioRequest
            {
                IdUsuario = idUsuarioVal.Value,
                IdRol = usuario.IdRol,
                Nombre = nombre.Trim(),
                Correo = correo.Trim(),
                Estado = usuario.Estado
            };

            var updateResult = await _usuarioService.ActualizarAsync(request);
            if (!updateResult.Success)
            {
                ViewBag.Error = updateResult.Error;
                return View(usuario);
            }

            // Actualizar variables de sesión
            HttpContext.Session.SetString("Nombre", nombre);
            HttpContext.Session.SetString("Correo", correo);

            // Recargar datos actualizados
            var usuarioActualizado = await _usuarioService.ObtenerAsync(idUsuarioVal.Value);
            ViewBag.Exito = "Perfil actualizado correctamente.";

            return View(usuarioActualizado);
        }
    }
}
