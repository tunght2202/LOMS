using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
