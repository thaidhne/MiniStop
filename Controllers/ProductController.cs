using Microsoft.AspNetCore.Mvc;

namespace MiniStop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
