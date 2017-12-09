using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class LogStringViewModel
    {
        public string RequestedAction { get; set; }
        public string InAddress { get; set; }
        public string IsSuccessful { get; set; }
        public string Result { get; set; }
        public string Date { get; set; }

        //public string User { get; set; }

        
        public string ToString(string sepereator)
        {
            return
                this.RequestedAction + sepereator +
                this.InAddress + sepereator +
                this.IsSuccessful + sepereator +
                this.Result + sepereator +
                this.Date;
        }

    }
}