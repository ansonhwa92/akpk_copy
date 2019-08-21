using FEP.Model;
using FEP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FEP.Helper;
using FEP.WebApiModel.Auth;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Auth")]
    public class AuthController : ApiController
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

        public int? Get(string LoginId, string Password)
        {

            var user = db.UserAccount.Where(u => u.LoginId == LoginId).FirstOrDefault();

            if (user != null)
            {
                if (Authentication.VerifyPassword(Password, user.HashPassword, user.Salt))
                {
                    return user.UserId;
                }
            }

            return null;
        }

        [Route("api/Auth/AuthenticatePassword")]
        [HttpGet]
        [ValidationActionFilter]
        public bool AuthenticatePassword(int id, string Password)
        {
            var user = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (user != null)
            {
                if (Authentication.VerifyPassword(Password, user.HashPassword, user.Salt))
                {
                    return true;
                }
            }

            return false;
        }

        [Route("api/Auth/RegisterIndividual")]
        [HttpPost]
        [ValidationActionFilter]
        public string RegisterIndividual([FromBody] RegisterIndividualModel model)
        {

            Authentication.GeneratePassword(model.Password);

            var account = new UserAccount
            {
                LoginId = model.Email,
                IsEnable = false,
                HashPassword = Authentication.HashPassword,
                Salt = Authentication.Salt,
                LoginAttempt = 0
            };

            var user = new User
            {
                UserType = UserType.Individual,
                Name = model.Name,
                Email = model.Email,
                ICNo = model.ICNo,
                MobileNo = model.MobileNo,
                Display = false,
                CreatedBy = null,
                CreatedDate = DateTime.Now,
                UserAccount = account
            };

            db.User.Add(user);

            ActivateAccount activateaccount = new ActivateAccount
            {
                UID = Authentication.RandomString(50, true),//random alphanumeric
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                IsActivate = false
            };

            db.ActivateAccount.Add(activateaccount);

            db.SaveChanges();

            return activateaccount.UID;

        }

        [Route("api/Auth/RegisterAgency")]
        [HttpPost]
        [ValidationActionFilter]
        public string RegisterAgency([FromBody] RegisterAgencyModel model)
        {

            Authentication.GeneratePassword(model.Password);

            var account = new UserAccount
            {
                LoginId = model.Email,
                IsEnable = false,
                HashPassword = Authentication.HashPassword,
                Salt = Authentication.Salt,
                LoginAttempt = 0
            };

            var company = new CompanyProfile
            {
                CompanyName = model.CompanyName,
                CompanyRegNo = model.CompanyRegNo,
                SectorId = model.SectorId,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                PostCode = model.PostCode,
                State = model.State,
                CompanyPhoneNo = model.CompanyPhoneNo
            };

            var user = new User
            {
                UserType = UserType.Company,
                Name = model.Name,
                Email = model.Email,
                ICNo = model.ICNo,
                MobileNo = model.MobileNo,
                Display = false,
                CreatedBy = null,
                CreatedDate = DateTime.Now,
                UserAccount = account,
                CompanyProfile = company
            };

            db.User.Add(user);

            ActivateAccount activateaccount = new ActivateAccount
            {
                UID = Authentication.RandomString(50, true),//random alphanumeric
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                IsActivate = false
            };

            db.ActivateAccount.Add(activateaccount);

            db.SaveChanges();

            return activateaccount.UID;
        }

        [Route("api/Auth/ActivateAccount")]
        [HttpPost]
        [ValidationActionFilter]
        public bool ActivateAccount([FromBody] string UID)
        {

            var activateaccount = db.ActivateAccount.Where(m => m.UID == UID && m.IsActivate == false).FirstOrDefault();

            if (activateaccount != null)
            {
                activateaccount.IsActivate = true;
                activateaccount.ActivateDate = DateTime.Now;
                db.ActivateAccount.Attach(activateaccount);
                db.Entry(activateaccount).Property(e => e.IsActivate).IsModified = true;
                db.Entry(activateaccount).Property(e => e.ActivateDate).IsModified = true;

                UserAccount userAccount = new UserAccount()
                {
                    UserId = activateaccount.UserId,
                    IsEnable = true
                };

                db.UserAccount.Attach(userAccount);
                db.Entry(userAccount).Property(e => e.IsEnable).IsModified = true;

                User user = new User
                {
                    Id = activateaccount.UserId,
                    Display = true
                };

                db.User.Attach(user);
                db.Entry(user).Property(e => e.Display).IsModified = true;

                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return true;

            }

            return false;

        }

        [Route("api/Auth/ResetPassword")]
        [HttpPost]
        [ValidationActionFilter]
        public ResetPasswordResponseModel ResetPassword([FromBody] ResetPasswordModel model)
        {
            //check email if exist.                
            var user = db.User.Where(u => u.Email == model.Email).FirstOrDefault();

            if (user != null)
            {
                db.PasswordReset.RemoveRange(db.PasswordReset.Where(x => x.UserId == user.Id && x.IsReset == false));
                db.SaveChanges();

                PasswordReset pwdreset = new PasswordReset
                {
                    UID = Authentication.RandomString(50, true),//random alphanumeric
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                    IsReset = false
                };

                db.PasswordReset.Add(pwdreset);
                db.SaveChanges();

                return new ResetPasswordResponseModel { Name = user.Name, UID = pwdreset.UID };
            }

            return null;

        }


        [Route("api/Auth/GetSetPassword")]
        [HttpGet]
        public SetPasswordModel GetSetPassword(string uid)
        {

            var pwdreset = db.PasswordReset.Where(m => m.UID == uid && m.IsReset == false).FirstOrDefault();
            if (pwdreset != null)
            {
                var setpwd = new SetPasswordModel() { PasswordResetId = pwdreset.Id, UID = uid };
                return setpwd;

            }

            return null;


        }

        [Route("api/Auth/SetPassword")]
        [HttpPost]
        [ValidationActionFilter]
        public bool SetPassword([FromBody] SetPasswordModel model)
        {

            var pwdreset = db.PasswordReset.Where(m => m.Id == model.PasswordResetId && m.UID == model.UID && m.IsReset == false).FirstOrDefault();

            if (pwdreset != null)
            {

                var user = db.UserAccount.Where(u => u.UserId == pwdreset.UserId).FirstOrDefault();

                if (user != null)
                {

                    //set password
                    Authentication.GeneratePassword(model.Password);

                    user.HashPassword = Authentication.HashPassword;
                    user.Salt = Authentication.Salt;

                    db.UserAccount.Attach(user);
                    db.Entry(user).Property(e => e.HashPassword).IsModified = true;
                    db.Entry(user).Property(e => e.Salt).IsModified = true;

                                       
                    //update password reset
                    pwdreset.IsReset = true;
                    pwdreset.ResetDate = DateTime.Now;
                    db.PasswordReset.Attach(pwdreset);
                    db.Entry(pwdreset).Property(e => e.IsReset).IsModified = true;
                    db.Entry(pwdreset).Property(e => e.ResetDate).IsModified = true;

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    return true;

                }

            }

            return false;


        }

    }
}
