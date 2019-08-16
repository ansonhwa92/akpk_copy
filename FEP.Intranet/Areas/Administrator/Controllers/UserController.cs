using FEP.Helper;
using FEP.Intranet.Areas.Administrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class UserController : FEPController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

    }
}