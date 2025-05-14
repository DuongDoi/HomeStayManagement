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
    public class UsersController : ControllerBase
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UsersController));

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



                DataSet ds = DataAccess.USERS_INSERT(users, pass, phone, email,name);
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


        [Route("api/[controller]/[action]/data")]
        [HttpPost]
        public IActionResult usersUpdate([FromBody] dynamic sendData)
        {

            logger.Info("New request income user update :" + sendData.ToString());

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
                string type = Convert.ToString(userObj["type"]);
                if (string.IsNullOrEmpty(type)) type = "1";


                DataSet ds = DataAccess.USERS_UPDATE(users, pass,phone,email,name,type);
                string errrCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                string errrMsg = ds.Tables[0].Rows[0]["errMsg"].ToString();

                _obj.errCode = errrCode;
                _obj.errMsgs = errrMsg;

            }
            catch (Exception ex)
            {
                logger.Error("Loi :" + ex.ToString());
            }
            logger.Info("Pro USERS UPDATE succ and ressponse :" + _obj.ToString());
            return Ok(_obj);

        }



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
