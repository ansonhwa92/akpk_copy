using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning
{
    public class eLearningAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "eLearning";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "eLearning_default",
                "eLearning/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}