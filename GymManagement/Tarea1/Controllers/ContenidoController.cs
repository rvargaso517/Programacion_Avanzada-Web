using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Tarea1.Models;
using Dapper;
using Tarea1.Data;
using Tarea1.Helpers;
using System.Collections.Generic;

namespace Tarea1.Controllers
{
    public class ContenidoController : Controller
    {
        private readonly IDbConnectionFactory _factory;

        public ContenidoController(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        private string GetContentPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "contenido.json");
        }

        private async Task<ContenidoPaginaModel> LoadContentAsync()
        {
            var path = GetContentPath();
            if (!System.IO.File.Exists(path))
            {
                return new ContenidoPaginaModel();
            }
            var json = await System.IO.File.ReadAllTextAsync(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<ContenidoPaginaModel>(json, options) ?? new ContenidoPaginaModel();
        }

        private async Task SaveContentAsync(ContenidoPaginaModel model)
        {
            var path = GetContentPath();
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(model, options);
            await System.IO.File.WriteAllTextAsync(path, json);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var rol = HttpContext.Session.GetInt32("Rol");
            if (rol != 1 && rol != 2) // Solo admin/recepcionista
                return RedirectToAction("Index", "Dashboard");

            ViewData["Layout"] = "_LayoutAdmin";
            var model = await LoadContentAsync();

            // Cargar roles y permisos
            using var db = _factory.CreateConnection();
            var roles = await db.QueryAsync<dynamic>("SELECT IdRol, Nombre, Descripcion FROM dbo.Roles");
            ViewBag.Roles = roles;
            ViewBag.Permisos = PermisosHelper.CargarPermisos();
            ViewBag.Menus = PermisosHelper.ListarTodosLosMenus();

            // Cargar log de errores para la vista administrativa
            var errores = await db.QueryAsync<dynamic>("EXEC dbo.sp_LogError_Listar @Top = 50");
            ViewBag.Errores = errores;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarChoseUs(ChoseUsSection choseUs)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var model = await LoadContentAsync();
            model.ChoseUs = choseUs;
            await SaveContentAsync(model);

            TempData["Exito"] = "Sección '¿Por qué elegirnos?' guardada con éxito.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarClasses(ClassesSection classes)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var model = await LoadContentAsync();

            for (int i = 0; i < classes.Items.Count; i++)
            {
                var file = Request.Form.Files[$"imagenFile_{i}"];
                if (file != null && file.Length > 0)
                {
                    var ext = Path.GetExtension(file.FileName);
                    var newFileName = $"class_upload_{i + 1}_{DateTime.Now:yyyyMMdd_HHmmss}{ext}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", newFileName);
                    
                    var dir = Path.GetDirectoryName(path);
                    if (dir != null && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    classes.Items[i].Imagen = $"/imagenes/{newFileName}";
                }
                else
                {
                    if (model.Classes.Items != null && model.Classes.Items.Count > i)
                    {
                        classes.Items[i].Imagen = model.Classes.Items[i].Imagen;
                    }
                }
            }

            model.Classes = classes;
            await SaveContentAsync(model);

            TempData["Exito"] = "Sección 'Nuestras Clases' guardada con éxito.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarTeam(TeamSection team)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            var model = await LoadContentAsync();

            for (int i = 0; i < team.Items.Count; i++)
            {
                var file = Request.Form.Files[$"teamFile_{i}"];
                if (file != null && file.Length > 0)
                {
                    var ext = Path.GetExtension(file.FileName);
                    var newFileName = $"team_upload_{i + 1}_{DateTime.Now:yyyyMMdd_HHmmss}{ext}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", newFileName);
                    
                    var dir = Path.GetDirectoryName(path);
                    if (dir != null && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    team.Items[i].Imagen = $"/imagenes/{newFileName}";
                }
                else
                {
                    if (model.Team.Items != null && model.Team.Items.Count > i)
                    {
                        team.Items[i].Imagen = model.Team.Items[i].Imagen;
                    }
                }
            }

            model.Team = team;
            await SaveContentAsync(model);

            TempData["Exito"] = "Sección 'Nuestro Equipo' guardada con éxito.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuardarPermisos(Dictionary<string, List<string>> permisos)
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            var rol = HttpContext.Session.GetInt32("Rol");
            if (string.IsNullOrEmpty(autenticado) || rol != 1) // Solo Administradores pueden gestionar accesos
                return RedirectToAction("Login", "Home");

            PermisosHelper.GuardarPermisos(permisos ?? new());

            TempData["Exito"] = "Roles y permisos actualizados con éxito.";
            return RedirectToAction(nameof(Index));
        }
    }
}
