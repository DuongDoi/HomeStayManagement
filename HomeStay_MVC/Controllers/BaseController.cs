using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace HomeStay_MVC.Controllers
{
    public class BaseController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));


        protected void CreateAuthToken(UserInfor _obj)
        {
            HttpContext.Session.Clear();
            // Tạo chuỗi ngẫu nhiên dùng làm mã xác thực
            string authId = GenerateAuthId();

            // Lưu vào session (với giá trị null thì thay bằng chuỗi rỗng)
            HttpContext.Session.SetString("AuthID", authId);
            HttpContext.Session.SetString("User", _obj.Users ?? "");
            HttpContext.Session.SetString("Pass", _obj.Pass ?? "");
            HttpContext.Session.SetString("Email", _obj.Email ?? "");
            HttpContext.Session.SetString("Save_Code", _obj.Save_Code ?? "");
            HttpContext.Session.SetString("Role", _obj.Role ?? "");
            HttpContext.Session.SetString("IsLock", _obj.IsLock ?? "");
            HttpContext.Session.SetString("Is_First_Login", _obj.Is_First_Login ?? "");
            HttpContext.Session.SetString("Homestays_Id", _obj.Homestays_Id ?? "");
            HttpContext.Session.SetString("Create_By", _obj.Create_By ?? "");

            // Log kiểm tra
            string sessionValue = HttpContext.Session.GetString("AuthID");
            log.Error("CreateAuthToken session value authid: " + sessionValue);
        }

        protected bool CheckAuthToken()
        {

            string sessionValue = HttpContext.Session.GetString("AuthID");

            log.Error("CheckAuthToken session value authid: " + sessionValue);

            if (string.IsNullOrEmpty(sessionValue))
            {
                // Invalidate the session and log out the current user.
                return false;
            }
            else
            {
                return true;
            }
        }
        protected bool ClearCache()
        {
            HttpContext.Session.Clear();

            return true;
        }
        private string GenerateAuthId()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }


    }
}
