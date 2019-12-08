using FEP.Helper;
using FEP.WebApiModel.KMC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.KMC.Controllers
{
    public class HomeController : FEPController
    {

        [ChildActionOnly]
        public ActionResult _Menu()
        {
            return PartialView("_Menu");
        }

        // GET: KMC/Index
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var model = new CategoryModel();

            ViewBag.Categories = new List<CategoryModel>();

            var responseCategory = await WepApiMethod.SendApiAsync<List<CategoryModel>>(HttpVerbs.Get, $"KMC/Category");

            if (responseCategory.isSuccess)
            {
                ViewBag.Categories = responseCategory.Data;
            }

            model = responseCategory.Data.FirstOrDefault();

            return View(model);
        }

        // GET: KMC/Browse
        public async Task<ActionResult> Browse()
        {
            var model = new CategoryModel();

            ViewBag.Categories = new List<CategoryModel>();

            var responseCategory = await WepApiMethod.SendApiAsync<List<CategoryModel>>(HttpVerbs.Get, $"KMC/Category");

            if (responseCategory.isSuccess)
            {
                ViewBag.Categories = responseCategory.Data;
            }
         
            model = responseCategory.Data.FirstOrDefault();
            
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> _Content(int Id)
        {
            var response = await WepApiMethod.SendApiAsync<DetailsKMCModel>(HttpVerbs.Get, $"KMC/Manage?id={Id}");

            var model = new DetailsKMCModel();

            if (response.isSuccess)
            {
                model = response.Data;
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> _List(int? Id, FilterKMCModel filter)
        {
            var response = await WepApiMethod.SendApiAsync<List<KMCModel>>(HttpVerbs.Post, $"KMC/Home/GetAll?userId={CurrentUser.UserId}&categoryId={Id}", filter);

            var model = new List<KMCModel>();

            if (response.isSuccess)
            {
                model = response.Data;
            }

            if (model.Count > 0)
            {
                ViewBag.PageInfo = "Showing 1 - " + model.Count + " of " + model.Count + " results";
            }
            else
            {
                ViewBag.PageInfo = "Showing 0 - 0 of 0 results";
            }

            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetContent(int Id)
        {
            return await FileMethod.DownloadFile(Id);
        }
    }
}