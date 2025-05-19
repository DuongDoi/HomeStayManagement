using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class FoodsController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));


        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            logger.Info("New request income select All Food :");
            string food_name = "0";
            string user_id = "0";
            string v_type = "0";
            DataSet ds = DataAccess.FOODS_GET_LIST(food_name, user_id, v_type);
            List<Foods> foods = new List<Foods>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Foods _obj = new Foods();
                _obj.ID = dr["ID"].ToString();
                _obj.FOODS_NAME = dr["FOODS_NAME"].ToString();
                _obj.FOODS_PRICE = dr["FOODS_PRICE"].ToString();
                var food_type =  dr["FOODS_TYPE"].ToString();
                if (food_type == "FOOD")
                    _obj.FOODS_TYPE = "Đồ ăn";
                else
                    _obj.FOODS_TYPE = "Đồ uống";
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
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
    }
}
