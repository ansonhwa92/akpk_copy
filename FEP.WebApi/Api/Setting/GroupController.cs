using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.RnP;
using FEP.WebApiModel.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;


namespace FEP.WebApi.Api.Setting
{
    [Route("api/Setting/Group")]
    public class GroupController : ApiController
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

        // Get cities
        // GET: api/Setting/Group/GetCities
        [Route("api/Setting/Group/GetCities")]
        public List<ReturnTargetedGroupCities> GetCities()
        {
            var pubcities = db.TargetedGroupCities.OrderBy(gc => gc.StateID).OrderBy(gc => gc.Name).Select(s => new ReturnTargetedGroupCities
            {
                ID = s.ID,
                StateID = s.StateID,
                Code = s.Code,
                Name = s.Name
            }).ToList();

            return pubcities;
        }

        // GET: Groups List (all) - NOT USED
        // GET: api/Setting/Group/List
        [Route("api/Setting/Group/List")]
        [HttpGet]
        public List<TargetedGroup> List()
        {
            // includes inactives
            var groups = db.TargetedGroups.OrderBy(g => g.Name).Select(s => new TargetedGroup
            {
                //ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                MinAge = s.MinAge,
                MaxAge = s.MaxAge,
                Gender = s.Gender,
                MinSalary = s.MinSalary,
                MaxSalary = s.MaxSalary,
                DMPStatus = s.DMPStatus,
                Delinquent = s.Delinquent,
                EmploymentType = s.EmploymentType,
                State = s.State,
                CityCode = s.CityCode
            }).ToList();

            return groups;
        }

        // GET: Groups List (active) - NOT USED
        // GET: api/Setting/Group/GetActive
        [Route("api/Setting/Group/GetActive")]
        [HttpGet]
        public List<TargetedGroup> GetActive()
        {
            var groups = db.TargetedGroups.Where(g => g.Active == true).OrderBy(g => g.Name).Select(s => new TargetedGroup
            {
                //ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                MinAge = s.MinAge,
                MaxAge = s.MaxAge,
                Gender = s.Gender,
                MinSalary = s.MinSalary,
                MaxSalary = s.MaxSalary,
                DMPStatus = s.DMPStatus,
                Delinquent = s.Delinquent,
                EmploymentType = s.EmploymentType,
                State = s.State,
                CityCode = s.CityCode
            }).ToList();

            return groups;
        }

        // Main DataTable function for listing and filtering
        // POST: api/Setting/Group/GetAll (DataTable)
        [Route("api/Setting/Group/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterTargetedGroup request)
        {

            var query = db.TargetedGroups.Where(p => p.Name != "");

            var totalCount = query.Count();

            //advance search
            query = query.Where(p => (request.Name == null || p.Name.Contains(request.Name))
               && (request.Description == null || p.Description.Contains(request.Description)));

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Name.Contains(value) || p.Description.Contains(value));
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

                    case "Description":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Description);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Description);
                        }

                        break;

                    case "DMPStatus":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.DMPStatus);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.DMPStatus);
                        }

                        break;

                    case "Active":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Active);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Active);
                        }

                        break;

                    default:
                        query = query.OrderBy(o => o.Name);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(o => o.Name);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new ViewTargetedGroup
                {
                    ID = s.ID,
                    Name = s.Name,
                    Description = s.Description,
                    MinAge = s.MinAge,
                    MaxAge = s.MaxAge,
                    Gender = s.Gender,
                    MinSalary = s.MinSalary,
                    MaxSalary = s.MaxSalary,
                    DMPStatus = s.DMPStatus,
                    Delinquent = s.Delinquent,
                    EmploymentType = s.EmploymentType,
                    State = s.State,
                    CityCode = s.CityCode,
                    Active = s.Active
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        // GET: Groups Dropdown List (active)
        // GET: api/Setting/Group/GetActiveDropdown
        [Route("api/Setting/Group/GetActiveDropdown")]
        [HttpGet]
        public List<TargetedGroupDropdown> GetActiveDropdown()
        {
            var groups = db.TargetedGroups.Where(g => g.Active == true).OrderBy(g => g.Name).Select(s => new TargetedGroupDropdown
            {
                ID = s.ID,
                Name = s.Name,
            }).ToList();

            return groups;
        }

        // GET: Members List
        // GET: api/Setting/Group/GetMembers
        [Route("api/Setting/Group/GetMembers")]
        [HttpGet]
        public List<TargetedGroupMember> GetMembers(int groupid)
        {
            var members = db.TargetedGroupMembers.Where(gm => gm.TargetedGroupID == groupid).OrderBy(gm => gm.Name).Select(s => new TargetedGroupMember
            {
                ID = s.ID,
                TargetedGroupID = s.TargetedGroupID,
                Name = s.Name,
                Email = s.Email,
                ContactNo = s.ContactNo,
                Source = s.Source
            }).ToList();

            return members;
        }

        // GET: Member emails List
        // GET: api/Setting/Group/GetMemberEmails
        [Route("api/Setting/Group/GetMemberEmails")]
        [HttpGet]
        public List<string> GetMemberEmails(int groupid)
        {
            List<string> emails = new List<string> { };

            var members = db.TargetedGroupMembers.Where(gm => gm.TargetedGroupID == groupid).OrderBy(gm => gm.Email).ToList();
            foreach (TargetedGroupMembers mymember in members)
            {
                emails.Add(mymember.Email);
            }

            // return unique emails only
            if (emails.Count > 0)
            {
                List<string> uniqueemails = emails.Distinct().ToList();
                emails = uniqueemails;
            }

            return emails;
        }

        // Function to get a single group
        // GET: api/Setting/Group/GetSingle/5
        [Route("api/Setting/Group/GetSingle")]
        [HttpGet]
        public IHttpActionResult GetSingle(int id)
        {
            var targetgroup = db.TargetedGroups.Where(p => p.ID == id).Select(s => new ViewTargetedGroup
            {
                ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                MinAge = s.MinAge,
                MaxAge = s.MaxAge,
                Gender = s.Gender,
                MinSalary = s.MinSalary,
                MaxSalary = s.MaxSalary,
                DMPStatus = s.DMPStatus,
                Delinquent = s.Delinquent,
                EmploymentType = s.EmploymentType,
                State = s.State,
                CityCode = s.CityCode,
                Active = s.Active
            }).FirstOrDefault();

            if (targetgroup == null)
            {
                return NotFound();
            }

            return Ok(targetgroup);
        }

        // Function to check if group name exists
        // GET: api/Setting/Group/NameExists
        [Route("api/Setting/Group/NameExists")]
        [HttpGet]
        public IHttpActionResult NameExists(int? id, string name)
        {

            if (id == null)
            {
                if (db.TargetedGroups.Any(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
                    return Ok(true);
            }
            else
            {
                if (db.TargetedGroups.Any(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && p.ID != id))
                    return Ok(true);
            }

            return NotFound();
        }

        // Function to create a targeted group.
        // POST: api/Setting/Group/Create
        [Route("api/Setting/Group/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public string Create([FromBody] CreateTargetedGroup model)
        {

            if (ModelState.IsValid)
            {
                var targetgroup = new TargetedGroups
                {
                    Name = model.Name,
                    Description = model.Description,
                    MinAge = model.MinAge,
                    MaxAge = model.MaxAge,
                    Gender = model.Gender,
                    MinSalary = model.MinSalary,
                    MaxSalary = model.MaxSalary,
                    DMPStatus = model.DMPStatus,
                    Delinquent = model.Delinquent,
                    EmploymentType = model.EmploymentType,
                    State = model.State,
                    CityCode = model.CityCode,
                    DateCreated = DateTime.Now,
                    CreatorId = model.CreatorId,
                    Active = model.Active
                };

                db.TargetedGroups.Add(targetgroup);
                db.SaveChanges();

                return model.Name;
            }
            return "";
        }

        // Function to update a targeted group.
        // POST: api/Setting/Group/Update
        [Route("api/Setting/Group/Update")]
        [HttpPost]
        [ValidationActionFilter]
        public string Update([FromBody] EditTargetedGroup model)
        {

            if (ModelState.IsValid)
            {
                var targetgroup = db.TargetedGroups.Where(p => p.ID == model.ID).FirstOrDefault();

                if (targetgroup != null)
                {
                    targetgroup.Name = model.Name;
                    targetgroup.Description = model.Description;
                    targetgroup.MinAge = model.MinAge;
                    targetgroup.MaxAge = model.MaxAge;
                    targetgroup.Gender = model.Gender;
                    targetgroup.MinSalary = model.MinSalary;
                    targetgroup.MaxSalary = model.MaxSalary;
                    targetgroup.DMPStatus = model.DMPStatus;
                    targetgroup.Delinquent = model.Delinquent;
                    targetgroup.EmploymentType = model.EmploymentType;
                    targetgroup.State = model.State;
                    targetgroup.CityCode = model.CityCode;
                    targetgroup.Active = model.Active;

                    db.Entry(targetgroup).State = EntityState.Modified;

                    db.SaveChanges();

                    return model.Name;
                }
            }
            return "";
        }

        // Function to delete a targeted group.
        [Route("api/Setting/Group/Delete")]
        //[HttpPost]
        public string Delete(int id)
        {
            var targetgroup = db.TargetedGroups.Where(p => p.ID == id).FirstOrDefault();

            if (targetgroup != null)
            {
                string pname = targetgroup.Name;

                // delete record
                db.TargetedGroups.Remove(targetgroup);

                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return pname;
            }

            return "";
        }

    }

}