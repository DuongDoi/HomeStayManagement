using System.Data;
using HomeStay_MVC.Model;
using HomeStay_MVC.Models;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ResfullApi.Models;

namespace HomeStay_MVC.Controllers
{
    public class UsersController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));


        public IActionResult Profile()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            var user = GetCurrentUser();
            return View(user);
        }
        [HttpGet]
        public IActionResult EditProfile()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            var user = GetCurrentUser();
            return View(user);
        }
        [HttpPost]
        public IActionResult EditProfile(string username,UserInfor model)
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
            var user = GetCurrentUser();
            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.USERS_UPDATE_INFOR(user.Users, model.PHONE, model.Email, model.Name, model.CARD_NUMBER, "1");
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
                SetCurrentUser(model.PHONE,model.Email,model.Name,model.CARD_NUMBER);

                return RedirectToAction("Profile");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult ChangePass()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            var userName = HttpContext.Session.GetString("User");
            var model = new ChangePass
            {
                UserName = userName
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangePass(ChangePass model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            if(model.NewPass1 != model.NewPass2)
            {
                ViewBag.Message = "Mật khẩu mới không khớp";
            }
            var old_pass = HttpContext.Session.GetString("Pass"); ;
            if (model.OldPass != old_pass)
            {
                ViewBag.Message = "Sai mật khẩu cũ";
            }
            var user = GetCurrentUser();
            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.USERS_UPDATE_PASS(user.Users,user.Email,user.Save_Code, model.NewPass1);
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
                TempData["Success"] = "Cập nhật mật khẩu thành công.";
                SetCurrentPass(model.NewPass1);

                return RedirectToAction("Index","AdminHome");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult ChangePIN()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            var userName = HttpContext.Session.GetString("User");
            var model = new ChangePIN
            {
                UserName = userName
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult ChangePIN(ChangePIN model)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(model.NewPIN1 ?? "", @"^\d{8}$"))
            {
                ModelState.AddModelError("", "Mã PIN phải gồm đúng 8 chữ số.");
            }
            if (model.NewPIN1 != model.NewPIN2)
            {
                ViewBag.Message = "Mã PIN mới không khớp";
            }
            if (!checkPIN(model.OldPIN))
            {
                ViewBag.Message = "Sai mã PIN cũ";
            }
            var user = GetCurrentUser();
            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.USERS_UPDATE_PIN(user.Users, model.NewPIN1);
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
                TempData["Success"] = "Cập nhật mã PIN thành công.";
                SetCurrentPIN(model.NewPIN1);

                return RedirectToAction("Index", "AdminHome");
            }
            else
            {
                ViewBag.Message = _obj.errMsgs;
                return View(model);
            }
        }


        private void SetCurrentUser(string phone, string email, string name, string card_number)
        {
            HttpContext.Session.SetString("Name", name ?? "");
            HttpContext.Session.SetString("Email", email ?? "");
            HttpContext.Session.SetString("CARD_NUMBER", card_number ?? "");
            HttpContext.Session.SetString("PHONE", phone ?? "");
        }


            private void SetCurrentPass(string pass)
        {
            HttpContext.Session.SetString("Pass", pass ?? "");
        }

        private void SetCurrentPIN(string pin)
        {
            HttpContext.Session.SetString("Save_Code", pin ?? "");
        }
        private UserInfor GetCurrentUser() {
            var user = new UserInfor();
            user.Users = HttpContext.Session.GetString("User");
            user.Pass = HttpContext.Session.GetString("Pass");
            user.Name = HttpContext.Session.GetString("Name");
            user.PHONE = HttpContext.Session.GetString("PHONE");
            user.Save_Code = HttpContext.Session.GetString("Save_Code");
            user.CARD_NUMBER = HttpContext.Session.GetString("CARD_NUMBER");
            user.Email = HttpContext.Session.GetString("Email");
            user.AVATAR_PATH = HttpContext.Session.GetString("AVATAR_PATH");
            return user;
        }































        [Route("api/[controller]/[action]/data")]

        [HttpPost]
        public IActionResult usersLogin([FromBody] dynamic sendData)
        {

            logger.Info("New request income user Login :" + sendData.ToString());

            ResponseObjs  _obj = new ResponseObjs();
            try
            {
                var userObj = JObject.Parse(sendData.ToString());
                string users = Convert.ToString(userObj["users"]);
                string pass = Convert.ToString(userObj["pass"]);

                DataSet ds = DataAccess.LOGIN(users, pass);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {

            }
            logger.Info("Pro admin Login succ and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }









        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult usersInsert([FromBody] dynamic sendData)
        {

            logger.Info("New request income Insert User :" + sendData.ToString());

            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var userObj = JObject.Parse(sendData.ToString());
                string users = Convert.ToString(userObj["users"]);
                string pass = Convert.ToString(userObj["pass"]);
                string phone = Convert.ToString(userObj["phone"]);
                string email = Convert.ToString(userObj["email"]);
                string name = Convert.ToString(userObj["name"]);



                DataSet ds = DataAccess.USERS_INSERT(users, pass, phone, email,name,"");
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {

            }
            logger.Info("Pro USERS INSERT success and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }


        //[Route("api/[controller]/[action]/data")]
        //[HttpPost]
        //public IActionResult usersUpdate([FromBody] dynamic sendData)
        //{

        //    logger.Info("New request income user update :" + sendData.ToString());

        //    ResponseObjs _obj = new ResponseObjs();
        //    _obj.errCode = "-1";
        //    _obj.errMsgs = "unknow";
        //    try
        //    {
        //        var userObj = JObject.Parse(sendData.ToString());
        //        string users = Convert.ToString(userObj["users"]);
        //        string pass = Convert.ToString(userObj["pass"]);
        //        string phone = Convert.ToString(userObj["phone"]);
        //        string email = Convert.ToString(userObj["email"]);
        //        string name = Convert.ToString(userObj["name"]);
        //        string type = Convert.ToString(userObj["type"]);
        //        if (string.IsNullOrEmpty(type)) type = "1";


        //        DataSet ds = DataAccess.USERS_UPDATE(users, pass,phone,email,name,type);
        //        string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
        //        string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

        //        _obj.errCode = errrCode;
        //        _obj.errMsgs = errrMsg;

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Loi :" + ex.ToString());
        //    }
        //    logger.Info("Pro USERS UPDATE succ and ressponse :" + _obj.ToString());
        //    return Ok(_obj);

        //}



        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult usersGetList([FromBody] dynamic sendData)
        {

            logger.Info("New request income user Get List :" + sendData.ToString());
            ResponseObjs _obj = new ResponseObjs();
            List<UsersObj> L = new List<UsersObj>();
           
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow";
            try
            {
                var userObj = JObject.Parse(sendData.ToString());
                string users = Convert.ToString(userObj["users"]);
                if (string.IsNullOrEmpty(users)) users = "-1";


                DataSet ds = DataAccess.USERS_GET_LIST(users);

                _obj.errCode = "0";
                _obj.errMsgs = "Success";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string _id = dr["id"].ToString();
                    string _users = dr["users"].ToString();
                    string _pass = dr["pass"].ToString();
                    string _phone = dr["phone"].ToString();
                    string _email = dr["email"].ToString();
                    string _name = dr["name"].ToString();
                    UsersObj _objUser = new UsersObj();
                    _objUser.id = _id;
                    _objUser.users = _users;
                    _objUser.pass = _pass;
                    _objUser.phone = _phone;
                    _objUser.email = _email;
                    _objUser.name = _name;
                    L.Add(_objUser);
                }
                _obj.data = L;

                //for(int i = 0; i < ds.Tables[0].Rows.Count;i++)
                //{
                //    string _id = ds.Tables[0].Rows[i]["id"].ToString();
                //    string _users = ds.Tables[0].Rows[i]["users"].ToString();
                //    string _pass = ds.Tables[0].Rows[i]["pass"].ToString();
                //    usersObj _objUser = new usersObj();
                //    _objUser.id = _id;
                //    _objUser.users = _users;
                //    _objUser.pass = _pass;
                //    L.Add(_objUser);

                //}


            }
            catch (Exception ex)
            {
                logger.Error("Loi :" + ex.ToString());
            }
            logger.Info("Pro Users Get List succ and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }






    }
}
