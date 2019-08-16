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

        [Route("api/Auth/RegisterIndividual")]
        [HttpPost]
        [ValidationActionFilter]
        public string RegisterIndividual([FromBody] RegisterIndividualModel model)
        {

            //var password = Authentication.RandomString(10);
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

            //var password = Authentication.RandomString(10);
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
                StateId = model.StateId,
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
        public bool ActivateAccount([FromBody] string UID)
        {

            if (ModelState.IsValid)
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
            }
            return false;


        }
    }
}
