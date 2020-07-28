using Microsoft.AspNetCore.Mvc;

namespace NbSites.Areas.Web.Demo.Controllers
{
    [Area("Demo")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DiTest()
        {
            return View();
        }
    }
}
