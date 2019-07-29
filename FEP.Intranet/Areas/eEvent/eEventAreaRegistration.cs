using System.Web.Mvc;

namespace FEP.Intranet.Areas.PublicEvent
{
    public class PublicEventAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PublicEvent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PublicEvent_default",
                "PublicEvent/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}