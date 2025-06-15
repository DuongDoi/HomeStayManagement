using HomeStay_MVC.ModelCommon;
using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ResfullApi.Models;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomeStay_MVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        string root = Common.GetValuesAppSetting("webConfig", "root");
        public RegisterController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Pass1 != model.Pass2)
                {
                    ViewBag.Message = "Mật khẩu phải giống nhau!";
                    return View();
                }
                else
                {
                    var hasher = new PasswordHasher<string>();
                    var pass = hasher.HashPassword(null, model.Pass1);
                    ResponseObjs _obj = new ResponseObjs();
                    _obj.errCode = "-1";
                    _obj.errMsgs = "Đăng ký thất bại!";
                    try
                    {
                        DataSet ds = DataAccess.USERS_INSERT(model.Users, pass, model.Phone, model.Email, model.Name,"owner","",model.Users);
                        string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                        string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();
                        _obj.errCode = errrCode;
                        _obj.errMsgs = errrMsg;
                    }
                    catch (HttpRequestException ex)
                    {
                        ViewBag.Message = $"Đã xảy ra lỗi khi gửi yêu cầu: {ex.Message}";
                        return View();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = $"Lỗi không xác định: {ex.Message}";
                        return View();
                    }
                    if (_obj.errCode != "-1")
                    {
                        TempData["Success"] = "Đăng ký thành công. Quay lại trang đăng nhập.";
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.Message = "Đăng ký thất bại!";
                        return View();
                    }
                }
            }

            return View(model);
        }
    }
}
