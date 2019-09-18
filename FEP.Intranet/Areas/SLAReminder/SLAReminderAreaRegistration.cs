using System.Web.Mvc;

namespace FEP.Intranet.Areas.SLAReminder
{
    public class SLAReminderAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SLAReminder";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SLAReminder_default",
                "SLAReminder/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.SLAReminder.Controllers" }
            );
        }
    }
}