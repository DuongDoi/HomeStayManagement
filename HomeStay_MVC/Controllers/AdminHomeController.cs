using Microsoft.AspNetCore.Mvc;
using HomeStay_MVC.Controllers;
using HomeStay_MVC.ModelCommon;
using ResfullApi.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeStay_MVC.Models;
using System.Security.Cryptography;

namespace HomeStay_MVC.Controllers
{
    public class AdminHomeController : BaseController
    {
        string root = Common.GetValuesAppSetting("webConfig", "root");
        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            gettop();
            string userId = "-1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "owner") userId = HttpContext.Session.GetString("ID");
            DataSet datareport = DataAccess.GET_DATA_REPORT(userId,"3");
            List<DataReportModel> listDataReportModel = new List<DataReportModel>();
            if (datareport != null && datareport.Tables.Count > 0)
            {
                foreach (DataRow row in datareport.Tables[0].Rows)
                {
                    DataReportModel _obj = new DataReportModel();
                    _obj.Date_value = row["Date_value"].ToString();
                    _obj.Total_in = Convert.ToInt32(row["Total_in"]);
                    _obj.Total_out = Convert.ToInt32(row["Total_out"]);
                    listDataReportModel.Add(_obj);
                }

            }
            ViewBag.listDataReportModel = listDataReportModel;
            ViewBag.TypeSelect = "3";
            return View();
        }


        [HttpGet]
        public IActionResult Filter(string TypeSelect)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            gettop();

            string userId = "-1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "owner") userId = HttpContext.Session.GetString("ID");
            DataSet datareport = null;

            switch (TypeSelect)
            {
                case "1": // 6 tháng
                    datareport = DataAccess.GET_DATA_REPORT(userId,"1");
                    break;
                case "2": // 6 tuần
                    datareport = DataAccess.GET_DATA_REPORT(userId, "2");
                    break;
                case "3": // 6 ngày
                    datareport = DataAccess.GET_DATA_REPORT(userId, "3");
                    break;
                default:
                    datareport = DataAccess.GET_DATA_REPORT(userId, "1"); 
                    break;
            }

            List<DataReportModel> listDataReportModel = new List<DataReportModel>();
            if (datareport != null && datareport.Tables.Count > 0)
            {
                foreach (DataRow row in datareport.Tables[0].Rows)
                {
                    listDataReportModel.Add(new DataReportModel
                    {
                        Date_value = row["Date_value"].ToString(),
                        Total_in = Convert.ToInt32(row["Total_in"]),
                        Total_out = Convert.ToInt32(row["Total_out"]),
                    });
                }
            }
            ViewBag.TypeSelect = TypeSelect;
            ViewBag.listDataReportModel = listDataReportModel;
            return View("Index");
        
        }








        private void gettop()
        {
            string user_id = "-1";

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
                    }
                }
            }
            DataSet dsf = DataAccess.GET_TOP(user_id, "1");
            List<TopItemViewModel> topFoods = new List<TopItemViewModel>();
            if (dsf != null && dsf.Tables.Count > 0)
            {
                foreach (DataRow row in dsf.Tables[0].Rows)
                {
                    var _USERS_ID = row["USERS_ID"].ToString();
                    string _AVATAR_PATH;
                    string _avatar_name = row["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _AVATAR_PATH = $"{_USERS_ID}/{_avatar_name}";
                    }
                    else { _AVATAR_PATH = "no_image.png"; }
                    topFoods.Add(new TopItemViewModel
                    {
                        Id = Convert.ToInt32(row["ID"]),
                        Name = row["FOODS_NAME"].ToString(),
                        Price = Convert.ToInt32(row["FOODS_PRICE"]),
                        Avatar = _AVATAR_PATH,
                        UsedCount = Convert.ToInt32(row["TOTAL_USED"])
                    });
                }
            }

            ViewBag.TopFoods = topFoods;

            DataSet ddf = DataAccess.GET_TOP(user_id, "2");
            List<TopItemViewModel> topDrinks = new List<TopItemViewModel>();
            if (ddf != null && ddf.Tables.Count > 0)
            {
                foreach (DataRow row in ddf.Tables[0].Rows)
                {
                    var _USERS_ID = row["USERS_ID"].ToString();
                    string _AVATAR_PATH;
                    string _avatar_name = row["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _AVATAR_PATH = $"{_USERS_ID}/{_avatar_name}";
                    }
                    else { _AVATAR_PATH = "no_image.png"; }
                    topDrinks.Add(new TopItemViewModel
                    {
                        Id = Convert.ToInt32(row["ID"]),
                        Name = row["FOODS_NAME"].ToString(),
                        Price = Convert.ToInt32(row["FOODS_PRICE"]),
                        Avatar = _AVATAR_PATH,
                        UsedCount = Convert.ToInt32(row["TOTAL_USED"])
                    });
                }
            }

            ViewBag.TopDrinks = topDrinks;
            DataSet dds = DataAccess.GET_TOP(user_id, "3");
            List<TopItemViewModel> topServices = new List<TopItemViewModel>();
            if (dds != null && dds.Tables.Count > 0)
            {
                foreach (DataRow row in dds.Tables[0].Rows)
                {
                    var _USERS_ID = row["USERS_ID"].ToString();
                    string _AVATAR_PATH;
                    string _avatar_name = row["AVATAR_PATH"].ToString();
                    if (!string.IsNullOrWhiteSpace(_avatar_name))
                    {

                        _AVATAR_PATH = $"{_USERS_ID}/{_avatar_name}";
                    }
                    else { _AVATAR_PATH = "no_image.png"; }
                    topServices.Add(new TopItemViewModel
                    {
                        Id = Convert.ToInt32(row["ID"]),
                        Name = row["SERVICES_NAME"].ToString(),
                        Price = Convert.ToInt32(row["SERVICES_PRICE"]),
                        Avatar = _AVATAR_PATH,
                        UsedCount = Convert.ToInt32(row["TOTAL_USED"])
                    });
                }
            }

            ViewBag.topServices = topServices;
        }
    }
}
