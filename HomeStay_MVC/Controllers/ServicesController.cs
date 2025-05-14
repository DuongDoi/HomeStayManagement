using Microsoft.AspNetCore.Mvc;

namespace HomeStay_MVC.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
