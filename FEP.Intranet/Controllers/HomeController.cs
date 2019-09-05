using FEP.Helper;
using FEP.Intranet.Models;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                var response = await WepApiMethod.SendApiAsync<IndividualProfileModel>(HttpVerbs.Get, $"Home/Profile/GetIndividualProfile?id={userid}");

                var profile = response.Data;

                return View("MyProfileIndividual", profile);

            }
            else if (CurrentUser.UserType == UserType.Company)
            {

                var response = await WepApiMethod.SendApiAsync<CompanyProfileModel>(HttpVerbs.Get, $"Home/Profile/GetCompanyProfile?id={userid}");

                var profile = response.Data;

                return View("MyProfileCompany", profile);

            }
            else if (CurrentUser.UserType == UserType.Staff)
            {

                return View("MyProfileStaff");
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            var userid = CurrentUser.UserId;

            if (CurrentUser.UserType == UserType.Individual || CurrentUser.UserType == UserType.SystemAdmin)
            {
                var response = await WepApiMethod.SendApiAsync<IndividualProfileModel>(HttpVerbs.Get, $"Home/Profile/GetIndividualProfile?id={userid}");

                if (response.Data == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                var profile = new EditIndividualProfileModel
                {
                    Name = response.Data.Name,
                    Email = response.Data.Email,
                    ICNo = response.Data.ICNo,
                    MobileNo = response.Data.MobileNo
                };

                return View("EditProfileIndividual", profile);
            }
            else if (CurrentUser.UserType == UserType.Company)
            {

                var response = await WepApiMethod.SendApiAsync<CompanyProfileModel>(HttpVerbs.Get, $"Home/Profile/GetCompanyProfile?id={userid}");

                if (response.Data == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                var profile = new EditCompanyProfileModel
                {
                    CompanyName = response.Data.CompanyName,
                    CompanyRegNo = response.Data.CompanyRegNo,
                    SectorId = response.Data.SectorId,
                    Address1 = response.Data.Address1,
                    Address2 = response.Data.Address2,
                    City = response.Data.City,                    
                    PostCode = response.Data.PostCode,
                    State = response.Data.State,
                    CompanyPhoneNo = response.Data.CompanyPhoneNo,

                    Name = response.Data.Name,
                    Email = response.Data.Email,
                    ICNo = response.Data.ICNo,
                    MobileNo = response.Data.MobileNo
                };

                profile.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);

                //profile.States = new SelectList(await GetStates(), "Id", "Name", 0);
                
                return View("EditProfileCompany", profile);
            }
            
            return RedirectToAction("MyProfile", "Home", new { area = "" });
             
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(int? id)
        {

            if (CurrentUser.UserType == UserType.Individual || CurrentUser.UserType == UserType.SystemAdmin)
            {
                var model = new EditIndividualProfileModel();

                if (TryUpdateModel(model))
                {
                    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Home/Profile/EditIndividualProfile?id={CurrentUser.UserId}", model);

                    if (response.Data)
                    {
                        LogActivity(Modules.Admin, "Update Profile", model);

                        TempData["SuccessMessage"] = "Profile successfully updated.";

                        return RedirectToAction("MyProfile", "Home", new { area = "" });
                    }
                }

                return View("EditProfileIndividual", model);

            }
            else if (CurrentUser.UserType == UserType.Company)
            {

                var model = new EditCompanyProfileModel();

                if (TryUpdateModel(model))
                {

                    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Home/Profile/EditCompanyProfile?id={CurrentUser.UserId}", model);

                    if (response.Data)
                    {
                        LogActivity(Modules.Admin, "Update Profile", model);

                        TempData["SuccessMessage"] = "Profile successfully updated.";

                        return RedirectToAction("MyProfile", "Home", new { area = "" });
                    }

                }

                model.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);

                return View("EditProfileCompany", model);

            }

            return RedirectToAction("MyProfile", "Home", new { area = "" });

        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Home/Profile/ChangePassword?id={CurrentUser.UserId}", model);

                if (response.Data)
                {
                    LogActivity(Modules.Admin, "Change Password");

                    TempData["SuccessMessage"] = "Password successfully updated.";

                    return RedirectToAction("MyProfile", "Home", new { area = "" });
                }
                

            }

            TempData["SuccessMessage"] = "Fail to change password.";

            return View();

        }

        public async Task<JsonResult> CheckCurrentPassword(string CurrentPassword)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Auth/AuthenticatePassword?id={CurrentUser.UserId}&Password={CurrentPassword}");

            if (response.Data)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidatePassword(string Password)
        {
            //var hasNumber = new Regex(@"[0-9]+");
            //var hasUpperChar = new Regex(@"[A-Z]+");
            //var hasMiniMaxChars = new Regex(@".{8,15}");
            //var hasLowerChar = new Regex(@"[a-z]+");
            //var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            //var config = db.AccountSetting.FirstOrDefault();

            //if (config.IsContainLowerCase && !hasLowerChar.IsMatch(Password))
            //{
            //    return Json("Password should contain at least one lower case letter", JsonRequestBehavior.AllowGet);
            //}
            //else if (config.IsContainUpperCase && !hasUpperChar.IsMatch(Password))
            //{
            //    return Json("Password should contain at least one upper case letter", JsonRequestBehavior.AllowGet);
            //}
            //else if (config.IsContainNumeric && !hasNumber.IsMatch(Password))
            //{
            //    return Json("Password should contain at least one numeric value", JsonRequestBehavior.AllowGet);
            //}
            //else if (config.IsContainSymbol && !hasSymbols.IsMatch(Password))
            //{
            //    return Json("Password should contain at least one special case characters", JsonRequestBehavior.AllowGet);
            //}
            //else if (config.IsLengthLimit && !hasMiniMaxChars.IsMatch(Password))
            //{
            //    return Json("Password should not be less than 8 characters", JsonRequestBehavior.AllowGet);
            //}

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private async Task<IEnumerable<SectorModel>> GetSectors()
        {

            var sectors = Enumerable.Empty<SectorModel>();

            var response = await WepApiMethod.SendApiAsync<List<SectorModel>>(HttpVerbs.Get, $"Administration/Sector");

            if (response.isSuccess)
            {
                sectors = response.Data;
            }

            return sectors;

        }

    }
}