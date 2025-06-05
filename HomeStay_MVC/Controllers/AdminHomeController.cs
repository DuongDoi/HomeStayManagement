using Microsoft.AspNetCore.Mvc;
using HomeStay_MVC.Controllers;
using HomeStay_MVC.ModelCommon;
using ResfullApi.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeStay_MVC.Models;

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
                    else return RedirectToAction("Index", "Login");
                }
            }
            DataSet dsf = DataAccess.GET_TOP_FOOD(user_id);
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

            DataSet ddf = DataAccess.GET_TOP_DRINK(user_id);
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

            return View();
        }
    }
}
