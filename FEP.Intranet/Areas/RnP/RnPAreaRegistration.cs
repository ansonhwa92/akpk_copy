using System.Web.Mvc;

namespace FEP.Intranet.Areas.RnP
{
    public class RnPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RnP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RnP_default",
                "RnP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}