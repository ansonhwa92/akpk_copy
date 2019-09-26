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
    
    [Route("api/Administration/Role")]
    public class RoleController : ApiController
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
            var roles = db.Role.Where(u => u.Display).Select(s => new RoleModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(roles);
        }

        public IHttpActionResult Get(int id)
        {
            var role = db.Role.Where(u => u.Display && u.Id == id).Select(s => new RoleModel
            {
                Id = s.Id,
                Name = s.Name
            }).FirstOrDefault();

            return Ok(role);
        }

        [ValidationActionFilter]
        public IHttpActionResult Post(CreateRoleModel model)
        {
            var role = new Role
            {
                Name = model.Name,
                Description = model.Description,
                CreatedDate = DateTime.Now,
                Display = true
            };

            db.Role.Add(role);

            db.SaveChanges();

            return Ok(role.Id);
        }

        [ValidationActionFilter]
        public IHttpActionResult Put(EditRoleModel model)
        {
            var role = db.Role.Where(r => r.Id == model.Id && r.Display).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }

            role.Name = model.Name;
            role.Description = model.Description;

            db.Entry(role).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(true);            
        }

        public IHttpActionResult Delete(int id)
        {
            var role = db.Role.Where(r => r.Id == id && r.Display).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }

            role.Display = false;
           
            db.Entry(role).State = EntityState.Modified;

            db.SaveChanges();
            return Ok(true);
        }

        [Route("api/Administration/Role/GetAccess")]
        [HttpGet]
        public IHttpActionResult GetAccess(int RoleId, Modules? Module = null)
        {
            var role = db.Role.Where(r => r.Id == RoleId).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }

            var model = new RoleAccessModel();

            model.RoleName = role.Name;

            var accesses = db.RoleAccess.Where(r => r.RoleId == RoleId && (Module == null || r.Access.Module == Module)).Select(s => new UserAccessModel { UserAccess = s.UserAccess, IsCheck = true }).ToList();

            model.UserAccesses = accesses;

            var access = db.Access.Where(a => Module == null || a.Module == Module).Select(s => s.UserAccess).ToList();

            foreach (UserAccess type in access)
            {
                if (!model.UserAccesses.Any(a => a.UserAccess == type))
                {
                    model.UserAccesses.Add(new UserAccessModel { UserAccess = type, IsCheck = false });
                }
            }

            return Ok(model);
        }

        [Route("api/Administration/Role/UpdateAccess")]
        [HttpPost]
        public IHttpActionResult UpdateAccess(UpdateRoleAccessModel model)
        {

            var role = db.Role.Where(r => r.Id == model.RoleId && r.Display).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }

            foreach(var access in model.Access)
            {

                var roleaccess = db.RoleAccess.Where(r => r.RoleId == model.RoleId && r.UserAccess == access.UserAccess).FirstOrDefault();

                if (access.IsCheck)
                {
                    if (roleaccess == null)
                    {                       
                        db.RoleAccess.Add(new RoleAccess { RoleId = model.RoleId, UserAccess = access.UserAccess });
                    }
                }
                else
                {
                    if (roleaccess != null)
                    {                        
                        db.RoleAccess.Remove(roleaccess);
                    }
                }

            }

            db.SaveChanges();

            return Ok(true);

        }

        [Route("api/Administration/Role/GetUser")]
        [HttpPost]
        public IHttpActionResult GetUser([FromUri]int RoleId, [FromBody] FilterUserModel request)
        {
            var role = db.Role.Where(r => r.Id == RoleId).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }

            var query = db.UserRole.Where(ur => ur.RoleId == RoleId);

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.Name == null || s.UserAccount.User.Name.Contains(request.Name))              
               && (request.Email == null || s.UserAccount.User.Email.Contains(request.Email))
               && (request.UserType == null || s.UserAccount.User.UserType == request.UserType)
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.UserAccount.User.Name.Contains(value)                
                || p.UserAccount.User.Email.Contains(value)               
                );
            }

            var filteredCount = query.Count();

            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Name":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.UserAccount.User.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.UserAccount.User.Name);
                        }

                        break;

                    case "UserType":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.UserAccount.User.UserType);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.UserAccount.User.UserType);
                        }

                        break;

                    case "Email":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.UserAccount.User.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.UserAccount.User.Email);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.UserAccount.User.Name);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.UserAccount.User.Name);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new UserModel
                {
                    Id = s.UserId,
                    Name = s.UserAccount.User.Name,
                    Email = s.UserAccount.User.Email,
                    UserType = s.UserAccount.User.UserType
                }).ToList();

            data.ForEach(item => { item.UserTypeDesc = item.UserType.GetDisplayName(); });


            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }


        [Route("api/Administration/Role/AddUser")]
        [HttpPost]
        public IHttpActionResult AddUser(UpdateUserRoleModel model)
        {

            var role = db.Role.Where(r => r.Id == model.RoleId && r.Display).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }
                        
            foreach (var userid in model.UserId)
            {

                var userrole = db.UserRole.Where(r => r.RoleId == model.RoleId && r.UserId == userid).FirstOrDefault();
                                
                if (userrole == null)
                {
                    db.UserRole.Add(new UserRole { RoleId = model.RoleId, UserId = userid });
                }
               
            }

            db.SaveChanges();

            return Ok(true);

        }

        [Route("api/Administration/Role/DeleteUser")]
        [HttpPost]
        public IHttpActionResult DeleteUser(UpdateUserRoleModel model)
        {

            var role = db.Role.Where(r => r.Id == model.RoleId && r.Display).FirstOrDefault();

            if (role == null)
            {
                return NotFound();
            }

            foreach (var userid in model.UserId)
            {

                var userrole = db.UserRole.Where(r => r.RoleId == model.RoleId && r.UserId == userid).FirstOrDefault();

                if (userrole != null)
                {
                    db.UserRole.Remove(userrole);
                }

            }

            db.SaveChanges();

            return Ok(true);

        }

        [Route("api/Administration/Role/IsRoleNameExist")]
        [HttpGet]
        public IHttpActionResult IsRoleNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.Role.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.Role.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }


    }
}
