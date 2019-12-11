using System.Web.Mvc;

namespace FEP.Intranet.Areas.NewsArticleManagement
{
    public class NewsArticleManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NewsArticleManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NewsArticleManagement_default",
                "NewsArticleManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.NewsArticleManagement.Controllers" }
            );
        }
    }
}

