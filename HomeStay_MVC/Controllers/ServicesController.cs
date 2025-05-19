using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class ServicesController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));


        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            logger.Info("New request income select All Service :");
            string service_name = "0";
            string user_id = "0";
            string v_type = "0";
            DataSet ds = DataAccess.SERVICES_GET_LIST(service_name, user_id, v_type);
            List<Services> services = new List<Services>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Services _obj = new Services();
                _obj.ID = dr["ID"].ToString();
                _obj.SERVICES_NAME = dr["SERVICES_NAME"].ToString();
                _obj.SERVICES_PRICE = dr["SERVICES_PRICE"].ToString();
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                _obj.USERS = dr["NAME"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                services.Add(_obj);

            }

            logger.Info("Pro Select All Service success.");
            return View(services);
        }
    }
}
