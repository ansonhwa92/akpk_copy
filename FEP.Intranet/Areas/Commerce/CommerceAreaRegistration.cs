using System.Web.Mvc;

namespace FEP.Intranet.Areas.Commerce
{
    public class CommerceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Commerce";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Commerce_default",
                "Commerce/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.RnP.Controllers" }
            );
        }
    }
}