using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class CompanyController : FEPController
    {
        
        public ActionResult List()
        {
            return View();
        }
    }
}