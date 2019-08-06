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
using FEP.WebApiModel;
using FEP.Model;

namespace FEP.Intranet.Controllers
{
    public class AuthController : FEPController
    {
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult _Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _ExternalLogIn(LogInModel model)
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

                        return Json(new { isSuccess = true, returnUrl = GetRedirectUrl(model.ReturnUrl) });

                    }

                }
                
            }

            return Json(new { isSuccess = false });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _InternalLogIn(LogInModel model)
        {

            if (ModelState.IsValid)
            {
                
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

            return RedirectToAction("Index", "Home", routeValues : null);

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

    }
}