using AspNetCoreGeneratedDocument;
using HomeStay_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResfullApi.Models;
using System.Buffers;
using System.Data;
using System.Security.Cryptography;

namespace HomeStay_MVC.Controllers
{
    public class BillsController : BaseController
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BillsController));
        public IActionResult Index(int page = 1, int pageSize = 5)
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
            //  Phân trang dữ liệu
            int totalItems = bills.Count;
            var pagedBills = bills
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.HomestayId = "0";
            ViewBag.BillType = "3";

            logger.Info("Pro select all bill success.");
            return View(pagedBills);
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



        [HttpGet]
        public IActionResult Add()
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            Bills model = new Bills();
            PrepareDropDownOptions(model);
            if(model!=null) return View(model);
            else  return RedirectToAction("Index", "Login");
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
                bool hasRoom = billModel.Rooms != null && billModel.Rooms.Count > 0;
                bool hasService = billModel.Services != null && billModel.Services.Count > 0;
                bool hasFood = billModel.Foods != null && billModel.Foods.Count > 0;
                if (!hasRoom && !hasService && !hasFood)
                {
                    return ReturnWithErrorAdd("Cần chọn ít nhất 1 phòng/ dịch vụ/ món ăn để lưu hóa đơn", billModel);
                }

                if (hasRoom)
                {
                    for (int i = 0; i < billModel.Rooms.Count; i++)
                    {


                        if (billModel.Rooms[i].CheckInDate > billModel.Rooms[i].CheckOutDate)
                        {
                            i += 1;
                            return ReturnWithErrorAdd("Ngày nhận phòng phải nhỏ hơn ngày trả phòng: phòng thứ " + i, billModel);

                        }
                    }
                    foreach (var room in billModel.Rooms)
                    {

                        var dsRoom = DataAccess.CHECK_ROOM_IN_USE("-1", room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                        var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                        if (errCode != "0")
                            return ReturnWithErrorAdd(dsRoom.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                    }
                }
                // Tạo hóa đơn
                var ds = DataAccess.BILLS_INSERT(customerCardId, userId, homestayId);
                response.errCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                response.errMsgs = ds.Tables[0].Rows[0]["errMsg"].ToString();
                var bill_id = ds.Tables[0].Rows[0]["p_bills_id"].ToString();
                if (response.errCode != "0")
                    return ReturnWithErrorAdd(response.errMsgs, billModel);



                // Thêm phòng
                foreach (var room in billModel.Rooms)
                {
                    var dsRoom = DataAccess.BILLS_ROOMS_INSERT(bill_id, room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                    var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithErrorAdd("Thêm mới phòng thất bại: " + dsRoom.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                }

                // Thêm đồ ăn
                foreach (var food in billModel.Foods)
                {
                    var dsFood = DataAccess.BILLS_FOODS_INSERT(bill_id, food.FoodId, food.Quantity.ToString());
                    var errCode = dsFood.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithErrorAdd("Thêm mới đồ ăn thất bại: " + dsFood.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                }

                // Thêm dịch vụ
                foreach (var service in billModel.Services)
                {
                    var dsService = DataAccess.BILLS_SERVICES_INSERT(bill_id, service.ServiceId, service.Quantity.ToString());
                    var errCode = dsService.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithErrorAdd("Thêm mới dịch vụ thất bại: " + dsService.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                }

                TempData["Success"] = "Thêm mới thành công.";
                return RedirectToAction("Index", "Bills");
            }
            catch (Exception ex)
            {
                return ReturnWithErrorAdd("Lỗi không xác định: " + ex.Message, billModel);
            }
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



        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (!CheckAuthToken())
            {
                return RedirectToAction("Index", "Login");
            }
            Bills model = new Bills();
            get_infor(id, model);
            PrepareDropDownOptions(model);
            return View(model);
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
                return ReturnWithErrorEdit(Message,billModel);
            }
            if (!checkPIN(billModel.Save_code))
            {
                var Message = "Sai mã PIN";
                return ReturnWithErrorEdit(Message, billModel);
            }
            var homestayId = billModel.ID_HOMESTAYS;
            var userId = HttpContext.Session.GetString("ID");
            var customerCardId = billModel.CUSTOMERS_CARD_ID;

            ResponseObjs response = new ResponseObjs { errCode = "-1", errMsgs = "Thêm mới thất bại!" };

            try
            {

                bool hasRoom = billModel.Rooms != null && billModel.Rooms.Count > 0;
                bool hasService = billModel.Services != null && billModel.Services.Count > 0;
                bool hasFood = billModel.Foods != null && billModel.Foods.Count > 0;
                if (!hasRoom && !hasService && !hasFood)
                {
                    return ReturnWithErrorEdit("Cần chọn ít nhất 1 phòng/ dịch vụ/ món ăn để lưu hóa đơn", billModel);
                }

                if (hasRoom)
                {

                    for (int i = 0; i < billModel.Rooms.Count; i++)
                    {
                        for (int j = i + 1; j < billModel.Rooms.Count; j++)
                        {
                            if (billModel.Rooms[i].RoomId == billModel.Rooms[j].RoomId)
                            {
                                return ReturnWithErrorEdit("Phát hiện trùng phòng thứ " + j, billModel);
                            }
                        }
                    }
                    for (int i = 0; i < billModel.Rooms.Count; i++)
                    {


                        if (billModel.Rooms[i].CheckInDate > billModel.Rooms[i].CheckOutDate)
                        {
                            i += 1;
                            return ReturnWithErrorEdit("Ngày nhận phòng phải nhỏ hơn ngày trả phòng: phòng thứ " + i, billModel);
                        }
                    }
                    foreach (var room in billModel.Rooms)
                    {

                        var dsRoom = DataAccess.CHECK_ROOM_IN_USE(id, room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                        var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                        if (errCode != "0")
                            return ReturnWithErrorEdit(dsRoom.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                    }
                }

                

                // Cập nhật hóa đơn gốc
                var ds = DataAccess.BILLS_UPDATE(id,customerCardId, userId, homestayId,"1");
                response.errCode = ds.Tables[0].Rows[0]["errCode"].ToString();
                response.errMsgs = ds.Tables[0].Rows[0]["errMsg"].ToString();

                if (response.errCode != "0")
                    return ReturnWithErrorEdit(response.errMsgs, billModel);

                //Xóa các bảng liên kết
                var dsDel = DataAccess.BILLS_RFS_DELETE(id);
                var errrCode = dsDel.Tables[0].Rows[0]["errCode"].ToString();
                if (errrCode != "0")
                    return ReturnWithErrorEdit("Xóa thất bại: " + dsDel.Tables[0].Rows[0]["errMsg"].ToString(),billModel);

                // Tạo mới lại các phòng
                foreach (var room in billModel.Rooms)
                {
                    var dsRoom = DataAccess.BILLS_ROOMS_UPDATE(id, room.RoomId, room.CheckInDate.ToString("yyyy-MM-dd"), room.CheckOutDate.ToString("yyyy-MM-dd"));
                    var errCode = dsRoom.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithErrorEdit("Thêm mới phòng thất bại: " + dsRoom.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                }

                // Tạo mới lại đồ ăn
                foreach (var food in billModel.Foods)
                {
                    var dsFood = DataAccess.BILLS_FOODS_UPDATE(id,food.FoodId, food.Quantity.ToString());
                    var errCode = dsFood.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithErrorEdit("Thêm mới đồ ăn thất bại: " + dsFood.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                }

                // Tạo mới lại dịch vụ
                foreach (var service in billModel.Services)
                {
                    var dsService = DataAccess.BILLS_SERVICES_UPDATE(id, service.ServiceId, service.Quantity.ToString());
                    var errCode = dsService.Tables[0].Rows[0]["errCode"].ToString();
                    if (errCode != "0")
                        return ReturnWithErrorEdit("Thêm mới dịch vụ thất bại: " + dsService.Tables[0].Rows[0]["errMsg"].ToString(), billModel);
                }

                TempData["Success"] = "Cập nhật thành công.";
                return RedirectToAction("Index", "Bills");
            }
            catch (Exception ex)
            {
                return ReturnWithErrorEdit("Lỗi không xác định: " + ex.Message, billModel);
            }
        }







        private IActionResult ReturnWithErrorEdit(string errorMessage, Bills model)
        {
            ViewBag.Message = errorMessage;

            PrepareDropDownOptions(model);

            return View("Edit", model);
        }
        private IActionResult ReturnWithErrorAdd(string errorMessage, Bills model)
        {
            ViewBag.Message = errorMessage;

            PrepareDropDownOptions(model);

            return View("Add", model);
        }


        private void PrepareDropDownOptions(Bills model)
        {
            string user_id, ht_id, v_type;
            GetUserContext(out user_id, out ht_id, out v_type);

            // HOMESTAY dropdown
            var ds = DataAccess.HOMESTAYS_GET_LIST(ht_id, user_id, v_type);
            model.HomestayOptions = ds.Tables[0].AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Text = row["HOMESTAYS_NAME"].ToString(),
                    Value = row["ID"].ToString(),
                    Selected = (row["ID"].ToString() == model.ID_HOMESTAYS)
                }).ToList();

            // CUSTOMERS dropdown
            var dsc = DataAccess.CUSTOMERS_GET_LIST("-1", user_id, "1");
            model.CustomerOptions = dsc.Tables[0].AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Text = $"{row["CUSTOMERS_NAME"]} - {row["CUSTOMERS_CARD_NUMBER"]}",
                    Value = row["CUSTOMERS_CARD_NUMBER"].ToString(),
                    Selected = (row["CUSTOMERS_CARD_NUMBER"].ToString() == model.CUSTOMERS_CARD_ID)
                }).ToList();

            // ROOMS dropdown
            var dsr = DataAccess.ROOMS_GET_LIST(ht_id, "-1", user_id, "3");
            model.RoomOptions = dsr.Tables[0].AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Text = row["ROOMS_NAME"].ToString(),
                    Value = row["ID"].ToString()
                }).ToList();

            // FOODS dropdown
            var dsf = DataAccess.FOODS_GET_LIST("-1", user_id, "1");
            model.FoodDrinkOptions = dsf.Tables[0].AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Text = row["FOODS_NAME"].ToString(),
                    Value = row["ID"].ToString()
                }).ToList();

            // SERVICES dropdown
            var dss = DataAccess.SERVICES_GET_LIST("-1", user_id, "1");
            model.ServiceOptions = dss.Tables[0].AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Text = row["SERVICES_NAME"].ToString(),
                    Value = row["ID"].ToString()
                }).ToList(); 
        }


    



        private void get_infor(string id,Bills model)
        {
            string user_id, ht_id, v_type;
            GetUserContext(out user_id, out ht_id, out v_type);
            string bill_id = id;
            string bill_status = "-1";

            model.Rooms = new List<RoomDetail>();
            model.Foods = new List<FoodDetail>();
            model.Services = new List<ServiceDetail>();
            string selectedHomestayId = "";
            string selectedCustomer = "";
            var dsht = DataAccess.BILLS_GET_LIST(bill_id, ht_id, user_id,bill_status, v_type);
            if (dsht.Tables[0].Rows.Count > 0)
            {
                DataRow drht = dsht.Tables[0].Rows[0];
                selectedHomestayId = drht["ID_HOMESTAYS"].ToString();
                selectedCustomer = drht["CUSTOMERS_CARD_ID"].ToString();
            }
            model.ID_HOMESTAYS = selectedHomestayId;
            model.CUSTOMERS_CARD_ID = selectedCustomer;



            //Room

            var dsr_selected = DataAccess.BILLS_ROOMS_GET_LIST(bill_id);
            var selectedRoomIds = new HashSet<string>();
            foreach (DataRow dr_selected in dsr_selected.Tables[0].Rows)
            {
                string roomId = dr_selected["ROOMS_ID"].ToString();
                string roomName = dr_selected["ROOMS_NAME"].ToString();

                DateTime checkInDate = DateTime.MinValue;
                DateTime checkOutDate = DateTime.MinValue;

                if (dr_selected["CHECKIN_DATE"] != DBNull.Value)
                {
                    checkInDate = Convert.ToDateTime(dr_selected["CHECKIN_DATE"]);
                }

                if (dr_selected["CHECKOUT_DATE"] != DBNull.Value)
                {
                    checkOutDate = Convert.ToDateTime(dr_selected["CHECKOUT_DATE"]);
                }

                RoomDetail roomDetail = new RoomDetail
                {
                    RoomId = roomId,
                    ROOMS_NAME = roomName,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate
                };

                model.Rooms.Add(roomDetail);
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

        }


        private void GetUserContext(out string user_id, out string ht_id, out string v_type)
        {
            user_id = "-1";
            ht_id = "-1";
            v_type = "0";

            string _role = HttpContext.Session.GetString("Role");
            if (_role == "admin" || _role == "owner")
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
            {   var user_confirm = "Nhân viên thanh toán: " + name + " - CCCD: " + HttpContext.Session.GetString("CARD_NUMBER");
                var type_report = "Thu";
                var v_category = "Thanh toán hóa đơn";
                var v_descript = "Thanh toán hóa đơn tại cơ sở: " + model.HOMESTAYS_NAME + ". Khách hàng: " + model.CUSTOMERS_NAME +". "+ user_confirm+". ngày thanh toán: " + System.DateTime.Now.ToString() + ".";
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
                    TempData["Success"] = "Xác nhận thanh toán thành công";
                    return RedirectToAction("Index");
                }
                
            }
        }



        [HttpGet]
        public IActionResult Filter(int? HomestayId, int? BILL_TYPE_VALUE, int page = 1, int pageSize = 5)
        {
            if (!CheckAuthToken())
                return RedirectToAction("Index", "Login");

            string _role = HttpContext.Session.GetString("Role");
            string userID = "-1", ht_id = "-1";

            // Xác định user và homestay
            if (_role == "admin")
            {
                // admin xem toàn bộ
            }
            else if (_role == "owner")
            {
                userID = HttpContext.Session.GetString("ID");
            }
            else
            {
                var ds = DataAccess.USERS_GET_LIST(HttpContext.Session.GetString("Create_By"));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var dr = ds.Tables[0].Rows[0];
                    userID = dr["ID"].ToString();
                    ht_id = HttpContext.Session.GetString("Homestays_Id");
                }
                else return RedirectToAction("Index", "Login");
            }

            // Lấy danh sách homestay
            var dsHomestays = DataAccess.HOMESTAYS_GET_LIST(ht_id, userID, "1");
            var homestayList = dsHomestays.Tables[0].AsEnumerable()
                .Select(dr => new SelectListItem
                {
                    Value = dr["ID"].ToString(),
                    Text = dr["HOMESTAYS_NAME"].ToString()
                }).ToList();
            ViewBag.HomestayList = homestayList;

            // Lọc
            string selectedHomestay = (HomestayId.HasValue && HomestayId.Value != 0) ? HomestayId.Value.ToString() : "-1";
            if (_role == "manager") selectedHomestay = ht_id;

            string billStatus;
            if(BILL_TYPE_VALUE == 3)
            {
                billStatus = "-1";
            }
            else
            {
                billStatus = (BILL_TYPE_VALUE.HasValue && BILL_TYPE_VALUE.Value == 1) ? "PAID" : "UNPAID";
            }
                string billId = "-1", v_type = "1";

            var dsBills = DataAccess.BILLS_GET_LIST(billId, selectedHomestay, userID, billStatus, v_type);

            List<Bills> bills = new List<Bills>();
            if (dsBills.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsBills.Tables[0].Rows)
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
            }
            else
            {
                ViewBag.notfound = "Không tìm thấy kết quả phù hợp";
            }


            // Phân trang
            int totalItems = bills.Count;
            var pagedBills = bills.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.HomestayId = HomestayId;
            ViewBag.BillType = BILL_TYPE_VALUE;

            return View("Index", pagedBills);
        }

    }
}
