using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;
using FEP.Intranet.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using FEP.WebApiModel.Auth;
using FEP.WebApiModel.Administration;
using FEP.Model;
using FEP.WebApiModel.SLAReminder;
using System.Text.RegularExpressions;
using FEP.WebApiModel.Setting;
using FEP.WebApiModel.Notification;

namespace FEP.Intranet.Controllers
{

    [LogError(Modules.Admin)]
    public class AuthController : FEPController
    {
        [NonAction]
        [AllowAnonymous]
        public async Task<ActionResult> Test()
        {
            //ParameterListToSend paramToSend = new ParameterListToSend();
            //paramToSend.EventCode = "";
            //paramToSend.EventName = "";
            //paramToSend.EventApproval = "Pending Approval";

            //CreateAutoReminder reminder = new CreateAutoReminder
            //{
            //    NotificationType = NotificationType.Verify_Public_Event_Creation,
            //    NotificationCategory = NotificationCategory.Event,
            //    ParameterListToSend = paramToSend,
            //    StartNotificationDate = DateTime.Now,
            //    ReceiverId = new List<int> { 1 },
            //};

            var model = new CreateNotificationModel
            {
                UserId = 1,
                NotificationType = NotificationType.ActivateAccount,
                Category = NotificationCategory.Event,
                Message = "tetst",
                Link = ""
            };

            var response2 = await WepApiMethod.SendApiAsync<long>(HttpVerbs.Post, $"System/Notification", model);

            return Content("");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View(new LogInModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        public ActionResult StaffLogin(string returnUrl)
        {
            return View(new StaffLogInModel { ReturnUrl = returnUrl });
        }

        // firus
        [AllowAnonymous]
        public ActionResult LoginAndReturn(string returnurl)
        {
            ViewBag.returnurl = returnurl;
            return View();
        }

        [AllowAnonymous]
        public ActionResult RedirectToLogin()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.AppendFormat("window.location = '" + Url.Action("Login", "Auth", new { }) + "';");
            sb.Append("</script>");

            return Content(sb.ToString());
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> RegisterIndividual()
        {
            var model = new RegisterIndividualModel();

            model.IsMalaysian = true;

            model = await InitRegisterIndividual(model);

            return View(model);
        }

        [NonAction]
        private async Task<RegisterIndividualModel> InitRegisterIndividual(RegisterIndividualModel model)
        {
            model.Citizenships = Enumerable.Empty<SelectListItem>();
            model.Countries = Enumerable.Empty<SelectListItem>();
            model.States = Enumerable.Empty<SelectListItem>();

            var countryResponse = await WepApiMethod.SendApiAsync<List<CountryModel>>(HttpVerbs.Get, $"Administration/Country");

            if (countryResponse.isSuccess)
            {
                model.Citizenships = new SelectList(countryResponse.Data.Where(c => c.Name != "Malaysia").OrderBy(o => o.Name), "Id", "Name", 0);
                model.Countries = model.Citizenships;
                model.MalaysiaCountryId = countryResponse.Data.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();
                model.CountryCode = countryResponse.Data.Where(c => c.Name == "Malaysia").Select(s => s.CountryCode).FirstOrDefault();
            }

            var stateResponse = await WepApiMethod.SendApiAsync<List<StateModel>>(HttpVerbs.Get, $"Administration/State");

            if (stateResponse.isSuccess)
            {
                var states = stateResponse.Data;
                model.States = new SelectList(states.OrderBy(o => o.Name), "Id", "Name", 0);
            }

            return model;
        }

        [AllowAnonymous]
        public async Task<ActionResult> RegisterAgency()
        {
            var model = new RegisterAgencyModel();

            model.Type = CompanyType.Government;

            model = await InitRegisterCompany(model);

            return View(model);
        }

        [NonAction]
        private async Task<RegisterAgencyModel> InitRegisterCompany(RegisterAgencyModel model)
        {
            model.Sectors = Enumerable.Empty<SelectListItem>();
            model.Countries = Enumerable.Empty<SelectListItem>();
            model.States = Enumerable.Empty<SelectListItem>();
            model.Ministries = Enumerable.Empty<SelectListItem>();

            var sectorResponse = await WepApiMethod.SendApiAsync<List<SectorModel>>(HttpVerbs.Get, $"Administration/Sector");

            if (sectorResponse.isSuccess)
            {
                model.Sectors = new SelectList(sectorResponse.Data.OrderBy(o => o.Name), "Id", "Name", 0);
            }

            var countryResponse = await WepApiMethod.SendApiAsync<List<CountryModel>>(HttpVerbs.Get, $"Administration/Country");

            if (countryResponse.isSuccess)
            {
                model.Countries = new SelectList(countryResponse.Data.Where(c => c.Name != "Malaysia").OrderBy(o => o.Name), "Id", "Name", 0);
                model.MalaysiaCountryId = countryResponse.Data.Where(c => c.Name == "Malaysia").Select(s => s.Id).FirstOrDefault();
                model.CountryCode = countryResponse.Data.Where(c => c.Name == "Malaysia").Select(s => s.CountryCode).FirstOrDefault();
            }

            var stateResponse = await WepApiMethod.SendApiAsync<List<StateModel>>(HttpVerbs.Get, $"Administration/State");

            if (stateResponse.isSuccess)
            {
                var states = stateResponse.Data;

                model.States = new SelectList(states.OrderBy(o => o.Name), "Id", "Name", 0);

            }

            var stateMinistry = await WepApiMethod.SendApiAsync<List<MinistryModel>>(HttpVerbs.Get, $"Administration/Ministry");

            if (stateMinistry.isSuccess)
            {
                var ministries = stateMinistry.Data;

                model.Ministries = new SelectList(ministries.OrderBy(o => o.Name), "Id", "Name", 0);

            }

            return model;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterIndividual(RegisterIndividualModel model)
        {
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
            }

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={null}&email={model.Email}");

            if (emailResponse.Data)
            {
                ModelState.AddModelError("Email", Language.Auth.ValidIsExistEmail);
            }

            var icno = model.ICNo;

            if (!model.IsMalaysian)
            {
                icno = model.PassportNo;
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={null}&icno={icno}");

            if (icnoResponse.Data)
            {
                if (model.IsMalaysian)
                    ModelState.AddModelError("ICNo", Language.Auth.ValidIsExistICNo);
                else
                    ModelState.AddModelError("PassportNo", Language.Auth.ValidIsExistPassportNo);
            }

            var passwordResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Auth/ValidatePassword?password={model.Password}");

            if (!passwordResponse.isSuccess)
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(passwordResponse.ErrorMessage);

                if (error.ContainsKey("Message"))
                    ModelState.AddModelError("Password", error["Message"]);
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<dynamic>(HttpVerbs.Post, $"Auth/RegisterIndividual", model);

                if (response.isSuccess)
                {

                    ParameterListToSend notificationParameter = new ParameterListToSend();
                    notificationParameter.UserFullName = model.Name;
                    notificationParameter.Link = $"<a href = '" + BaseURL + "/Auth/ActivateAccount/" + response.Data.UID + "' > here </a>";
                    notificationParameter.LoginDetail = $"Email: { model.Email }\nPassword: { model.Password }";

                    CreateAutoReminder notification = new CreateAutoReminder
                    {
                        NotificationType = NotificationType.ActivateAccount,
                        NotificationCategory = NotificationCategory.Event,
                        ParameterListToSend = notificationParameter,
                        StartNotificationDate = DateTime.Now,
                        ReceiverId = new List<int> { (int)response.Data.UserId }
                    };

                    var responseNotification = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", notification);

                    TempData["SuccessMessage"] = Language.Auth.AlertRegisterSuccess;

                    return RedirectToAction("Login", "Auth", new { area = "" });

                }

            }

            model = await InitRegisterIndividual(model);

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAgency(RegisterAgencyModel model)
        {
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

            var emailResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsEmailExist?id={null}&email={model.Email}");

            if (emailResponse.Data)
            {
                ModelState.AddModelError("Email", Language.Auth.ValidIsExistEmail);
            }

            var icno = model.ICNo;

            if (model.Type == CompanyType.NonMalaysianCompany)
            {
                icno = model.PassportNo;
            }

            var icnoResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Administration/User/IsICNoExist?id={null}&icno={icno}");

            if (icnoResponse.Data)
            {
                if (model.Type == CompanyType.NonMalaysianCompany)
                    ModelState.AddModelError("PassportNo", Language.Auth.ValidIsExistPassportNo);
                else
                    ModelState.AddModelError("ICNo", Language.Auth.ValidIsExistICNo);
            }

            var passwordResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Auth/ValidatePassword?password={model.Password}");

            if (!passwordResponse.isSuccess)
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(passwordResponse.ErrorMessage);

                if (error.ContainsKey("Message"))
                    ModelState.AddModelError("Password", error["Message"]);
            }

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<dynamic>(HttpVerbs.Post, $"Auth/RegisterAgency", model);

                if (response.isSuccess)
                {

                    ParameterListToSend notificationParameter = new ParameterListToSend();
                    notificationParameter.UserFullName = model.Name;
                    notificationParameter.Link = $"<a href = '" + BaseURL + "/Auth/ActivateAccount/" + response.Data.UID + "' > here </a>";
                    notificationParameter.LoginDetail = $"Email: { model.Email }\nPassword: { model.Password }";

                    CreateAutoReminder notification = new CreateAutoReminder
                    {
                        NotificationType = NotificationType.ActivateAccount,
                        NotificationCategory = NotificationCategory.Event,
                        ParameterListToSend = notificationParameter,
                        StartNotificationDate = DateTime.Now,
                        ReceiverId = new List<int> { (int)response.Data.UserId }
                    };

                    var responseNotification = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", notification);

                    TempData["SuccessMessage"] = Language.Auth.AlertRegisterSuccess;

                    return RedirectToAction("Login", "Auth", new { area = "" });

                }

            }

            model = await InitRegisterCompany(model);

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StaffLogIn(StaffLogInModel model)
        {
            if (ModelState.IsValid)
            {
                if (await ADMethod.Login(model.LoginId, model.Password))
                {

                    var resUser = await WepApiMethod.SendApiAsync<DetailsUserModel>(HttpVerbs.Get, $"Administration/User?loginId={model.LoginId}");

                    if (resUser.isSuccess)
                    {
                        var user = resUser.Data;

                        if (user != null)
                        {
                            SignInUser(
                                new CurrentUserModel
                                {
                                    userid = user.Id,
                                    usertype = user.UserType.ToString(),
                                    loginid = user.LoginId,
                                    name = user.Name,
                                    email = user.Email,
                                    isenable = user.IsEnable,
                                    validfrom = user.ValidFrom,
                                    validto = user.ValidTo,
                                    access = user.UserAccesses.Select(s => ((int)s).ToString()).ToList()
                                }
                            );

                        }

                        await LogActivity(Modules.Admin, "User Sign In", new { UserId = user.Id, Name = user.Name }, user.Id);

                        return Redirect(GetRedirectUrl(model.ReturnUrl));

                    }

                }
                else
                {
                    TempData["ErrorMessage"] = Language.Auth.AlertLoginFail;
                }
            }

            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LogInModel model)
        {

            if (ModelState.IsValid)
            {
                var resLogin = await WepApiMethod.SendApiAsync<int?>(HttpVerbs.Get, $"Auth?loginId={model.LoginId}&password={model.Password}");

                if (resLogin.isSuccess)
                {
                    var userId = resLogin.Data;

                    if (userId != null)
                    {
                        var resUser = await WepApiMethod.SendApiAsync<DetailsUserModel>(HttpVerbs.Get, $"Administration/User?id={userId}");

                        if (resUser.isSuccess)
                        {
                            var user = resUser.Data;

                            if (user != null)
                            {
                                SignInUser(
                                    new CurrentUserModel
                                    {
                                        userid = user.Id,
                                        usertype = user.UserType.ToString(),
                                        loginid = user.LoginId,
                                        name = user.Name,
                                        email = user.Email,
                                        isenable = user.IsEnable,
                                        validfrom = user.ValidFrom,
                                        validto = user.ValidTo,
                                        access = user.UserAccesses.Select(s => ((int)s).ToString()).ToList()
                                    }
                                );

                            }

                            await LogActivity(Modules.Admin, "User Sign In", new { UserId = user.Id, Name = user.Name }, user.Id);

                            return Redirect(GetRedirectUrl(model.ReturnUrl));

                        }

                    }
                    else
                    {
                        TempData["ErrorMessage"] = Language.Auth.AlertLoginFail;
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = Language.Auth.AlertLoginFail;
                }

            }

            return View(model);
        }


        [NonAction]
        private void SignInUser(CurrentUserModel usermodel)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usermodel.userid.ToString()),
                new Claim("UserId", usermodel.userid.ToString()),
                new Claim("Name", usermodel.name != null ? usermodel.name.ToString() : ""),
                new Claim("Email", usermodel.email != null ? usermodel.email.ToString() : ""),
                new Claim("UserType", usermodel.usertype != null ? usermodel.usertype.ToString() : ""),
            }, "FEPCookie");

            foreach (string access in usermodel.access)
            {
                identity.AddClaim(new Claim("Access", access));
            }

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

        }

        [HttpGet]
        public ActionResult LogOut()
        {

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("FEPCookie");

            LogActivity(Modules.Admin, "User Sign Out");

            return RedirectToAction("Index", "Home", routeValues: null);

        }

        [NonAction]
        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Dashboard", "Home", routeValues: null);
            }

            return returnUrl;
        }

        [NonAction]
        private bool ADAuthenticate(string LoginId, string Password)
        {

            //by pass
            return true;

        }

        [AllowAnonymous]
        public async Task<ActionResult> ActivateAccount(string id)
        {

            if (id != null)
            {
                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Auth/ActivateAccount", id);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.Auth.AlertActivateSuccess;
                }
            }

            return RedirectToAction("LogIn");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<dynamic>(HttpVerbs.Post, $"Auth/ResetPassword", model);

                if (response.Data != null)
                {
                    //var uid = response.Data.UID;

                    //StringBuilder body = new StringBuilder();
                    //body.Append("Dear " + response.Data.Name + ",");
                    //body.Append("<br />");
                    //body.Append("You can reset your password <a href = '" + BaseURL + Url.Action("SetPassword", "Auth", new { id = response.Data }) + "' > here </a>");

                    //await EmailMethod.SendEmail("FE Portal Account Password Reset", body.ToString(), new EmailAddress { DisplayName = response.Data.Name, Address = model.Email });

                    ParameterListToSend notificationParameter = new ParameterListToSend();
                    notificationParameter.UserFullName = response.Data.Name;
                    notificationParameter.Link = $"<a href = '" + BaseURL + "/Auth/SetPassword/" + response.Data.UID + "' > here </a>";

                    CreateAutoReminder notification = new CreateAutoReminder
                    {
                        NotificationType = NotificationType.ResetPassword,
                        NotificationCategory = NotificationCategory.Event,
                        ParameterListToSend = notificationParameter,
                        StartNotificationDate = DateTime.Now,
                        ReceiverId = new List<int> { (int)response.Data.UserId }
                    };

                    var responseNotification = await WepApiMethod.SendApiAsync<ReminderResponse>(HttpVerbs.Post, $"Reminder/SLA/GenerateAutoNotificationReminder/", notification);
                }

                TempData["Message"] = Language.Auth.AlertResetPasswordSuccess;
                return RedirectToAction("Login");

            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SetPassword(string id)
        {

            var response = await WepApiMethod.SendApiAsync<SetPasswordModel>(HttpVerbs.Get, $"Auth/GetSetPassword?uid={id}");

            if (response.Data != null)
            {
                return View(response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = Language.Auth.AlertSetPasswordFail;
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordModel model)
        {
            var passwordResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"Auth/ValidatePassword?password={model.Password}");

            if (!passwordResponse.isSuccess)
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(passwordResponse.ErrorMessage);

                if (error.ContainsKey("Message"))
                    ModelState.AddModelError("Password", error["Message"]);
            }

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"Auth/SetPassword", model);

            if (response.Data)
            {
                TempData["SuccessMessage"] = Language.Auth.AlertSetPasswordSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = Language.Auth.AlertSetPasswordFail;
            }

            return RedirectToAction("Login", "Auth", new { area = "" });
        }


    }
}