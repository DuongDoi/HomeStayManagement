using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class ReportController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BillsController));
        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string user_id = "-1";
            string report_id = "-1";
            string ht_id = "-1";
            string report_type = "-1";
            string v_type = "1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {

            }
            else
            {
                if (_role == "owner")
                {
                    user_id = HttpContext.Session.GetString("ID");
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                        report_type = "Chi";
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }

            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id, v_type);
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

            DataSet ds = DataAccess.REPORT_GET_LIST(report_id, user_id, ht_id, "-1", "-1", "-1", "1");
            List<ReportModel> reports = new List<ReportModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ReportModel _obj = new ReportModel();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString();
                _obj.AMOUNT = int.TryParse(dr["AMOUNT"]?.ToString(), out int value) ? value : 0;
                _obj.BILLS_ID = dr["BILLS_ID"].ToString();
                _obj.CATEGORY = dr["CATEGORY"].ToString();
                _obj.DESCRIPT = dr["DESCRIPT"].ToString();
                _obj.TYPE = dr["TYPE"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();

                _obj.UPDATE_BY_USERS_NAME = dr["UPDATE_BY_USERS_NAME"].ToString();
                _obj.UPDATE_BY = dr["UPDATE_BY"].ToString();
                _obj.CREATE_BY_USERS_NAME = dr["CREATE_BY_USERS_NAME"].ToString();
                _obj.CREATE_BY = dr["CREATE_BY"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                reports.Add(_obj);

            }
            return View(reports);
        }

        [HttpGet]
        public IActionResult Filter(int? HomestayId, DateTime? start_date, DateTime? end_date, int? TYPE_VALUE)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string user_id = "-1";
            string report_id = "-1";
            string ht_id = "-1";
            string report_type = "-1";
            string v_type = "1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {

            }
            else
            {
                if (_role == "owner")
                {
                    user_id = HttpContext.Session.GetString("ID");
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                        report_type = "Chi";
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }

            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id, v_type);
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
            string _start_date = start_date.HasValue ? start_date.Value.ToString("yyyy-MM-dd") : "";
            string _end_date = end_date.HasValue ? end_date.Value.ToString("yyyy-MM-dd") : "";
            string _report_type = TYPE_VALUE.HasValue ? (TYPE_VALUE.Value != 2 ? "Thu" : "Chi") : "-1";
            if (_role == "manager") _id_homestay = ht_id;

            DataSet ds = DataAccess.REPORT_GET_LIST(report_id, user_id, _id_homestay, _start_date, _end_date, _report_type, "1");
            List<ReportModel> reports = new List<ReportModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ReportModel _obj = new ReportModel();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString();
                _obj.AMOUNT = int.TryParse(dr["AMOUNT"]?.ToString(), out int value) ? value : 0;
                _obj.BILLS_ID = dr["BILLS_ID"].ToString();
                _obj.CATEGORY = dr["CATEGORY"].ToString();
                _obj.DESCRIPT = dr["DESCRIPT"].ToString();
                _obj.TYPE = dr["TYPE"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();

                _obj.UPDATE_BY_USERS_NAME = dr["UPDATE_BY_USERS_NAME"].ToString();
                _obj.UPDATE_BY = dr["UPDATE_BY"].ToString();
                _obj.CREATE_BY_USERS_NAME = dr["CREATE_BY_USERS_NAME"].ToString();
                _obj.CREATE_BY = dr["CREATE_BY"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                reports.Add(_obj);

            }
            ViewBag.id_homestay = _id_homestay;
            ViewBag.start_date = _start_date;
            ViewBag.end_date = _end_date;
            ViewBag.report_type = _report_type;
            logger.Info("Report filtering completed successfully.");
            return View("Index", reports);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string user_id = "-1";
            string report_id = "-1";
            string ht_id = "-1";
            string report_type = "-1";
            string v_type = "1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {

            }
            else
            {
                if (_role == "owner")
                {
                    user_id = HttpContext.Session.GetString("ID");
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                        report_type = "Chi";
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }
            DataSet ds = DataAccess.REPORT_GET_LIST(id, user_id, ht_id, "-1", "-1", "-1", "1");
            ReportModel reports = new ReportModel();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ReportModel _obj = new ReportModel();
                reports.ID = dr["ID"].ToString();
                reports.HOMESTAYS_ID = dr["HOMESTAYS_ID"].ToString();
                reports.AMOUNT = int.TryParse(dr["AMOUNT"]?.ToString(), out int value) ? value : 0;
                reports.BILLS_ID = dr["BILLS_ID"].ToString();
                reports.CATEGORY = dr["CATEGORY"].ToString();
                reports.DESCRIPT = dr["DESCRIPT"].ToString();
                reports.TYPE = dr["TYPE"].ToString();
                reports.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();

                reports.UPDATE_BY_USERS_NAME = dr["UPDATE_BY_USERS_NAME"].ToString();
                reports.UPDATE_BY = dr["UPDATE_BY"].ToString();
                reports.CREATE_BY_USERS_NAME = dr["CREATE_BY_USERS_NAME"].ToString();
                reports.CREATE_BY = dr["CREATE_BY"].ToString();
                try { reports.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { reports.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }
            }
            else
            {
                return RedirectToAction("Index");
            }


            return View(reports);
        }


        [HttpGet]
        public IActionResult Add()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string user_id = "-1";
            string ht_id = "-1";
            string v_type = "1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {

            }
            else
            {
                if (_role == "owner")
                {
                    user_id = HttpContext.Session.GetString("ID");
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }

            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id, v_type);
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

            ReportModel model = new ReportModel();
            model.TypeOptions = new List<SelectListItem>{
                    new SelectListItem { Text = "Thu", Value = "Thu" },
                    new SelectListItem { Text = "Chi", Value = "Chi" }
                };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ReportModel model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }

            var userID = HttpContext.Session.GetString("ID");
            string _role = HttpContext.Session.GetString("Role");
            string ht_id = "-1";
            string user_id = "-1";
            string v_type = "1";

            // Lấy user_id và homestay_id tương ứng theo vai trò
            if (_role == "admin")
            {
                // Nếu cần, admin có thể xem toàn bộ danh sách
                user_id = "0";
                ht_id = "0";
            }
            else if (_role == "owner")
            {
                user_id = userID;
            }
            else
            {
                var createdBy = HttpContext.Session.GetString("Create_By");
                var dsUser = DataAccess.USERS_GET_LIST(createdBy);
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    var dr = dsUser.Tables[0].Rows[0];
                    user_id = dr["ID"].ToString();
                    ht_id = HttpContext.Session.GetString("Homestays_Id");
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }

            // Load danh sách cơ sở homestay
            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id, v_type);
            var homestayList = new List<SelectListItem>();
            foreach (DataRow dr in dsHomestays.Tables[0].Rows)
            {
                homestayList.Add(new SelectListItem
                {
                    Value = dr["ID"].ToString(),
                    Text = dr["HOMESTAYS_NAME"].ToString()
                });
            }
            ViewBag.HomestayList = homestayList;

            // Gán lại TypeOptions để tránh mất lựa chọn trong View
            model.TypeOptions = new List<SelectListItem>
    {
        new SelectListItem { Text = "Thu", Value = "Thu" },
        new SelectListItem { Text = "Chi", Value = "Chi" }
    };

            // Gọi thủ tục thêm mới
            ResponseObjs _obj = new ResponseObjs { errCode = "-1", errMsgs = "Thêm mới thất bại!" };
            try
            {
                var ds = DataAccess.REPORT_INSERT(
                    model.TYPE,
                    model.CATEGORY,
                    model.DESCRIPT,
                    "-1",                   
                    userID,
                    model.HOMESTAYS_ID,
                    model.AMOUNT.ToString(),
                    "1"                     
                );

                _obj.errCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                _obj.errMsgs = ds.Tables[0].Rows[0]["errMsg"].ToString();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Message = $"Đã xảy ra lỗi khi gửi yêu cầu: {ex.Message}";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Lỗi không xác định: {ex.Message}";
                return View(model);
            }

            // Kiểm tra kết quả thêm
            if (_obj.errCode == "0")
            {
                TempData["Success"] = "Thêm mới thành công.";
                return RedirectToAction("Index", "Report");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }




    }
}
