using FEP.Helper;
using FEP.Model;
using FEP.WebApi.Method;
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
                    MobileNo = s.CountryCode + s.MobileNo,
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
                    CountryCode = s.CountryCode,
                    ICNo = s.ICNo,
                    PassportNo = s.ICNo,
                    IsMalaysian = s.IndividualProfile.IsMalaysian,
                    Citizenship = s.IndividualProfile.CitizenshipId != null ? new CountryModel { Id = s.IndividualProfile.CitizenshipId.Value, Name = s.IndividualProfile.Citizenship.Name } : null,
                    Address1 = s.IndividualProfile.Address1,
                    Address2 = s.IndividualProfile.Address2,
                    PostCodeMalaysian = s.IndividualProfile.PostCode,
                    PostCodeNonMalaysian = s.IndividualProfile.PostCode,
                    City = s.IndividualProfile.City,   
                    State = s.IndividualProfile.StateId != null ? new StateModel { Id = s.IndividualProfile.StateId.Value, Name = s.IndividualProfile.State.Name } : new StateModel { Id = 0, Name = s.IndividualProfile.StateName },
                    Country = new CountryModel { Id = s.IndividualProfile.CountryId, Name = s.IndividualProfile.Country.Name },
                    Status = s.UserAccount != null ? s.UserAccount.IsEnable : false,
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            user.Roles = db.UserRole.Where(u => u.UserId == id).Select(s => new RoleModel { Id = s.RoleId, Name = s.Role.Name, Description = s.Role.Description }).ToList();
            
            return Ok(user);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CreateIndividualModel model)
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

                var countryCode = db.Country.Where(c => c.Id == model.CountryId && c.Display).FirstOrDefault();

                if (countryCode == null)
                {
                    return InternalServerError();
                }

                var password = "abc123";

                if (FEPMethod.CurrentSystemMode() != SystemMode.Development)
                {
                    password = Authentication.RandomString(10, true);
                }

                Authentication.GeneratePassword(password);

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
                    CountryCode = countryCode.CountryCode1,
                    Display = true,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    UserAccount = account,
                    IndividualProfile = individual
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

                return Ok(new { UserId = user.Id, Password = password, UID = activateaccount.UID });
            }

            return BadRequest(ModelState);

        }

        public IHttpActionResult Put(int id, [FromBody] EditIndividualModel model)
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
                var countryCode = db.Country.Where(c => c.Id == model.CountryId && c.Display).FirstOrDefault();

                if (countryCode == null)
                {
                    return InternalServerError();
                }

                user.Name = model.Name;
                user.ICNo = model.ICNo;
                user.Email = model.Email;
                user.MobileNo = model.MobileNo;
                user.CountryCode = countryCode.CountryCode1;

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
                db.Entry(user).Property(x => x.CountryCode).IsModified = true;

                db.UserAccount.Attach(useraccount);
                db.Entry(useraccount).Property(x => x.LoginId).IsModified = true;

                db.Entry(individual).State = EntityState.Modified;

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

            return BadRequest(ModelState);

        }

    }
}
