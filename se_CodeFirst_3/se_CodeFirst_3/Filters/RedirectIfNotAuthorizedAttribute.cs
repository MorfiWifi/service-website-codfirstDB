using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace se_CodeFirst_3.Filters
{
    public class RedirectIfNotAuthorizedAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["loginToken"] == null)
            {
                //the session lastPageBeforeLogIn is for redirecting back to current url before login:
                HttpContext.Current.Session["lastPageBeforeLogIn"] = filterContext.RequestContext.HttpContext.Request.Url;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                        { "Controller", "Home" },
                        { "Action", "LogIn" } });
            }

        }
    }
}