using System.Web.Mvc;

namespace FEP.Intranet.Areas.Reward
{
    public class RewardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Reward";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Reward_default",
                "Reward/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "FEP.Intranet.Areas.Reward.Controllers" }
            );
        }
    }
}