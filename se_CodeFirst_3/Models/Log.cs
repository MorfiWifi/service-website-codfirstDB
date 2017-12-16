using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Log
    {
        [Display(Name = "شماره")]
        public int Id { get; set; }

        [Display(Name = "محتوی")]
        public string Content { get; set; }

        public State State { get; set; }

    }

    public enum State
    {
        Successful_KnownUser = 0,
        Failure_KnownUser = 1,
        Successful_AnonymouseUser = 2,
        Failure_AnonymouseUser = 3,
    }
}