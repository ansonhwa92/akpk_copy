using System.Web.Mvc;

namespace FEP.Intranet.Areas.Template
{
    public class TemplateAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Template";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Template_default",
                "Template/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.Template.Controllers" }
            );
        }
    }
}