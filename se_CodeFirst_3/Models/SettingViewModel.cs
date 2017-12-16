using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class SettingViewModel
    {
        [Display(Name = "شماره")]
        public int Id { get; set; }

        [Display(Name = "آپشن")]
        public string SettingName { get; set; }

        [Display(Name = "مقدار")]
        public string SettingValue { get; set; }
    }
}