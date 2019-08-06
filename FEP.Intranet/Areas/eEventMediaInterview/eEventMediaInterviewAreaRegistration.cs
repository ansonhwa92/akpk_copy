using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEventMediaInterview
{
    public class eEventMediaInterviewAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "eEventMediaInterview";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "eEventMediaInterview_default",
                "eEventMediaInterview/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}