﻿using HomeStay_MVC.ModelCommon;
using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ResfullApi.Models;
using System.Data;
using System.Text;

namespace HomeStay_MVC.Controllers
{

    public class LoginController : BaseController
    {
        private readonly IHttpClientFactory _clientFactory;
        string root = Common.GetValuesAppSetting("webConfig", "root");
        public LoginController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModels model)
        {
            if (ModelState.IsValid)
            {

                ResponseObjs _obj = new ResponseObjs();
                _obj.errCode = "-1";
                _obj.errMsgs = "unknow!";
                try
                {
                    DataSet ds = DataAccess.LOGIN(model.Users, model.Pass);
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
                    DataSet ds = DataAccess.USERS_GET_LIST(model.Users);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[0].Rows[0];

                        UserInfor _userInfor = new UserInfor();
                        _userInfor.ID = dr["ID"].ToString();
                        _userInfor.Users = dr["USERS"].ToString();
                        _userInfor.Pass = dr["PASS"].ToString();
                        _userInfor.Email = dr["EMAIL"].ToString();
                        _userInfor.Save_Code = dr["SAVE_CODE"].ToString();
                        _userInfor.Name = dr["NAME"].ToString();
                        _userInfor.Role = dr["ROLE"].ToString();
                        _userInfor.IsLock = dr["ISLOCK"].ToString();
                        _userInfor.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                        _userInfor.CARD_NUMBER = dr["CARD_NUMBER"].ToString();
                        _userInfor.PHONE = dr["PHONE"].ToString();
                        _userInfor.Is_First_Login = dr["IS_FIRST_LOGIN"].ToString();
                        _userInfor.Homestays_Id = dr["HOMESTAYS_ID"].ToString();
                        _userInfor.Create_By = dr["CREATE_BY"].ToString();
                        if (_userInfor.Is_First_Login == "0")
                        {
                            if (_userInfor.IsLock == "0") 
                            {
                                DataSet ds1 = DataAccess.USERS_GET_LIST(_userInfor.Create_By);
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    DataRow dr1 = ds1.Tables[0].Rows[0];

                                    UserInfor _userInfor1 = new UserInfor();
                                    _userInfor1.IsLock = dr1["ISLOCK"].ToString();
                                    if (_userInfor1.IsLock == "1")
                                    {
                                        ViewBag.Message = "Tài khoản của chủ cơ sở đang bị khóa do hết hạn, hãy liên hệ Admin để được hỗ trợ.";
                                        return View(model);
                                    }
                                    else
                                    {

                                        // Tạo session
                                        CreateAuthToken(_userInfor);
                                        TempData["Success"] = "Đăng nhập thành công. Chào mừng bạn quay lại ^^";

                                        return RedirectToAction("Index", "AdminHome");
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = "Người dùng không tồn tại, vui lòng liên hệ Admin để được hỗ trợ!";
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Tài khoản của bạn đang bị khóa do hết hạn, hãy liên hệ Admin để được hỗ trợ.";
                                return View(model);
                            }

                        }
                        else
                        {
                            TempData["Username"] = model.Users;
                            return RedirectToAction("CreatePIN", "Login");
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "Sai tên đăng nhập hoặc mật khẩu!";
                }


                return View(model);
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreatePIN()
        {
            var model = new CreatePIN();

            if (TempData["Username"] != null)
            {
                model.Users = TempData["Username"].ToString();
                TempData.Keep("Username");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult CreatePIN(CreatePIN model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PIN1 != model.PIN2)
            {
                ModelState.AddModelError("", "Mã PIN nhập lại không khớp.");
                return View(model);
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(model.PIN1 ?? "", @"^\d{8}$"))
            {
                ModelState.AddModelError("", "Mã PIN phải gồm đúng 8 chữ số.");
                return View(model);
            }
            ResponseObjs _obj = new ResponseObjs();
            _obj.errCode = "-1";
            _obj.errMsgs = "unknow!";
            try
            {
                DataSet ds = DataAccess.USERS_UPDATE_PIN(model.Users, model.PIN1);
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
                try
                {
                    DataSet ds = DataAccess.USERS_GET_LIST(model.Users);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        DataRow dr = ds.Tables[0].Rows[0];

                        UserInfor _userInfor = new UserInfor();
                        _userInfor.ID = dr["ID"].ToString();
                        _userInfor.Users = dr["USERS"].ToString();
                        _userInfor.Pass = dr["PASS"].ToString();
                        _userInfor.Email = dr["EMAIL"].ToString();
                        _userInfor.Save_Code = dr["SAVE_CODE"].ToString();
                        _userInfor.Name = dr["NAME"].ToString();
                        _userInfor.Role = dr["ROLE"].ToString();
                        _userInfor.IsLock = dr["ISLOCK"].ToString();
                        _userInfor.AVATAR_PATH = dr["AVATAR_PATH"].ToString();
                        _userInfor.CARD_NUMBER = dr["CARD_NUMBER"].ToString();
                        _userInfor.PHONE = dr["PHONE"].ToString();
                        _userInfor.Is_First_Login = dr["IS_FIRST_LOGIN"].ToString();
                        _userInfor.Homestays_Id = dr["HOMESTAYS_ID"].ToString();
                        _userInfor.Create_By = dr["CREATE_BY"].ToString();

                        CreateAuthToken(_userInfor);
                        TempData["Success"] = "Tạo mã PIN thành công. Đăng nhập thành công.";
                        return RedirectToAction("Index", "AdminHome");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewBag.Message = $"Đã xảy ra lỗi khi gửi yêu cầu: {ex.Message}";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Lỗi không xác định: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "Không thể tạo mã PIN.";
            }

            return View(model);
        }

    }
}