using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
//using FEP.Model;
using FEP.WebApiModel.RnP;


namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class HomeController : FEPController
    {
        // Landing page - redirects to dashboard or browse publications based on login state
        [AllowAnonymous]
        public ActionResult Index()
        {
            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";

            if (CurrentUser.IsAuthenticated())
            {
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }

            return RedirectToAction("BrowsePublications", "Home", new { area = "RnP" });
        }

        // Browse publications
        // TODO: Handle search/filtering, include star rating
        // GET: RnP/Home/BrowsePublications
        [AllowAnonymous]
        public async Task<ActionResult> BrowsePublications()
        {
            var resPubs = await WepApiMethod.SendApiAsync<IEnumerable<ReturnPublicationModel>>(HttpVerbs.Get, $"RnP/Publication");

            if (!resPubs.isSuccess)
            {
                return HttpNotFound();
            }

            var publications = resPubs.Data;

            if (publications == null)
            {
                return HttpNotFound();
            }

            return View(publications);
        }

        // Publication details
        // TODO: include ratings and reviews info
        // GET: RnP/Home/PublicationDetails
        [AllowAnonymous]
        public async Task<ActionResult> PublicationDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var publication = resPub.Data;

            if (publication == null)
            {
                return HttpNotFound();
            }

            return View(publication);
        }

        // Select format to purchase
        // GET: RnP/Home/SelectFormat
        [AllowAnonymous]
        public async Task<ActionResult> SelectFormat(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var publication = resPub.Data;

            if (publication == null)
            {
                return HttpNotFound();
            }

            return View(publication);
        }

        // Purchase publication
        // GET: RnP/Home/PurchasePublication
        [AllowAnonymous]
        public async Task<ActionResult> PurchasePublication(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var resPub = await WepApiMethod.SendApiAsync<ReturnPublicationModel>(HttpVerbs.Get, $"RnP/Publication?id={id}");

            if (!resPub.isSuccess)
            {
                return HttpNotFound();
            }

            var publication = resPub.Data;

            if (publication == null)
            {
                return HttpNotFound();
            }

            return View(publication);
        }
    }
}