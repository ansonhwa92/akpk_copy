using FEP.Helper;
using FEP.Intranet.Models;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Controllers
{
    public class HomeController : FEPController
    {

        private DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {

            var view = View();
            view.MasterName = "~/Views/Shared/_LayoutLandingPage.cshtml";
           
            if (CurrentUser.IsAuthenticated())
            {
                view.MasterName = "~/Views/Shared/_LayoutPublic.cshtml";
            }

            return view;
        }


        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Profiles()
        {

            var userid = CurrentUser.UserId;

            var model = db.User.Where(u => u.Id == userid).Select(s => new ProfileModel
            {
                Name = s.Name,
                ICNo = s.ICNo,
                Email = s.Email,
                MobileNo = s.MobileNo,
                UserType = s.UserType,
                CompanyName = s.CompanyProfile.CompanyName,
                CompanyRegNo = s.CompanyProfile.CompanyRegNo,
                Sector = s.CompanyProfile.Sector.Name,
                Address1 = s.CompanyProfile.Address1,
                Address2 = s.CompanyProfile.Address2,
                City = s.CompanyProfile.City,
                PostCode = s.CompanyProfile.PostCode,
                CompanyPhoneNo = s.CompanyProfile.CompanyPhoneNo,
                LastLogin = s.UserAccount.LastLogin,
                State = s.CompanyProfile.State.Name  
            }).FirstOrDefault();

            return View(model);
        }
        
    }
}