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
        private readonly IWebHostEnvironment _env;
        public RoomsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            logger.Info("New request income select All Rooms :");
            string room_id = "-1";
            string ht_id = "-1";
            string userID = "-1";
            string v_type = "1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {
                room_id = "-1";
                ht_id = "-1";
                userID = "-1";
                v_type = "1";

            }
            else
            {
                if (_role == "owner")
                {
                    userID = HttpContext.Session.GetString("ID");

                    ht_id = "-1";
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        userID = dr1["ID"].ToString();
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }
            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, userID, "1");
            List<SelectListItem> homestayList = new List<SelectListItem>();
            foreach (DataRow dr in dsHomestays.Tables[0].Rows)
            {
                homestayList.Add(new SelectListItem
                {
                    Value = dr["ID"].ToString(),
                    Text = dr["HOMESTAYS_NAME"].ToString()
                });
            }
            ViewBag.HomestayList = homestayList;



            DataSet ds = DataAccess.ROOMS_GET_LIST(ht_id, room_id,userID, v_type);
            List<Rooms> rooms = new List<Rooms>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Rooms _obj = new Rooms();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();

                var _status = dr["ROOMS_STATUS"].ToString();
                if (_status == "Available") _obj.ROOMS_STATUS = "Hoạt động";
                else _obj.ROOMS_STATUS = "Đang sửa chữa";

                var _type = dr["TYPE"].ToString();
                if (_type == "1") _obj.TYPE = "Phòng đơn";
                else
                {
                    if (_type == "2") _obj.TYPE = "Phòng đôi";
                    else _obj.TYPE = "Phòng nhiều người";
                }
                _obj.NUMBER_BED = dr["NUMBER_BED"].ToString();
                _obj.SQUARE = dr["SQUARE"].ToString();

                _obj.HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString();
                string _avatar_name = dr["AVATAR_PATH"].ToString();
                if (!string.IsNullOrWhiteSpace(_avatar_name))
                {

                    _obj.AVATAR_PATH = $"{_obj.HOMESTAYS_ID}/{_avatar_name}";
                }
                else { _obj.AVATAR_PATH = "no_image.png"; }


                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                rooms.Add(_obj);

            }

            logger.Info("Pro Rooms Select All success.");
            return View(rooms);
        }


        [HttpGet]
        public IActionResult Filter(int? HomestayId,DateTime? checkin_date, DateTime? checkout_date, int? TYPE_VALUE, int? STATUS_VALUE)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }

            logger.Info("Filtering rooms by check-in, check-out and room type.");

            string _role = HttpContext.Session.GetString("Role");
            string userID = "-1";
            string ht_id = "-1";
            string room_id = "-1";
            string v_type = "1";

            if (_role == "admin")
            {
                userID = "-1";
                ht_id = "-1";
            }
            else if (_role == "owner")
            {
                userID = HttpContext.Session.GetString("ID");
            }
            else
            {
                DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr1 = ds1.Tables[0].Rows[0];
                    userID = dr1["ID"].ToString();
                    ht_id = HttpContext.Session.GetString("Homestays_Id");
                }
                else return RedirectToAction("Index", "Login");
            }
            if (checkout_date < checkin_date) return RedirectToAction("Index");
            // Lấy danh sách homestay để hiển thị lại trong view
            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, userID, "1");
            List<SelectListItem> homestayList = new List<SelectListItem>();
            foreach (DataRow dr in dsHomestays.Tables[0].Rows)
            {
                homestayList.Add(new SelectListItem
                {
                    Value = dr["ID"].ToString(),
                    Text = dr["HOMESTAYS_NAME"].ToString()
                });
            }
            ViewBag.HomestayList = homestayList;

            // Format tham số lọc
            string _id_homestay = (HomestayId.HasValue && HomestayId.Value != 0) ? HomestayId.Value.ToString() : "-1";
            string _checkin = checkin_date.HasValue ? checkin_date.Value.ToString("yyyy-MM-dd") : "";
            string _checkout = checkout_date.HasValue ? checkout_date.Value.ToString("yyyy-MM-dd") : "";
            string _room_type = (TYPE_VALUE.HasValue && TYPE_VALUE.Value != 0) ? TYPE_VALUE.Value.ToString() : "-1";
            string _status_value = STATUS_VALUE.HasValue ? STATUS_VALUE.Value.ToString() : "";
            if (_role == "manager") _id_homestay = ht_id;
            
            DataSet ds = DataAccess.ROOMS_FILTER_LIST(_id_homestay, _checkin, _checkout, _room_type,_status_value);

            List<Rooms> rooms = new List<Rooms>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Rooms _obj = new Rooms
                {
                    ID = dr["ID"].ToString(),
                    HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString(),
                    ROOMS_NAME = dr["ROOMS_NAME"].ToString(),
                    ROOMS_PRICE = dr["ROOMS_PRICE"].ToString(),
                    NUMBER_BED = dr["NUMBER_BED"].ToString(),
                    SQUARE = dr["SQUARE"].ToString(),
                    HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString()
                };

                // Avatar
                string _avatar_name = dr["AVATAR_PATH"].ToString();
                _obj.AVATAR_PATH = !string.IsNullOrWhiteSpace(_avatar_name)
                    ? $"{_obj.HOMESTAYS_ID}/{_avatar_name}"
                    : "no_image.png";

                // Status
                string _status = dr["ROOMS_STATUS"].ToString();
                _obj.ROOMS_STATUS = (_status == "Available") ? "Hoạt động" : "Đang sửa chữa";

                // Type
                string _type = dr["TYPE"].ToString();
                _obj.TYPE = _type switch
                {
                    "1" => "Phòng đơn",
                    "2" => "Phòng đôi",
                    _ => "Phòng nhiều người"
                };

                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); } catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); } catch { }

                rooms.Add(_obj);
            }

            logger.Info("Room filtering completed successfully.");
            return View("Index", rooms);
        }

        [HttpGet]
        public IActionResult GetRoomsByHomestay(string homestayId)
        {


            DataSet dsr = DataAccess.ROOMS_GET_LIST( homestayId, "-1", "-1", "1");

            var rooms = new List<object>();
            foreach (DataRow dr in dsr.Tables[0].Rows)
            {
                Rooms _obj = new Rooms();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();

                var _status = dr["ROOMS_STATUS"].ToString();
                if (_status == "Available") _obj.ROOMS_STATUS = "Hoạt động";
                else _obj.ROOMS_STATUS = "Đang sửa chữa";


                _obj.HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString();
                string _avatar_name = dr["AVATAR_PATH"].ToString();
                if (!string.IsNullOrWhiteSpace(_avatar_name))
                {

                    _obj.AVATAR_PATH = $"{_obj.HOMESTAYS_ID}/{_avatar_name}";
                }
                else { _obj.AVATAR_PATH = "no_image.png"; }


                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }
                rooms.Add(_obj);
            }

            return Json(rooms);
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
                DataSet ds = DataAccess.ROOMS_GET_LIST("-1", id, "-1", "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Rooms _obj = new Rooms();
                    _obj.ID = dr["ID"].ToString();
                    _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                    _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                    _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();
                    
                    _obj.USERS_ID = dr["USERS_ID"].ToString();
                    var _user_id = HttpContext.Session.GetString("ID");
                    if (_user_id != _obj.USERS_ID && _role != "admin") return RedirectToAction("Index", "Rooms");


                    var _status = dr["ROOMS_STATUS"].ToString();
                    if (_status == "Available") _obj.ROOMS_STATUS = "Hoạt động";
                    else _obj.ROOMS_STATUS = "Đang sửa chữa";
                    
                    
                    var _type = dr["TYPE"].ToString();
                    if (_type == "1") _obj.TYPE = "Phòng đơn";
                    else
                    {
                        if(_type == "2") _obj.TYPE = "Phòng đôi";
                        else _obj.TYPE = "Phòng nhiều người";
                    }

                    _obj.NUMBER_BED = dr["NUMBER_BED"].ToString();
                    _obj.SQUARE = dr["SQUARE"].ToString();

                    _obj.HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString();
                    string _avatar_name = dr["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _obj.AVATAR_PATH = $"{_obj.HOMESTAYS_ID}/{_avatar_name}";
                    }
                    else { _obj.AVATAR_PATH = "no_image.png"; }


                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    return View(_obj);
                }
                return RedirectToAction("Index", "Rooms");
            }
            else return RedirectToAction("Index", "Login");
            
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
                DataSet ds = DataAccess.ROOMS_GET_LIST("-1", id, "-1", "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Rooms _obj = new Rooms();
                    _obj.ID = dr["ID"].ToString();
                    _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                    _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                    _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();

                    _obj.USERS_ID = dr["USERS_ID"].ToString();
                    var _user_id = HttpContext.Session.GetString("ID");
                    if (_user_id != _obj.USERS_ID && _role != "admin") return RedirectToAction("Index", "Rooms");

                    

                    _obj.HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString();
                    string _avatar_name = dr["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _obj.AVATAR_PATH = $"{_obj.HOMESTAYS_ID}/{_avatar_name}";
                    }
                    else { _obj.AVATAR_PATH = "no_image.png"; }

                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    var userID = HttpContext.Session.GetString("ID");
                    var type = "1";
                    var ht_id = "-1";
                    DataSet ds1 = DataAccess.HOMESTAYS_GET_LIST(ht_id, userID, type);
                    _obj.HomeStayOptions = new List<SelectListItem>();
                    foreach (DataRow dr1 in ds1.Tables[0].Rows)
                    {
                        var _ht_id = dr1["ID"].ToString();
                        var _ht_name = dr1["HOMESTAYS_NAME"].ToString();
                        _obj.HomeStayOptions.Add(new SelectListItem
                        {
                            Text = _ht_name,
                            Value = _ht_id,
                            Selected = _ht_id == _obj.HOMESTAYS_ID
                        });
                    }

                    _obj.ROOMS_STATUS = dr["ROOMS_STATUS"].ToString();
                    _obj.StatusOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Hoạt động", Value = "Available",Selected = _obj.ROOMS_STATUS == "Available" },
                        new SelectListItem { Text = "Đang sửa chữa", Value = "unAvailable",Selected = _obj.ROOMS_STATUS == "unAvailable" }
                    };
                    

                    _obj.TYPE = dr["TYPE"].ToString();
                    _obj.TypeOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Phòng đơn", Value = "1" ,Selected = _obj.TYPE == "1"},
                        new SelectListItem { Text = "Phòng đôi", Value = "2",Selected = _obj.TYPE == "2" },
                        new SelectListItem { Text = "Phòng nhiều người", Value = "3" , Selected = _obj.TYPE == "3"}
                    };

                    _obj.NUMBER_BED = dr["NUMBER_BED"].ToString();
                    _obj.SQUARE = dr["SQUARE"].ToString();

                    return View(_obj);
                }
                return RedirectToAction("Index", "Rooms");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpPost]
        public IActionResult Edit(string id, Rooms model,IFormFile avatarFile)
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

            string savedFileName = SaveImageToUploads(avatarFile,model.HOMESTAYS_ID);
            string avatar_path;
            if (!string.IsNullOrEmpty(savedFileName)) { avatar_path = savedFileName; }
            else
            {
                string _userID = HttpContext.Session.GetString("ID");
                DataSet ds1 = DataAccess.ROOMS_GET_LIST("-1",id, _userID, "1");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr1 = ds1.Tables[0].Rows[0];
                    avatar_path = dr1["AVATAR_PATH"].ToString(); ;
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.ROOMS_UPDATE(id, model.ROOMS_NAME, model.ROOMS_PRICE, model.ROOMS_STATUS, avatar_path,model.TYPE,model.NUMBER_BED,model.SQUARE,model.HOMESTAYS_ID, "1");
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
                model.TypeOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Phòng đơn", Value = "1" ,Selected = model.TYPE == "1"},
                        new SelectListItem { Text = "Phòng đôi", Value = "2",Selected = model.TYPE == "2" },
                        new SelectListItem { Text = "Phòng nhiều người", Value = "3" , Selected = model.TYPE == "3"}
                    };
                model.StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Hoạt động", Value = "Available",Selected = model.ROOMS_STATUS == "Available" },
                    new SelectListItem { Text = "Đang sửa chữa", Value = "unAvailable",Selected = model.ROOMS_STATUS == "unAvailable" }
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
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                DataSet ds = DataAccess.ROOMS_GET_LIST("-1", id, "-1", "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Rooms _obj = new Rooms();
                    _obj.ID = id;
                    _obj.USERS_ID = dr["USERS_ID"].ToString();

                    var _user_id = HttpContext.Session.GetString("ID");
                    if (_user_id != _obj.USERS_ID && _role!="admin") return RedirectToAction("Index", "Rooms");
                    else
                        return View(_obj);
                }
                return RedirectToAction("Index", "Rooms");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
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

            DataSet ds = DataAccess.ROOMS_UPDATE(id,"","","","","","", "", "", "2"); ;
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
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                Rooms model = new Rooms();
                model.TypeOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Phòng đơn", Value = "1"},
                        new SelectListItem { Text = "Phòng đôi", Value = "2"},
                        new SelectListItem { Text = "Phòng nhiều người", Value = "3"}
                    };
                model.StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Hoạt động", Value = "Available"},
                    new SelectListItem { Text = "Đang sửa chữa", Value = "unAvailable" }
                };
                var userID = HttpContext.Session.GetString("ID");
                var type = "1";
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
                DataSet ds = DataAccess.ROOMS_INSERT(model.HOMESTAYS_ID,model.ROOMS_NAME,model.ROOMS_PRICE,model.ROOMS_STATUS,model.TYPE,model.NUMBER_BED,model.SQUARE);
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
                var userID = HttpContext.Session.GetString("ID");
                var type = "1";
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
                        Value = _ht_id,
                        Selected = _ht_id == model.HOMESTAYS_ID
                    });
                }
                model.TypeOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Phòng đơn", Value = "1" ,Selected = model.TYPE == "1"},
                        new SelectListItem { Text = "Phòng đôi", Value = "2",Selected = model.TYPE == "2" },
                        new SelectListItem { Text = "Phòng nhiều người", Value = "3" , Selected = model.TYPE == "3"}
                    };
                model.StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Hoạt động", Value = "Available",Selected = model.ROOMS_STATUS == "Available" },
                    new SelectListItem { Text = "Đang sửa chữa", Value = "unAvailable",Selected = model.ROOMS_STATUS == "unAvailable" }
                };
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }


        protected string SaveImageToUploads(IFormFile file,string ht_id)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                
                // Đảm bảo thư mục tồn tại
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "AvatarRoom", ht_id);
                Directory.CreateDirectory(uploadsFolder);

                // Tạo tên file duy nhất
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Đường dẫn tuyệt đối
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Trả về tên file (để lưu vào DB hoặc hiển thị lại)
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving image: " + ex.Message);
                return null;
            }
        }

    }
}
