using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using FEP.WebApiModel.Home;
using System.Web.Http;
using FEP.Model.eLearning;
using System.Reflection;

namespace FEP.WebApi.Api.Home
{
    [Route("api/Home/Dashboard")]
    public class DashboardController : ApiController
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


        [Route("api/Home/Dashboard/GetDashbordList")]
        [HttpGet]
        public IHttpActionResult GetDashbordList(int userid, DashboardModule module)
        {
            //var userRole = db.UserRole.FirstOrDefault(x => x.UserId == 1);

            //if(userRole.RoleId == )

            DashboardList dashboardList = new DashboardList();

            if (module == DashboardModule.PublicEvent)
            {
                dashboardList = PublicEventDashboardStatusList();
            }
            else if(module == DashboardModule.Courses)
            {
                dashboardList = ELearningDashboardStatusList();
            }
            else if(module == DashboardModule.MediaInterview)
            {
                dashboardList = MediaEventDashboardStatusList();
            }
            else if (module == DashboardModule.Exhibition)
            {
                dashboardList = ExhibitionEventDashboardStatusList();
            }
            dashboardList.ModuleName = module;


            return Ok(dashboardList);

        }

        private DashboardList PublicEventDashboardStatusList()
        {
            var dashboardList = new DashboardList();
            //dashboardList.ModuleName = typeof(DashboardModule).GetEnumName(DashboardModule.eEvent);

            var eventStatusMapping = EventStatusMapping();

            foreach (var item in eventStatusMapping)
            {
                var count = db.PublicEvent.Count(x => (int)x.EventStatus == item.Key && x.Display == true);
                dashboardList.DashboardItemList.Add(new DashboardItemList()
                {
                    StatusID = item.Key,
                    StatusName = item.Value,
                    Count = count
                });
            }

            return dashboardList;
        }

        private Dictionary<int, string> EventStatusMapping()
        {
            var mapping = new Dictionary<int, string>();

            mapping.Add((int)EventStatus.New, "Draft");
            mapping.Add((int)EventStatus.PendingforVerification, "Pending Verification");
            mapping.Add((int)EventStatus.Verified, "Pending Approval I");
            mapping.Add((int)EventStatus.VerifiedbyFirstApprover, "Pending Approval II");
            mapping.Add((int)EventStatus.VerifiedbySecondApprover, "Pending Approval III");
            mapping.Add((int)EventStatus.Published, "Published");
            mapping.Add((int)EventStatus.RequireAmendment, "Pending Amendment");
            // REQUEST TO MODIFY/ WITHDRAW
            //Mapping.Add((int)EventStatus.New, "Request To Modify/ Withdraw");
            mapping.Add((int)EventStatus.Cancelled, "Withdrawn");

            return mapping;
        }


        private DashboardList ELearningDashboardStatusList()
        {
            var dashboardList = new DashboardList();

            var eventStatusMapping = ELearningStatusMapping();

            foreach (var item in eventStatusMapping)
            {
                var count = db.Courses.Count(x => (int)x.Status == item.Key && x.IsDeleted == false);
                dashboardList.DashboardItemList.Add(new DashboardItemList()
                {
                    StatusID = item.Key,
                    StatusName = item.Value,
                    Count = count
                });
            }

            return dashboardList;
        }

        private Dictionary<int, string> ELearningStatusMapping()
        {
            var mapping = new Dictionary<int, string>();

            mapping.Add((int)CourseStatus.Draft, "Draft");
            mapping.Add((int)CourseStatus.Submitted, "Pending Verification");
            mapping.Add((int)CourseStatus.FirstApproval, "Pending Approval I");
            mapping.Add((int)CourseStatus.SecondApproval, "Pending Approval II");
            mapping.Add((int)CourseStatus.ThirdApproval, "Pending Approval III");
            mapping.Add((int)CourseStatus.Published, "Published");
            mapping.Add((int)CourseStatus.Trial, "Active Trial Run");
            mapping.Add((int)CourseStatus.Amendment, "Pending Amendments");            

            return mapping;
        }

        private Dictionary<int, string> MediaEventStatusMapping()
        {
            var mapping = new Dictionary<int, string>();

            mapping.Add((int)CourseStatus.Draft, "Draft");
            mapping.Add((int)CourseStatus.Submitted, "Pending Verification");
            mapping.Add((int)CourseStatus.FirstApproval, "Pending Approval I");
            mapping.Add((int)CourseStatus.SecondApproval, "Pending Approval II");
            mapping.Add((int)CourseStatus.ThirdApproval, "Pending Approval III");
            mapping.Add((int)CourseStatus.Published, "Published");
            mapping.Add((int)CourseStatus.Trial, "Active Trial Run");
            mapping.Add((int)CourseStatus.Amendment, "Pending Amendments");

            return mapping;
        }

        private DashboardList MediaEventDashboardStatusList()
        {
            var dashboardList = new DashboardList();
            //dashboardList.ModuleName = typeof(DashboardModule).GetEnumName(DashboardModule.eEvent);

            var eventStatusMapping = EventStatusMapping();

            foreach (var item in eventStatusMapping)
            {
                var count = db.EventMediaInterviewRequest.Count(x => (int)x.MediaStatus == item.Key && x.Display == true);
                dashboardList.DashboardItemList.Add(new DashboardItemList()
                {
                    StatusID = item.Key,
                    StatusName = item.Value,
                    Count = count
                });
            }

            return dashboardList;
        }


        private DashboardList ExhibitionEventDashboardStatusList()
        {
            var dashboardList = new DashboardList();
            //dashboardList.ModuleName = typeof(DashboardModule).GetEnumName(DashboardModule.eEvent);

            var eventStatusMapping = EventStatusMapping();

            foreach (var item in eventStatusMapping)
            {
                var count = db.EventExhibitionRequest.Count(x => (int)x.ExhibitionStatus == item.Key && x.Display == true);
                dashboardList.DashboardItemList.Add(new DashboardItemList()
                {
                    StatusID = item.Key,
                    StatusName = item.Value,
                    Count = count
                });
            }

            return dashboardList;
        }
    }
}
