using se_CodeFirst_3.Filters;
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

        [Required(ErrorMessage = "متن پیام نمی تواند خالی باشد.")]
        [Display(Name = "پیام")]
        public string Message { get; set; }

        [Display(Name = "تاریخ")]
        //[FutureDate]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        //Navigation Properties:
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}