using se_CodeFirst_3.Helper;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Routing;

namespace se_CodeFirst_3.Filters
{
    public class LogApiAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// For Web API controllers
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            UsefulMethodsHelper methodHelper = new UsefulMethodsHelper();

            if (Boolean.Parse(methodHelper.LoadSettingFromWebConfig("SaveLogs")))
            {
                base.OnActionExecuted(actionExecutedContext);

                var request = actionExecutedContext.Request;
                var response = actionExecutedContext.Response;
                var actionContext = actionExecutedContext.ActionContext;


                LogStringViewModel logStringViewModel = new LogStringViewModel
                {
                    RequestedAction = "درخواست عمل: " + methodHelper.RequestProperMessage(actionExecutedContext),
                    InAddress = "در آدرس: " + request.RequestUri,
                    Result = "نتیجه درخواست: " + methodHelper.ResponseProperMessage(actionExecutedContext),
                    Date = "در تاریخ: " + methodHelper.ConvertDateTimeToPersian(DateTime.Now),
                    IsSuccessful = response.IsSuccessStatusCode.ToString(),
                };


                // Log API Call
                Log log = new Log
                {
                    Content = logStringViewModel.ToString(" __ ")
                };

                db.Logs.Add(log);
                db.SaveChanges();
            }

        }
    }
}