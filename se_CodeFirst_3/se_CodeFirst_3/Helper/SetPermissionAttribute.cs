using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace se_CodeFirst_3.Helper
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FutureDateAttribute : AuthorizeAttribute
    {
        //public CustomAuthorizeAttribute(params string[] roleKeys)
        //{

        //}

    }

    public class AllowOnlyCertainUsers : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            
            if ( true/*check if user OK or not*/)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                
            }
        }
    }

}