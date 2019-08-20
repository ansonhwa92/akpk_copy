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

        [Route("api/Home/Profile/GetIndividualProfile")]
        [HttpGet]
        public IndividualProfileModel GetIndividualProfile(int id)
        {
            var user = db.User.Where(u => u.Id == id)
                .Select(s => new IndividualProfileModel
                {
                    Name = s.Name,
                    Email = s.Email,
                    ICNo = s.ICNo,
                    MobileNo = s.MobileNo
                })
                .FirstOrDefault();

            return user;
        }

        [Route("api/Home/Profile/GetCompanyProfile")]
        [HttpGet]
        public CompanyProfileModel GetCompanyProfile(int id)
        {
            var user = db.User.Where(u => u.Id == id)
                .Select(s => new CompanyProfileModel
                {
                    CompanyName = s.CompanyProfile.CompanyName,
                    CompanyRegNo = s.CompanyProfile.CompanyRegNo,
                    SectorId = s.CompanyProfile.SectorId,
                    Sector = s.CompanyProfile.Sector.Name,
                    Address1 = s.CompanyProfile.Address1,
                    Address2 = s.CompanyProfile.Address2,
                    City = s.CompanyProfile.City,
                    PostCode = s.CompanyProfile.PostCode,
                    State = s.CompanyProfile.State,
                    CompanyPhoneNo = s.CompanyProfile.CompanyPhoneNo,
                    Name = s.Name,
                    Email = s.Email,
                    ICNo = s.ICNo,
                    MobileNo = s.MobileNo
                })
                .FirstOrDefault();

            return user;
        }

        [Route("api/Home/Profile/EditIndividualProfile")]
        [HttpPut]
        [ValidationActionFilter]
        public bool EditIndividualProfile(int id, [FromBody] EditIndividualProfileModel model)
        {
            var user = db.User.Where(u => u.Id == id).FirstOrDefault();

            user.Name = model.Name;
            user.MobileNo = model.MobileNo;

            db.User.Attach(user);
            db.Entry(user).Property(m => m.Name).IsModified = true;
            db.Entry(user).Property(m => m.MobileNo).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = false;

            db.SaveChanges();

            return true;
        }

        [Route("api/Home/Profile/EditCompanyProfile")]
        [HttpPut]
        [ValidationActionFilter]
        public bool EditCompanyProfile(int id, [FromBody] EditCompanyProfileModel model)
        {
            var user = db.User.Where(u => u.Id == id).FirstOrDefault();

            user.Name = model.Name;
            user.MobileNo = model.MobileNo;

            db.User.Attach(user);
            db.Entry(user).Property(m => m.Name).IsModified = true;
            db.Entry(user).Property(m => m.MobileNo).IsModified = true;

            var company = db.CompanyProfile.Where(c => c.UserId == id).FirstOrDefault();

            company.CompanyName = model.CompanyName;
            company.CompanyRegNo = model.CompanyRegNo;
            company.SectorId = model.SectorId;
            company.Address1 = model.Address1;
            company.Address2 = model.Address2;
            company.City = model.Address2;
            company.PostCode = model.PostCode;
            company.State = model.State;
            company.CompanyPhoneNo = model.CompanyPhoneNo;

            db.Entry(company).State = EntityState.Modified;

            db.SaveChanges();

            return true;

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
