using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OpenDayTelerik.Models
{
    public class USER_MODEL
    {
        [Required(ErrorMessage = "Vui lòng điền Họ tên.")]
        public string FULLNAME { get; set; }

        [Required(ErrorMessage = "Vui lòng điền Email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Vui lòng điền Số điện thoại.")]
        public string PHONE { get; set; }


        [Required(ErrorMessage = "Hãy chọn Đối tượng.")]
        public string STUDENT { get; set; }

      
        public string SCHOOL { get; set; }

       
        public string CLASS { get; set; }

        [Required(ErrorMessage = "Vui lòng điền Thành phố.")]
        public string CITY { get; set; }
    }
}

