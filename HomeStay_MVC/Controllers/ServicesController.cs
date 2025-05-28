using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class ServicesController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));


        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            string id = "0";
            string user_id = "0";
            string v_type = "0";
            if (_role == "admin")
            {
                id = "0";
                user_id = "0";
                v_type = "0";
            }
            else
            {
                if (_role == "owner")
                {
                    id = "-1";
                    user_id = HttpContext.Session.GetString("ID");
                    v_type = "1";
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        id = "-1";
                        v_type = "1";
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }
            logger.Info("New request income select All Service :");
            DataSet ds = DataAccess.SERVICES_GET_LIST(id, user_id, v_type);
            List<Services> services = new List<Services>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Services _obj = new Services();
                _obj.ID = dr["ID"].ToString();
                _obj.SERVICES_NAME = dr["SERVICES_NAME"].ToString();
                _obj.SERVICES_PRICE = dr["SERVICES_PRICE"].ToString();
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                _obj.USERS = dr["NAME"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                services.Add(_obj);

            }

            logger.Info("Pro Select All Service success.");
            return View(services);
        }


        public IActionResult Details(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                var userID = HttpContext.Session.GetString("ID");
                DataSet ds = DataAccess.SERVICES_GET_LIST(id, "-1", "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Services _obj = new Services();
                    _obj.ID = dr["ID"].ToString();
                    _obj.SERVICES_NAME = dr["SERVICES_NAME"].ToString();
                    _obj.SERVICES_PRICE = dr["SERVICES_PRICE"].ToString();
                    _obj.USERS = dr["NAME"].ToString();
                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    return View(_obj);
                }
                return RedirectToAction("Index", "Services");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                var userID = HttpContext.Session.GetString("ID");
                DataSet ds = DataAccess.SERVICES_GET_LIST(id, userID, "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Services _obj = new Services();
                    _obj.ID = dr["ID"].ToString();
                    _obj.SERVICES_NAME = dr["SERVICES_NAME"].ToString();
                    _obj.SERVICES_PRICE = dr["SERVICES_PRICE"].ToString();
                    _obj.USERS = dr["NAME"].ToString();
                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    return View(_obj);
                }
                return RedirectToAction("Index", "Services");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Edit(string id, Services model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!checkPIN(model.Save_code))
            {
                ViewBag.Message = "Sai mã PIN";
                return View(model);
            }



            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            var userID = HttpContext.Session.GetString("ID");
            try
            {
                DataSet ds = DataAccess.SERVICES_UPDATE(id, model.SERVICES_NAME, model.SERVICES_PRICE, userID  , "1");
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
        public IActionResult Delete(string id)
        {

            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                var obj = new Services();
                obj.ID = id;
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Delete(string id, Services model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            if (!checkPIN(model.Save_code))
            {
                ViewBag.Message = "Mã PIN không chính xác.";
                return View(model);
            }
            var userID = HttpContext.Session.GetString("ID");
            DataSet ds = DataAccess.SERVICES_UPDATE(id, "", "", userID, "2"); ;
            var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

            if (errCode == "0")
            {
                TempData["Success"] = "Xóa dịch vụ thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Không thể xóa dịch vụ.";
            return View(model);
        }


        [HttpGet]
        public IActionResult Add()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            Services model = new Services();
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Services model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                var userID = HttpContext.Session.GetString("ID");
                ResponseObjs _obj = new ResponseObjs();
                _obj.errCode = "-1";
                _obj.errMsgs = "Thêm mới thất bại!";
                try
                {
                    DataSet ds = DataAccess.SERVICES_INSERT(model.SERVICES_NAME, model.SERVICES_PRICE, userID);
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
                    TempData["Success"] = "Thêm mới thành công.";
                    return RedirectToAction("Index", "Services");
                }
                else
                {
                    ViewBag.Message = _obj.errMsgs;
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

    }
}
