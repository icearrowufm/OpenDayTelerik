using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenDayTelerik.Models;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;

namespace OpenDayTelerik.Controllers
{
    public class HomeController : Controller
    {
        readonly ODdataDataContext ODdata = new ODdataDataContext(ConfigurationManager.ConnectionStrings["OpendayConnectionString"].ConnectionString);
        public ActionResult RequestPage()
        {
            return View();
        }

        public ActionResult Download()
        {
            var user = Session["user"]?.ToString();
            // Log kiểm tra ID được lưu
            System.Diagnostics.Debug.WriteLine($"User ID stored in session: {user}");
            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("RequestPage", "Home");
            }

            var model = ODdata.View_Regist_Fulls.Where(w => w.USER_ID == user).OrderBy(o => o.GROUPCLASS).ToList();
            if (model.Count == 0)
            {
                return Content(@"
                    <!DOCTYPE html>
                    <html lang='vi'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Thông báo</title>
                        <style>
                            body {
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                height: 100vh;
                                margin: 0;
                                background-color: #f0f0f0;
                            }
                            .notification {
                                background-color: #fff;
                                border: 1px solid #ccc;
                                border-radius: 8px;
                                padding: 20px;
                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                                text-align: center;
                                width: 300px;
                            }
                            .notification h2 {
                                margin: 0;
                                color: #333;
                            }
                            .notification p {
                                color: #555;
                            }
                            .close-btn {
                                background-color: #007BFF;
                                color: white;
                                border: none;
                                border-radius: 5px;
                                padding: 10px 15px;
                                cursor: pointer;
                            }
                            .close-btn:hover {
                                background-color: #0056b3;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='notification' id='notification'>
                            <h2>Thông báo</h2>
                            <p>Bạn vui lòng đăng ký nội dung</p>
                            <button class='close-btn' onclick='redirectToCreate()'>Đóng</button>
                        </div>

                        <script>
                            function redirectToCreate() {
                                window.location.replace('/Home/Index');
                            }

                            setTimeout(redirectToCreate, 3000); // Thay đổi thời gian chờ ở đây (3000ms = 3 giây)
                        </script>
                    </body>
                    </html>
                    ");
            }
            else { return View(model); }
            
        }

        public ActionResult khaimac()
        {
            var date = new DateTime(2025, 12, 02,7,0,0);
            if (DateTime.Now > date)
            {
                return RedirectToAction("RequestPage", "Home");
            }
            return null; // Hoặc bạn có thể trả về một view mặc định nếu cần.
        }

        public ActionResult admin()
        {
            var user = Session["user"]?.ToString();
            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("RequestPage", "Home");
            }

            var no = ODdata.USERs.FirstOrDefault(w => w.ID.Trim() == user).NO;
            //if (user != null && no > 10)
            //{
            //    return RedirectToAction("RequestPage", "Home");
            //}
             if (user != null && no <= 1)
            { return RedirectToAction("Index", "Regist"); }
            return null; // Hoặc bạn có thể trả về một view mặc định nếu cần.
        }


        public ActionResult Index()
        {
            var redirectResult = khaimac();
            var usercheck = admin();
            if (usercheck != null)
            { return usercheck; }
            else if (redirectResult != null) // Nếu không phải null, nghĩa là có yêu cầu redirect
            {
                return redirectResult; // Trả về kết quả redirect
            }

            var user = Session["user"]?.ToString();
            // Log kiểm tra ID được lưu
            System.Diagnostics.Debug.WriteLine($"User ID stored in session: {user}");

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("RequestPage", "Home");
            }

            // Khởi tạo trạng thái session ban đầu
            InitializeSessionStatus();

            var moc1 = new TimeSpan(9, 30, 0);
            var moc2 = new TimeSpan(11, 0, 0);
            var moc3 = new TimeSpan(13, 30, 0);
            var moc4 = new TimeSpan(15, 00, 0);

            // Lấy danh sách đăng ký của user
            var userRegistrations = ODdata.REGISTs.Where(w => w.USER_ID.ToString() == user).ToList();

            // Duyệt qua các đăng ký của user
            foreach (var registration in userRegistrations)
            {
                var classModel = ODdata._CLASSes.FirstOrDefault(w => w.ID == registration.CLASS_ID);
                if (classModel == null) continue;

                int groupClass = int.Parse(classModel.GROUPCLASS.ToString());
                string type = classModel.TYPE.ToString();

                // Kiểm tra số lượng đăng ký loại 2 và loại 3
                int checkCol2Count = userRegistrations.Count(w => w.TYPE.Trim() == "2");
               // int checkCol3Count = userRegistrations.Count(w => w.TYPE.Trim() == "3");

                // Cập nhật trạng thái session cho các lớp loại 2
                if (checkCol2Count > 1)
                {
                    SetUnavailableForType("2", groupClass);
                }

                // Cập nhật trạng thái session cho các lớp loại 3
                //if (checkCol3Count > 0)
                //{
                //    SetUnavailableForType("3", groupClass);
                //}

                // Đánh dấu lớp đã đăng ký là "active"
                SetActiveClassStatus(userRegistrations);
            }

            // Cập nhật trạng thái theo thời gian hiện tại
            //------
                       UpdateSessionBasedOnCurrentTime(moc1, moc2, moc3, moc4);
            //------
            if (Session["roles"]?.ToString() == "Phụ huynh")
            {
                SetUnavailableFor1Type("2");
            }
            var model = ODdata.CLASSes.OrderBy(o => o.ID).ToList();
            return View(model);
        }

        // Khởi tạo trạng thái session ban đầu
        private void InitializeSessionStatus()
        {
            for (int i = 1; i <= 4; i++)
            {
                Session["2" + i] = "available";
                Session["3" + i] = "available";
            }
            Session["11"] = "unavailable";
        }

        // Cập nhật session cho các lớp không khả dụng theo loại và groupClass
        private void SetUnavailableForType(string type, int groupClass)
        {
            for (var i = 1; i < 5; i++)
            {
                if (i != groupClass)
                {
                    Session[type + i] = "unavailable";
                }
            }
        }

        private void SetUnavailableFor1Type(string type)
        {
            for (var i = 1; i < 5; i++)
            {
                 Session[type + i] = "unavailable";
            }
        }

        // Đặt trạng thái "active" cho các lớp mà user đã đăng ký
        private void SetActiveClassStatus(List<REGIST> registrations)
        {
            foreach (var registration in registrations)
            {
                Session[registration.TYPE.Trim() + registration.GROUPCLASS.ToString().Trim()] = "active";
            }
        }

        // Cập nhật trạng thái session dựa trên thời gian hiện tại
        private void UpdateSessionBasedOnCurrentTime(TimeSpan moc1, TimeSpan moc2, TimeSpan moc3, TimeSpan moc4)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            if (currentTime > moc4)
            {
                SetAllUnavailable();
            }
            else if (currentTime > moc3)
            {
                SetUnavailableForTime(3);
            }
            else if (currentTime > moc2)
            {
                SetUnavailableForTime(2);
            }
            else if (currentTime > moc1)
            {
                SetUnavailableForTime(1);
            }
        }

        // Cập nhật trạng thái không khả dụng cho toàn bộ
        private void SetAllUnavailable()
        {
            for (int i = 1; i <= 4; i++)
            {
                Session["2" + i] = "unavailable";
                Session["3" + i] = "unavailable";
            }
            Session["11"] = "unavailable";
        }

        // Cập nhật trạng thái không khả dụng dựa trên thời gian cụ thể
        private void SetUnavailableForTime(int threshold)
        {
            for (int i = 1; i <= threshold; i++)
            {
                Session["2" + i] = "unavailable";
                Session["3" + i] = "unavailable";
            }
        }


        [HttpPost]
        public JsonResult deleteRegist(FormCollection form)
        {
            var user = Session["user"]?.ToString();
            var class_id = form["CLASS_ID"].ToString();
            var model = ODdata.REGISTs.FirstOrDefault(w => w.USER_ID.ToString() == user && w.CLASS_ID.ToString() == class_id);
            ODdata.REGISTs.DeleteOnSubmit(model);
            ODdata.SubmitChanges();
            return Json(new { NOTICE = "Xóa lịch thành công" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getRegist(FormCollection form)
        {
            var user = Session["user"]?.ToString();
            var combobox = form["classCombobox"]?.ToString();
            var _combobox = form["ID_Class"]?.ToString();
            if (combobox is null)
            { combobox = _combobox; }
            var timestart = form["TIMESTART"].ToString();
            var type = form["COLSTART"].ToString();
            var timeIdMapping = new Dictionary<string, string>
            {
                { "09:00", type + "1" },
                { "10:30", type + "2" },
                { "13:00", type + "3" },
                { "14:30", type + "4" }
            };
            string id = timeIdMapping.ContainsKey(timestart) ? timeIdMapping[timestart] : string.Empty;
            var checktime = ODdata.REGISTs.FirstOrDefault(w => w.TIMESTART.Trim() == timestart.Trim() && w.USER_ID.ToString() == user);
            var checkcol2 = ODdata.REGISTs.Where(w => w.USER_ID.ToString() == user && w.TYPE == "2")?.Count();
        //    var checkcol3 = ODdata.REGISTs.Where(w => w.USER_ID.ToString() == user && w.TYPE == "3")?.Count();
            var regist_pax = ODdata.REGISTs.Where(w => w.CLASS_ID.ToString() == combobox).Count();
            var checkpax = ODdata._CLASSes.Where(w => w.ID.ToString() == combobox).ToList();
            var pax = ODdata._CLASSes.FirstOrDefault(w => w.ID.ToString() == combobox).PAX;

            //    if (checktime is null && ((type == "2" && checkcol2 < 2) || (type == "3" && checkcol3 < 1)) && regist_pax < pax)
            if (checktime is null && ((type == "2" && checkcol2 < 2)|| type == "3") && regist_pax < pax)
            {
                ODdata.sp_AddRegist(int.Parse(combobox), user, type, timestart, int.Parse(checkpax.FirstOrDefault().GROUPCLASS.ToString()));
                ODdata.SubmitChanges();
                // Dữ liệu mẫu, bạn có thể thay thế bằng dữ liệu từ database hoặc model thực tế
                var data = ODdata._CLASSes.FirstOrDefault(w => w.ID.ToString() == combobox);
                // Nếu tìm thấy dữ liệu, trả về dưới dạng JSON
                if (data != null)
                {
                    if (data.TYPE == 1)
                    {
                        type = "HOẠT ĐỘNG CÂU LẠC BỘ";
                    }
                    else if (data.TYPE == 2)
                    {
                        type = "LỚP HỌC THỬ";
                    }
                    else if (data.TYPE == 3)
                    {
                        type = "HỘI THẢO";
                    }
                    return Json(new { NOTICE = "ĐĂNG KÝ THÀNH CÔNG!", ID = data.ID, CLASSNAME = data.CLASSNAME.Trim(), TIMESTART = data.TIMESTART.Trim(), TIMEEND = data.TIMEEND.Trim(), TYPE = type, PAX = data.PAX, ROOM = data.ROOM.Trim(), INFO = data.INFO.Trim() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var data = ODdata._CLASSes.FirstOrDefault(w => w.ID.ToString() == combobox);
                var notice = "";
                if (checktime != null)
                {
                    notice = "ĐĂNG KÝ KHÔNG THÀNH CÔNG! TRÙNG GIỜ VỚI HOẠT ĐỘNG KHÁC";
                }
                else if (checkcol2 >= 2)
                {
                    notice = "ĐĂNG KÝ KHÔNG THÀNH CÔNG! QUÁ SỐ LƯỢNG QUY ĐỊNH";
                }
                else if (regist_pax >= pax)
                {
                    notice = "ĐĂNG KÝ KHÔNG THÀNH CÔNG! QUÁ SỐ LƯỢNG CỦA LỚP HỌC";
                }
                // Nếu tìm thấy dữ liệu, trả về dưới dạng JSON
                if (data != null)
                {
                    return Json(new { NOTICE = notice, ID = data.ID, CLASSNAME = data.CLASSNAME.Trim(), TIMESTART = data.TIMESTART.Trim(), TIMEEND = data.TIMEEND.Trim(), TYPE = data.TYPE, PAX = data.PAX, ROOM = data.ROOM, INFO = data.INFO.Trim() }, JsonRequestBehavior.AllowGet);
                }
            }

            // Nếu không tìm thấy dữ liệu tương ứng, trả về lỗi
            return Json(new { error = "Không tìm thấy dữ liệu" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModelData(string id)
        {
            // Dữ liệu mẫu, bạn có thể thay thế bằng dữ liệu từ database hoặc model thực tế
            var data = ODdata.CLASSes.FirstOrDefault(f => (f.COLSTART.ToString()+f.GROUPCLASS.ToString()) == id);

            // Nếu tìm thấy dữ liệu, trả về dưới dạng JSON
            if (data != null)
            {
                return Json(new { ID = data.ID, CLASSNAME = data.CLASSNAME.Trim(), TIMESTART = data.TIMESTART.Trim(), TIMEEND = data.TIMEEND.Trim(), TYPE = data.TYPE, PAX = data.PAX, ROOM = data.ROOM.Trim(), DESCRIPT = data.DESCRIPT.Trim(), COLSTART = data.COLSTART }, JsonRequestBehavior.AllowGet);
            }

            // Nếu không tìm thấy dữ liệu tương ứng, trả về lỗi
            return Json(new { error = "Không tìm thấy dữ liệu" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClassData(int id, string timestart)
        {
            // Dữ liệu mẫu, bạn có thể thay thế bằng dữ liệu từ database hoặc model thực tế
            var data = ODdata._CLASSes.Where(w => w.TIMESTART.Trim() == timestart.Trim() && w.TYPE == id).ToList();
            
            if (data.Any())
            {
                var result = data.Select(c => new
                {
                    ID = c.ID,
                    CLASSNAME = c.CLASSNAME.Trim(),
                    TIMESTART = c.TIMESTART.Trim(),
                    TIMEEND = c.TIMEEND.Trim(),
                    TYPE = c.TYPE,
                    PAX = c.PAX,
                    ROOM = c.ROOM,
                    INFO = c.INFO.Trim(),
                    PAX_RES = ODdata.REGISTs.Where(w => w.CLASS_ID == c.ID).Count()
                }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
                // Nếu không tìm thấy dữ liệu tương ứng, trả về lỗi
                return Json(new { error = "Không tìm thấy dữ liệu" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClassDataById(int id)
        {
            // Dữ liệu mẫu, bạn có thể thay thế bằng dữ liệu từ database hoặc model thực tế
            var data = ODdata._CLASSes.FirstOrDefault(w =>w.ID == id);
            var pax_res = ODdata.REGISTs.Where(w => w.CLASS_ID == id).Count();
            // Nếu tìm thấy dữ liệu, trả về dưới dạng JSON
            if (data != null)
            {
                return Json(new { ID = data.ID, CLASSNAME = data.CLASSNAME.Trim(), TIMESTART = data.TIMESTART.Trim(), TIMEEND = data.TIMEEND.Trim(), TYPE = data.TYPE, PAX = data.PAX, ROOM = data.ROOM, INFO = data.INFO.Trim() }, JsonRequestBehavior.AllowGet);
            }
            // Nếu không tìm thấy dữ liệu tương ứng, trả về lỗi
            return Json(new { error = "Không tìm thấy dữ liệu" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_ClassDataById(int id)
        {
            var user = Session["user"]?.ToString();
            // Dữ liệu mẫu, bạn có thể thay thế bằng dữ liệu từ database hoặc model thực tế
            var regist = ODdata.REGISTs.FirstOrDefault(w => (w.TYPE.ToString().Trim()+ w.GROUPCLASS.ToString().Trim()) == id.ToString() && w.USER_ID.ToString() == user);
            var data = ODdata._CLASSes.FirstOrDefault(w => w.ID == regist.CLASS_ID);
            // Nếu tìm thấy dữ liệu, trả về dưới dạng JSON
            if (data != null)
            {
                var type = "";
                if (data.TYPE == 1)
                {
                    type = "HOẠT ĐỘNG CÂU LẠC BỘ";
                }
                else if (data.TYPE == 2)
                {
                    type = "LỚP HỌC THỬ";
                }
                else if (data.TYPE == 3)
                {
                    type = "SESSION INFO";
                }
                return Json(new { ID = data.ID, CLASSNAME = data.CLASSNAME.Trim(), TIMESTART = data.TIMESTART.Trim(), TIMEEND = data.TIMEEND.Trim(), TYPE = type, PAX = data.PAX, ROOM = data.ROOM, INFO = data.INFO.Trim() }, JsonRequestBehavior.AllowGet);
            }
            // Nếu không tìm thấy dữ liệu tương ứng, trả về lỗi
            return Json(new { error = "Không tìm thấy dữ liệu" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SignIn(string id)
        {
            var model = ODdata.USERs.FirstOrDefault(u => u.ID == id);
            if (model != null)
            {
                Session["user"] = id;
                Session["fullname"] = model.FULLNAME.Trim();
                Session["roles"] = model.STUDENT.Trim();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Content(@"
                    <!DOCTYPE html>
                    <html lang='vi'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Thông báo</title>
                        <style>
                            body {
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                height: 100vh;
                                margin: 0;
                                background-color: #f0f0f0;
                            }
                            .notification {
                                background-color: #fff;
                                border: 1px solid #ccc;
                                border-radius: 8px;
                                padding: 20px;
                                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                                text-align: center;
                                width: 300px;
                            }
                            .notification h2 {
                                margin: 0;
                                color: #333;
                            }
                            .notification p {
                                color: #555;
                            }
                            .close-btn {
                                background-color: #007BFF;
                                color: white;
                                border: none;
                                border-radius: 5px;
                                padding: 10px 15px;
                                cursor: pointer;
                            }
                            .close-btn:hover {
                                background-color: #0056b3;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='notification' id='notification'>
                            <h2>Thông báo</h2>
                            <p>Bạn vui lòng đăng ký thành viên</p>
                            <button class='close-btn' onclick='redirectToCreate()'>Đóng</button>
                        </div>

                        <script>
                            function redirectToCreate() {
                                window.location.replace('/User/Create');
                            }

                            setTimeout(redirectToCreate, 3000); // Thay đổi thời gian chờ ở đây (3000ms = 3 giây)
                        </script>
                    </body>
                    </html>
                    ");

            }

        }

      

    }
}
