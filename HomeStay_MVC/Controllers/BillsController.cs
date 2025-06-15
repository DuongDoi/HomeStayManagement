using AspNetCoreGeneratedDocument;
using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResfullApi.Models;
using System.Data;
using System.Security.Cryptography;

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
            string user_id = "-1";
            string bill_id = "-1";
            string ht_id = "-1";
            string bill_status = "-1";
            string v_type = "1";

            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {

            }
            else
            {
                if (_role == "owner")
                {
                     
                    user_id = HttpContext.Session.GetString("ID");
                    bill_status = "UNPAID";
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                        bill_status = "UNPAID";
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }
            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id,"1"); 
            List<SelectListItem> homestayList = new List<SelectListItem>();
            foreach (DataRow dr in dsHomestays.Tables[0].Rows)
            {
                homestayList.Add(new SelectListItem
                {
                    Value = dr["ID"].ToString(),
                    Text = dr["HOMESTAYS_NAME"].ToString()
                });
            }
            ViewBag.HomestayList = homestayList;


            DataSet ds = DataAccess.BILLS_GET_LIST(bill_id, ht_id,user_id,bill_status, v_type);
            List<Bills> bills = new List<Bills>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Bills _obj = new Bills();
                _obj.ID = dr["ID"].ToString();
                var _status = dr["BILLS_STATUS"].ToString();
                if (_status == "PAID") _obj.BILLS_STATUS = "Đã thanh toán";
                else _obj.BILLS_STATUS = "Chưa thanh toán";
                _obj.TOTAL_MONEY = int.TryParse(dr["TOTAL_MONEY"]?.ToString(), out int value) ? value : 0;
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


        [HttpGet]
        public IActionResult GetBillsByHomestay(string homestayId)
        {

            string _role = HttpContext.Session.GetString("Role");
            var bill_status = "";
            if (_role != "manager") bill_status = "-1";
            else bill_status = "UNPAID";
                DataSet dsr = DataAccess.BILLS_GET_LIST("-1", homestayId, "-1",bill_status, "1");

            var bills = new List<object>();
            foreach (DataRow drr in dsr.Tables[0].Rows)
            {
                Bills _obj = new Bills();
                _obj.ID = drr["ID"].ToString();
                var _status = drr["BILLS_STATUS"].ToString();
                if (_status == "PAID") _obj.BILLS_STATUS = "Đã thanh toán";
                else _obj.BILLS_STATUS = "Chưa thanh toán";
                _obj.TOTAL_MONEY = int.TryParse(drr["TOTAL_MONEY"]?.ToString(), out int value) ? value : 0;
                _obj.CREATE_BY = drr["CREATE_BY"].ToString();
                _obj.UPDATE_BY = drr["UPDATE_BY"].ToString();
                _obj.CUSTOMERS_NAME = drr["CUSTOMERS_NAME"].ToString();
                _obj.ID_HOMESTAYS = drr["ID_HOMESTAYS"].ToString();
                _obj.HOMESTAYS_NAME = drr["HOMESTAYS_NAME"].ToString();
                try { _obj.CREATE_AT = DateTime.Parse(drr["CREATE_AT"].ToString()); }
                catch { }
                try { _obj.UPDATE_AT = DateTime.Parse(drr["UPDATE_AT"].ToString()); }
                catch { }
                bills.Add(_obj);
            }

            return Json(bills);
        }




        public IActionResult Add()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            Bills model = new Bills();
            model = get_infor();
            if(model!=null) return View(model);
            else  return RedirectToAction("Index", "Login");
        }

 

        [HttpGet]
        public IActionResult GetRoomsByHomestay(string homestayId)
        {

            
            DataSet dsr = DataAccess.ROOMS_GET_LIST(homestayId, "-1", "-1", "3");

            var rooms = new List<object>();
            foreach (DataRow drr in dsr.Tables[0].Rows)
            {
                rooms.Add(new
                {
                    value = drr["ID"].ToString(),
                    text = drr["ROOMS_NAME"].ToString()
                });
            }

            return Json(rooms);
        }

        [HttpGet]
        public IActionResult GetCustomersByCCCD(string cccd)
        {
            string user_id = "";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {
                user_id = "-1";
            }
            else
            {
                if (_role == "owner")
                {

                    user_id = HttpContext.Session.GetString("ID");
                }
                else
                {
                    DataSet ds1 = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        user_id = dr1["ID"].ToString();
                    }
                    else return RedirectToAction("Index", "Login");
                }
            }
            DataSet ds = DataAccess.CUSTOMERS_GET_LIST(cccd, user_id, "3");
            var customers = new List<object>();
            foreach (DataRow drr in ds.Tables[0].Rows)
            {
                customers.Add(new
                {
                    value = drr["CUSTOMERS_CARD_NUMBER"].ToString(),
                    text = drr["CUSTOMERS_NAME"].ToString() + " - " + drr["CUSTOMERS_CARD_NUMBER"].ToString()
                });
            }

            return Json(customers);
        }


        [HttpPost]
        public IActionResult Add(Bills billModel)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            var homestayId = billModel.ID_HOMESTAYS;
            var userId = HttpContext.Session.GetString("ID");
            var customerCardId = billModel.CUSTOMERS_CARD_ID;

            ResponseObjs response = new ResponseObjs { errCode = "-1", errMsgs = "Thêm mới thất bại!" };

            try
            {
                
                foreach (var room in billModel.Rooms)
                {
                    var dsRoom = DataAccess.CHECK_ROOM_IN_USE("-1", room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                    var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError(dsRoom.Tables[0].Rows[0]["errMsg"].ToString());
                }
                // Tạo hóa đơn
                var ds = DataAccess.BILLS_INSERT(customerCardId, userId, homestayId);
                response.errCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                response.errMsgs = ds.Tables[0].Rows[0]["errMsg"].ToString();
                var bill_id = ds.Tables[0].Rows[0]["p_bills_id"].ToString();
                if (response.errCode != "0")
                    return ReturnWithError(response.errMsgs);

               

                // Thêm phòng
                foreach (var room in billModel.Rooms)
                {
                    var dsRoom = DataAccess.BILLS_ROOMS_INSERT(bill_id,room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                    var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError("Thêm mới phòng thất bại: " + dsRoom.Tables[0].Rows[0]["errMsg"].ToString());
                }

                // Thêm đồ ăn
                foreach (var food in billModel.Foods)
                {
                    var dsFood = DataAccess.BILLS_FOODS_INSERT(bill_id,food.FoodId, food.Quantity.ToString());
                    var errCode = dsFood.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError("Thêm mới đồ ăn thất bại: " + dsFood.Tables[0].Rows[0]["errMsg"].ToString());
                }

                // Thêm dịch vụ
                foreach (var service in billModel.Services)
                {
                    var dsService = DataAccess.BILLS_SERVICES_INSERT(bill_id,service.ServiceId, service.Quantity.ToString());
                    var errCode = dsService.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError("Thêm mới dịch vụ thất bại: " + dsService.Tables[0].Rows[0]["errMsg"].ToString());
                }

                TempData["Success"] = "Thêm mới thành công.";
                return RedirectToAction("Index", "Bills");
            }
            catch (Exception ex)
            {
                return ReturnWithError("Lỗi không xác định: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Edit(string ID,Bills billModel)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            var id = ID;
            if (!ModelState.IsValid)
            {
                var Message = "Dữ liệu không hợp lệ.";
                return ReturnWithError(Message,id);
            }
            if (!checkPIN(billModel.Save_code))
            {
                var Message = "Sai mã PIN";
                return ReturnWithError(Message, id);
            }
            var homestayId = billModel.ID_HOMESTAYS;
            var userId = HttpContext.Session.GetString("ID");
            var customerCardId = billModel.CUSTOMERS_CARD_ID;

            ResponseObjs response = new ResponseObjs { errCode = "-1", errMsgs = "Thêm mới thất bại!" };

            try
            {

                for (int i = 0; i < billModel.Rooms.Count; i++)
                {
                    for (int j = i + 1; j < billModel.Rooms.Count; j++)
                    {
                        if (billModel.Rooms[i].RoomId == billModel.Rooms[j].RoomId)
                        {
                            return ReturnWithError("Phát hiện trùng phòng: " + billModel.Rooms[i].RoomId, id);
                        }
                    }
                }

                foreach (var room in billModel.Rooms)
                {
                    var dsRoom = DataAccess.CHECK_ROOM_IN_USE(id, room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                    var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError(dsRoom.Tables[0].Rows[0]["errMsg"].ToString(), id);
                }
                // Cập nhật hóa đơn gốc
                var ds = DataAccess.BILLS_UPDATE(id,customerCardId, userId, homestayId,"1");
                response.errCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                response.errMsgs = ds.Tables[0].Rows[0]["errMsg"].ToString();

                if (response.errCode != "0")
                    return ReturnWithError(response.errMsgs, id);

                //Xóa các bảng liên kết
                var dsDel = DataAccess.BILLS_RFS_DELETE(id);
                var errrCode = dsDel.Tables[0].Rows[0]["errCode"].ToString();
                if (errrCode != "0")
                    return ReturnWithError("Xóa thất bại: " + dsDel.Tables[0].Rows[0]["errMsg"].ToString(),id);

                // Tạo mới lại các phòng
                foreach (var room in billModel.Rooms)
                {
                    var dsRoom = DataAccess.BILLS_ROOMS_UPDATE(id, room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                    var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError("Thêm mới phòng thất bại: " + dsRoom.Tables[0].Rows[0]["errMsg"].ToString(), id);
                }

                // Tạo mới lại đồ ăn
                foreach (var food in billModel.Foods)
                {
                    var dsFood = DataAccess.BILLS_FOODS_UPDATE(id,food.FoodId, food.Quantity.ToString());
                    var errCode = dsFood.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError("Thêm mới đồ ăn thất bại: " + dsFood.Tables[0].Rows[0]["errMsg"].ToString(), id);
                }

                // Tạo mới lại dịch vụ
                foreach (var service in billModel.Services)
                {
                    var dsService = DataAccess.BILLS_SERVICES_UPDATE(id, service.ServiceId, service.Quantity.ToString());
                    var errCode = dsService.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithError("Thêm mới dịch vụ thất bại: " + dsService.Tables[0].Rows[0]["errMsg"].ToString(), id);
                }

                TempData["Success"] = "Cập nhật thành công.";
                return RedirectToAction("Index", "Bills");
            }
            catch (Exception ex)
            {
                return ReturnWithError("Lỗi không xác định: " + ex.Message, id);
            }
        }






        private IActionResult ReturnWithError(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            var model = get_infor();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        private IActionResult ReturnWithError(string errorMessage,string id)
        {
            ViewBag.Message = errorMessage;
            var model = get_infor(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        private Bills get_infor()
        {
            string user_id = "-1";
            string bill_id = "-1";
            string ht_id = "-1";
            string v_type = "0";

            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {
                v_type = "1";
                user_id = HttpContext.Session.GetString("ID");
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
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                        v_type = "1";
                    }
                    else return null;
                }
            }
            var model = new Bills();

            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id, "1");
            model.HomestayOptions = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var _ht_id = dr["ID"].ToString();
                var _ht_name = dr["HOMESTAYS_NAME"].ToString();
                model.HomestayOptions.Add(new SelectListItem
                {
                    Text = _ht_name,
                    Value = _ht_id
                });
            }

            DataSet dsr = DataAccess.ROOMS_GET_LIST(ht_id, "-1", user_id, "3");
            model.RoomOptions = new List<SelectListItem>();
            foreach (DataRow drr in dsr.Tables[0].Rows)
            {
                var _r_id = drr["ID"].ToString();
                var _r_name = drr["ROOMS_NAME"].ToString();
                model.RoomOptions.Add(new SelectListItem
                {
                    Text = _r_name,
                    Value = _r_id
                });
            }

            DataSet dsf = DataAccess.FOODS_GET_LIST("-1", user_id, "1");
            model.FoodDrinkOptions = new List<SelectListItem>();
            foreach (DataRow drf in dsf.Tables[0].Rows)
            {
                var _f_id = drf["ID"].ToString();
                var _f_name = drf["FOODS_NAME"].ToString();
                var _f_price = int.Parse(drf["FOODS_PRICE"].ToString());

                model.FoodDrinkOptions.Add(new SelectListItem
                {
                    Text = _f_name,
                    Value = _f_id
                });
            }

            DataSet dss = DataAccess.SERVICES_GET_LIST("-1", user_id, "1");
            model.ServiceOptions = new List<SelectListItem>();
            foreach (DataRow drs in dss.Tables[0].Rows)
            {
                var _s_id = drs["ID"].ToString();
                var _s_name = drs["SERVICES_NAME"].ToString();
                model.ServiceOptions.Add(new SelectListItem
                {
                    Text = _s_name,
                    Value = _s_id
                });
            }

            DataSet dsc = DataAccess.CUSTOMERS_GET_LIST("-1", user_id, "1");
            model.CustomerOptions = new List<SelectListItem>();
            foreach (DataRow drc in dsc.Tables[0].Rows)
            {
                var _c_id = drc["CUSTOMERS_CARD_NUMBER"].ToString();
                var _c_name = drc["CUSTOMERS_NAME"].ToString();
                model.CustomerOptions.Add(new SelectListItem
                {
                    Text = _c_name + " - " + _c_id,
                    Value = _c_id
                });
            }
            return model;

        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            Bills model = new Bills();
            model = get_infor(id);
            if (model != null) return View(model);
            else return RedirectToAction("Index", "Bills");
        }

        private Bills get_infor(string id)
        {
            string user_id = "-1";
            string bill_id = id;
            string ht_id = "-1";
            string v_type = "0";
            string bill_status = "-1";
            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {
                user_id = HttpContext.Session.GetString("ID");
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
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                        bill_status = "UNPAID";
                        v_type = "1";
                    }
                    else return null;
                }
            }
            var model = new Bills();
            model.Rooms = new List<RoomDetail>();
            model.Foods = new List<FoodDetail>();
            model.Services = new List<ServiceDetail>();
            string selectedHomestayId = "";
            string selectedCustomer = "";
            var dsht = DataAccess.BILLS_GET_LIST(bill_id, "-1", user_id,bill_status, "1");
            if (dsht.Tables[0].Rows.Count > 0)
            {
                DataRow drht = dsht.Tables[0].Rows[0];
                selectedHomestayId = drht["ID_HOMESTAYS"].ToString();
                selectedCustomer = drht["CUSTOMERS_CARD_ID"].ToString();
            }
            else return null;


            //Homestay
            DataSet ds = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id, "1");
            model.HomestayOptions = new List<SelectListItem>();
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var _ht_id = dr["ID"].ToString();
                var _ht_name = dr["HOMESTAYS_NAME"].ToString();
                model.HomestayOptions.Add(new SelectListItem
                {
                    Text = _ht_name,
                    Value = _ht_id,
                    Selected = (_ht_id == selectedHomestayId)
                });
            }

            //Customer
            DataSet dsc = DataAccess.CUSTOMERS_GET_LIST("-1", user_id, "1");
            model.CustomerOptions = new List<SelectListItem>();
            foreach (DataRow drc in dsc.Tables[0].Rows)
            {
                
                var _c_id = drc["CUSTOMERS_CARD_NUMBER"].ToString();
                var _c_name = drc["CUSTOMERS_NAME"].ToString();
                
                model.CustomerOptions.Add(new SelectListItem
                {
                    Text = _c_name + " - " + _c_id,
                    Value = _c_id,
                    Selected = (_c_id == selectedCustomer)
                });
            }

            //Room

            var dsr_selected = DataAccess.BILLS_ROOMS_GET_LIST(bill_id);
            var selectedRoomIds = new HashSet<string>();
            foreach (DataRow dr_selected in dsr_selected.Tables[0].Rows)
            {
                var roomId = dr_selected["ROOMS_ID"].ToString();
                selectedRoomIds.Add(roomId);
                model.Rooms.Add(new RoomDetail
                {
                    RoomId = roomId,
                    ROOMS_NAME =  dr_selected["ROOMS_NAME"].ToString()
                });
            }

            DataSet dsr = DataAccess.ROOMS_GET_LIST(ht_id, "-1", user_id, "3");
            model.RoomOptions = new List<SelectListItem>();
            foreach (DataRow drr in dsr.Tables[0].Rows)
            {
                var _r_id = drr["ID"].ToString();
                var _r_name = drr["ROOMS_NAME"].ToString();
                model.RoomOptions.Add(new SelectListItem
                {
                    Text = _r_name,
                    Value = _r_id
                });
            }



            //Food
            var dsf_selected = DataAccess.BILLS_FOODS_GET_LIST(bill_id);
            var selectedFoodIds = new HashSet<string>();
            foreach (DataRow drf_selected in dsf_selected.Tables[0].Rows)
            {
                var foodId = drf_selected["FOODS_ID"].ToString();
                selectedFoodIds.Add(foodId);

                model.Foods.Add(new FoodDetail
                {
                    FoodId = foodId,
                    Quantity =Convert.ToInt32(drf_selected["QUANTITY_FOOD"])
                });
            }

            DataSet dsf = DataAccess.FOODS_GET_LIST("-1", user_id, "1");
            model.FoodDrinkOptions = new List<SelectListItem>();
            foreach (DataRow drf in dsf.Tables[0].Rows)
            {
                var _f_id = drf["ID"].ToString();
                var _f_name = drf["FOODS_NAME"].ToString();
                var _f_price = int.Parse(drf["FOODS_PRICE"].ToString());

                model.FoodDrinkOptions.Add(new SelectListItem
                {
                    Text = _f_name,
                    Value = _f_id
                });
            }


            //service
            var dss_selected = DataAccess.BILLS_SERVICES_GET_LIST(bill_id);
            var selectedServiceIds = new HashSet<string>();
            foreach (DataRow drs_selected in dss_selected.Tables[0].Rows)
            {
                var serviceId = drs_selected["SERVICES_ID"].ToString();
                selectedServiceIds.Add(serviceId);
                model.Services.Add(new ServiceDetail
                {
                    ServiceId = serviceId,
                    Quantity = Convert.ToInt32(drs_selected["QUANTITY_SERVICE"])
                });
            }

            DataSet dss = DataAccess.SERVICES_GET_LIST("-1", user_id, "1");
            model.ServiceOptions = new List<SelectListItem>();
            foreach (DataRow drs in dss.Tables[0].Rows)
            {
                var _s_id = drs["ID"].ToString();
                var _s_name = drs["SERVICES_NAME"].ToString();
                model.ServiceOptions.Add(new SelectListItem
                {
                    Text = _s_name,
                    Value = _s_id
                });
            }



            
            return model;

        }


        [HttpGet]
        public IActionResult Delete(string id)
        {

            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }

            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
            {
                var user_id = HttpContext.Session.GetString("ID");
                DataSet ds = DataAccess.BILLS_GET_LIST(id,"-1", user_id,"-1", "1");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Bills _obj = new Bills();
                    _obj.ID = id;
                    return View(_obj);
                }
                return RedirectToAction("Index", "Bills");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult Delete(string id, Bills model)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            if (!checkPIN(model.Save_code))
            {
                ViewBag.Message = "Mã PIN không chính xác.";
                return View(model);
            }

            DataSet ds = DataAccess.BILLS_UPDATE(id, "","","", "2"); ;
            var errCode = ds.Tables[0].Rows[0]["errCode"].ToString();

            if (errCode == "0")
            {
                TempData["Success"] = "Xóa thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Đã có lỗi xảy ra!!";
            return View(model);
        }



        [HttpGet]
        public IActionResult Details(string id)
        {
            if(!CheckAuthToken()) return RedirectToAction("Index", "Login");
            string user_id = "-1";
            string bill_id = id;
            string ht_id = "-1";
            string v_type = "0";

            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin")
            {
                user_id = HttpContext.Session.GetString("ID");
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
                        ht_id = HttpContext.Session.GetString("Homestays_Id");
                        v_type = "1";
                    }
                    else  return RedirectToAction("Index", "Login");
                }
            }

            var model = new Bills();
            model.Rooms = new List<RoomDetail>();
            model.Foods = new List<FoodDetail>();
            model.Services = new List<ServiceDetail>();
            var dsht = DataAccess.BILLS_GET_LIST(bill_id, "-1", "-1","-1", "1");
            if (dsht.Tables[0].Rows.Count > 0)
            {
                DataRow drht = dsht.Tables[0].Rows[0];
                model.ID_HOMESTAYS = drht["ID_HOMESTAYS"].ToString();
                model.HOMESTAYS_NAME = drht["HOMESTAYS_NAME"].ToString();
                model.HOMESTAYS_ADDRESS = drht["HOMESTAYS_ADDRESS"].ToString();
                model.MANAGER_PHONE = drht["MANAGER_PHONE"].ToString();

                model.CUSTOMERS_CARD_ID = drht["CUSTOMERS_CARD_ID"].ToString();
                model.CUSTOMERS_NAME = drht["CUSTOMERS_NAME"].ToString();
                model.CUSTOMERS_ADDRESS = drht["CUSTOMERS_ADDRESS"].ToString();
                model.CUSTOMERS_PHONE = drht["CUSTOMERS_PHONE"].ToString();

                model.ID = drht["ID"].ToString();
                model.CREATE_AT = Convert.ToDateTime(drht["CREATE_AT"].ToString());
                model.UPDATE_AT = Convert.ToDateTime(drht["UPDATE_AT"].ToString());
                model.UPDATE_BY = drht["UPDATE_BY"].ToString();
                model.CREATE_BY = drht["CREATE_BY"].ToString();
                var _status = drht["BILLS_STATUS"].ToString();
                if (_status == "PAID") model.BILLS_STATUS = "Đã thanh toán";
                else model.BILLS_STATUS = "Chưa thanh toán";
                model.TOTAL_MONEY = int.TryParse(drht["TOTAL_MONEY"]?.ToString(), out int value) ? value : 0;

            }
            else
            {
                return RedirectToAction("Index", "Bills");
            }

            var dsr_selected = DataAccess.BILLS_ROOMS_GET_LIST(bill_id);
            if (dsr_selected.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_selected in dsr_selected.Tables[0].Rows)
                {
                    var roomId = dr_selected["ROOMS_ID"].ToString();
                    model.Rooms.Add(new RoomDetail
                    {
                        RoomId = roomId,
                        ROOMS_NAME = dr_selected["ROOMS_NAME"].ToString(),
                        CheckInDate = Convert.ToDateTime(dr_selected["CHECKIN_DATE"]),
                        CheckOutDate = Convert.ToDateTime(dr_selected["CHECKOUT_DATE"]),
                        PRICE_PER_DAY = Convert.ToInt32(dr_selected["PRICE_PER_DAY"]),
                        TOTAL_DAYS = Convert.ToInt32(dr_selected["TOTAL_DAYS"]),
                        TOTAL_PRICE = Convert.ToInt32(dr_selected["TOTAL_PRICE"])
                    });
                }
            }

                var dsf_selected = DataAccess.BILLS_FOODS_GET_LIST(bill_id);
            if (dsf_selected.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow drf_selected in dsf_selected.Tables[0].Rows)
                {
                    var foodId = drf_selected["FOODS_ID"].ToString();

                    model.Foods.Add(new FoodDetail
                    {
                        FoodId = foodId,
                        FOODS_NAME = drf_selected["FOODS_NAME"].ToString(),
                        Quantity = Convert.ToInt32(drf_selected["QUANTITY_FOOD"]),
                        UNIT_PRICE_FOOD = Convert.ToInt32(drf_selected["UNIT_PRICE_FOOD"]),
                        TOTAL_PRICE_FOOD = Convert.ToInt32(drf_selected["TOTAL_PRICE_FOOD"])
                    });
                }
            }
            var dss_selected = DataAccess.BILLS_SERVICES_GET_LIST(bill_id);
            if (dss_selected.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drs_selected in dss_selected.Tables[0].Rows)
                {
                    var serviceId = drs_selected["SERVICES_ID"].ToString();
                    model.Services.Add(new ServiceDetail
                    {
                        ServiceId = serviceId,
                        SERVICES_NAME = drs_selected["SERVICES_NAME"].ToString(),
                        Quantity = Convert.ToInt32(drs_selected["QUANTITY_SERVICE"]),
                        UNIT_PRICE_SERVICE = Convert.ToInt32(drs_selected["UNIT_PRICE_SERVICE"]),
                        TOTAL_PRICE_SERVICE = Convert.ToInt32(drs_selected["TOTAL_PRICE_SERVICE"])
                    });
                }
            }
            return View(model);
        }
        [HttpPost]
        public  IActionResult pay(string id,Bills model)
        {
            if (!CheckAuthToken()) return RedirectToAction("Index", "Login");
            var name = HttpContext.Session.GetString("Name");
            var dspay = DataAccess.BILLS_PAY(id,name);
            var errCode = dspay.Tables[0].Rows[0]["errCode"].ToString();
            var errMsgs = dspay.Tables[0].Rows[0]["errMsg"].ToString();
            if(errCode != "0")
            {
                ViewBag.Message = errMsgs;
                return View();
            }
            else
            {
                var type_report = "Thu";
                var v_category = "Thanh toán hóa đơn";
                var v_descript = "Thanh toán hóa đơn tại cơ sở: " + model.HOMESTAYS_NAME + ". Khách hàng: " + model.CUSTOMERS_NAME + ". ngày thanh toán: " + System.DateTime.Now.ToString() + ".";
                var v_bills_id = id;
                var v_create_by = HttpContext.Session.GetString("ID");
                var v_homestay_id = model.ID_HOMESTAYS;
                var v_amount = model.TOTAL_MONEY.ToString();
                var v_type = "2";
                var dspay1 = DataAccess.REPORT_INSERT(type_report,v_category,v_descript,v_bills_id,v_create_by,v_homestay_id,v_amount,v_type);
                var errCode1 = dspay.Tables[0].Rows[0]["errCode"].ToString();
                var errMsgs1 = dspay.Tables[0].Rows[0]["errMsg"].ToString();
                if (errCode1 != "0")
                {
                    ViewBag.Message = errMsgs;
                    return View();
                }
                else
                {
                    TempData["Success"] = errMsgs;
                    return RedirectToAction("Index");
                }
                
            }
        }



        [HttpGet]
        public IActionResult Filter(int? HomestayId, int? BILL_TYPE_VALUE)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }


            string _role = HttpContext.Session.GetString("Role");
            string userID = "-1";
            string ht_id = "-1";
            string bill_id = "-1";
            string v_type = "1";

            if (_role == "admin")
            {
            }
            else if (_role == "owner")
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
                    ht_id = HttpContext.Session.GetString("Homestays_Id");
                }
                else return RedirectToAction("Index", "Login");
            }
            // Lấy danh sách homestay để hiển thị lại trong view
            DataSet dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, userID, "1");
            List<SelectListItem> homestayList = new List<SelectListItem>();
            foreach (DataRow dr in dsHomestays.Tables[0].Rows)
            {
                homestayList.Add(new SelectListItem
                {
                    Value = dr["ID"].ToString(),
                    Text = dr["HOMESTAYS_NAME"].ToString()
                });
            }
            ViewBag.HomestayList = homestayList;

            // Format tham số lọc
            string _id_homestay = (HomestayId.HasValue && HomestayId.Value != 0) ? HomestayId.Value.ToString() : "-1";
            string bill_status = (BILL_TYPE_VALUE.HasValue && BILL_TYPE_VALUE.Value != 0) ? "PAID" : "UNPAID";
            if (_role == "manager") _id_homestay = ht_id;

            DataSet ds = DataAccess.BILLS_GET_LIST(bill_id, _id_homestay, userID, bill_status, v_type);
            List<Bills> bills = new List<Bills>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Bills _obj = new Bills();
                _obj.ID = dr["ID"].ToString();
                var _status = dr["BILLS_STATUS"].ToString();
                if (_status == "PAID") _obj.BILLS_STATUS = "Đã thanh toán";
                else _obj.BILLS_STATUS = "Chưa thanh toán";
                _obj.TOTAL_MONEY = int.TryParse(dr["TOTAL_MONEY"]?.ToString(), out int value) ? value : 0;
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

            return View("Index", bills);
        }
    }
}
