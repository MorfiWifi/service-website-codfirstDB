using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "متن قرارداد")]
        public string Content { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }


        //Navigation Properties:
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Customer Customer { get; set; }
    }
}