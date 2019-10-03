using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FEP.Model;

namespace FEP.WebApi
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }
        }
       
    }

    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            //log error
            try
            {

                string actionName = filterContext.ActionContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;

                using (var db = new DbEntities())
                {
                    var log = new ErrorLog
                    {
                        CreatedDate = DateTime.Now,
                        UserId = null,
                        Module = null,
                        Source = " Controller: " + controllerName + " Action: " + actionName,
                        ErrorDescription = filterContext.Exception.Message,
                        ErrorDetails = filterContext.Exception.InnerException + " | " + filterContext.Exception.StackTrace,
                        IPAddress = "",
                    };

                    db.ErrorLog.Add(log);
                    db.SaveChanges();
                }

            }
            catch(Exception ex)
            {

            }

            filterContext.Response = filterContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, filterContext.Exception.Message);
        }
    }
}