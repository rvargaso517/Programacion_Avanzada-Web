using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tarea1.Models.Dtos;
using Tarea1.Services;

namespace Tarea1.Controllers
{
    /// <summary>CRUD de usuarios (vistas MVC). Acceso solo para el rol Administrador.</summary>
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        private bool ValidarAdmin()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            var rol = HttpContext.Session.GetInt32("Rol");
            return !string.IsNullOrEmpty(autenticado) && rol == 1;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            var usuarios = await _usuarioService.ListarAsync();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            var usuario = await _usuarioService.ObtenerAsync(id);
            if (usuario is null) return NotFound();
            return View(usuario);
        }

        // GET: Usuarios/Create
        public async Task<IActionResult> Create()
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            await CargarRolesAsync();
            return View(new CrearUsuarioRequest());
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearUsuarioRequest model)
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(model.IdRol);
                return View(model);
            }

            var result = await _usuarioService.CrearAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                await CargarRolesAsync(model.IdRol);
                return View(model);
            }

            TempData["Exito"] = "Usuario creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            var usuario = await _usuarioService.ObtenerAsync(id);
            if (usuario is null) return NotFound();

            var model = new ActualizarUsuarioRequest
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                IdRol = usuario.IdRol,
                Estado = usuario.Estado
            };
            await CargarRolesAsync(usuario.IdRol);
            return View(model);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActualizarUsuarioRequest model)
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(model.IdRol);
                return View(model);
            }

            var result = await _usuarioService.ActualizarAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                await CargarRolesAsync(model.IdRol);
                return View(model);
            }

            TempData["Exito"] = "Usuario actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            var usuario = await _usuarioService.ObtenerAsync(id);
            if (usuario is null) return NotFound();
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarAdmin()) return RedirectToAction("Login", "Home");

            var result = await _usuarioService.EliminarAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction(nameof(Index));
            }

            TempData["Exito"] = "Usuario eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarRolesAsync(int? seleccionado = null)
        {
            var roles = await _usuarioService.ListarRolesAsync();
            ViewBag.Roles = new SelectList(roles, "IdRol", "Nombre", seleccionado);
        }
    }
}
