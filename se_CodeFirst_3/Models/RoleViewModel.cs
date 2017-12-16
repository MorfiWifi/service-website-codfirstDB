using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "نقش کاربری")]
        public string Name { get; set; }
    }
}