using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Contract
    {
        [Display(Name = "شماره قرارداد")]
        public int Id { get; set; }

        [Required(ErrorMessage = "متن قرارداد نمی تواند خالی باشد.")]
        [Display(Name = "متن قرارداد")]
        public string Content { get; set; }

        //Navigation Properties:
        public virtual ICollection<Order> Orders { get; set; }
    }
}