using System.Web.Mvc;

namespace FEP.Intranet.Areas.KMC
{
    public class KMCAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KMC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KMC_default",
                "KMC/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}