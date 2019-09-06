using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/Company")]
    public class CompanyController : ApiController
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

        [Route("api/Administration/Company/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterCompanyModel request)
        {

            var query = db.CompanyProfile.Where(u => u.User.Display && u.User.UserType == UserType.Company);

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.CompanyName == null || s.CompanyName.Contains(request.CompanyName))
               && (request.CompanyRegNo == null || s.CompanyRegNo.Contains(request.CompanyRegNo))
               && (request.Email == null || s.User.Email.Contains(request.Email))
               && (request.SectorId == null || s.SectorId == request.SectorId)
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.CompanyName.Contains(value)
                || p.CompanyRegNo.Contains(value)
                || p.Sector.Name.Contains(value)
                || p.User.Email.Contains(value)
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
                    case "CompanyName":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.CompanyName);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.CompanyName);
                        }

                        break;

                    case "CompanyRegNo":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.CompanyRegNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.CompanyRegNo);
                        }

                        break;

                    case "Email":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.User.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.User.Email);
                        }

                        break;

                    case "Sector":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Sector.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Sector.Name);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.CompanyName);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.CompanyName);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new CompanyModel
                {
                    Id = s.UserId,
                    CompanyName = s.CompanyName,
                    Email = s.User.Email,
                    CompanyRegNo = s.CompanyRegNo,
                    Sector = s.Sector.Name,
                    Status = s.User.UserAccount.IsEnable
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
            var user = db.CompanyProfile.Where(u => u.UserId == id)
                .Select(s => new DetailsCompanyModel
                {
                    Id = s.UserId,
                    CompanyName = s.CompanyName,
                    CompanyRegNo = s.CompanyRegNo,
                    Sector = s.Sector.Name,
                    SectorId = s.SectorId,
                    Address1 = s.Address1,
                    Address2 = s.Address2,
                    PostCode = s.PostCode,
                    City = s.City,
                    State = s.State,
                    CompanyPhoneNo = s.CompanyPhoneNo,
                    Name = s.User.Name,
                    Email = s.User.Email,
                    MobileNo = s.User.MobileNo,
                    ICNo = s.User.ICNo,
                    Status = s.User.UserAccount.IsEnable
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
        public IHttpActionResult Post([FromBody] CreateCompanyModel model)
        {

            //var password = Authentication.RandomString(10, true);

            var password = "abc123";

            Authentication.GeneratePassword(password);

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
                State = model.State,
                CompanyPhoneNo = model.CompanyPhoneNo
            };

            var user = new User
            {
                UserType = UserType.Company,
                Name = model.Name,
                Email = model.Email,
                ICNo = model.ICNo,
                MobileNo = model.MobileNo,
                Display = true,
                CreatedBy = null,
                CreatedDate = DateTime.Now,
                UserAccount = account,
                CompanyProfile = company
            };

            foreach (var roleid in model.RoleIds)
            {
                var userrole = new UserRole
                {
                    RoleId = roleid,
                    User = user,
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

        public IHttpActionResult Put(int id, [FromBody] EditCompanyModel model)
        {
            var user = db.User.Where(u => u.Id == id).FirstOrDefault();
            var company = db.CompanyProfile.Where(c => c.UserId == id).FirstOrDefault();

            if (user == null || company == null)
            {
                //return Content(HttpStatusCode.BadRequest, "Any object");
                return NotFound();
            }

            user.Name = model.Name;
            user.ICNo = model.ICNo;
            user.Email = model.Email;
            user.MobileNo = model.MobileNo;

            company.CompanyName = model.CompanyName;
            company.CompanyRegNo = model.CompanyRegNo;
            company.SectorId = model.SectorId;
            company.Address1 = model.Address1;
            company.Address2 = model.Address2;
            company.City = model.City;
            company.State = model.State;
            company.PostCode = model.PostCode;
            company.CompanyPhoneNo = model.CompanyPhoneNo;

            db.User.Attach(user);
            db.Entry(user).Property(x => x.Name).IsModified = true;
            db.Entry(user).Property(x => x.ICNo).IsModified = true;
            db.Entry(user).Property(x => x.Email).IsModified = true;
            db.Entry(user).Property(x => x.MobileNo).IsModified = true;

            db.Entry(company).State = EntityState.Modified;

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
