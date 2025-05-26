using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResfullApi.Models;
using System.Data;
using System.Net.NetworkInformation;

namespace HomeStay_MVC.Controllers
{
    public class RoomsController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));


        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            logger.Info("New request income select All Rooms :");
            string room_id = "0";
            string ht_id = "0";
            string v_type = "0";
            DataSet ds = DataAccess.ROOMS_GET_LIST(ht_id, room_id, v_type);
            List<Rooms> rooms = new List<Rooms>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Rooms _obj = new Rooms();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();
                var _status = dr["ROOMS_STATUS"].ToString();
                if (_status == "Available") _obj.ROOMS_STATUS = "Còn trống";
                else _obj.ROOMS_STATUS = "Đang cho thuê";
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                rooms.Add(_obj);

            }

            logger.Info("Pro Rooms Select All success.");
            return View(rooms);
        }

        public IActionResult Details(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.ROOMS_GET_LIST("-1",id, "1");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Rooms _obj = new Rooms();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();
                var _status = dr["ROOMS_STATUS"].ToString();
                if (_status == "Available") _obj.ROOMS_STATUS = "Còn trống";
                else _obj.ROOMS_STATUS = "Đang cho thuê";
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                return View(_obj);
            }
            return RedirectToAction("Index", "Rooms");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.ROOMS_GET_LIST("-1", id, "1");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Rooms _obj = new Rooms();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();
                var _status = dr["ROOMS_STATUS"].ToString();
                if (_status == "Available") _obj.ROOMS_STATUS = "Còn trống";
                else _obj.ROOMS_STATUS = "Đang cho thuê";
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }
                _obj.StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Còn trống", Value = "Available" },
                    new SelectListItem { Text = "Đang cho thuê", Value = "unAvailable" }
                };
                return View(_obj);
            }
            return RedirectToAction("Index", "Rooms");
        }

        [HttpPost]
        public IActionResult Edit(string id, Rooms model)
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
            try
            {
                DataSet ds = DataAccess.ROOMS_UPDATE(id, model.ROOMS_NAME, model.ROOMS_PRICE, model.ROOMS_STATUS, "1");
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
                model.StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Còn trống", Value = "Available" },
                    new SelectListItem { Text = "Đang cho thuê", Value = "unAvailable" }
                };

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
            var obj = new Rooms();
            obj.ID = id;
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(string id, Rooms model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            if (!checkPIN(model.Save_code))
            {
                ViewBag.Message = "Mã PIN không chính xác.";
                return View(model);
            }

            DataSet ds = DataAccess.ROOMS_UPDATE(id,"","","", "2"); ;
            var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

            if (errCode == "0")
            {
                TempData["Success"] = "Xóa phòng thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Không thể xóa phòng.";
            return View(model);
        }


        [HttpGet]
        public IActionResult Add()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            Rooms model = new Rooms();
            model.StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Còn trống", Value = "Available" },
                    new SelectListItem { Text = "Đang cho thuê", Value = "unAvailable" }
                };
            var userID = HttpContext.Session.GetString("ID");
            var type = "1";
            var ht_id = "-1";
            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(ht_id,userID ,type );
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

        [HttpPost]
        public IActionResult Add(Rooms model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }

            //var userID = HttpContext.Session.GetString("ID");
            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "Thêm mới thất bại!";
            try
            {
                //var role = model.role;
                DataSet ds = DataAccess.ROOMS_INSERT(model.HOMESTAYS_ID,model.ROOMS_NAME,model.ROOMS_PRICE,model.ROOMS_STATUS);
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
                return RedirectToAction("Index", "Rooms");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }




    }
}
