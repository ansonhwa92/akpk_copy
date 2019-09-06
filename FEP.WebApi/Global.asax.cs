using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FEP.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(DateTime), new DateModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateModelBinder());

        }
    }

    public class DateModelBinder : IModelBinder, System.Web.Http.ModelBinding.IModelBinder
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

        //for web mvc
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
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

        //for web api
        public bool BindModel(HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        {
            DateTime dt;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (DateTime.TryParseExact(value.AttemptedValue, _dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                bindingContext.Model = dt;
                return true;
            }

            if (DateTime.TryParseExact(value.AttemptedValue, _dateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                bindingContext.Model = dt;
                return true;
            }

            if (DateTime.TryParseExact(value.AttemptedValue, _timeOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                bindingContext.Model = dt;
                return true;
            }

            return false;
        }

    }
}
