using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
    public class ExhibitionRoadshowRequestController : FEPController
    {
		private DbEntities db = new DbEntities();

		// GET: eEvent/ExhibitionRoadshowRequest
		public async Task<ActionResult> Index()
        {
			var exroad = await WepApiMethod.SendApiAsync<IEnumerable<ReturnExhibitionRoadshowRequestModel>>(HttpVerbs.Get, $"eEvent/ExhibitionRoadshowRequest");

			if (exroad.isSuccess)
			{
				var exhibition = exroad.Data;
				if (exhibition == null)
				{
					return HttpNotFound();
				}
				return View(exhibition);
			}
            return View();
        }
    }
}