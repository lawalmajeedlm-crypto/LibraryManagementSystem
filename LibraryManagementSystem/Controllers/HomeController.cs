using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Simple view for unauthorized access
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}