using Microsoft.AspNetCore.Mvc;
using HomeStay_MVC.Controllers;
using HomeStay_MVC.ModelCommon;

namespace HomeStay_MVC.Controllers
{
    public class AdminHomeController : BaseController
    {
        string root = Common.GetValuesAppSetting("webConfig", "root");
        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
