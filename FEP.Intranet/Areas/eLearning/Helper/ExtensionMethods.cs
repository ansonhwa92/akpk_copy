using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Helper
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayName = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();

            if (displayName != null)
                return displayName;
            else
                return enumValue.ToString();
        }
    }

    public static class HtmlExtensions
    {
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues = null)
        {

            string scheme = url.RequestContext.HttpContext.Request.Url.Scheme;

            return url.Action(action, controller, routeValues, scheme);


            //Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            //string absoluteAction = string.Format(
            //    "{0}://{1}{2}",
            //    requestUrl.Scheme,
            //    requestUrl.Authority,
            //    url.Action(action, controller));

            //return absoluteAction;
        }
    }
}