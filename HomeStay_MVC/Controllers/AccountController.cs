using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string users = "-1";
            DataSet ds = DataAccess.USERS_GET_LIST(users);
            List<AccountInfor> accounts = new List<AccountInfor>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AccountInfor _obj = new AccountInfor();
                _obj.Id = dr["Id"].ToString();
                _obj.Users = dr["Users"].ToString();
                _obj.Pass = dr["Pass"].ToString();
                _obj.Phone = dr["Phone"].ToString();
                _obj.Email = dr["Email"].ToString();
                _obj.Name = dr["Name"].ToString();
                _obj.Card_Number = dr["Card_Number"].ToString();
                _obj.Save_Code = dr["Save_Code"].ToString();
                _obj.Role = dr["Role"].ToString();
                _obj.IsLock = dr["IsLock"].ToString();
                _obj.Avatar_Path = dr["Avatar_Path"].ToString();
                try { _obj.Create_At = DateTime.Parse(dr["Create_At"].ToString()); }
                catch { }
                try { _obj.Update_At = DateTime.Parse(dr["Update_At"].ToString()); }
                catch { }
                _obj.Is_First_Login = dr["Is_First_Login"].ToString();
                _obj.Homestays_Id = dr["Homestays_Id"].ToString();
                _obj.Create_By = dr["Create_By"].ToString();

                accounts.Add(_obj);

            }
            return View(accounts);
        }

        public IActionResult Details(string username)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.USERS_GET_LIST(username);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                AccountInfor _obj = new AccountInfor();
                _obj.Id = dr["Id"].ToString();
                _obj.Users = dr["Users"].ToString();
                _obj.Pass = dr["Pass"].ToString();
                _obj.Phone = dr["Phone"].ToString();
                _obj.Email = dr["Email"].ToString();
                _obj.Name = dr["Name"].ToString();
                _obj.Card_Number = dr["Card_Number"].ToString();
                _obj.Save_Code = dr["Save_Code"].ToString();
                _obj.Role = dr["Role"].ToString();
                var islock = dr["IsLock"].ToString();
                if (islock == "0") _obj.IsLock = "Đang hoạt động";
                if (islock == "1") _obj.IsLock = "Bị khóa";
                _obj.Avatar_Path = dr["Avatar_Path"].ToString();
                try { _obj.Create_At = DateTime.Parse(dr["Create_At"].ToString()); }
                catch { }
                try { _obj.Update_At = DateTime.Parse(dr["Update_At"].ToString()); }
                catch { }
                _obj.Is_First_Login = dr["Is_First_Login"].ToString();
                var id_homestay = dr["Homestays_Id"].ToString();
                if (id_homestay != "") _obj.Homestays_Id = id_homestay;
                else _obj.Homestays_Id = "Không";
                var create_by = dr["Create_By"].ToString();
                if (create_by != "") _obj.Create_By = create_by;
                else _obj.Create_By = "Không";

                return View(_obj);
            }
            return RedirectToAction("Index", "Account");
        }
        [HttpGet]
        public IActionResult Edit(string username)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.USERS_GET_LIST(username);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                AccountInfor _obj = new AccountInfor();
                _obj.Id = dr["Id"].ToString();
                _obj.Users = dr["Users"].ToString();
                _obj.Pass = dr["Pass"].ToString();
                _obj.Phone = dr["Phone"].ToString();
                _obj.Email = dr["Email"].ToString();
                _obj.Name = dr["Name"].ToString();
                _obj.Card_Number = dr["Card_Number"].ToString();
                _obj.Save_Code = dr["Save_Code"].ToString();
                _obj.Role = dr["Role"].ToString();
                var islock = dr["IsLock"].ToString();
                if (islock == "0") _obj.IsLock = "Đang hoạt động";
                if (islock == "1") _obj.IsLock = "Bị khóa";
                _obj.Avatar_Path = dr["Avatar_Path"].ToString();
                try { _obj.Create_At = DateTime.Parse(dr["Create_At"].ToString()); }
                catch { }
                try { _obj.Update_At = DateTime.Parse(dr["Update_At"].ToString()); }
                catch { }
                _obj.Is_First_Login = dr["Is_First_Login"].ToString();
                var id_homestay = dr["Homestays_Id"].ToString();
                if (id_homestay != "") _obj.Homestays_Id = id_homestay;
                else _obj.Homestays_Id = "Không";
                var create_by = dr["Create_By"].ToString();
                if (create_by != "") _obj.Create_By = create_by;
                else _obj.Create_By = "Không";

                return View(_obj);
            }
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        public IActionResult Edit(string username, AccountInfor model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!checkPIN(model.Save_Code))
            {
                ViewBag.Message = "Sai mã PIN";
                return View(model);
            }

            

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.USERS_UPDATE_INFOR(username,model.Phone,model.Email,model.Name,model.Card_Number,"1");
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
                TempData["Success"] = "Cập nhật thông tin thành công.";

                return RedirectToAction("Index");
            }
            else 
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Delete(string username)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.USERS_GET_LIST(username);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                AccountInfor _obj = new AccountInfor();
                _obj.Id = dr["Id"].ToString();
                _obj.Users = dr["Users"].ToString();
                _obj.Pass = dr["Pass"].ToString();
                _obj.Phone = dr["Phone"].ToString();
                _obj.Email = dr["Email"].ToString();
                _obj.Name = dr["Name"].ToString();
                _obj.Card_Number = dr["Card_Number"].ToString();
                _obj.Save_Code = dr["Save_Code"].ToString();
                _obj.Role = dr["Role"].ToString();
                var islock = dr["IsLock"].ToString();
                if (islock == "0") _obj.IsLock = "Đang hoạt động";
                if (islock == "1") _obj.IsLock = "Bị khóa";
                _obj.Avatar_Path = dr["Avatar_Path"].ToString();
                try { _obj.Create_At = DateTime.Parse(dr["Create_At"].ToString()); }
                catch { }
                try { _obj.Update_At = DateTime.Parse(dr["Update_At"].ToString()); }
                catch { }
                _obj.Is_First_Login = dr["Is_First_Login"].ToString();
                var id_homestay = dr["Homestays_Id"].ToString();
                if (id_homestay != "") _obj.Homestays_Id = id_homestay;
                else _obj.Homestays_Id = "Không";
                var create_by = dr["Create_By"].ToString();
                if (create_by != "") _obj.Create_By = create_by;
                else _obj.Create_By = "Không";

                return View(_obj);
            }
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        public IActionResult Delete(string username, AccountInfor model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            if (!checkPIN(model.Save_Code))
            {
                ViewBag.Message = "Mã PIN không chính xác.";
                return View(model);
            }
            if(username=="admin")
            {
                ViewBag.Message = "Không thể xóa tài khoản này.";
                return View(model);
            }

            var ds = DataAccess.USERS_UPDATE_INFOR(username, "", "", "", "", "2");
            var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

            if (errCode == "0")
            {
                TempData["Success"] = "Xóa tài khoản thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Không thể xóa tài khoản.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Add(string username)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            var model = new RegisterModel
            {
                role = "manager",
                RoleOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Admin", Value = "admin" },
            new SelectListItem { Text = "Manager", Value = "manager" },
            new SelectListItem { Text = "Employee", Value = "employee" }
        }
            };
            return View(model);
        }

    }
}
