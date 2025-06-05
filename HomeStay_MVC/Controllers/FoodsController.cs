using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class FoodsController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));
        private readonly IWebHostEnvironment _env;
        public FoodsController(IWebHostEnvironment env)
        {
            _env = env;
        }


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
            logger.Info("New request income select All Food :");
                DataSet ds = DataAccess.FOODS_GET_LIST(id, user_id, v_type);
                List<FOODS> foods = new List<FOODS>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FOODS _obj = new FOODS();
                    _obj.ID = dr["ID"].ToString();
                    _obj.FOODS_NAME = dr["FOODS_NAME"].ToString();
                    _obj.FOODS_PRICE = dr["FOODS_PRICE"].ToString();
                    var food_type = dr["FOODS_TYPE"].ToString();
                    if (food_type == "FOOD")
                        _obj.FOODS_TYPE = "Đồ ăn";
                    else
                        _obj.FOODS_TYPE = "Đồ uống";

                    _obj.USERS_ID = dr["USERS_ID"].ToString();
                    string _avatar_name = dr["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _obj.AVATAR_PATH = $"{_obj.USERS_ID}/{_avatar_name}";
                    }
                    else { _obj.AVATAR_PATH = "no_image.png"; }

                _obj.USERS = dr["NAME"].ToString();
                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    foods.Add(_obj);

                }

                logger.Info("Pro Food Select All success.");
                return View(foods);
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
                DataSet ds = DataAccess.FOODS_GET_LIST(id, "-1", "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    FOODS _obj = new FOODS();
                    _obj.ID = dr["ID"].ToString();
                    _obj.FOODS_NAME = dr["FOODS_NAME"].ToString();
                    _obj.FOODS_PRICE = dr["FOODS_PRICE"].ToString();
                    var food_type = dr["FOODS_TYPE"].ToString();
                    if (food_type == "FOOD")
                        _obj.FOODS_TYPE = "Đồ ăn";
                    else
                        _obj.FOODS_TYPE = "Đồ uống";

                    _obj.USERS_ID = dr["USERS_ID"].ToString();
                    string _avatar_name = dr["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _obj.AVATAR_PATH = $"{_obj.USERS_ID}/{_avatar_name}";
                    }
                    else { _obj.AVATAR_PATH = "no_image.png"; }


                    _obj.USERS = dr["NAME"].ToString();
                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    return View(_obj);
                }
                return RedirectToAction("Index", "Foods");
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

                var userID = "";
                if (_role == "admin") userID = "-1";
                else
                    userID = HttpContext.Session.GetString("ID");
                DataSet ds = DataAccess.FOODS_GET_LIST(id, userID, "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    FOODS _obj = new FOODS();
                    _obj.ID = dr["ID"].ToString();
                    _obj.FOODS_NAME = dr["FOODS_NAME"].ToString();
                    _obj.FOODS_PRICE = dr["FOODS_PRICE"].ToString();
                    var food_type = dr["FOODS_TYPE"].ToString();
                    if (food_type == "FOOD")
                        _obj.FOODS_TYPE = "Đồ ăn";
                    else
                        _obj.FOODS_TYPE = "Đồ uống";

                    _obj.TypeOptions = new List<SelectListItem>{
                    new SelectListItem { Text = "Đồ ăn", Value = "FOOD" },
                    new SelectListItem { Text = "Đồ uống", Value = "DRINK" }
                    };

                    _obj.USERS_ID = dr["USERS_ID"].ToString();
                    if (_obj.USERS_ID != userID && _role != "admin") return RedirectToAction("Index", "Foods");

                    string _avatar_name = dr["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _obj.AVATAR_PATH = $"{_obj.USERS_ID}/{_avatar_name}";
                    }
                    else { _obj.AVATAR_PATH = "no_image.png"; }


                    _obj.USERS = dr["NAME"].ToString();
                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    return View(_obj);
                }
                return RedirectToAction("Index", "Foods");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Edit(string id, FOODS model, IFormFile avatarFile)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!checkPIN(model.Save_code))
            {
                var ds = DataAccess.FOODS_GET_LIST(id, "-1", "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    string _avatar_name = dr["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {
                        model.AVATAR_PATH = $"{dr["USERS_ID"]}/{_avatar_name}";
                    }
                    else
                    {
                        model.AVATAR_PATH = "no_image.png";
                    }
                }
                model.TypeOptions = new List<SelectListItem>{
                    new SelectListItem { Text = "Đồ ăn", Value = "FOOD" },
                    new SelectListItem { Text = "Đồ uống", Value = "DRINK" }
                    };
                ViewBag.Message = "Sai mã PIN";
                return View(model);
            }

            string savedFileName = SaveImageToUploads(model.USERS_ID, avatarFile);
            string avatar_path;
            if (!string.IsNullOrEmpty(savedFileName)) { avatar_path = savedFileName; }
            else
            {
                
                DataSet ds1 = DataAccess.FOODS_GET_LIST(id, "-1", "1");
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
                DataSet ds = DataAccess.FOODS_UPDATE(id,model.FOODS_NAME, model.FOODS_PRICE,model.FOODS_TYPE, model.USERS_ID,avatar_path, "1");
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
                model.TypeOptions = new List<SelectListItem>{
                    new SelectListItem { Text = "Đồ ăn", Value = "FOOD" },
                    new SelectListItem { Text = "Đồ uống", Value = "DRINK" }
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
                var obj = new FOODS();
                obj.ID = id;
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Delete(string id, FOODS model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            if (!checkPIN(model.Save_code))
            {
                ViewBag.Message = "Mã PIN không chính xác.";
                return View(model);
            }
            string _role = HttpContext.Session.GetString("Role");
            var userID = "";
            if (_role == "admin") userID = "-1";
            else
                userID = HttpContext.Session.GetString("ID");
            DataSet ds = DataAccess.FOODS_UPDATE(id, "", "","", userID,"", "2"); ;
            var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

            if (errCode == "0")
            {
                TempData["Success"] = "Xóa món thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Không thể xóa món.";
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
                FOODS model = new FOODS();
                model.TypeOptions = new List<SelectListItem>{
                    new SelectListItem { Text = "Đồ ăn", Value = "FOOD" },
                    new SelectListItem { Text = "Đồ uống", Value = "DRINK" }
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Add(FOODS model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }

            var userID = HttpContext.Session.GetString("ID");
            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "Thêm mới thất bại!";
            try
            {
                DataSet ds = DataAccess.FOODS_INSERT(model.FOODS_NAME, model.FOODS_PRICE,model.FOODS_TYPE, userID);
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
                return RedirectToAction("Index", "Foods");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }





        protected string SaveImageToUploads(string userID,IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                // Đảm bảo thư mục tồn tại
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "AvatarFoodDrink", userID);
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
