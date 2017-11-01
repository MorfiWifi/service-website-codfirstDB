using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }

        [Display(Name = "حقوق خالص")]
        public int Salary { get; set; }

        [Display(Name = "غیبت")]
        public int AbsentDays { get; set; }

        [Display(Name = "اضافه کاری")]
        public int OverTime { get; set; }

        [Display(Name = "مزایا")]
        public int Benefits { get; set; }

        [Display(Name = "حقوق پرداختی")]
        public int FinalSalary { get; set; }

        [Display(Name = "نقش کاربری")]
        public IList<string> Roles { get; set; }
    }
}