//using FEP.Helper;
//using FEP.Model;
//using FEP.WebApiModel.RnP;
//using FEP.WebApiModel.Integration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Data.Entity;


//namespace FEP.WebApi.Api.Integration
//{
//    [Route("api/Integration/Group")]
//    public class GroupController : ApiController
//    {
//        private DbEntities db = new DbEntities();

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        // GET: Groups List (all)
//        // GET: api/Integration/Group/GetAll
//        [Route("api/Integration/Group/GetAll")]
//        [HttpGet]
//        public List<TargetedGroup> GetAll()
//        {
//            // includes inactives
//            var groups = db.TargetedGroups.OrderBy(g => g.Name).Select(s => new TargetedGroup
//            {
//                ID = s.ID,
//                Name = s.Name,
//                Description = s.Description,
//                MinAge = s.MinAge,
//                MaxAge = s.MaxAge,
//                Gender = s.Gender,
//                MinSalary = s.MinSalary,
//                MaxSalary = s.MaxSalary,
//                Status = s.Status,
//                PaymentStatus = s.PaymentStatus,
//                Delinquent = s.Delinquent,
//                EmploymentType = s.EmploymentType,
//                State = s.State,
//                City = s.City
//            }).ToList();

//            return groups;
//        }

//        // GET: Groups List (active)
//        // GET: api/Integration/Group/GetActive
//        [Route("api/Integration/Group/GetActive")]
//        [HttpGet]
//        public List<TargetedGroup> GetActive()
//        {
//            var groups = db.TargetedGroups.Where(g => g.Active == true).OrderBy(g => g.Name).Select(s => new TargetedGroup
//            {
//                ID = s.ID,
//                Name = s.Name,
//                Description = s.Description,
//                MinAge = s.MinAge,
//                MaxAge = s.MaxAge,
//                Gender = s.Gender,
//                MinSalary = s.MinSalary,
//                MaxSalary = s.MaxSalary,
//                Status = s.Status,
//                PaymentStatus = s.PaymentStatus,
//                Delinquent = s.Delinquent,
//                EmploymentType = s.EmploymentType,
//                State = s.State,
//                City = s.City
//            }).ToList();

//            return groups;
//        }

//        // GET: Groups Dropdown List (active)
//        // GET: api/Integration/Group/GetActiveDropdown
//        [Route("api/Integration/Group/GetActiveDropdown")]
//        [HttpGet]
//        public List<TargetedGroupDropdown> GetActiveDropdown()
//        {
//            var groups = db.TargetedGroups.Where(g => g.Active == true).OrderBy(g => g.Name).Select(s => new TargetedGroupDropdown
//            {
//                ID = s.ID,
//                Name = s.Name,
//            }).ToList();

//            return groups;
//        }

//        // GET: Members List
//        // GET: api/Integration/Group/GetMembers
//        [Route("api/Integration/Group/GetMembers")]
//        [HttpGet]
//        public List<TargetedGroupMember> GetMembers(int groupid)
//        {
//            var members = db.TargetedGroupMembers.Where(gm => gm.TargetedGroupID == groupid).OrderBy(gm => gm.Name).Select(s => new TargetedGroupMember
//            {
//                ID = s.ID,
//                TargetedGroupID = s.TargetedGroupID,
//                Name = s.Name,
//                Email = s.Email,
//                ContactNo = s.ContactNo,
//                Source = s.Source
//            }).ToList();

//            return members;
//        }

//        // GET: Member emails List
//        // GET: api/Integration/Group/GetMemberEmails
//        [Route("api/Integration/Group/GetMemberEmails")]
//        [HttpGet]
//        public List<string> GetMemberEmails(int groupid)
//        {
//            List<string> emails = new List<string> { };

//            var members = db.TargetedGroupMembers.Where(gm => gm.TargetedGroupID == groupid).OrderBy(gm => gm.Email).ToList();
//            foreach (TargetedGroupMembers mymember in members)
//            {
//                emails.Add(mymember.Email);
//            }

//            // return unique emails only
//            if (emails.Count > 0)
//            {
//                List<string> uniqueemails = emails.Distinct().ToList();
//                emails = uniqueemails;
//            }

//            return emails;
//        }

//    }

//}