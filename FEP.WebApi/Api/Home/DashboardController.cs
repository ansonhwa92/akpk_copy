using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using FEP.WebApiModel.Home;
using System.Web.Http;
using FEP.Model.eLearning;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

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
            else if (module == DashboardModule.KMC)
            {
                dashboardList = KMCList();
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

            mapping.Add((int)MediaStatus.New, EnumHelper.GetDisplayName(MediaStatus.New));
            mapping.Add((int)MediaStatus.PendingVerified, EnumHelper.GetDisplayName(MediaStatus.PendingVerified));
            mapping.Add((int)MediaStatus.Verified, EnumHelper.GetDisplayName(MediaStatus.Verified));
            mapping.Add((int)MediaStatus.ApprovedByApprover1, EnumHelper.GetDisplayName(MediaStatus.ApprovedByApprover1));
            mapping.Add((int)MediaStatus.ApprovedByApprover2, EnumHelper.GetDisplayName(MediaStatus.ApprovedByApprover2));
            mapping.Add((int)MediaStatus.ApprovedByApprover3, EnumHelper.GetDisplayName(MediaStatus.ApprovedByApprover3));
            mapping.Add((int)MediaStatus.RequireAmendment, EnumHelper.GetDisplayName(MediaStatus.RequireAmendment));

            return mapping;
        }

        private DashboardList MediaEventDashboardStatusList()
        {
            var dashboardList = new DashboardList();

            var eventStatusMapping = MediaEventStatusMapping();

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
        
        private Dictionary<int, string> ExhibitionEventStatusMapping()
        {
            var mapping = new Dictionary<int, string>();

            mapping.Add((int)ExhibitionStatus.New, EnumHelper.GetDisplayName(ExhibitionStatus.New));
            mapping.Add((int)ExhibitionStatus.PendingVerified, EnumHelper.GetDisplayName(ExhibitionStatus.PendingVerified));
            mapping.Add((int)ExhibitionStatus.Verified, EnumHelper.GetDisplayName(ExhibitionStatus.Verified));
            mapping.Add((int)ExhibitionStatus.ApprovedByApprover1, EnumHelper.GetDisplayName(ExhibitionStatus.ApprovedByApprover1));
            mapping.Add((int)ExhibitionStatus.ApprovedByApprover2, EnumHelper.GetDisplayName(ExhibitionStatus.ApprovedByApprover2));
            mapping.Add((int)ExhibitionStatus.RequireAmendment, EnumHelper.GetDisplayName(ExhibitionStatus.RequireAmendment));
            mapping.Add((int)ExhibitionStatus.ApprovedByApprover3, EnumHelper.GetDisplayName(ExhibitionStatus.ApprovedByApprover3));

            return mapping;
        }

        private DashboardList ExhibitionEventDashboardStatusList()
        {
            var dashboardList = new DashboardList();
            //dashboardList.ModuleName = typeof(DashboardModule).GetEnumName(DashboardModule.eEvent);

            var eventStatusMapping = ExhibitionEventStatusMapping();

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
        private DashboardList KMCList()
        {
            var dashboardList = new DashboardList();

            var items = db.KMCs.GroupBy(x => x.KMCCategoryId,
                                        (key, group) => new { CategoryID = key, Items = group.ToList() })
                        .ToList();

            foreach(var item in items)
            {
                var itemList = new DashboardItemList();

                itemList.Count = item.Items.Count;
                itemList.StatusName = db.KMCCategory.FirstOrDefault(x => x.Id == item.CategoryID).Title;
                itemList.RedirectLink = "/KMC/Manage/List/" + db.KMCCategory.FirstOrDefault(x => x.Id == item.CategoryID).Id;

                dashboardList.DashboardItemList.Add(itemList);
            }
    
            return dashboardList;
        }
    }
}

public static class EnumHelper
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName();
    }
}
