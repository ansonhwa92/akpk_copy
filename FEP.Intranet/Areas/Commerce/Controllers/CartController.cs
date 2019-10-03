using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using FEP.Model;
using FEP.WebApiModel.RnP;
using FEP.WebApiModel.SLAReminder;


namespace FEP.Intranet.Areas.Commerce.Controllers
{
    public class CartController : FEPController
    {
        private DbEntities db = new DbEntities();

        // GET: Commerce/Cart
        public ActionResult Index()
        {
            return View();
        }
    }
}
