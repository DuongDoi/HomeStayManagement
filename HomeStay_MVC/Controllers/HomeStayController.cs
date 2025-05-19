using System.Data;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ResfullApi.Models;
using HomeStay_MVC.Models;

namespace HomeStay_MVC.Controllers
{
    public class HomeStayController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));


        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            logger.Info("New request income select All HomeStay :");
            string users = "0";
            string ht_id = "0";
            string v_type = "0";
            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(ht_id,users,v_type);
            List<HomestaysObj> homestays = new List<HomestaysObj>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HomestaysObj _obj = new HomestaysObj();
                _obj.id = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString();
                _obj.HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString();
                _obj.HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString();
                _obj.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                _obj.MANAGER_PHONE = dr["MANAGER_PHONE"].ToString();
                _obj.USERS_ID = dr["USERS_ID"].ToString();
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                homestays.Add(_obj);

            }

            logger.Info("Pro HOMESTAYS Select All success.");
            return View(homestays);
        }

        public IActionResult Details(string id,string userId)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(id,userId,"1");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                HomestaysObj _obj = new HomestaysObj();
                _obj.id = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString();
                _obj.HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString();
                _obj.HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString();
                _obj.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                _obj.MANAGER_PHONE = dr["MANAGER_PHONE"].ToString();
                _obj.USERS_ID = dr["USERS_ID"].ToString();
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                return View(_obj);
            }
            return RedirectToAction("Index", "HomeStay");
        }

        [HttpGet]
        public IActionResult Edit(string id, string userId)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(id, userId, "1");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                HomestaysObj _obj = new HomestaysObj();
                _obj.id = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString();
                _obj.HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString();
                _obj.HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString();
                _obj.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                _obj.MANAGER_PHONE = dr["MANAGER_PHONE"].ToString();
                _obj.USERS_ID = dr["USERS_ID"].ToString();
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                return View(_obj);
            }
            return RedirectToAction("Index", "HomeStay");
        }

        [HttpPost]
        public IActionResult Edit(string id, HomestaysObj model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!checkPIN(model.Save_code))
            {
                ViewBag.Message = "Sai mã PIN";
                return View(model);
            }



            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.HOMESTAYS_UPDATE(id, model.HOMESTAYS_NAME, model.MANAGER_CARD_NUMBER, model.HOMESTAYS_ADDRESS, model.HOMESTAY_DESCRIPTION, model.MANAGER_NAME, model.MANAGER_PHONE, "1");
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
        public IActionResult Delete(string id, string userId)
        {

            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(id, userId, "1");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                HomestaysObj _obj = new HomestaysObj();
                _obj.id = dr["ID"].ToString();
                _obj.HOMESTAYS_NAME = dr["HOMESTAYS_NAME"].ToString();
                _obj.MANAGER_CARD_NUMBER = dr["MANAGER_CARD_NUMBER"].ToString();
                _obj.HOMESTAYS_ADDRESS = dr["HOMESTAYS_ADDRESS"].ToString();
                _obj.HOMESTAY_DESCRIPTION = dr["HOMESTAY_DESCRIPTION"].ToString();
                _obj.MANAGER_NAME = dr["MANAGER_NAME"].ToString();
                _obj.MANAGER_PHONE = dr["MANAGER_PHONE"].ToString();
                _obj.USERS_ID = dr["USERS_ID"].ToString();
                _obj.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(dr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(dr["UPDATE_AT"].ToString()); }
                catch { }

                return View(_obj);
            }
            return RedirectToAction("Index", "HomeStay");
        }

        [HttpPost]
        public IActionResult Delete(string id, HomestaysObj model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            if (!checkPIN(model.Save_code))
            {
                ViewBag.Message = "Mã PIN không chính xác.";
                return View(model);
            }

            var ds = DataAccess.HOMESTAYS_UPDATE(id,"","","","","","", "2");
            var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

            if (errCode == "0")
            {
                TempData["Success"] = "Xóa cơ sở thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Không thể xóa cơ sở.";
            return View(model);
        }



























        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult homestaysInsert([FromBody] dynamic sendData)
        {

            logger.Info("New request income Insert HomeStay :" + sendData.ToString());

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var homestayObj = JObject.Parse(sendData.ToString());
                string name = Convert.ToString(homestayObj["name"]);
                string card = Convert.ToString(homestayObj["card"]);
                string address = Convert.ToString(homestayObj["address"]);
                string description = Convert.ToString(homestayObj["description"]);
                string manager = Convert.ToString(homestayObj["manager"]);
                string phone = Convert.ToString(homestayObj["phone"]);
                string user_id = Convert.ToString(homestayObj["user_id"]);



                DataSet ds = DataAccess.HOMESTAYS_INSERT(name,card,address,description,manager,phone,user_id);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {

            }
            logger.Info("Pro HOMESTAYS INSERT success and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }


        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult homestaysUpdate([FromBody] dynamic sendData)
        {

            logger.Info("New request income homestay update :" + sendData.ToString());

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var homestayObj = JObject.Parse(sendData.ToString());
                string name = Convert.ToString(homestayObj["name"]);
                string card = Convert.ToString(homestayObj["card"]);
                string address = Convert.ToString(homestayObj["address"]);
                string description = Convert.ToString(homestayObj["description"]);
                string manager = Convert.ToString(homestayObj["manager"]);
                string phone = Convert.ToString(homestayObj["phone"]);
                string user_id = Convert.ToString(homestayObj["user_id"]);
                string type = Convert.ToString(homestayObj["type"]);
                if (string.IsNullOrEmpty(type)) type = "1"; //TYPE 1 UPDATE, TYPE 2 XOA


                DataSet ds = DataAccess.HOMESTAYS_UPDATE("1",name,card, address,description, manager,phone,type);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {
                logger.Error("Loi :" + ex.ToString());
            }
            logger.Info("Pro HOMESTAY UPDATE succ and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }



        //[Route("api/[controller]/[action]/data")]
        //[HttpPost]
        //public IActionResult homestaysGetList([FromBody] dynamic sendData)
        //{

        //    logger.Info("New request income homestay Get List :" + sendData.ToString());
        //    ResponseObjs _obj = new ResponseObjs();
        //    List<HomestaysObj> L = new List<HomestaysObj>();

        //    _obj.errCode = "-1";
        //    _obj.errMsgs = "unknow";
        //    try
        //    {
        //        var homestayObj = JObject.Parse(sendData.ToString());
        //        string name = Convert.ToString(homestayObj["name"]);
        //        string user_id = Convert.ToString(homestayObj["user_id"]);
        //        if (string.IsNullOrEmpty(name)) name = "-1";


        //        DataSet ds = DataAccess.HOMESTAYS_GET_LIST(name,user_id);

        //        _obj.errCode = "0";
        //        _obj.errMsgs = "Success";
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            string _id = dr["id"].ToString();
        //            string _name = dr["name"].ToString();
        //            string _card = dr["card"].ToString();
        //            string _address = dr["address"].ToString();
        //            string _description = dr["description"].ToString();
        //            string _manager = dr["manager"].ToString();
        //            string _phone = dr["phone"].ToString();
        //            string _user_id = dr["user_id"].ToString();
        //            HomestaysObj _objHT = new HomestaysObj();
        //            _objHT.id = _id;
        //            _objHT.name = _name;
        //            _objHT.card = _card;
        //            _objHT.address = _address;
        //            _objHT.description = _description;
        //            _objHT.manager = _manager;
        //            _objHT.phone = _phone;
        //            _objHT.user_id = _user_id;

        //            L.Add(_objHT);
        //        }
        //        _obj.data = L;


        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Loi :" + ex.ToString());
        //    }
        //    logger.Info("Pro HomeStay Get List succ and ressponse :" + _obj.ToString());
        //    return Ok(_obj);

        //}






    }
}
