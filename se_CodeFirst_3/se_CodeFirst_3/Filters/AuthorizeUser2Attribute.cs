using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace se_CodeFirst_3.Filters
{
    public class AuthorizeUser2Attribute : AuthorizeAttribute
    {
        // Custom property
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            ApplicationDbContext db = new ApplicationDbContext();

            var claims = db.Users.Find("5dd9e98e-3048-4e07-a6c5-c9b45460f136").Claims;
            var a = true;
            foreach (var item in claims)
            {
                if (item.ClaimType == ClaimType && item.ClaimValue == ClaimValue)
                {
                    a = false;
                }
            }

            return a;
        }
    }
}
