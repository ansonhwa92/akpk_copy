using System.Web.Mvc;

namespace FEP.Intranet.Areas.CarouselManagement
{
    public class CarouselManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CarouselManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CarouselManagement_default",
                "CarouselManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.CarouselManagement.Controllers" }
            );
        }
    }
}