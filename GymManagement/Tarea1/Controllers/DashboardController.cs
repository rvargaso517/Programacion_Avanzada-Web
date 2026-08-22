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

            var rol = HttpContext.Session.GetInt32("Rol");
            if (rol == 4) // Si es cliente, denegar acceso al panel de administración
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Citas");
        }
    }
}
