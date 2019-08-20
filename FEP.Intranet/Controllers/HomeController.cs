using FEP.Helper;
using FEP.Intranet.Models;
using FEP.Model;
using FEP.WebApiModel.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Controllers
{
    public class HomeController : FEPController
    {
        
        [AllowAnonymous]
        public ActionResult Index()
        {
            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPagePublic.cshtml";
           
            if (CurrentUser.IsAuthenticated())
            {
                return RedirectToAction("Dashboard", "Home", new { area = "" });
            }

            return view;
        }
        
        public ActionResult Dashboard()
        {
            return View();
        }

        public async Task<ActionResult> MyProfile()
        {
            var userid = CurrentUser.UserId;

            if (CurrentUser.UserType == UserType.Individual || CurrentUser.UserType == UserType.SystemAdmin)
            {

                var response = await WepApiMethod.SendApiAsync<IndividualProfileModel>(HttpVerbs.Get, $"Administration/User/GetIndividualProfile?id={userid}");

                var profile = response.Data;
                
                return View("MyProfileIndividual", profile);

            }
            else if(CurrentUser.UserType == UserType.Company)
            {

                var response = await WepApiMethod.SendApiAsync<CompanyProfileModel>(HttpVerbs.Get, $"Administration/User/GetCompanyProfile?id={userid}");

                var profile = response.Data;

                return View("MyProfileCompany", profile);

            }
            else if(CurrentUser.UserType == UserType.Staff)
            {

                return View("MyProfileStaff");
            }

            return View();
        }
        
    }
}