using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ResfullApi.Models;
using System.Data;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
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
            string _role = HttpContext.Session.GetString("Role");
            string create_by = HttpContext.Session.GetString("User");
            string usersID = "", homestay_id="";
            if (_role == "admin")
            {
                usersID = "-1";
                create_by = "-1";
            }
            else
            {
                if (_role == "owner")
                {
                    usersID = "-1";
                }
                else return RedirectToAction("Index", "Login");
            }

            homestay_id = "-1";

            DataSet ds = DataAccess.USERS_GET_LIST_EMPLOYEE(usersID, homestay_id, create_by);
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
                if (_role == "manager") _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                else _obj.HOMESTAYS_NAME = "Không";

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
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
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
                    if (create_by != "")
                    {
                        if(_role == "admin" || create_by == HttpContext.Session.GetString("User"))
                        {
                            _obj.Create_By = create_by;
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else _obj.Create_By = "Không";
                    if (_obj.Role == "manager") _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                    else _obj.HOMESTAYS_NAME = "Không";
                    return View(_obj);
                }
                return RedirectToAction("Index", "Account");
            }
            else return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public IActionResult Edit(string username)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
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
                    if (_role == "manager") _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                    else _obj.HOMESTAYS_NAME = "Không";

                        return View(_obj);
                }
                return RedirectToAction("Index", "Account");
            }
            else return RedirectToAction("Index", "Login");
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
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                ResponseObjs _obj = new ResponseObjs();
                _obj.errCode = "-1";
                _obj.errMsgs = "unknow!";
                try
                {
                    DataSet ds = DataAccess.USERS_UPDATE_INFOR(username, model.Phone, model.Email, model.Name, model.Card_Number, "1");
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
            else return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public IActionResult Delete(string username)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
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
            else return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Delete(string username, AccountInfor model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                if (!checkPIN(model.Save_Code))
                {
                    ViewBag.Message = "Mã PIN không chính xác.";
                    return View(model);
                }
                if (username == "admin")
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
                else
                {
                    ViewBag.Message = "Không thể xóa tài khoản.";
                    return View(model);
                }

                   
            }
            else return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Add(string username)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");

            if (_role == "admin" || _role == "owner")
            {
                var type = "1";
                var model = new RegisterModel();
                if (_role == "admin")
                {
                    type = "0";
                    model.role = "owner";
                    if(HttpContext.Session.GetString("User") == "admin")
                    {
                        model.RoleOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Admin", Value = "admin" },
                        new SelectListItem { Text = "Chủ sở hữu", Value = "owner" },
                        new SelectListItem { Text = "Quản lý", Value = "manager" }
                    };
                    } else
                    {
                        model.RoleOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Chủ sở hữu", Value = "owner" },
                        new SelectListItem { Text = "Quản lý", Value = "manager" }
                    };
                    }
                }
                else
                {
                    model.role = "manager";
                    model.RoleOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Quản lý", Value = "manager" }
                    };
                }
                var userID = HttpContext.Session.GetString("ID");
                var ht_id = "-1";
                DataSet ds = DataAccess.HOMESTAYS_GET_LIST(ht_id, userID, type);
                model.HomeStayOptions = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var _ht_id = dr["ID"].ToString();
                    var _ht_name = dr["HOMESTAYS_NAME"].ToString();
                    model.HomeStayOptions.Add(new SelectListItem
                    {
                        Text = _ht_name,
                        Value = _ht_id
                    });
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Add(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Pass1 != model.Pass2)
                {
                    ViewBag.Message = "Mật khẩu phải giống nhau!";
                    model = addOptionModel(model);
                    return View(model);
                }
                else
                {
                    var hasher = new PasswordHasher<string>();
                    var pass = hasher.HashPassword(null, model.Pass1);
                    ResponseObjs _obj = new ResponseObjs();
                    _obj.errCode = "-1";
                    _obj.errMsgs = "Thêm mới thất bại!";
                    try
                    {
                        var user = HttpContext.Session.GetString("User");
                        if (model.role != "manager") model.HOMESTAYS_ID = "";
                        if (model.role == "owner") user = model.Users;
                        if(model.role == "manager")
                        {
                            var ds1 = DataAccess.HOMESTAYS_GET_LIST(model.HOMESTAYS_ID, "-1", "1");
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                user = ds1.Tables[0].Rows[0]["USERS"].ToString();
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        DataSet ds = DataAccess.USERS_INSERT(model.Users, pass, model.Phone, model.Email, model.Name,model.role,model.HOMESTAYS_ID,user);
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
                    if (_obj.errCode != "-1")
                    {
                        if (_obj.errCode == "0")
                        {
                            TempData["Success"] = "Thêm mới tài khoản thành công.";
                            return RedirectToAction("Index", "Account");
                        }
                        else
                        {
                            ViewBag.Message = "Tài khoản đã tồn tại"; 
                            model = addOptionModel(model);
                            return View(model);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Đăng ký thất bại!";
                        model = addOptionModel(model);
                        return View(model);
                    }

                }
            }
            model = addOptionModel(model);
            return View(model);
        }


        private RegisterModel addOptionModel(RegisterModel model)
        {
            string _role = HttpContext.Session.GetString("Role");

            if (_role == "admin" || _role == "owner")
            {
                var type = "1";
                if (_role == "admin")
                {
                    type = "0";
                    model.role = "owner";
                    if (HttpContext.Session.GetString("User") == "admin")
                    {
                        model.RoleOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Admin", Value = "admin" },
                        new SelectListItem { Text = "Chủ sở hữu", Value = "owner" },
                        new SelectListItem { Text = "Quản lý", Value = "manager" }
                    };
                    }
                    else
                    {
                        model.RoleOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Chủ sở hữu", Value = "owner" },
                        new SelectListItem { Text = "Quản lý", Value = "manager" }
                    };
                    }
                }
                else
                {
                    model.RoleOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Quản lý", Value = "manager" }
                    };
                }
                var userID = HttpContext.Session.GetString("ID");
                var ht_id = "-1";
                DataSet ds = DataAccess.HOMESTAYS_GET_LIST(ht_id, userID, type);
                model.HomeStayOptions = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var _ht_id = dr["ID"].ToString();
                    var _ht_name = dr["HOMESTAYS_NAME"].ToString();
                    model.HomeStayOptions.Add(new SelectListItem
                    {
                        Text = _ht_name,
                        Value = _ht_id
                    });

                }
                return model;
            }
            else
            {
                return model;
            }
        }
        
    }

    
}
