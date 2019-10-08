using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(FEP.Intranet.Startup))]

namespace FEP.Intranet
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            int expired;
            int.TryParse(WebConfigurationManager.AppSettings["CookieExpiredDuration"], out expired);

            app.UseCookieAuthentication(
            new CookieAuthenticationOptions
            {
                AuthenticationType = "FEPCookie",
                LoginPath = new PathString("/Auth/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(expired != 0 ? expired : 15),//default 15 if not set
                SlidingExpiration = true,
                Provider = new CookieAuthenticationProvider
                {
                    OnApplyRedirect = ApplyRedirect
                },
            });

            //app.UseCheckLanguageMiddleware(); not use anymore. replace in Helper BeginExecuteCore

        }

        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {

            //bool IsSSO = false;
            //bool.TryParse(WebConfigurationManager.AppSettings["EnableSSO"], out IsSSO);

            //string SSOLoginPath = null;
            //string ClientId = null;

            //SSOLoginPath = WebConfigurationManager.AppSettings["SSOLoginPath"];
            //ClientId = WebConfigurationManager.AppSettings["ClientId"];



            ////redirect to SSO login page if enable
            //if (IsSSO && !string.IsNullOrEmpty(SSOLoginPath) && !string.IsNullOrEmpty(ClientId))
            //{
            //    context.RedirectUri = SSOLoginPath + "/" + ClientId + new QueryString(
            //            context.Options.ReturnUrlParameter,
            //            context.Request.Uri.AbsoluteUri);
            //}

            //prevent redirect to login page for api
            //web api
            if (!IsApiRequest(context.Request))
            {
                context.Response.Redirect(context.RedirectUri);
            }

            //ajax request
            if (IsAjaxRequest(context.Request))
            {
                UrlHelper _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                String actionUri = _url.Action("RedirectToLogin", "Auth", new { area = "" });

                context.Response.Redirect(actionUri);
            }

        }

        private static bool IsApiRequest(IOwinRequest request)
        {
            string apiPath = VirtualPathUtility.ToAbsolute("~/api/");
            return request.Uri.LocalPath.StartsWith(apiPath);
        }

        private static bool IsAjaxRequest(IOwinRequest request)
        {
            // check http header parameter in request query items collection
            IReadableStringCollection query = request.Query;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }

            // check http header parameter in http request header items collection
            IHeaderDictionary headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}
