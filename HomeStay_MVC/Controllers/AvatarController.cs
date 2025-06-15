using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
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
            if (!string.IsNullOrWhiteSpace(_avatar_name))
            {
                var User = HttpContext.Session.GetString("User");
                av.Avatar_name = $"{User}/{_avatar_name}";
            }
            else { av.Avatar_name = "no_image.png"; }

            return View(av);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile avatarFile)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            string errorMessage;
            string savedFileName = SaveImageToUploads(avatarFile, out errorMessage);

            AvatarModel av = new AvatarModel();
            string user = HttpContext.Session.GetString("User");
            string currentAvatar = HttpContext.Session.GetString("AVATAR_PATH");
            if (!string.IsNullOrEmpty(savedFileName))
            {
                var ds = DataAccess.USERS_UPDATE_AVATAR(user, savedFileName);
                var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                var errMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                if (errCode == "0")
                {
                    HttpContext.Session.SetString("AVATAR_PATH", savedFileName);
                    av.Avatar_name = $"{user}/{savedFileName}";
                    ViewBag.Message = "Cập nhật ảnh thành công!";
                }
                else
                {
                    av.Avatar_name = string.IsNullOrEmpty(currentAvatar) ? "no_image.png" : $"{user}/{currentAvatar}";
                    ViewBag.Message = errMsg;
                }
            }
            else
            {
                av.Avatar_name = string.IsNullOrEmpty(currentAvatar) ? "no_image.png" : $"{user}/{currentAvatar}";
                ViewBag.Message = errorMessage ?? "Không thể cập nhật ảnh đại diện.";
            }

            return View(av);
        }

        protected string SaveImageToUploads(IFormFile file, out string errorMessage)
        {
            errorMessage = null;

            if (file == null || file.Length == 0)
            {
                errorMessage = "Vui lòng chọn một file hợp lệ.";
                return null;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                errorMessage = "Chỉ cho phép định dạng ảnh: .jpg, .jpeg, .png";
                return null;
            }

            try
            {
                var user = HttpContext.Session.GetString("User");
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "AvatarUser", user);
                Directory.CreateDirectory(uploadsFolder);

                // Tên file cố định theo user (hoặc ID người dùng nếu có)
                string fileName = "avatar" + fileExtension;
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Ghi đè nếu đã tồn tại
                using (var stream = new FileStream(filePath, FileMode.Create)) // FileMode.Create sẽ tự động ghi đè
                {
                    file.CopyTo(stream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                errorMessage = "Có lỗi xảy ra khi lưu file.";
                Console.WriteLine("Error saving image: " + ex.Message);
                return null;
            }
        }

    }
}
