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
            else if (module == DashboardModule.Publication)
            {
                dashboardList = PublicationStatusList();
            }
            else if (module == DashboardModule.Survey)
            {
                dashboardList = SurveyStatusList();
            }

            dashboardList.ModuleName = module;


            return Ok(dashboardList);

        }

        #region Public Event
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
        #endregion

        #region Course
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
        #endregion

        #region Media
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
        #endregion

        #region Exhibition
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
#endregion

        #region KMC
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
        #endregion

        #region Publication
        private Dictionary<int, Dictionary<int, string>> PublicationStatusMapping()
        {
            var mapping = new Dictionary<int, Dictionary<int,string>>();

            mapping.Add((int)PublicationStatus.New, new Dictionary<int, string> { { -1, "Draft" } });
            mapping.Add((int)PublicationStatus.Submitted, new Dictionary<int, string> { { -1, "Pending Verification" } });
            mapping.Add((int)PublicationStatus.Verified, new Dictionary<int, string> { { (int)PublicationApprovalLevels.Approver1, "Pending Approval 1" },
                                                                                       { (int)PublicationApprovalLevels.Approver2, "Pending Approval 2" },
                                                                                       { (int)PublicationApprovalLevels.Approver3, "Pending Approval 3" } });
            mapping.Add((int)PublicationStatus.Published, new Dictionary<int, string> { { -1, "Published" } });
            mapping.Add((int)PublicationStatus.VerifierRejected, new Dictionary<int, string> { { -1, "Require Amendment" } });
            mapping.Add((int)PublicationStatus.Trashed, new Dictionary<int, string> { { -1, "Withdrawn" } });
            
            return mapping;
        }

        private DashboardList PublicationStatusList()
        {
            var dashboardList = new DashboardList();

            var eventStatusMapping = PublicationStatusMapping();

            foreach (var item in eventStatusMapping)
            {

                var count = 0;

                if(item.Key == (int)PublicationStatus.Verified)
                {
                    foreach(var _item in item.Value)
                    {
                        count = 0;
                        count = db.Publication.Count(q => db.PublicationApproval.Where(pa => pa.PublicationID == q.ID && pa.Status == PublicationApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level == (PublicationApprovalLevels)_item.Key);

                        dashboardList.DashboardItemList.Add(new DashboardItemList()
                        {
                            StatusID = item.Key,
                            StatusName = _item.Value,
                            Count = count,
                            ApprovalLevel = _item.Key
                        });
                    }
                }
                else
                {
                    if(item.Key == (int)PublicationStatus.VerifierRejected)
                    {
                        count = db.Publication.Count(q => q.Status == PublicationStatus.VerifierRejected
                                                    || q.Status == PublicationStatus.ApproverRejected
                                                    || q.Status == PublicationStatus.WithdrawalVerifierRejected
                                                    || q.Status == PublicationStatus.WithdrawalApproverRejected);
                    }
                    else if (item.Key == (int)PublicationStatus.Trashed)
                    {
                        count = db.Publication.Count(q => q.Status == PublicationStatus.Trashed || q.Status == PublicationStatus.WithdrawalTrashed);
                    }
                    else
                    {
                        count = db.Publication.Count(q => (int)q.Status == item.Key);
                    }                   

                    dashboardList.DashboardItemList.Add(new DashboardItemList()
                    {
                        StatusID = item.Key,
                        StatusName = item.Value.First().Value,
                        Count = count
                    });
                }
            }

            return dashboardList;
        }
        #endregion
        
        #region Survey
        private Dictionary<int, Dictionary<int, string>> SurveyStatusMapping()
        {
            var mapping = new Dictionary<int, Dictionary<int, string>>();

            mapping.Add((int)SurveyStatus.New, new Dictionary<int, string> { { -1, "Draft" } });
            mapping.Add((int)SurveyStatus.Submitted, new Dictionary<int, string> { { -1, "Pending Verification" } });
            mapping.Add((int)SurveyStatus.Verified, new Dictionary<int, string> { { (int)SurveyApprovalLevels.Approver1, "Pending Approval 1" },
                                                                                       { (int)SurveyApprovalLevels.Approver2, "Pending Approval 2" },
                                                                                       { (int)SurveyApprovalLevels.Approver3, "Pending Approval 3" } });
            mapping.Add((int)SurveyStatus.Published, new Dictionary<int, string> { { -1, "Published" } });
            mapping.Add((int)SurveyStatus.VerifierRejected, new Dictionary<int, string> { { -1, "Require Amendment" } });
            mapping.Add((int)SurveyStatus.Trashed, new Dictionary<int, string> { { -1, "Withdrawn" } });

            return mapping;
        }

        private DashboardList SurveyStatusList()
        {
            var dashboardList = new DashboardList();

            var SurveyStatusMapping = this.SurveyStatusMapping();

            foreach (var item in SurveyStatusMapping)
            {

                var count = 0;

                if (item.Key == (int)SurveyStatus.Verified)
                {
                    foreach (var _item in item.Value)
                    {
                        count = 0;
                        count = db.Survey.Count(q => db.SurveyApproval.Where(pa => pa.SurveyID == q.ID && pa.Status == SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level == (SurveyApprovalLevels)_item.Key);

                        dashboardList.DashboardItemList.Add(new DashboardItemList()
                        {
                            StatusID = item.Key,
                            StatusName = _item.Value,
                            Count = count,
                            ApprovalLevel = _item.Key
                        });
                    }
                }
                else
                {
                    if (item.Key == (int)SurveyStatus.VerifierRejected)
                    {
                        count = db.Survey.Count(q => q.Status == SurveyStatus.VerifierRejected
                                                    || q.Status == SurveyStatus.ApproverRejected);
                    }
                    else
                    {
                        count = db.Survey.Count(q => (int)q.Status == item.Key);
                    }

                    dashboardList.DashboardItemList.Add(new DashboardItemList()
                    {
                        StatusID = item.Key,
                        StatusName = item.Value.First().Value,
                        Count = count
                    });
                }
            }

            return dashboardList;
        }
        #endregion


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
