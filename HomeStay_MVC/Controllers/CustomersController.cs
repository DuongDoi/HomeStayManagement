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
            string _role = HttpContext.Session.GetString("Role");
            string id = "0";
            string user_id = "0";
            string v_type = "0";
            if (_role == "admin")
            {
                id = "0";
                user_id = "0";
                v_type = "0";
            }
            else
            {
                if (_role == "owner")
                {
                    id = "-1";
                    user_id = HttpContext.Session.GetString("ID");
                    v_type = "1";
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        id = "-1";
                        v_type = "1";
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }
            DataSet ds = DataAccess.CUSTOMERS_GET_LIST(id, user_id, v_type);
            List<Customers> customers = new List<Customers>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Customers _obj = new Customers();
                _obj.ID = dr["ID"].ToString();
                _obj.CUSTOMERS_CARD_NUMBER = dr["CUSTOMERS_CARD_NUMBER"].ToString();
                _obj.CUSTOMERS_NAME = dr["CUSTOMERS_NAME"].ToString();
                _obj.CUSTOMERS_PHONE = dr["CUSTOMERS_PHONE"].ToString();
                _obj.CUSTOMERS_ADDRESS = dr["CUSTOMERS_ADDRESS"].ToString();
                _obj.USERS_NAME = dr["NAME"].ToString();
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

        public IActionResult Details(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            string user_id = "0";
            string v_type = "0";
            if (_role == "admin" )
            {
                user_id = "-1";
                v_type = "1";
            }
            else
            {
                if (_role == "owner")
                {
                    user_id = HttpContext.Session.GetString("ID");
                    v_type = "1";
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        v_type = "1";
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }
            
            DataSet ds = DataAccess.CUSTOMERS_GET_LIST(id, user_id, v_type);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Customers _obj = new Customers();
                _obj.ID = dr["ID"].ToString();
                _obj.CUSTOMERS_CARD_NUMBER = dr["CUSTOMERS_CARD_NUMBER"].ToString();
                _obj.CUSTOMERS_NAME = dr["CUSTOMERS_NAME"].ToString();
                _obj.CUSTOMERS_PHONE = dr["CUSTOMERS_PHONE"].ToString();
                _obj.CUSTOMERS_ADDRESS = dr["CUSTOMERS_ADDRESS"].ToString();
                _obj.USERS_NAME = dr["NAME"].ToString();
                _obj.CREATE_BY = dr["CREATE_BY"].ToString();

                _obj.USERS_ID = dr["USERS_ID"].ToString();
                if(_obj.USERS_ID != user_id && _role!="admin") return RedirectToAction("Index", "Login");


                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                return View(_obj);
            }
            return RedirectToAction("Index", "Customers");
        }




        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string userID = "";
            string type = "1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "manager")
            {
                DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr1 = ds1.Tables[0].Rows[0];
                    userID = dr1["ID"].ToString();
                }
                else return RedirectToAction("Index", "Login");
            }
            else
            {
                if (_role == "admin") userID = "-1";
                else
                    userID = HttpContext.Session.GetString("ID");
            }
            DataSet ds = DataAccess.CUSTOMERS_GET_LIST(id, userID, type);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Customers _obj = new Customers();
                _obj.ID = dr["ID"].ToString();
                _obj.CUSTOMERS_CARD_NUMBER = dr["CUSTOMERS_CARD_NUMBER"].ToString();
                _obj.CUSTOMERS_NAME = dr["CUSTOMERS_NAME"].ToString();
                _obj.CUSTOMERS_PHONE = dr["CUSTOMERS_PHONE"].ToString();
                _obj.CUSTOMERS_ADDRESS = dr["CUSTOMERS_ADDRESS"].ToString();
                _obj.USERS_NAME = dr["NAME"].ToString();
                _obj.CREATE_BY = dr["CREATE_BY"].ToString();
                _obj.USERS_ID = dr["USERS_ID"].ToString();
                if(_obj.USERS_ID != userID && _role != "admin") return RedirectToAction("Index", "Customers");
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                return View(_obj);
            }
            return RedirectToAction("Index", "Customers");
        }

        [HttpPost]
        public IActionResult Edit(string id, Customers model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!checkPIN(model.Save_Code))
            {
                ViewBag.Message = "Sai mã PIN";
                return View(model);
            }



            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.CUSTOMERS_UPDATE(id,model.CUSTOMERS_CARD_NUMBER,model.CUSTOMERS_NAME,model.CUSTOMERS_PHONE,model.CUSTOMERS_ADDRESS,model.USERS_ID,"1");
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();
                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Message = $"Đã xảy ra lỗi khi gửi yêu cầu: {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Lỗi không xác định: {ex.Message}";
            }
            if (_obj.errCode == "0")
            {
                TempData["Success"] = "Cập nhật thông tin thành công.";

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {

            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            if (_role != "manager")
            {
                var userID = "";
                if (_role == "admin") userID = "-1";
                else  userID = HttpContext.Session.GetString("ID");
                DataSet ds = DataAccess.CUSTOMERS_GET_LIST(id, userID, "1");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataRow dr = ds.Tables[0].Rows[0];
                    Customers _obj = new Customers();
                    _obj.ID = dr["ID"].ToString();
                    _obj.CUSTOMERS_CARD_NUMBER = dr["CUSTOMERS_CARD_NUMBER"].ToString();
                    _obj.CUSTOMERS_NAME = dr["CUSTOMERS_NAME"].ToString();
                    _obj.CUSTOMERS_PHONE = dr["CUSTOMERS_PHONE"].ToString();
                    _obj.CUSTOMERS_ADDRESS = dr["CUSTOMERS_ADDRESS"].ToString();
                    _obj.USERS_NAME = dr["NAME"].ToString();
                    _obj.CREATE_BY = dr["CREATE_BY"].ToString();
                    _obj.USERS_ID = dr["USERS_ID"].ToString();
                    if (_obj.USERS_ID != userID && _role != "admin") return RedirectToAction("Index", "Customers");

                    try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                    catch { }
                    try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                    catch { }

                    return View(_obj);
                }
                return RedirectToAction("Index", "Customers");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Delete(string id, Customers model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            if (!checkPIN(model.Save_Code))
            {
                ViewBag.Message = "Mã PIN không chính xác.";
                return View(model);
            }
            
            var ds = DataAccess.CUSTOMERS_UPDATE(id,"","","","", "", "2");
            var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

            if (errCode == "0")
            {
                TempData["Success"] = "Xóa khách hàng thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Không thể xóa khách hàng.";
            return View(model);
        }


        [HttpGet]
        public IActionResult Add()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            Customers model = new Customers();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Customers model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string _role = HttpContext.Session.GetString("Role");
            string userID = "";
            if (_role != "manager")
            {
                userID = HttpContext.Session.GetString("ID");
            }
            else
            {
                DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr1 = ds1.Tables[0].Rows[0];
                    userID = dr1["ID"].ToString();
                }
                else return RedirectToAction("Index", "Login");
            }
            string user_name = HttpContext.Session.GetString("Name");
            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "Thêm mới thất bại!";
            try
            {
                //var role = model.role;
                DataSet ds = DataAccess.CUSTOMERS_INSERT(model.CUSTOMERS_CARD_NUMBER, model.CUSTOMERS_NAME, model.CUSTOMERS_PHONE, model.CUSTOMERS_ADDRESS, userID,user_name);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();
                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Message = $"Đã xảy ra lỗi khi gửi yêu cầu: {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Lỗi không xác định: {ex.Message}";
            }
            if (_obj.errCode != "-1")
            {
                TempData["Success"] = "Thêm mới thành công.";
                return RedirectToAction("Index", "Customers");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
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



                DataSet ds = DataAccess.CUSTOMERS_INSERT(customer_card_id, customer_name, customer_phone, customer_address, users_id,users_id);
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


                DataSet ds = DataAccess.CUSTOMERS_UPDATE("",customer_card_id,customer_name,customer_phone,customer_address,users_id, type);
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
            List<Customers> L = new List<Customers>();

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
                    Customers    _objCT = new Customers();
                    _objCT.ID = _id;
                    _objCT.CUSTOMERS_CARD_NUMBER = _customer_card_id;
                    _objCT.CUSTOMERS_NAME = _customer_name;
                    _objCT.CUSTOMERS_PHONE = _customer_phone;
                    _objCT.CUSTOMERS_ADDRESS = _customer_address;
                    _objCT.USERS_ID = _users_id;

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
