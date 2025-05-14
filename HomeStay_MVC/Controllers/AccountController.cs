using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using ResfullApi.Models;
using System.Data;

namespace HomeStay_MVC.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            string users = "-1";
            DataSet ds = DataAccess.USERS_GET_LIST(users);
            List<AccountInfor> accounts = new List<AccountInfor>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AccountInfor _obj = new AccountInfor();
                _obj.Id = dr["Id"].ToString();
                _obj.Users = dr["Users"].ToString();
                _obj.Pass = dr["Pass"].ToString();
                _obj.Phone = dr["Phone"].ToString();
                _obj.Email = dr["Email"].ToString();
                _obj.Name = dr["Name"].ToString();
                _obj.Card_Number = dr["Card_Number"].ToString();
                _obj.Save_Code = dr["Save_Code"].ToString();
                _obj.Role = dr["Role"].ToString();
                _obj.IsLock = dr["IsLock"].ToString();
                _obj.Avatar_Path = dr["Avatar_Path"].ToString();
                try { _obj.Create_At = DateTime.Parse(dr["Create_At"].ToString()); }
                catch { }
                try { _obj.Update_At = DateTime.Parse(dr["Update_At"].ToString()); }
                catch { }
                _obj.Is_First_Login = dr["Is_First_Login"].ToString();
                _obj.Homestays_Id = dr["Homestays_Id"].ToString();
                _obj.Create_By = dr["Create_By"].ToString();

                accounts.Add(_obj);

            }
            return View(accounts);
        }
    }
}
