using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Setting.Controllers
{
    public class HomeController : Controller
    {
        // GET: Setting/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Menu()
        {
            return View();
        }
    }
}