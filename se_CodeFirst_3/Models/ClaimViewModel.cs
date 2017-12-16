using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class ClaimViewModel
    {
        public int Id { get; set; }

        [Display(Name = "نوع دسترسی")]
        public string Type { get; set; }

        [Display(Name = "سطح دسترسی")]
        public string Value { get; set; }


        [Display(Name = "اکشن")]
        public string TypeToShow { get; set; }

        [Display(Name = "سطح دسترسی")]
        public string ValueToShow { get; set; }



        //public ClaimViewModel(string Type, string Value, string TypeToShow, string ValueToShow)
        //{
        //    this.Type = Type;
        //    this.Value = Value;
        //    this.TypeToShow = TypeToShow;
        //    this.ValueToShow = ValueToShow;
        //}
    }
}