using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ResfullApi.Models;
using HomeStay_MVC.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class CustomersController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));


        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            logger.Info("New request income select All Custommer :");
            string cus_card_number = "0";
            string user_id = "0";
            string v_type = "0";
            DataSet ds = DataAccess.CUSTOMERS_GET_LIST(cus_card_number, user_id, v_type);
            List<Customers> customers = new List<Customers>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Customers _obj = new Customers();
                _obj.ID = dr["ID"].ToString();
                _obj.CUSTOMERS_CARD_NUMBER = dr["CUSTOMERS_CARD_NUMBER"].ToString();
                _obj.CUSTOMERS_NAME = dr["CUSTOMERS_NAME"].ToString();
                _obj.CUSTOMERS_PHONE = dr["CUSTOMERS_PHONE"].ToString();
                _obj.CUSTOMERS_ADDRESS = dr["CUSTOMERS_ADDRESS"].ToString();
                _obj.USERS = dr["USERS"].ToString();
                _obj.CREATE_BY = dr["CREATE_BY"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                customers.Add(_obj);

            }

            logger.Info("Pro Customers Select All success.");
            return View(customers);
        }



        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult customersInsert([FromBody] dynamic sendData)
        {

            logger.Info("New request income Insert customer :" + sendData.ToString());

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var customerObj = JObject.Parse(sendData.ToString());
                string customer_card_id = Convert.ToString(customerObj["customer_card_id"]);
                string customer_name = Convert.ToString(customerObj["customer_name"]);
                string customer_phone = Convert.ToString(customerObj["customer_phone"]);
                string customer_address = Convert.ToString(customerObj["customer_address"]);
                string users_id = Convert.ToString(customerObj["users_id"]);



                DataSet ds = DataAccess.CUSTOMERS_INSERT(customer_card_id, customer_name, customer_phone, customer_address, users_id);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {

            }
            logger.Info("Pro CUSTOMERS INSERT success and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }


        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult customersUpdate([FromBody] dynamic sendData)
        {

            logger.Info("New request income custmer update :" + sendData.ToString());

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var customerObj = JObject.Parse(sendData.ToString());
                string customer_card_id = Convert.ToString(customerObj["customer_card_id"]);
                string customer_name = Convert.ToString(customerObj["customer_name"]);
                string customer_phone = Convert.ToString(customerObj["customer_phone"]);
                string customer_address = Convert.ToString(customerObj["customer_address"]);
                string users_id = Convert.ToString(customerObj["users_id"]);
                string type = Convert.ToString(customerObj["type"]);
                if (string.IsNullOrEmpty(type)) type = "1"; //TYPE 1 UPDATE, TYPE 2 XOA


                DataSet ds = DataAccess.CUSTOMERS_UPDATE(customer_card_id,customer_name,customer_phone,customer_address,users_id, type);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {
                logger.Error("Loi :" + ex.ToString());
            }
            logger.Info("Pro customer UPDATE succ and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }



        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult customersGetList([FromBody] dynamic sendData)
        {

            logger.Info("New request income customer Get List :" + sendData.ToString());
            ResponseObjs _obj = new ResponseObjs();
            List<CustomersObj> L = new List<CustomersObj>();

            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var customerObj = JObject.Parse(sendData.ToString());
                string customer_card_id = Convert.ToString(customerObj["customer_card_id"]);
                string users_id = Convert.ToString(customerObj["users_id"]);
                if (string.IsNullOrEmpty(customer_card_id)) customer_card_id = "-1";


                DataSet ds = DataAccess.CUSTOMERS_GET_LIST(customer_card_id, users_id,"0");

                _obj.errCode = "0";
                _obj.errMsgs = "Success";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string _id = dr["id"].ToString();
                    string _customer_card_id = dr["customer_card_id"].ToString();
                    string _customer_name = dr["customer_name"].ToString();
                    string _customer_phone = dr["customer_phone"].ToString();
                    string _customer_address = dr["customer_address"].ToString();
                    string _users_id = dr["users_id"].ToString();
                    CustomersObj _objCT = new CustomersObj();
                    _objCT.id = _id;
                    _objCT.customer_card_id = _customer_card_id;
                    _objCT.customer_name = _customer_name;
                    _objCT.customer_phone = _customer_phone;
                    _objCT.customer_address = _customer_address;
                    _objCT.users_id = _users_id;

                    L.Add(_objCT);
                }
                _obj.data = L;


            }
            catch (Exception ex)
            {
                logger.Error("Loi :" + ex.ToString());
            }
            logger.Info("Pro Customer Get List succ and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }






    }
}
