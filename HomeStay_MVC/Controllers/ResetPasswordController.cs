using HomeStay_MVC.ModelCommon;
using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        string root = Common.GetValuesAppSetting("webConfig", "root");
        public ResetPasswordController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                if (model.Pass1 != model.Pass2)
                {
                    ViewBag.Message = "Mật khẩu phải giống nhau!";
                    return View();
                }
                else
                {
                    var pass = model.Pass1;
                    ResponseObjs _obj = new ResponseObjs();
                    _obj.errCode = "-1";
                    _obj.errMsgs = "Lấy lại mật khẩu thất bại!";
                    try
                    {
                        DataSet ds = DataAccess.USERS_RESET_PASSWORD(model.Users, model.Email, model.PIN,pass);
                        string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                        string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();
                        _obj.errCode = errrCode;
                        _obj.errMsgs = errrMsg;
                    }
                    catch (HttpRequestException ex)
                    {
                        ViewBag.Message = $"Đã xảy ra lỗi khi gửi yêu cầu: {ex.Message}";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = $"Lỗi không xác định: {ex.Message}";
                    }
                    if (_obj.errCode == "0")
                    {
                        TempData["Success"] = "Lấy lại mật khẩu thành công. Quay lại trang đăng nhập.";
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.Message = _obj.errMsgs;
                    }
                }

                return View();
            }

            return View(model);
        }
    }
}

