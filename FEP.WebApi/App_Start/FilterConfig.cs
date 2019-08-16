using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace FEP.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());           
        }

        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ValidationActionFilter());
        }
    }

}
