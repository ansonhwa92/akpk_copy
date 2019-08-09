﻿using FEP.Model;
using FEP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FEP.Helper;

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

        [HttpPost]
        public int? RegisterIndividual(RegisterIndividualApiModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    var password = Authentication.RandomString(10);
            //    Authentication.GeneratePassword(password);

            //    var account = new UserAccount
            //    {
            //        LoginId = model.Email,
            //        IsEnable = false,
            //        HashPassword = Authentication.HashPassword,
            //        Salt = Authentication.Salt,
            //        LoginAttempt = 0
            //    };

            //    var user = new User
            //    {
            //        UserType = UserType.Individual,
            //        Name = model.Name,
            //        Email = model.Email,
            //        ICNo = model.ICNo,
            //        MobileNo = model.MobileNo,
            //        Display = true,
            //        CreatedBy = model.CreatedBy,
            //        UserAccount = account
            //    };

            //    db.User.Add(user);

            //    ActivateAccount activateaccount = new ActivateAccount
            //    {
            //        UID = Authentication.RandomString(50, true),//random alphanumeric
            //        UserId = user.Id,
            //        CreatedDate = DateTime.Now,
            //        IsActivate = false
            //    };

            //    db.ActivateAccount.Add(activateaccount);

            //    db.SaveChanges();
            //}

            return null;
        }


    }
}