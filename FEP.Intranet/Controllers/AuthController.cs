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
//using FEP.WebApiModel;
using FEP.Model;
using FEP.WebApiModel.Auth;
using FEP.WebApiModel.Administration;

namespace FEP.Intranet.Controllers
{
    public class AuthController : FEPController
    {

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterIndividual()
        {
            var model = new RegisterIndividualModel();
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> RegisterAgency()
        {
            var model = new RegisterAgencyModel();

            model.Sectors = Enumerable.Empty<SelectListItem>();

            var sectorResponse = await WepApiMethod.SendApiAsync<List<SectorModel>>(HttpVerbs.Get, $"Administration/Sector");

            if (sectorResponse.isSuccess)
            {
                model.Sectors = new SelectList(sectorResponse.Data, "Id", "Name", 0);
            }
                        
            model.States = Enumerable.Empty<SelectListItem>();

            var stateResponse = await WepApiMethod.SendApiAsync<List<StateModel>>(HttpVerbs.Get, $"Administration/State");

            if (stateResponse.isSuccess)
            {
                model.States = new SelectList(stateResponse.Data, "Id", "Name", 0);
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterIndividual(RegisterIndividualModel model)
        {

            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Auth/RegisterIndividual", model);

                if (response.isSuccess)
                {

                    EmailAddress receiver = new EmailAddress()
                    {
                        DisplayName = model.Name,
                        Address = model.Email
                    };

                    StringBuilder body = new StringBuilder();

                    body.Append("Dear " + model.Name + ",");
                    body.Append("<br />");
                    body.Append("You can activate your account <a href = '" + BaseURL + Url.Action("ActivateAccount", "Auth", new { id = response.Data }) + "' > here </a>");
                    body.Append("<br />");
                    body.Append("Your login details:");
                    body.Append("<br />");
                    body.Append("Login Id: " + model.Email);
                    body.Append("<br />");
                    body.Append("Password: " + model.Password);

                    SendEmail("FEP Account Activation", body.ToString(), receiver); //email

                    TempData["SuccessMessage"] = "Your account successfully created. Please check your registered email for login details.";

                    return RedirectToAction("Login", "Auth", new { area = "" });

                }

            }

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAgency(RegisterAgencyModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"Auth/RegisterAgency", model);

                if (response.isSuccess)
                {

                    EmailAddress receiver = new EmailAddress()
                    {
                        DisplayName = model.Name,
                        Address = model.Email
                    };

                    StringBuilder body = new StringBuilder();

                    body.Append("Dear " + model.Name + ",");
                    body.Append("<br />");
                    body.Append("You can activate your account <a href = '" + BaseURL + Url.Action("ActivateAccount", "Auth", new { id = response.Data }) + "' > here </a>");
                    body.Append("<br />");
                    body.Append("Your login details:");
                    body.Append("<br />");
                    body.Append("Login Id: " + model.Email);
                    body.Append("<br />");
                    body.Append("Password: " + model.Password);

                    SendEmail("FEP Account Activation", body.ToString(), receiver); //email

                    TempData["SuccessMessage"] = "Your account successfully created. Please check your registered email for login details.";

                    return RedirectToAction("Login", "Auth", new { area = "" });

                }

            }

            model.Sectors = Enumerable.Empty<SelectListItem>();

            var sectorResponse = await WepApiMethod.SendApiAsync<List<SectorModel>>(HttpVerbs.Get, $"Administration/Sector");

            if (sectorResponse.isSuccess)
            {
                model.Sectors = new SelectList(sectorResponse.Data, "Id", "Name", 0);
            }

            model.States = Enumerable.Empty<SelectListItem>();

            var stateResponse = await WepApiMethod.SendApiAsync<List<StateModel>>(HttpVerbs.Get, $"Administration/State");

            if (stateResponse.isSuccess)
            {
                model.States = new SelectList(stateResponse.Data, "Id", "Name", 0);
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
                        var resUser = await WepApiMethod.SendApiAsync<UserApiModel>(HttpVerbs.Get, $"Administration/User?id={userId}");

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
                        }

                        //LogActivity();

                        return Redirect(GetRedirectUrl(model.ReturnUrl));

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Sign in fail. Please use your email and correct password.";
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = "Sign in fail. Please use your email and correct password.";
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

            //LogActivity("Logout System", );

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
                    TempData["SuccessMessage"] = "Your account successfully activate. Please check your registered email for login details.";
                }
            }
                        
            return RedirectToAction("LogIn");
        }

    }
}