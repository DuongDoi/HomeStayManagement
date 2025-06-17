using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class HomeStayController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));
        private readonly IWebHostEnvironment _env;
        public HomeStayController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }

            logger.Info("New request income select HomeStay :");
            string _role = HttpContext.Session.GetString("Role");
            string users = "0";
            string ht_id = "0";
            string v_type = "0";

            if (_role == "admin")
            {
                users = "0";
                ht_id = "-1";
                v_type = "0";
            }
            else
            {
                if (_role == "owner")
                {
                    users = HttpContext.Session.GetString("ID");
                    ht_id = "-1";
                    v_type = "1";
                }
                else return RedirectToAction("Index", "Login");
            }

            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(ht_id, users, v_type);
            List<HomestaysObj> allHomestays = new List<HomestaysObj>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HomestaysObj _obj = new HomestaysObj
                {
                    id = dr["ID"].ToString(),
                    HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString(),
                    MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString(),
                    HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString(),
                    HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString(),
                    MANAGER_NAME = dr["MANAGER_NAME"].ToString(),
                    MANAGER_PHONE = dr["MANAGER_PHONE"].ToString(),
                    USERS_ID = dr["USERS_ID"].ToString(),
                    AVATAR_PATH = string.IsNullOrWhiteSpace(dr["AVATAR_PATH"].ToString())
                                  ? "no_image.png"
                                  : $"{dr["USERS_ID"]}/{dr["AVATAR_PATH"]}"
                };

                if (DateTime.TryParse(dr["CREATE_AT"].ToString(), out DateTime createAt))
                    _obj.CREATE_AT = createAt;

                if (DateTime.TryParse(dr["UPDATE_AT"].ToString(), out DateTime updateAt))
                    _obj.UPDATE_AT = updateAt;

                allHomestays.Add(_obj);
            }

            //  Phân trang dữ liệu
            int totalItems = allHomestays.Count;
            var pagedHomestays = allHomestays
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            logger.Info("Pro HOMESTAYS Select success.");
            return View(pagedHomestays);
        }



        [HttpGet]
        public IActionResult Filter(string search_value, int page = 1, int pageSize = 5)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }

            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                string userID = HttpContext.Session.GetString("ID");
                if (_role == "admin") userID = "-1";

                // Gọi procedure lấy danh sách
                DataSet ds = DataAccess.HomestayFilter(search_value, userID);
                List<HomestaysObj> homestays = new List<HomestaysObj>();

                if (ds.Tables[0].Rows.Count>0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        HomestaysObj _obj = new HomestaysObj
                        {
                            id = dr["ID"].ToString(),
                            HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString(),
                            MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString(),
                            HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString(),
                            HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString(),
                            MANAGER_NAME = dr["MANAGER_NAME"].ToString(),
                            MANAGER_PHONE = dr["MANAGER_PHONE"].ToString(),
                            USERS_ID = dr["USERS_ID"].ToString(),
                            AVATAR_PATH = string.IsNullOrWhiteSpace(dr["AVATAR_PATH"].ToString())
                                            ? "no_image.png"
                                            : $"{dr["USERS_ID"]}/{dr["AVATAR_PATH"]}"
                        };

                        if (DateTime.TryParse(dr["CREATE_AT"].ToString(), out DateTime created))
                            _obj.CREATE_AT = created;
                        if (DateTime.TryParse(dr["UPDATE_AT"].ToString(), out DateTime updated))
                            _obj.UPDATE_AT = updated;

                        homestays.Add(_obj);
                    }
                }
                else
                {
                    ViewBag.notfound = "Không tìm thấy kết quả phù hợp";
                }

                    // Phân trang
                    int totalItems = homestays.Count;
                var pagedList = homestays.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Truyền thông tin cho View
                ViewBag.SearchValue = search_value;
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                return View("Index", pagedList);
            }

            return RedirectToAction("Index", "Login");
        }


        public IActionResult Details(string id,string userId)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                string _userID = HttpContext.Session.GetString("ID");
                if(_userID != userId && _role != "admin") return RedirectToAction("Index", "HomeStay");
                else
                {
                    DataSet ds = DataAccess.HOMESTAYS_GET_LIST(id, userId, "1");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        HomestaysObj _obj = new HomestaysObj();
                        _obj.id = dr["ID"].ToString();
                        _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                        _obj.MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString();
                        _obj.HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString();
                        _obj.HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString();
                        _obj.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                        _obj.MANAGER_PHONE = dr["MANAGER_PHONE"].ToString();
                        _obj.USERS_ID = dr["USERS_ID"].ToString();


                        string _avatar_name = dr["AVATAR_PATH"].ToString();
                        if (!string.IsNullOrWhiteSpace(_avatar_name))
                        {
                            _obj.AVATAR_PATH = $"{_obj.USERS_ID}/{_avatar_name}";
                        }
                        else { _obj.AVATAR_PATH = "no_image.png"; }


                        try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                        catch { }
                        try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                        catch { }

                        return View(_obj);
                    }
                    return RedirectToAction("Index", "HomeStay");
                }
                    
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public IActionResult Edit(string id, string userId)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                string _userID = HttpContext.Session.GetString("ID");
                if (_userID != userId && _role != "admin") return RedirectToAction("Index", "HomeStay");
                else
                {
                    DataSet ds = DataAccess.HOMESTAYS_GET_LIST(id, userId, "1");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        HomestaysObj _obj = new HomestaysObj();
                        _obj.id = dr["ID"].ToString();
                        _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                        _obj.MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString();
                        _obj.HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString();
                        _obj.HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString();
                        _obj.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                        _obj.MANAGER_PHONE = dr["MANAGER_PHONE"].ToString();

                        _obj.USERS_ID = dr["USERS_ID"].ToString();
                        if (_obj.USERS_ID != userId && _role != "admin") return RedirectToAction("Index", "HomeStay");

                        string _avatar_name = dr["AVATAR_PATH"].ToString();
                        if (!string.IsNullOrWhiteSpace(_avatar_name))
                        {
                            _obj.AVATAR_PATH = $"{_obj.USERS_ID}/{_avatar_name}";
                        }
                        else { _obj.AVATAR_PATH = "no_image.png"; }


                        try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                        catch { }
                        try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                        catch { }

                        return View(_obj);
                    }
                    return RedirectToAction("Index", "HomeStay");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Edit(string id, HomestaysObj model, IFormFile avatarFile)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {

                if (!checkPIN(model.Save_code))
                {
                    ViewBag.Message = "Sai mã PIN";
                    return View(model);
                }

                string savedFileName = SaveImageToUploads(model.USERS_ID,avatarFile);
                string avatar_path;
                if (!string.IsNullOrEmpty(savedFileName)) { avatar_path = savedFileName; }
                else
                {
                    
                    DataSet ds1 = DataAccess.HOMESTAYS_GET_LIST(id, model.USERS_ID, "1");
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
                    DataSet ds = DataAccess.HOMESTAYS_UPDATE(id, model.HOMESTAYS_NAME, model.MANAGER_CARD_NUMBER, model.HOMESTAYS_ADDRESS, model.HOMESTAY_DESCRIPTION, model.MANAGER_NAME, model.MANAGER_PHONE,avatar_path, "1");
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
            else
            {
                return RedirectToAction("Index","Login");
            }
        }

        [HttpGet]
        public IActionResult Delete(string id, string userId)
        {

            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                string _userID = HttpContext.Session.GetString("ID");
                if (_userID != userId && _role != "admin") return RedirectToAction("Index", "HomeStay");
                else
                {
                    DataSet ds = DataAccess.HOMESTAYS_GET_LIST(id, userId, "1");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        HomestaysObj _obj = new HomestaysObj();
                        _obj.id = dr["ID"].ToString();
                        _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                        _obj.MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString();
                        _obj.HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString();
                        _obj.HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString();
                        _obj.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                        _obj.MANAGER_PHONE = dr["MANAGER_PHONE"].ToString();
                        _obj.USERS_ID = dr["USERS_ID"].ToString();
                        _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                        try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                        catch { }
                        try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                        catch { }

                        return View(_obj);
                    }
                    return RedirectToAction("Index", "HomeStay");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Delete(string id, HomestaysObj model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                if (!checkPIN(model.Save_code))
                {
                    ViewBag.Message = "Mã PIN không chính xác.";
                    return View(model);
                }

                var ds = DataAccess.HOMESTAYS_UPDATE(id, "", "", "", "", "", "", "","2");
                var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

                if (errCode == "0")
                {
                    TempData["Success"] = "Xóa cơ sở thành công.";
                    return RedirectToAction("Index");
                }

                ViewBag.Message = "Không thể xóa cơ sở.";
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
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
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Add(HomestaysObj model)
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
                    //var role = model.role;
                    DataSet ds = DataAccess.HOMESTAYS_INSERT(model.HOMESTAYS_NAME, model.MANAGER_CARD_NUMBER, model.HOMESTAYS_ADDRESS, model.HOMESTAY_DESCRIPTION, model.MANAGER_NAME, model.MANAGER_PHONE, userID);
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
                    TempData["Success"] = "Thêm mới cơ sở thành công.";
                    return RedirectToAction("Index", "HomeStay");
                }
                else
                {
                    ViewBag.Message = "Thêm mới thất bại!";
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        protected string SaveImageToUploads(string UserID,IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                
                // Đảm bảo thư mục tồn tại
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "AvatarHomestay", UserID);
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























        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult homestaysInsert([FromBody] dynamic sendData)
        {

            logger.Info("New request income Insert HomeStay :" + sendData.ToString());

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var homestayObj = JObject.Parse(sendData.ToString());
                string name = Convert.ToString(homestayObj["name"]);
                string card = Convert.ToString(homestayObj["card"]);
                string address = Convert.ToString(homestayObj["address"]);
                string description = Convert.ToString(homestayObj["description"]);
                string manager = Convert.ToString(homestayObj["manager"]);
                string phone = Convert.ToString(homestayObj["phone"]);
                string user_id = Convert.ToString(homestayObj["user_id"]);



                DataSet ds = DataAccess.HOMESTAYS_INSERT(name,card,address,description,manager,phone,user_id);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {

            }
            logger.Info("Pro HOMESTAYS INSERT success and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }


        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult homestaysUpdate([FromBody] dynamic sendData)
        {

            logger.Info("New request income homestay update :" + sendData.ToString());

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var homestayObj = JObject.Parse(sendData.ToString());
                string name = Convert.ToString(homestayObj["name"]);
                string card = Convert.ToString(homestayObj["card"]);
                string address = Convert.ToString(homestayObj["address"]);
                string description = Convert.ToString(homestayObj["description"]);
                string manager = Convert.ToString(homestayObj["manager"]);
                string phone = Convert.ToString(homestayObj["phone"]);
                string user_id = Convert.ToString(homestayObj["user_id"]);
                string type = Convert.ToString(homestayObj["type"]);
                if (string.IsNullOrEmpty(type)) type = "1"; //TYPE 1 UPDATE, TYPE 2 XOA


                DataSet ds = DataAccess.HOMESTAYS_UPDATE("1",name,card, address,description, manager,phone,"",type);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {
                logger.Error("Loi :" + ex.ToString());
            }
            logger.Info("Pro HOMESTAY UPDATE succ and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }



        //[Route("api/[controller]/[action]/data")]
        //[HttpPost]
        //public IActionResult homestaysGetList([FromBody] dynamic sendData)
        //{

        //    logger.Info("New request income homestay Get List :" + sendData.ToString());
        //    ResponseObjs _obj = new ResponseObjs();
        //    List<HomestaysObj> L = new List<HomestaysObj>();

        //    _obj.errCode = "-1";
        //    _obj.errMsgs = "unknow";
        //    try
        //    {
        //        var homestayObj = JObject.Parse(sendData.ToString());
        //        string name = Convert.ToString(homestayObj["name"]);
        //        string user_id = Convert.ToString(homestayObj["user_id"]);
        //        if (string.IsNullOrEmpty(name)) name = "-1";


        //        DataSet ds = DataAccess.HOMESTAYS_GET_LIST(name,user_id);

        //        _obj.errCode = "0";
        //        _obj.errMsgs = "Success";
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            string _id = dr["id"].ToString();
        //            string _name = dr["name"].ToString();
        //            string _card = dr["card"].ToString();
        //            string _address = dr["address"].ToString();
        //            string _description = dr["description"].ToString();
        //            string _manager = dr["manager"].ToString();
        //            string _phone = dr["phone"].ToString();
        //            string _user_id = dr["user_id"].ToString();
        //            HomestaysObj _objHT = new HomestaysObj();
        //            _objHT.id = _id;
        //            _objHT.name = _name;
        //            _objHT.card = _card;
        //            _objHT.address = _address;
        //            _objHT.description = _description;
        //            _objHT.manager = _manager;
        //            _objHT.phone = _phone;
        //            _objHT.user_id = _user_id;

        //            L.Add(_objHT);
        //        }
        //        _obj.data = L;


        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Loi :" + ex.ToString());
        //    }
        //    logger.Info("Pro HomeStay Get List succ and ressponse :" + _obj.ToString());
        //    return Ok(_obj);

        //}






    }
}
