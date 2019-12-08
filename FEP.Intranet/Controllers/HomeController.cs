using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Home;
using FEP.WebApiModel.Setting;
using FEP.WebApiModel.SLAReminder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View("/Views/Shared/Error.cshtml");
        }

        public async Task<ActionResult> Dashboard(DashboardModule? module)
        {
            var userid = CurrentUser.UserId;

            var availableModule = new List<DashboardModule>();
            GetAvailableModule(availableModule);

            if(module == null && availableModule.Count > 0)
            {
                module = availableModule.First();
            }

            var response = await WepApiMethod.SendApiAsync<DashboardList>(HttpVerbs.Get, $"Home/Dashboard/GetDashbordList?userid={userid}&module={module}");
            


            if (response.isSuccess)
            {
                response.Data.DashboardModuleByRole.AvailableModule = availableModule;
                var model = response.Data;
                return View(model);
            }

            return View(new DashboardList());

        }

        public async Task<ActionResult> MyProfile()
        {
            var userid = CurrentUser.UserId;

            if (CurrentUser.UserType == UserType.SystemAdmin)
            {
                var response = await WepApiMethod.SendApiAsync<AdminProfileModel>(HttpVerbs.Get, $"Administration/User?id={userid}");

                if (response.isSuccess)
                {
                    var profile = response.Data;

                    return View("MyProfileAdmin", profile);
                }

            }
            else if (CurrentUser.UserType == UserType.Individual)
            {

                var response = await WepApiMethod.SendApiAsync<IndividualProfileModel>(HttpVerbs.Get, $"Administration/Individual?id={userid}");

                if (response.isSuccess)
                {
                    var profile = response.Data;

                    return View("MyProfileIndividual", profile);
                }

            }
            else if (CurrentUser.UserType == UserType.Company)
            {
                var response = await WepApiMethod.SendApiAsync<CompanyProfileModel>(HttpVerbs.Get, $"Administration/Company?id={userid}");

                if (response.isSuccess)
                {
                    var profile = response.Data;

                    return View("MyProfileCompany", profile);
                }

            }
            else if (CurrentUser.UserType == UserType.Staff)
            {
                var response = await WepApiMethod.SendApiAsync<StaffProfileModel>(HttpVerbs.Get, $"Administration/Staff?id={userid}");

                if (response.isSuccess)
                {
                    var profile = response.Data;

                    return View("MyProfileStaff", profile);
                }
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            var userid = CurrentUser.UserId;

            if (CurrentUser.UserType == UserType.SystemAdmin)
            {
                var response = await WepApiMethod.SendApiAsync<DetailsUserModel>(HttpVerbs.Get, $"Administration/User?id={userid}");

                if (response.Data == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                var model = new EditAdminProfileModel
                {
                    Name = response.Data.Name,
                    Email = response.Data.Email,
                    MobileNo = response.Data.MobileNo,
                    CountryCode = response.Data.CountryCode
                };

                return View("EditProfileAdmin", model);

            }
            else if (CurrentUser.UserType == UserType.Individual)
            {
                var response = await WepApiMethod.SendApiAsync<DetailsIndividualModel>(HttpVerbs.Get, $"Administration/Individual?id={userid}");

                if (response.Data == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                var model = new EditIndividualProfileModel
                {
                    IsMalaysian = response.Data.IsMalaysian,
                    CitizenshipId = response.Data.Citizenship != null ? response.Data.Citizenship.Id : (int?)null,
                    Name = response.Data.Name,
                    ICNo = response.Data.ICNo,
                    PassportNo = response.Data.PassportNo,
                    MobileNo = response.Data.MobileNo,
                    CountryCode = response.Data.CountryCode,
                    Address1 = response.Data.Address1,
                    Address2 = response.Data.Address2,
                    PostCodeMalaysian = response.Data.PostCodeMalaysian,
                    PostCodeNonMalaysian = response.Data.PostCodeNonMalaysian,
                    City = response.Data.City,
                    StateId = response.Data.State.Id,
                    State = response.Data.State.Name,
                    CountryId = response.Data.Country.Id
                };

                var countries = await GetCountries();

                model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

                model.States = new SelectList(await GetStates(), "Id", "Name", 0);
                model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
                model.Citizenships = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);

                return View("EditProfileIndividual", model);
            }
            else if (CurrentUser.UserType == UserType.Company)
            {

                var response = await WepApiMethod.SendApiAsync<DetailsCompanyModel>(HttpVerbs.Get, $"Administration/Company?id={userid}");

                if (response.Data == null)
                {
                    return new HttpStatusCodeResult(404);
                }

                var model = new EditCompanyProfileModel
                {
                    Type = response.Data.Type,
                    CompanyName = response.Data.CompanyName,
                    AgencyName = response.Data.AgencyName,
                    MinistryId = response.Data.Ministry != null ? response.Data.Ministry.Id : (int?)null,
                    SectorId = response.Data.Sector != null ? response.Data.Sector.Id : (int?)null,
                    CompanyRegNo = response.Data.CompanyRegNo,
                    Address1 = response.Data.Address1,
                    Address2 = response.Data.Address2,
                    PostCodeMalaysian = response.Data.PostCodeMalaysian,
                    PostCodeNonMalaysian = response.Data.PostCodeNonMalaysian,
                    City = response.Data.City,
                    StateId = response.Data.State.Id,
                    State = response.Data.State.Name,
                    CountryId = response.Data.Country.Id,
                    CompanyPhoneNo = response.Data.CompanyPhoneNo,
                    Name = response.Data.Name,
                    ICNo = response.Data.ICNo,
                    PassportNo = response.Data.PassportNo,
                    MobileNo = response.Data.MobileNo,
                    CountryCode = response.Data.CountryCode,
                };

                var countries = await GetCountries();

                model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

                model.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);
                model.States = new SelectList(await GetStates(), "Id", "Name", 0);
                model.Ministries = new SelectList(await GetMinistry(), "Id", "Name", 0);
                model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);


                return View("EditProfileCompany", model);
            }

            return RedirectToAction("MyProfile", "Home", new { area = "" });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(int? id)
        {

            if (CurrentUser.UserType == UserType.SystemAdmin)
            {
                var model = new EditAdminProfileModel();

                if (TryUpdateModel(model))
                {

                    var modelapi = new EditUserModel();

                    modelapi.Name = model.Name;
                    modelapi.MobileNo = model.MobileNo;

                    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Administration/User?id={CurrentUser.UserId}", modelapi);

                    if (response.Data)
                    {
                        await LogActivity(Modules.Setting, "Update Profile", model);

                        TempData["SuccessMessage"] = Language.Profile.AlertSuccessUpdateProfile;

                        return RedirectToAction("MyProfile", "Home", new { area = "" });
                    }
                }

            }
            else if (CurrentUser.UserType == UserType.Individual)
            {
                var model = new EditIndividualProfileModel();

                TryUpdateModel(model);

                if (model.IsMalaysian)
                {
                    ModelState.Remove("PassportNo");
                    ModelState.Remove("CitizenshipId");
                    ModelState.Remove("PostCodeNonMalaysian");
                    ModelState.Remove("State");
                    ModelState.Remove("CountryId");

                    model.CountryId = model.MalaysiaCountryId;
                }
                else
                {
                    ModelState.Remove("ICNo");
                    ModelState.Remove("PostCodeMalaysian");
                    ModelState.Remove("StateId");
                    model.StateId = null;
                }

                if (ModelState.IsValid)
                {
                    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Home/Profile/EditIndividualProfile?id={CurrentUser.UserId}", model);

                    if (response.Data)
                    {
                        await LogActivity(Modules.Setting, "Update Profile", model);

                        TempData["SuccessMessage"] = Language.Profile.AlertSuccessUpdateProfile;

                        return RedirectToAction("MyProfile", "Home", new { area = "" });
                    }
                }

                var countries = await GetCountries();

                model.MalaysiaCountryId = countries.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();

                model.States = new SelectList(await GetStates(), "Id", "Name", 0);
                model.Countries = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);
                model.Citizenships = new SelectList(countries.Where(c => c.Name != "Malaysia"), "Id", "Name", 0);

                return View("EditProfileIndividual", model);

            }
            else if (CurrentUser.UserType == UserType.Company)
            {

                var model = new EditCompanyProfileModel();

                TryUpdateModel(model);

                if (model.Type == CompanyType.Government)
                {
                    ModelState.Remove("PassportNo");
                    ModelState.Remove("PostCodeNonMalaysian");
                    ModelState.Remove("State");
                    ModelState.Remove("CountryId");
                    ModelState.Remove("CompanyName");
                    ModelState.Remove("CompanyRegNo");
                    ModelState.Remove("SectorId");

                    model.CountryId = model.MalaysiaCountryId;
                }
                else if (model.Type == CompanyType.MalaysianCompany)
                {
                    ModelState.Remove("PassportNo");
                    ModelState.Remove("PostCodeNonMalaysian");
                    ModelState.Remove("State");
                    ModelState.Remove("CountryId");
                    ModelState.Remove("AgencyName");
                    ModelState.Remove("MinistryId");

                    model.CountryId = model.MalaysiaCountryId;
                }
                else
                {
                    ModelState.Remove("ICNo");
                    ModelState.Remove("PostCodeMalaysian");
                    ModelState.Remove("StateId");
                    ModelState.Remove("AgencyName");
                    ModelState.Remove("MinistryId");
                    ModelState.Remove("CompanyRegNo");
                }


                if (ModelState.IsValid)
                {

                    var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Home/Profile/EditCompanyProfile?id={CurrentUser.UserId}", model);

                    if (response.Data)
                    {
                        await LogActivity(Modules.Setting, "Update Profile", model);

                        TempData["SuccessMessage"] = Language.Profile.AlertSuccessUpdateProfile;

                        return RedirectToAction("MyProfile", "Home", new { area = "" });
                    }

                }

                model.Sectors = new SelectList(await GetSectors(), "Id", "Name", 0);

                return View("EditProfileCompany", model);

            }

            return RedirectToAction("MyProfile", "Home", new { area = "" });

        }

        [HttpGet]
        public async Task<ActionResult> UpdateAvatar()
        {
            var response = await WepApiMethod.SendApiAsync<DetailsUserModel>(HttpVerbs.Get, $"Administration/User?id={CurrentUser.UserId}");

            var model = new ProfileAvatarModel();

            if (response.isSuccess)
            {
                model.AvatarImageUrl = response.Data.AvatarImageUrl;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult LoadAvatar()
        {
            foreach (string file in Request.Files)
            {
                var fileContent = Request.Files[file];

                var image64 = FileMethod.ConvertImageToBase64(fileContent);

                if (image64 != null)
                {
                    return Content(JsonConvert.SerializeObject(new { image64 = image64 }), "application/json");
                }
            }

            return Content(JsonConvert.SerializeObject(new { image64 = "" }), "application/json");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateAvatar(ProfileAvatarModel model)
        {

            if (ModelState.IsValid)
            {
                var deletefilename = "";

                var responseUser = await WepApiMethod.SendApiAsync<DetailsUserModel>(HttpVerbs.Get, $"Administration/User?id={CurrentUser.UserId}");

                if (responseUser.isSuccess)
                {
                    deletefilename = responseUser.Data.AvatarImageUrl;
                }

                var filename = FileMethod.SaveFile(model.AvatarFile, Server.MapPath("~/img/avatar"), deletefilename);

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Home/Profile/UpdateAvatar?id={CurrentUser.UserId}&imageUrl={filename}");

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Change Avatar Photo");

                    TempData["SuccessMessage"] = Language.Profile.AlertSuccessUpdateAvatar;

                    return RedirectToAction("MyProfile", "Home", new { area = "" });
                }
            }

            TempData["ErrorMessage"] = Language.Profile.AlertFailUpdateAvatar;

            return View(model);
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

            var passwordResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Auth/ValidatePassword?password={model.Password}");

            if (!passwordResponse.isSuccess)
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(passwordResponse.ErrorMessage);

                if (error.ContainsKey("Message"))
                    ModelState.AddModelError("Password", error["Message"]);
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"Home/Profile/ChangePassword?id={CurrentUser.UserId}", model);

                if (response.Data)
                {
                    await LogActivity(Modules.Setting, "Change Password");

                    TempData["SuccessMessage"] = Language.Profile.AlertSuccessChangePassword;

                    return RedirectToAction("MyProfile", "Home", new { area = "" });
                }

            }

            TempData["ErrorMessage"] = Language.Profile.AlertFailChangePassword;

            return View();

        }

        [HttpGet]
        public ActionResult ChangeEmail()
        {

            TempData["Message"] = "You have to activate new email account if changed";

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailModel model)
        {

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={CurrentUser.UserId}&email={model.Email}");

            if (emailResponse.Data)
            {
                ModelState.AddModelError("Email", Language.Profile.ValidIsExistEmail);
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<dynamic>(HttpVerbs.Put, $"Home/Profile/ChangeEmail?id={CurrentUser.UserId}", model);

                if (response.isSuccess)
                {

                    ParameterListToSend notificationParameter = new ParameterListToSend();
                    notificationParameter.UserFullName = CurrentUser.Name;
                    notificationParameter.Link = $"<a href = '" + BaseURL + "/Auth/ActivateAccount/" + response.Data.UID + "' > here </a>";
                    notificationParameter.LoginDetail = $"Email: { model.Email }";

                    CreateAutoReminder notification = new CreateAutoReminder
                    {
                        NotificationType = NotificationType.ActivateAccount,
                        NotificationCategory = NotificationCategory.Event,
                        ParameterListToSend = notificationParameter,
                        StartNotificationDate = DateTime.Now,
                        ReceiverId = new List<int> { (int)CurrentUser.UserId }
                    };

                    var responseNotification = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", notification);

                    await LogActivity(Modules.Setting, "Change Email");

                    TempData["SuccessMessage"] = Language.Profile.AlertSuccessChangeEmail;

                    return RedirectToAction("MyProfile", "Home", new { area = "" });
                }

            }

            TempData["ErrorMessage"] = Language.Profile.AlertFailChangeEmail;

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

        public async Task<JsonResult> CheckCurrentEmail(string CurrentEmail)
        {
            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Auth/AuthenticateEmail?id={CurrentUser.UserId}&Email={CurrentEmail}");

            if (response.Data)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
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

        [NonAction]
        private async Task<IEnumerable<StateModel>> GetStates()
        {
            var states = Enumerable.Empty<StateModel>();

            var response = await WepApiMethod.SendApiAsync<List<StateModel>>(HttpVerbs.Get, $"Administration/State");

            if (response.isSuccess)
            {
                states = response.Data.OrderBy(o => o.Name);
            }

            return states;

        }

        [NonAction]
        private async Task<IEnumerable<CountryModel>> GetCountries()
        {
            var countries = Enumerable.Empty<CountryModel>();

            var response = await WepApiMethod.SendApiAsync<List<CountryModel>>(HttpVerbs.Get, $"Administration/Country");

            if (response.isSuccess)
            {
                countries = response.Data.OrderBy(o => o.Name);
            }

            return countries;

        }

        [NonAction]
        private async Task<IEnumerable<MinistryModel>> GetMinistry()
        {
            var ministries = Enumerable.Empty<MinistryModel>();

            var response = await WepApiMethod.SendApiAsync<List<MinistryModel>>(HttpVerbs.Get, $"Administration/Ministry");

            if (response.isSuccess)
            {
                ministries = response.Data.OrderBy(o => o.Name);
            }

            return ministries;

        }

        [NonAction]
        private void GetAvailableModule(List<DashboardModule> avaialbeModules)
        {
            if (CurrentUser.UserType == UserType.SystemAdmin || CurrentUser.UserType == UserType.Staff)
            {
                if (CurrentUser.HasAccess(UserAccess.EventMenu))
                {
                    avaialbeModules.Add(DashboardModule.PublicEvent);
                    avaialbeModules.Add(DashboardModule.MediaInterview);
                    avaialbeModules.Add(DashboardModule.Exhibition);
                }
                if (CurrentUser.HasAccess(UserAccess.LearningMenu))
                {
                    avaialbeModules.Add(DashboardModule.Courses);
                }
                if (CurrentUser.HasAccess(UserAccess.KMCMenu))
                {
                    avaialbeModules.Add(DashboardModule.KMC);
                }
                if (CurrentUser.HasAccess(UserAccess.RnPMenu))
                {
                    avaialbeModules.Add(DashboardModule.Publication);
                    avaialbeModules.Add(DashboardModule.Survey);
                }
            }
        }

    }
}