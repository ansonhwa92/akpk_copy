using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.KMC.Controllers
{
    public class HomeController : FEPController
    {
        // GET: KMC/Home
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _Menu()
        {
            return PartialView();
        }
    }
}