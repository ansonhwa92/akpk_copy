using FEP.Helper;
using FEP.Intranet.Areas.Administrator.Models;
using FEP.WebApiModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ActionResult> List()
        {
            var users = await WepApiMethod.SendApiAsync<List<IndividualModel>>(HttpVerbs.Get, $"Administration/Individual");

            return View(users);
        }

    }
}