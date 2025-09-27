using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenDayTelerik.Models;
using System.Data;
using System.Web.Mvc;
using System.Configuration;

namespace OpenDayTelerik.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        readonly ODdataDataContext ODdata = new ODdataDataContext(ConfigurationManager.ConnectionStrings["OpendayConnectionString"].ConnectionString);
        public ActionResult Index(int id)
        {
            if (Session["user"] == null)
            {
                // Nếu rỗng, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Request", "Home"); // Thay "Account" bằng tên controller của bạn
            }
            var model = ODdata.USERs.Take(id * 100).Skip((id - 1) * 100).ToList().OrderByDescending(o => o.NO);

            return View(model);
        }
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
            var model = ODdata.USERs.ToList().OrderBy(o => o.NO);

            return View(model);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            var model = new USER_MODEL();
            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(USER_MODEL model)
        {
            if (!ModelState.IsValid)
            {
                // Trả về View với model để hiển thị lỗi
                return View(model);
            }
            try
            {
                var user_model = new USER() 
                { ID = "user"+ (ODdata.USERs.Count()+1),
                    FULLNAME = model.FULLNAME,
                    CITY = model.CITY,
                    CLASS = model.CLASS,
                    EMAIL = model.EMAIL,
                    PHONE = model.PHONE,
                    SCHOOL = model.SCHOOL,
                    STUDENT = model.STUDENT
                };
                ODdata.USERs.InsertOnSubmit(user_model);
                ODdata.SubmitChanges();
                Session["user"] = user_model.ID;
                Session["fullname"] = user_model.FULLNAME.Trim();
                return RedirectToAction("Personal","User");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm người dùng. Vui lòng thử lại." + ex);
                return View(model);
            }
        }

        // GET: User/Edit/5
        public ActionResult Personal()
        {
            var user = Session["user"]?.ToString();
            // Thêm đoạn log để kiểm tra ID được lưu
            System.Diagnostics.Debug.WriteLine($"User ID stored in session: {Session["user"]}");
            if (user == null)
            {
                return RedirectToAction("RequestPage", "Home");
            }
            return View();
        }

        public ActionResult logoff()
        {
            Session.Abandon();
            return RedirectToAction("RequestPage","Home");
        }
    }
}
