using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Controllers
{
    public class HomeController : FEPController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Dashboard()
        {
            return View();
        }
        
    }
}