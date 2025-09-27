using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenDayTelerik.Controllers
{
    public class ErrorController : Controller
    {
       
            // Xử lý lỗi tổng quát
            public ActionResult General()
            {
                return View("General");
            }

            // Xử lý lỗi 404 - Không tìm thấy
            public ActionResult NotFound()
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            // Xử lý lỗi 500 - Lỗi nội bộ máy chủ
            public ActionResult InternalError()
            {
                Response.StatusCode = 500;
                return View("InternalError");
            }
        }
}