using FEP.Model;
using FEP.WebApiModel.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FEP.Helper;
using System.Diagnostics;
using System.Web;

namespace FEP.WebApi.Api.Home
{
    public class ProfileController : ApiController
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
                
        [Route("api/Home/Profile/EditIndividualProfile")]
        [HttpPut]        
        public IHttpActionResult EditIndividualProfile(int id, [FromBody] EditIndividualProfileModel model)
        {
            var user = db.User.Where(u => u.Id == id && u.Display).FirstOrDefault();
            var individual = db.IndividualProfile.Where(i => i.UserId == id).FirstOrDefault();
           
            if (user == null || individual == null)
            {
                return NotFound();
            }

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

            var countryCode = db.Country.Where(c => c.Id == model.CountryId && c.Display).FirstOrDefault();

            if (countryCode == null)
            {
                return InternalServerError();
            }

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.ICNo = model.ICNo;
                user.MobileNo = model.MobileNo;
                user.CountryCode = countryCode.CountryCode1;


                individual.IsMalaysian = model.IsMalaysian;
                individual.CitizenshipId = model.CitizenshipId;
                individual.Address1 = model.Address1;
                individual.Address2 = model.Address2;
                individual.PostCode = model.IsMalaysian ? model.PostCodeMalaysian : model.PostCodeNonMalaysian;
                individual.City = model.City;
                individual.StateId = model.IsMalaysian ? model.StateId : null;
                individual.StateName = model.IsMalaysian ? "" : model.State;
                individual.CountryId = model.CountryId;

                db.User.Attach(user);
                db.Entry(user).Property(x => x.Name).IsModified = true;
                db.Entry(user).Property(x => x.ICNo).IsModified = true;                
                db.Entry(user).Property(x => x.MobileNo).IsModified = true;
                db.Entry(user).Property(x => x.CountryCode).IsModified = true;

                db.Entry(individual).State = EntityState.Modified;

                db.Configuration.ValidateOnSaveEnabled = true;
                db.SaveChanges();

                return Ok(true);

            }

            return BadRequest(ModelState);

        }

        [Route("api/Home/Profile/EditCompanyProfile")]
        [HttpPut]        
        public IHttpActionResult EditCompanyProfile(int id, [FromBody] EditCompanyProfileModel model)
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

            var countryCode = db.Country.Where(c => c.Id == model.CountryId && c.Display).FirstOrDefault();

            if (countryCode == null)
            {
                return InternalServerError();
            }

            if (ModelState.IsValid)
            {

                var user = db.User.Where(u => u.Id == id).FirstOrDefault();
                var company = db.CompanyProfile.Where(c => c.UserId == id).FirstOrDefault();
               
                if (user == null || company == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.ICNo = model.Type == CompanyType.NonMalaysianCompany ? model.PassportNo : model.ICNo;                
                user.MobileNo = model.MobileNo;
                user.CountryCode = countryCode.CountryCode1;

                company.Type = model.Type;
                company.MinistryId = model.MinistryId;
                company.CompanyName = model.Type == CompanyType.Government ? model.AgencyName : model.CompanyName;
                company.CompanyRegNo = model.CompanyRegNo;
                company.SectorId = model.SectorId;
                company.Address1 = model.Address1;
                company.Address2 = model.Address2;
                company.City = model.City;
                company.StateId = model.StateId;
                company.StateName = model.State;
                company.PostCode = model.Type != CompanyType.NonMalaysianCompany ? model.PostCodeNonMalaysian : model.PostCodeMalaysian;
                company.CompanyPhoneNo = model.CompanyPhoneNo;
                company.CountryCode = countryCode.CountryCode1;

                db.User.Attach(user);
                db.Entry(user).Property(x => x.Name).IsModified = true;
                db.Entry(user).Property(x => x.ICNo).IsModified = true;                
                db.Entry(user).Property(x => x.MobileNo).IsModified = true;
                db.Entry(user).Property(x => x.CountryCode).IsModified = true;

                db.Entry(company).State = EntityState.Modified;

                db.Configuration.ValidateOnSaveEnabled = true;
                db.SaveChanges();

                return Ok(true);

            }

            return BadRequest(ModelState);
        }

        [Route("api/Home/Profile/ChangePassword")]
        [HttpPut]
        public IHttpActionResult ChangePassword(int id, ChangePasswordModel model)
        {

            if (model.ConfirmPassword != model.Password)
            {
                return BadRequest("Password not match");
            }

            var account = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (Authentication.VerifyPassword(model.Password, account.HashPassword, account.Salt))
            {
                return BadRequest("Password not match");
            }

            Authentication.GeneratePassword(model.Password);
            
            account.HashPassword = Authentication.HashPassword;
            account.Salt = Authentication.Salt;

            db.UserAccount.Attach(account);
            db.Entry(account).Property(e => e.HashPassword).IsModified = true;
            db.Entry(account).Property(e => e.Salt).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return Ok(true);

        }

        [Route("api/Home/Profile/ChangeEmail")]
        [HttpPut]
        public IHttpActionResult ChangeEmail(int id, ChangeEmailModel model)
        {
            
            var user = db.User.Where(u => u.Id == id).FirstOrDefault();
            var account = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (user == null || account == null)
                return BadRequest();

            user.Email = model.Email;
            account.LoginId = model.Email;
            account.IsEnable = false;
           
            db.User.Attach(user);
            db.Entry(user).Property(e => e.Email).IsModified = true;
           
            db.UserAccount.Attach(account);
            db.Entry(account).Property(e => e.LoginId).IsModified = true;
            db.Entry(account).Property(e => e.IsEnable).IsModified = true;

            ActivateAccount activateaccount = new ActivateAccount
            {
                UID = Authentication.RandomString(50, true),//random alphanumeric
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                IsActivate = false
            };

            db.ActivateAccount.Add(activateaccount);

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return Ok(new { UserId = user.Id, UID = activateaccount.UID });

        }

        [Route("api/Home/Profile/UpdateAvatar")]
        [HttpPut]
        [ValidationActionFilter]
        public IHttpActionResult UpdateAvatar(int id, string imageUrl)
        {           
            var account = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (account == null)
                return BadRequest();
                        
            account.Avatar = imageUrl;

            db.UserAccount.Attach(account);
            db.Entry(account).Property(e => e.Avatar).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return Ok();

        }
        
    }
}
