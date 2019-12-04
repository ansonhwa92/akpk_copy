using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using FEP.WebApiModel.Home;
using System.Web.Http;

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
        public IHttpActionResult GetDashbordList(int userid)
        {
            //var userRole = db.UserRole.FirstOrDefault(x => x.UserId == 1);

            //if(userRole.RoleId == )

            return Ok(EventDashboardStatusList());

        }

        private DashboardList EventDashboardStatusList()
        {
            var dashboardList = new DashboardList();
            var eventStatusMapping = EventStatusMapping();

            foreach (var item in eventStatusMapping)
            {
                var count = db.PublicEvent.Count(x => (int)x.EventStatus == item.Key);
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
            var Mapping = new Dictionary<int, string>();

            Mapping.Add((int)EventStatus.New, "Draft");
            Mapping.Add((int)EventStatus.PendingforVerification, "Pending Verification");
            Mapping.Add((int)EventStatus.Verified, "Pending Approval I");
            Mapping.Add((int)EventStatus.VerifiedbyFirstApprover, "Pending Approval II");
            Mapping.Add((int)EventStatus.VerifiedbySecondApprover, "Pending Approval III");
            Mapping.Add((int)EventStatus.Published, "Published");
            Mapping.Add((int)EventStatus.RequireAmendment, "Pending Amendment");
            // REQUEST TO MODIFY/ WITHDRAW
            //Mapping.Add((int)EventStatus.New, "Request To Modify/ Withdraw");
            Mapping.Add((int)EventStatus.Cancelled, "Withdrawn");

            return Mapping;
        }
    }
}
