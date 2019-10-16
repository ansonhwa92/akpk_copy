using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FEP.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.EnableCors();

            // attribute route
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "Administration Area Default",
            //    routeTemplate: "api/Administration/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


            //config.Filters.Add(new ValidationActionFilter()); developer can manually configured at action
            config.Filters.Add(new ExceptionFilter());

            config.BindParameter(typeof(DateTime), new DateModelBinder());
            config.BindParameter(typeof(DateTime?), new DateModelBinder());
        }
    }
}
