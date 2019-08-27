using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/Staff")]
    public class StaffController : ApiController
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

        [Route("api/Administration/Staff/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterStaffModel request)
        {

            var query = db.StaffProfile.Where(u => u.User.Display && u.User.UserType == UserType.Staff);

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.Name == null || s.User.Name.Contains(request.Name))
               && (request.BranchId == null || s.BranchId == request.BranchId)
               && (request.Email == null || s.User.Email.Contains(request.Email))
               && (request.DepartmentId == null || s.DepartmentId == request.DepartmentId)
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.User.Name.Contains(value)
                || p.User.Email.Contains(value)
                || p.Branch.Name.Contains(value)
                || p.Department.Name.Contains(value)
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
                            query = query.OrderBy(o => o.User.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.User.Name);
                        }

                        break;

                    case "Branch":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Branch.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Branch.Name);
                        }

                        break;

                    case "Department":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Department.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Department.Name);
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

                    default:
                        query = query.OrderByDescending(o => o.User.Name);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.User.Name);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new StaffModel
                {
                    Id = s.User.Id,
                    Name = s.User.Name,
                    Email = s.User.Email,
                    Branch = s.Branch.Name,
                    Department = s.Department.Name,
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
            var user = db.User.Where(u => u.Id == id)
                .Select(s => new DetailsStaffModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Branch = s.StaffProfile.Branch.Name,
                    Department = s.StaffProfile.Department.Name,
                    Status = s.UserAccount.IsEnable
                })
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


    }
}
