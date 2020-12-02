using Microsoft.AspNetCore.Mvc;

namespace NbSites.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ServiceLocator()
        {
            return View();
        }
    }
}
