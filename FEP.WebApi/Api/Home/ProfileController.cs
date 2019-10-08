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

        //[Route("api/Home/Profile/GetIndividualProfile")]
        //[HttpGet]
        //public IndividualProfileModel GetIndividualProfile(int id)
        //{
        //    var user = db.User.Where(u => u.Id == id)
        //        .Select(s => new IndividualProfileModel
        //        {
        //            Name = s.Name,
        //            Email = s.Email,
        //            ICNo = s.ICNo,
        //            MobileNo = s.MobileNo
        //        })
        //        .FirstOrDefault();

        //    return user;
        //}

        //[Route("api/Home/Profile/GetCompanyProfile")]
        //[HttpGet]
        //public CompanyProfileModel GetCompanyProfile(int id)
        //{
        //    var user = db.User.Where(u => u.Id == id)
        //        .Select(s => new CompanyProfileModel
        //        {
        //            CompanyName = s.CompanyProfile.CompanyName,
        //            CompanyRegNo = s.CompanyProfile.CompanyRegNo,
        //            SectorId = s.CompanyProfile.SectorId,
        //            Sector = s.CompanyProfile.Sector.Name,
        //            Address1 = s.CompanyProfile.Address1,
        //            Address2 = s.CompanyProfile.Address2,
        //            City = s.CompanyProfile.City,
        //            PostCode = s.CompanyProfile.PostCode,
        //            State = s.CompanyProfile.StateName,
        //            CompanyPhoneNo = s.CompanyProfile.CompanyPhoneNo,
        //            Name = s.Name,
        //            Email = s.Email,
        //            ICNo = s.ICNo,
        //            MobileNo = s.MobileNo
        //        })
        //        .FirstOrDefault();

        //    return user;
        //}

        [Route("api/Home/Profile/EditIndividualProfile")]
        [HttpPut]        
        public IHttpActionResult EditIndividualProfile(int id, [FromBody] EditIndividualProfileModel model)
        {
            var user = db.User.Where(u => u.Id == id && u.Display).FirstOrDefault();
            var individual = db.IndividualProfile.Where(i => i.UserId == id).FirstOrDefault();
            var useraccount = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (user == null || individual == null || useraccount == null)
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

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.ICNo = model.ICNo;
                user.Email = model.Email;
                user.MobileNo = model.MobileNo;

                useraccount.LoginId = model.Email;

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
                db.Entry(user).Property(x => x.Email).IsModified = true;
                db.Entry(user).Property(x => x.MobileNo).IsModified = true;

                db.UserAccount.Attach(useraccount);
                db.Entry(useraccount).Property(x => x.LoginId).IsModified = true;

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

            if (ModelState.IsValid)
            {

                var user = db.User.Where(u => u.Id == id).FirstOrDefault();
                var company = db.CompanyProfile.Where(c => c.UserId == id).FirstOrDefault();
                var useraccount = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

                if (user == null || company == null || useraccount == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.ICNo = model.Type == CompanyType.NonMalaysianCompany ? model.PassportNo : model.ICNo;
                user.Email = model.Email;
                user.MobileNo = model.MobileNo;

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

                useraccount.LoginId = model.Email;

                db.User.Attach(user);
                db.Entry(user).Property(x => x.Name).IsModified = true;
                db.Entry(user).Property(x => x.ICNo).IsModified = true;
                db.Entry(user).Property(x => x.Email).IsModified = true;
                db.Entry(user).Property(x => x.MobileNo).IsModified = true;

                db.UserAccount.Attach(useraccount);
                db.Entry(useraccount).Property(x => x.LoginId).IsModified = true;

                db.Entry(company).State = EntityState.Modified;

                db.Configuration.ValidateOnSaveEnabled = true;
                db.SaveChanges();

                return Ok(true);

            }

            return BadRequest(ModelState);
        }

        [Route("api/Home/Profile/ChangePassword")]
        [HttpPut]
        public bool ChangePassword(int id, ChangePasswordModel model)
        {

            if (model.ConfirmPassword != model.Password)
            {
                return false;
            }

            var account = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (Authentication.VerifyPassword(model.Password, account.HashPassword, account.Salt))
            {
                return false;
            }

            Authentication.GeneratePassword(model.Password);
            
            account.HashPassword = Authentication.HashPassword;
            account.Salt = Authentication.Salt;

            db.UserAccount.Attach(account);
            db.Entry(account).Property(e => e.HashPassword).IsModified = true;
            db.Entry(account).Property(e => e.Salt).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            return true;

        }


        
    }
}
