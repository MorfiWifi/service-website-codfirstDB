using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class IncomingCall
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "پیام")]
        public string Message { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        //Navigation Properties:
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}