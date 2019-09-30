using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/Individual")]
    public class IndividualController : ApiController
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

        [Route("api/Administration/Individual/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterIndividualModel request)
        {

            var query = db.User.Where(u => u.Display && u.UserType == UserType.Individual);

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.Name == null || s.Name.Contains(request.Name))
               && (request.ICNo == null || s.ICNo.Contains(request.ICNo))
               && (request.Email == null || s.Email.Contains(request.Email))
               && (request.MobileNo == null || s.MobileNo.Contains(request.MobileNo))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.Name.Contains(value)
                || p.ICNo.Contains(value)
                || p.Email.Contains(value)
                || p.MobileNo.Contains(value)
                );
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Name":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Name);
                        }

                        break;

                    case "ICNo":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.ICNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.ICNo);
                        }

                        break;

                    case "Email":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Email);
                        }

                        break;

                    case "MobileNo":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.MobileNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.MobileNo);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.Name);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.Name);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new IndividualModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    ICNo = s.ICNo,
                    MobileNo = s.MobileNo,
                    Status = s.UserAccount.IsEnable
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }


        public IHttpActionResult Get(int id)
        {
            var user = db.User.Where(u => u.Id == id)
                .Select(s => new DetailsIndividualModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    MobileNo = s.MobileNo,
                    ICNo = s.ICNo,
                    Status = s.UserAccount.IsEnable
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            user.RoleIds = db.UserRole.Where(u => u.UserId == id).Select(s => s.RoleId).ToArray();


            return Ok(user);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CreateIndividualModel model)
        {

            //var password = Authentication.RandomString(10, true);

            var password = "abc123";

            Authentication.GeneratePassword(password);

            var account = new UserAccount
            {
                LoginId = model.Email,
                IsEnable = true,
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
                Display = true,
                CreatedBy = null,
                CreatedDate = DateTime.Now,
                UserAccount = account
            };

            foreach (var roleid in model.RoleIds)
            {
                var userrole = new UserRole
                {
                    RoleId = roleid,
                    UserAccount = account,
                };

                db.UserRole.Add(userrole);
            }
            
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

            return Ok<CreateUserResponse>(new CreateUserResponse { Password = password, UID = activateaccount.UID });

        }

        public IHttpActionResult Put(int id, [FromBody] EditIndividualModel model)
        {
            var user = db.User.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                //return Content(HttpStatusCode.BadRequest, "Any object");
                return NotFound();
            }

            user.Name = model.Name;
            user.ICNo = model.ICNo;
            user.Email = model.Email;
            user.MobileNo = model.MobileNo;

            db.User.Attach(user);
            db.Entry(user).Property(x => x.Name).IsModified = true;
            db.Entry(user).Property(x => x.ICNo).IsModified = true;
            db.Entry(user).Property(x => x.Email).IsModified = true;
            db.Entry(user).Property(x => x.MobileNo).IsModified = true;

            db.UserRole.RemoveRange(db.UserRole.Where(u => u.UserId == id));//remove all
            foreach (var roleid in model.RoleIds)
            {
                var userrole = new UserRole
                {
                    RoleId = roleid,
                    UserId = id,
                };

                db.UserRole.Add(userrole);
            }
            
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok(true);
        }



    }
}
