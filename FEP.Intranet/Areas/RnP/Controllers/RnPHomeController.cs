using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.RnP.Controllers
{
    public class RnPHomeController : FEPController
    {
        // GET: RnP/RnPHome
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
