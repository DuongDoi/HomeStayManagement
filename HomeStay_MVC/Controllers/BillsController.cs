using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class BillsController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BillsController));
        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            logger.Info("New request income select All Bill :");
            string bill_id = "0";
            string ht_id = "0";
            string v_type = "0";
            DataSet ds = DataAccess.BILLS_GET_LIST(bill_id, ht_id, v_type);
            List<Bills> bills = new List<Bills>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Bills _obj = new Bills();
                _obj.ID = dr["ID"].ToString();
                var _status = dr["BILLS_STATUS"].ToString();
                if (_status == "PAID") _obj.BILLS_STATUS = "Đã thanh toán";
                else _obj.BILLS_STATUS = "Chưa thanh toán";
                _obj.TOTAL_MONEY = dr["TOTAL_MONEY"].ToString();
                _obj.CREATE_BY = dr["CREATE_BY"].ToString();
                _obj.UPDATE_BY = dr["UPDATE_BY"].ToString();
                _obj.CUSTOMERS_NAME = dr["CUSTOMERS_NAME"].ToString();
                _obj.ID_HOMESTAYS = dr["ID_HOMESTAYS"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                bills.Add(_obj);

            }

            logger.Info("Pro select all bill success.");
            return View(bills);
        }



        public IActionResult Details(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.BILLS_GET_LIST(id,"","");
            List<Bills> billDetails = new List<Bills>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Bills bill = new Bills();
                    bill.ID = dr["Id"].ToString();

                    billDetails.Add(bill);
                }
            }

            return View(billDetails);
        }
    }
}
