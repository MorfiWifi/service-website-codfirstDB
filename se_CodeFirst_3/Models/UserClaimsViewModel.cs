using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class UserClaimsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "آی دی کاربر")]
        public string UserId { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "نقش کاربری")]
        public ICollection<Claim> Claims { get; set; }

    }
}