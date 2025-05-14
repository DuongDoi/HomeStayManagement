using Microsoft.AspNetCore.Mvc;

namespace HomeStay_MVC.Controllers
{
    public class FoodsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
