using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OpenDayTelerik.Models;


namespace OpenDayTelerik.Controllers
{
    public class RegistController : Controller
    {
        // GET: Regist
        readonly ODdataDataContext ODdata = new ODdataDataContext(ConfigurationManager.ConnectionStrings["OpendayConnectionString"].ConnectionString);
        public ActionResult admin()
        {
            var user = Session["user"]?.ToString();
            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("RequestPage", "Home");
            }

            var no = ODdata.USERs.FirstOrDefault(w => w.ID.Trim() == user).NO;
            if (user != null && no > 10)
            {
                return RedirectToAction("RequestPage", "Home");
            }

            return null; // Hoặc bạn có thể trả về một view mặc định nếu cần.
        }
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var redirectResult = admin();
            if (redirectResult != null)
            { return redirectResult; }
            return View();
        }

        public ActionResult CheckIn_All()
        {
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var redirectResult = admin();
            if (redirectResult != null)
            { return redirectResult; }
            return View();
        }
        public ActionResult GetData_List_Checkin_Full([DataSourceRequest] DataSourceRequest request)
        {
            var model = ODdata.View_Checkin_Fulls.ToList(); // Lấy dữ liệu từ cơ sở dữ liệu
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        // Action để đọc dữ liệu cho grid
        public ActionResult GetData_Regist_Full([DataSourceRequest] DataSourceRequest request)
        {
            var model = ODdata.View_Regist_Fulls.ToList(); // Lấy dữ liệu từ cơ sở dữ liệu
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // Action để xử lý xuất file Excel
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }


        public ActionResult Pivot_Class()
        {
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var redirectResult = admin();
            if (redirectResult != null)
            { return redirectResult; }
            return View();
        }

        public ActionResult GetData_Pivot_Class([DataSourceRequest] DataSourceRequest request)
        {
            var model = ODdata.View_Pivot_Classes.OrderBy(o => o.ID).ToList();
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pivot_Class_Checkin()
        {
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var redirectResult = admin();
            if (redirectResult != null)
            { return redirectResult; }
            var model = ODdata.View_Pivot_Classes.OrderBy(o => o.ID).ToList();
            return View(model);
        }

        public ActionResult List_Regist_Class(int id)
        {
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var redirectResult = admin();
            if (redirectResult != null)
            { return redirectResult; }
            Session["classid"] = id;
            var model = ODdata.View_Regist_Fulls.Where(w => w.CLASS_ID == id).ToList();
            return View(model);
        }

        public ActionResult GetData_List_Regist_Class([DataSourceRequest] DataSourceRequest request)
        {
            var id = Session["classid"]?.ToString();
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); 
            }
            var model = ODdata.View_Regist_Fulls.Where(w => w.CLASS_ID.ToString() == id).ToList();
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult List_Checkin_Class(int id)
        {
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var redirectResult = admin();
            if (redirectResult != null)
            { return redirectResult; }
            Session["classid"] = id;
            var model = ODdata.View_Checkin_Fulls.Where(w => w.CLASS_ID.Trim() == id.ToString()).ToList();
            return View(model);
        }

        public ActionResult Getdata_List_Checkin_Class([DataSourceRequest] DataSourceRequest request)
        {
            var id = Session["classid"].ToString();
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var model = ODdata.View_Checkin_Fulls.Where(w => w.CLASS_ID.Trim() == id.ToString()).ToList();
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Checkin_Class(int id)
        {
            // Kiểm tra xem Session["user"] có rỗng không
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("RequestPage", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var redirectResult = admin();
            if (redirectResult != null)
            { return redirectResult; }
            Session["classid"] = id;
            var model = ODdata._CLASSes.FirstOrDefault(w => w.ID.ToString() == id.ToString());
            return View(model);
        }

        public ActionResult _binding_checkin_class([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["classid"] == null)
            {
                return Json(new { success = false, message = "Class ID is missing." }, JsonRequestBehavior.AllowGet);
            }

            string id = Session["classid"].ToString();
            var model = ODdata.View_Checkin_Fulls.Where(w => w.CLASS_ID.Trim() == id).ToList().OrderByDescending(o => o.checkinID).Take(10);
            return Json(model.ToDataSourceResult(request));
        }

        public JsonResult _Checkin_User(string id)
        {
            var classid = Session["classid"]?.ToString();
            var usercheck = Session["user"]?.ToString();
            var _class = ODdata._CLASSes.FirstOrDefault(w => w.ID.ToString() == classid);
            var type = _class?.TYPE;

            // Kiểm tra xem người dùng đã đăng ký chưa
            var existingRegist = ODdata.REGISTs
                .FirstOrDefault(c => c.CLASS_ID.ToString() == classid && c.USER_ID == id);

            if ((type == 2 && existingRegist != null)|type == 3)
            {
                // Kiểm tra xem người dùng đã check-in vào lớp học này chưa
                var existingCheckin = ODdata.CHECKINs
                    .FirstOrDefault(c => c.CLASS_ID == classid && c.USER_ID == id);

                string message;
                if (existingCheckin == null)
                {
                    // Nếu chưa check-in, thực hiện thêm mới
                    var checkin = new CHECKIN
                    {
                        CLASS_ID = classid,
                        USER_ID = id,
                        USER_CHECK = usercheck,
                        TIME = DateTime.Now
                    };
                    ODdata.CHECKINs.InsertOnSubmit(checkin);
                    ODdata.SubmitChanges();

                    message = "Check-in thành công!";
                }
                else
                {
                    // Nếu đã check-in, trả về thông báo
                    message = "Check-in thành công!";
                }

                return Json(new { message = message });
            }
            else
            {
                string message;
                message = "Người dùng này chưa đăng ký tham gia!";
                return Json(new { message = message });
            }
        }

    }
}