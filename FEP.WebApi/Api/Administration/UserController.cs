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

        // GET: api/User
        public List<UserApiModel> Get()
        {
            var users = db.User.Where(u => u.Display).Select(s => new UserApiModel
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
                        
            return users;
        }

        // GET: api/User/5
        public UserApiModel Get(int id)
        {
            var user = db.User.Where(u => u.Display && u.Id == id).Select(s => new UserApiModel
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

            //access
            var access = db.RoleAccess.Join(db.UserRole.Where(u => u.UserId == user.Id), s => s.RoleId, s => s.RoleId, (r, u) => new { Role = r }).Select(s => s.Role.UserAccess).ToList();

            user.UserAccesses = access;

            return user;
        }
                
        // POST: api/User
        public HttpResponseMessage Post([FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // PUT: api/User/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // DELETE: api/User/5
        public bool Delete(int id)
        {
            var user = db.User.Where(u => u.Id == id).FirstOrDefault();

            if (user != null)
            {
                user.Display = false;

                db.User.Attach(user);
                db.Entry(user).Property(m => m.Display).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return true;
            }

            return false;
            
        }

        

    }
}
