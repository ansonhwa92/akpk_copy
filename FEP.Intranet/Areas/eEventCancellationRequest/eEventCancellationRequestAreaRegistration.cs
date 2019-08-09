using System.Web.Mvc;

namespace FEP.Intranet.Areas.Publication
{
    public class PublicationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Publication";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Publication_default",
                "Publication/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.Publication.Controllers" }
            );
        }
    }
}