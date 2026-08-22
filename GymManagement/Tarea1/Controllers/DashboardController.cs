using Microsoft.AspNetCore.Mvc;

namespace Tarea1.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var autenticado = HttpContext.Session.GetString("Autenticado");
            if (string.IsNullOrEmpty(autenticado))
                return RedirectToAction("Login", "Home");

            // Redirigir al módulo principal (Citas / Agenda) por defecto
            return RedirectToAction("Index", "Citas");
        }
    }
}
