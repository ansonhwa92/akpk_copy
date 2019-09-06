using FEP.Model;
using FEP.WebApiModel.Setting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Setting
{
    [Route("api/Setting/AccountSetting")]
    public class AccountSettingController : ApiController
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


        public IHttpActionResult Get()
        {
            var setting = db.AccountSetting.Select(s => new AccountSettingModel
            {
                IsPasswordExpiry = s.IsPasswordExpiry,
                PasswordExpiryDuration = s.PasswordExpiryDuration,
                IsLimitLoginAttempt = s.IsLimitLoginAttempt,
                LoginAttemptLimit = s.LoginAttemptLimit,
                IsLengthLimit = s.IsLengthLimit,
                IsContainUpperCase = s.IsContainUpperCase,
                IsContainLowerCase = s.IsContainLowerCase,
                IsContainNumeric = s.IsContainNumeric,
                IsContainSymbol = s.IsContainSymbol                            
            }).FirstOrDefault();

            if (setting != null)
            {
                return Ok(setting);
            }

            return NotFound();
        }


        [ValidationActionFilter]
        public IHttpActionResult Put([FromBody]EditAccountSettingModel model)
        {

            var setting = db.AccountSetting.FirstOrDefault();

            if (setting != null)
            {
                setting.IsPasswordExpiry = model.IsPasswordExpiry;
                setting.PasswordExpiryDuration = model.PasswordExpiryDuration;
                setting.IsLimitLoginAttempt = model.IsLimitLoginAttempt;
                setting.LoginAttemptLimit = model.LoginAttemptLimit;
                setting.IsLengthLimit = model.IsLengthLimit;
                setting.IsContainUpperCase = model.IsContainUpperCase;
                setting.IsContainLowerCase = model.IsContainLowerCase;
                setting.IsContainNumeric = model.IsContainNumeric;
                setting.IsContainSymbol = model.IsContainSymbol;

                db.Entry(setting).State = EntityState.Modified;
                
                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }


    }
}
