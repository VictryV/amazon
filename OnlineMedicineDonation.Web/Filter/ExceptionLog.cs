using OnlineMedicineDonation.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace OnlineMedicineDonation.Filter
{
    public class ExceptionLog : ExceptionFilterAttribute,IExceptionFilter
    {
        Log log = Log.Getinstance;

        public override void OnException(ExceptionContext context)
        {
            var exceptionMsg = context.Exception.Message;
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();

            string log_msg = controllerName + "/" + actionName + "ERR:- " + exceptionMsg;
            log.WriteLog(log_msg);
            context.Result = new RedirectResult("~/Account/Authentication/error");
            base.OnException(context);
        }
    }
}
