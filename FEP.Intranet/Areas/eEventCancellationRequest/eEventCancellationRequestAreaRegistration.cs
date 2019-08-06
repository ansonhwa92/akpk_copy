using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEventCancellationRequest
{
    public class eEventCancellationRequestAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "eEventCancellationRequest";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "eEventCancellationRequest_default",
                "eEventCancellationRequest/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}