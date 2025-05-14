using HomeStay_MVC.ModelCommon;
using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ResfullApi.Models;
using System.Data;
using System.Text;

namespace HomeStay_MVC.Controllers
{
    
    public class LoginController : BaseController
    {
        private readonly IHttpClientFactory _clientFactory;
        string root = Common.GetValuesAppSetting("webConfig", "root");
        public LoginController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModels model)
        {
            if (ModelState.IsValid)
            {
                
                    ResponseObjs _obj = new ResponseObjs();
                    _obj.errCode = "-1";
                    _obj.errMsgs = "unknow!";
                    try
                    {
                        DataSet ds = DataAccess.LOGIN(model.Users, model.Pass);
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
                    DataSet ds = DataAccess.USERS_GET_LIST(model.Users);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[0].Rows[0];

                        UserInfor _userInfor = new UserInfor();
                        _userInfor.Users = dr["USERS"].ToString();
                        _userInfor.Pass = dr["PASS"].ToString();
                        _userInfor.Email = dr["EMAIL"].ToString();
                        _userInfor.Save_Code = dr["SAVE_CODE"].ToString();
                        _userInfor.Role = dr["ROLE"].ToString();
                        _userInfor.IsLock = dr["ISLOCK"].ToString();
                        _userInfor.Is_First_Login = dr["IS_FIRST_LOGIN"].ToString();
                        _userInfor.Homestays_Id = dr["HOMESTAYS_ID"].ToString();
                        _userInfor.Create_By = dr["CREATE_BY"].ToString();
                        // Tạo session
                        CreateAuthToken(_userInfor);
                        TempData["Success"] = "Đăng nhập thành công.";

                        return RedirectToAction("Index", "AdminHome");
                    }
                    }
                    else
                    {
                        ViewBag.Message = "Sai tên đăng nhập hoặc mật khẩu!";
                    }
                

                return View();
            }

            return View(model);
        }
    }
}
