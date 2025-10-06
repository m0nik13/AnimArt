using Microsoft.AspNetCore.Mvc;

namespace AnimArt.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}