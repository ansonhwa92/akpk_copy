using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/User")]
    public class UserController : ApiController
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

        [HttpGet]
        public IHttpActionResult Get()
        {
            var users = db.User.Where(u => u.Display).Select(s => new UserModel
            {
                Id = s.Id,
                LoginId = s.UserAccount.LoginId,
                Name = s.Name,
                Email = s.Email,
                UserType = s.UserType,               
                IsEnable = s.UserAccount.IsEnable,
                ValidFrom = s.UserAccount.ValidFrom,
                ValidTo = s.UserAccount.ValidTo,
                LastLogin = s.UserAccount.LastLogin,
                LoginAttempt = s.UserAccount.LoginAttempt,
                LastPasswordChange = s.UserAccount.LastPasswordChange,
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate
            }).ToList();
                        
            return Ok(users);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var user = db.User.Where(u => u.Display && u.Id == id).Select(s => new UserModel
            {
                Id = s.Id,
                LoginId = s.UserAccount.LoginId,
                Name = s.Name,
                Email = s.Email,
                UserType = s.UserType,
                IsEnable = s.UserAccount.IsEnable,
                ValidFrom = s.UserAccount.ValidFrom,
                ValidTo = s.UserAccount.ValidTo,
                LastLogin = s.UserAccount.LastLogin,
                LoginAttempt = s.UserAccount.LoginAttempt,
                LastPasswordChange = s.UserAccount.LastPasswordChange,
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate
            }).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            //access
            var access = db.RoleAccess.Join(db.UserRole.Where(u => u.UserId == user.Id), s => s.RoleId, s => s.RoleId, (r, u) => new { Role = r }).Select(s => s.Role.UserAccess).ToList();

            user.UserAccesses = access;

            return Ok(user);
        }


        [HttpPost]
        [Route("api/Administration/User/GetUsers")]
        public IHttpActionResult Get(List<int> ids)
        {

            var users = new List<UserModel>();

            foreach (var id in ids)
            {
                var user = db.User.Where(u => u.Display && u.Id == id).Select(s => new UserModel
                {
                    Id = s.Id,
                    LoginId = s.UserAccount.LoginId,
                    Name = s.Name,
                    Email = s.Email,
                    UserType = s.UserType,
                    IsEnable = s.UserAccount.IsEnable,
                    ValidFrom = s.UserAccount.ValidFrom,
                    ValidTo = s.UserAccount.ValidTo,
                    LastLogin = s.UserAccount.LastLogin,
                    LoginAttempt = s.UserAccount.LoginAttempt,
                    LastPasswordChange = s.UserAccount.LastPasswordChange,
                    CreatedBy = s.CreatedBy,
                    CreatedDate = s.CreatedDate
                }).FirstOrDefault();

                if (user != null)
                {
                    users.Add(user);
                }
                
            }
            
            return Ok(users);
        }


        [Route("api/Administration/User/IsEmailExist")]
        [HttpGet]
        public IHttpActionResult IsEmailExist(int? id, string email)
        {

            if (id == null)
            {
                if (db.User.Any(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.User.Any(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }
            
            return NotFound();
        }

        [Route("api/Administration/User/IsICNoExist")]
        [HttpGet]
        public IHttpActionResult IsICNoExist(int? id, string icno)
        {
            if (id == null)
            {
                if (db.User.Any(u => u.ICNo.Equals(icno, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.User.Any(u => u.ICNo.Equals(icno, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }

        [Route("api/Administration/User/Activate")]
        [HttpPut]
        public IHttpActionResult Activate(int id)
        {
            var user = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (user == null)
            {
                //return Content(HttpStatusCode.BadRequest, "Any object");
                return NotFound();
            }

            user.IsEnable = true;

            db.UserAccount.Attach(user);
            db.Entry(user).Property(x => x.IsEnable).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok(true);
        }

        [Route("api/Administration/User/Deactivate")]
        [HttpPut]
        public IHttpActionResult Deactivate(int id)
        {
            var user = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (user == null)
            {
                //return Content(HttpStatusCode.BadRequest, "Any object");
                return NotFound();
            }

            user.IsEnable = false;

            db.UserAccount.Attach(user);
            db.Entry(user).Property(x => x.IsEnable).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok(true);
        }

        [Route("api/Administration/User/ResetPassword")]
        [HttpPut]
        public IHttpActionResult ResetPassword(int id)
        {
            var user = db.UserAccount.Where(u => u.UserId == id).FirstOrDefault();

            if (user == null)
            {
                //return Content(HttpStatusCode.BadRequest, "Any object");
                return NotFound();
            }

            user.IsEnable = false;

            db.UserAccount.Attach(user);
            db.Entry(user).Property(x => x.IsEnable).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok(true);
        }

        [Route("api/Administration/User/Delete")]
        [HttpPut]
        public IHttpActionResult Delete(int id)
        {
            var user = db.User.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                //return Content(HttpStatusCode.BadRequest, "Any object");
                return NotFound();
            }

            user.Display = false;

            db.User.Attach(user);
            db.Entry(user).Property(x => x.Display).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok(true);
        }

    }
}
