using Microsoft.AspNetCore.Mvc;

namespace HomeStay_MVC.Controllers
{
    public class LogoutController : BaseController
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
