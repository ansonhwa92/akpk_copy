using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class StaffController : FEPController
    {
        // GET: Administrator/Staff
        public ActionResult Index()
        {
            return View();
        }
    }
}