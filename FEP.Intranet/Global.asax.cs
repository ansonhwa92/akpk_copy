using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FEP.Intranet
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime), new DateModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateModelBinder());

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        private void Application_Error(object sender, EventArgs e)
        {

            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            string ipAddress = httpContext.Request.UserHostAddress;

            var ex = Server.GetLastError();

            string details = ex.InnerException + " | " + ex.StackTrace;





            httpContext.ClearError();
            httpContext.Response.Clear();
                       
            Response.Redirect("~/Home/Error");
        }
    }

    public class DateModelBinder : DefaultModelBinder
    {
        private string _dateTimeFormat;
        private string _dateOnlyFormat;
        private string _timeOnlyFormat;

        public DateModelBinder(string dateOnlyFormat = "dd/MM/yyyy", string timeOnlyFormat = "HH:mm", string dateTimeFormat = "dd/MM/yyyy HH:mm")
        {
            _dateOnlyFormat = dateOnlyFormat;
            _timeOnlyFormat = timeOnlyFormat;
            _dateTimeFormat = dateTimeFormat;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            DateTime dt;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (DateTime.TryParseExact(value.AttemptedValue, _dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }

            if (DateTime.TryParseExact(value.AttemptedValue, _dateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }

            if (DateTime.TryParseExact(value.AttemptedValue, _timeOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }


            return null;
        }
    }
}
