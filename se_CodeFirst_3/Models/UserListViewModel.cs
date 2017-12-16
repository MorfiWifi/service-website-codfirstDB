using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class UserListViewModel
    {
        [Display(Name = "شماره کاربر")]
        public string Id { get; set; }

        [Display(Name = "پسورد")]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "نقش کاربری")]
        public string Role { get; set; }

        [Display(Name = "حقوق")]
        public int Salary { get; set; }

        [Display(Name = "غیبت")]
        public int AbsentDays { get; set; }

        [Display(Name = "اضافه کاری")]
        public int OverTime { get; set; }

        [Display(Name = "مزایا")]
        public int Benefits { get; set; }
    }
}