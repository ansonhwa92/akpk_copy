using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent
{
    public class eEventAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "eEvent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "eEvent_default",
                "eEvent/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.eEvent.Controllers" }
            );
        }
    }
}