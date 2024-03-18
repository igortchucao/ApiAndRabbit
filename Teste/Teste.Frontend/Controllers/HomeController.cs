using Microsoft.AspNetCore.Mvc;

namespace Teste.Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
