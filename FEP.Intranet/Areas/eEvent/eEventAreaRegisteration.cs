using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent
{
	public class eEventAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "eEvent";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"eEvent_default",
				"eEvent/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}