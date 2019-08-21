using FEP.Helper;
using FEP.WebApiModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class IndividualController : FEPController
    {
        // GET: Administrator/Individual
        public ActionResult List()
        {            
            return View();
        }
    }
}