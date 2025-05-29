using Microsoft.AspNetCore.Mvc;
using HomeStay_MVC.Models;
using ResfullApi.Models;

namespace HomeStay_MVC.Controllers
{
    public class AvatarController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        public AvatarController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");
            AvatarModel av = new AvatarModel();
            string _avatar_name = HttpContext.Session.GetString("AVATAR_PATH");
            if (!string.IsNullOrWhiteSpace(_avatar_name)) {
                var User = HttpContext.Session.GetString("User");
                av.Avatar_name=$"{User}/{_avatar_name}";
            }
            else { av.Avatar_name = "no_image.png"; }
                
            return View(av);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile avatarFile)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            string savedFileName = SaveImageToUploads(avatarFile);
            string errCode = "-1";
            string errMsg = "Không thể cập nhật ảnh đại diện";
            if (!string.IsNullOrEmpty(savedFileName))
            {
                var username = HttpContext.Session.GetString("User");
                var ds = DataAccess.USERS_UPDATE_AVATAR(username, savedFileName);
                errCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                errMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();
                if (errCode == "0")
                {
                    var User = HttpContext.Session.GetString("User");
                    HttpContext.Session.SetString("AVATAR_PATH", savedFileName ?? "");
                    AvatarModel av = new AvatarModel();
                    av.Avatar_name = $"{User}/{savedFileName}";
                    ViewBag.Message = errMsg;
                    return View(av);
                }
                else
                {
                    AvatarModel av = new AvatarModel();
                    string _avatar_name1 = HttpContext.Session.GetString("AVATAR_PATH");
                    if (!string.IsNullOrWhiteSpace(_avatar_name1)) av.Avatar_name = _avatar_name1;
                    else
                        av.Avatar_name = "no_image.png";
                    ViewBag.Message = errMsg;
                    return View(av);
                }
            }
            AvatarModel av1 = new AvatarModel();
            string _avatar_name = HttpContext.Session.GetString("AVATAR_PATH");
            if (!string.IsNullOrWhiteSpace(_avatar_name)) av1.Avatar_name = _avatar_name;
            else
                av1.Avatar_name = "no_image.png";
            ViewBag.Message = errMsg;
            return View(av1);


        }
        protected string SaveImageToUploads(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                var User = HttpContext.Session.GetString("User");
                // Đảm bảo thư mục tồn tại
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads","AvatarUser",User);
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
