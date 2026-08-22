using Microsoft.AspNetCore.Mvc;

namespace GymManagement_API.Controllers
{
    public class NotificacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
