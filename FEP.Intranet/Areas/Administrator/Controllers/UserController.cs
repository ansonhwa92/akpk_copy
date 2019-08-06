using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Administrator.Controllers
{
    public class UserController : FEPController
    {
        // GET: Administrator/User
        public ActionResult Index()
        {
            return View();
        }
    }
}