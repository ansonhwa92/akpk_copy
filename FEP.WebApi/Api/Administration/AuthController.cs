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

        public IHttpActionResult Get(string LoginId, string Password)
        {

            var user = db.UserAccount.Where(u => u.LoginId == LoginId && u.IsEnable).FirstOrDefault();

            if (user != null)
            {
                if (Authentication.VerifyPassword(Password, user.HashPassword, user.Salt))
                {
                    return Ok(user.UserId);
                }
            }

            return NotFound();
        }

        [Route("api/Auth/AuthenticatePassword")]
        [HttpGet]
        [ValidationActionFilter]
        public IHttpActionResult AuthenticatePassword(int id, string Password)
        {
            var user = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (user != null)
            {
                if (Authentication.VerifyPassword(Password, user.HashPassword, user.Salt))
                {
                    return Ok(true);
                }
            }

            return NotFound();
        }

        [Route("api/Auth/RegisterIndividual")]
        [HttpPost]        
        public IHttpActionResult RegisterIndividual([FromBody] RegisterIndividualModel model)
        {

            if (model.IsMalaysian)
            {
                ModelState.Remove("model.PassportNo");
                ModelState.Remove("model.CitizenshipId");
                ModelState.Remove("model.PostCodeNonMalaysian");
                ModelState.Remove("model.State");                              
            }
            else
            {
                ModelState.Remove("model.ICNo");
                ModelState.Remove("model.PostCodeMalaysian");
                ModelState.Remove("model.StateId");
            }

            if (ModelState.IsValid)
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

                var individual = new IndividualProfile
                {
                    IsMalaysian = model.IsMalaysian,
                    CitizenshipId = model.CitizenshipId,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    PostCode = model.IsMalaysian ? model.PostCodeMalaysian : model.PostCodeNonMalaysian,
                    City = model.City,
                    StateName = model.State,
                    StateId = model.StateId,
                    CountryId = model.CountryId                    
                };

                var user = new User
                {
                    UserType = UserType.Individual,
                    Name = model.Name,
                    Email = model.Email,
                    ICNo = model.ICNo,
                    MobileNo = model.MobileNo,
                    Display = true,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    UserAccount = account,
                    IndividualProfile = individual
                };
                                
                db.User.Add(user);

                ActivateAccount activateAccount = new ActivateAccount
                {
                    UID = Authentication.RandomString(50, true),//random alphanumeric
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                    IsActivate = false
                };

                db.ActivateAccount.Add(activateAccount);

                db.SaveChanges();

                return Ok(new { UserId = user.Id, UID = activateAccount.UID });
            }

            return BadRequest(ModelState);

        }

        [Route("api/Auth/RegisterAgency")]
        [HttpPost]
        public IHttpActionResult RegisterAgency([FromBody] RegisterAgencyModel model)
        {
            if (model.Type == CompanyType.Government)
            {
                ModelState.Remove("model.PassportNo");
                ModelState.Remove("model.PostCodeNonMalaysian");
                ModelState.Remove("model.State");
                ModelState.Remove("model.CountryId");
                ModelState.Remove("model.CompanyName");
                ModelState.Remove("model.CompanyRegNo");
                ModelState.Remove("model.SectorId");               
            }
            else if (model.Type == CompanyType.MalaysianCompany)
            {
                ModelState.Remove("model.PassportNo");
                ModelState.Remove("model.PostCodeNonMalaysian");
                ModelState.Remove("model.State");
                ModelState.Remove("model.CountryId");
                ModelState.Remove("model.AgencyName");
                ModelState.Remove("model.MinistryId");                
            }
            else
            {
                ModelState.Remove("model.ICNo");
                ModelState.Remove("model.PostCodeMalaysian");
                ModelState.Remove("model.StateId");
                ModelState.Remove("model.AgencyName");
                ModelState.Remove("model.MinistryId");
                ModelState.Remove("model.CompanyRegNo");
            }

            if (ModelState.IsValid)
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
                    Type = model.Type,
                    CompanyName = model.Type == CompanyType.Government ? model.AgencyName : model.CompanyName,
                    MinistryId = model.MinistryId,
                    CompanyRegNo = model.CompanyRegNo,
                    SectorId = model.SectorId,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    PostCode = model.Type == CompanyType.NonMalaysianCompany ? model.PostCodeNonMalaysian : model.PostCodeMalaysian,
                    StateId = model.StateId,
                    StateName = model.State,
                    CountryId = model.CountryId,
                    CompanyPhoneNo = model.CompanyPhoneNo
                };

                var user = new User
                {
                    UserType = UserType.Company,
                    Name = model.Name,
                    Email = model.Email,
                    ICNo = model.Type == CompanyType.NonMalaysianCompany ? model.PassportNo : model.ICNo,
                    MobileNo = model.MobileNo,
                    Display = true,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    UserAccount = account,
                    CompanyProfile = company
                };

                db.User.Add(user);

                ActivateAccount activateAccount = new ActivateAccount
                {
                    UID = Authentication.RandomString(50, true),//random alphanumeric
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                    IsActivate = false
                };

                db.ActivateAccount.Add(activateAccount);

                db.SaveChanges();

                return Ok(new { UserId = user.Id, UID = activateAccount.UID });
            }

            return BadRequest(ModelState);
        }

        [Route("api/Auth/ActivateAccount")]
        [HttpPost]
        [ValidationActionFilter]
        public IHttpActionResult ActivateAccount([FromBody] string UID)
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

                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);

            }

            return NotFound();

        }

        [Route("api/Auth/ResetPassword")]
        [HttpPost]
        [ValidationActionFilter]
        public IHttpActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            //check email if exist.                
            var user = db.User.Where(u => u.Email == model.Email && u.Display).FirstOrDefault();

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

                return Ok(new { UserId = user.Id, Name = user.Name, UID = pwdreset.UID });
            }

            return NotFound();

        }


        [Route("api/Auth/GetSetPassword")]
        [HttpGet]
        public IHttpActionResult GetSetPassword(string uid)
        {

            var pwdreset = db.PasswordReset.Where(m => m.UID == uid && m.IsReset == false).FirstOrDefault();
            if (pwdreset != null)
            {
                var setpwd = new SetPasswordModel() { PasswordResetId = pwdreset.Id, UID = uid };
                return Ok(setpwd);
            }

            return NotFound();

        }

        [Route("api/Auth/SetPassword")]
        [HttpPost]
        [ValidationActionFilter]
        public IHttpActionResult SetPassword([FromBody] SetPasswordModel model)
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

                    return Ok(true);

                }

            }

            return NotFound();


        }

    }
}
