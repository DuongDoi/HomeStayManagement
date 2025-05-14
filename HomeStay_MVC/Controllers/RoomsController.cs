using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using ResfullApi.Models;
using System.Data;

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
            string room_name = "0";
            string ht_id = "0";
            string v_type = "0";
            DataSet ds = DataAccess.ROOMS_GET_LIST(ht_id, room_name, v_type);
            List<Rooms> rooms = new List<Rooms>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Rooms _obj = new Rooms();
                _obj.ID = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.ROOMS_NAME = dr["ROOMS_NAME"].ToString();
                _obj.ROOMS_PRICE = dr["ROOMS_PRICE"].ToString();
                _obj.ROOMS_STATUS = dr["ROOMS_STATUS"].ToString();
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
    }
}
