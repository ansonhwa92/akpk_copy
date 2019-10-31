using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{

	[Table("Access")]
	public class Access
	{
		[Key]
		public UserAccess UserAccess { get; set; }
		public Modules? Module { get; set; }
		public string Description { get; set; }
		public virtual ICollection<RoleAccess> RoleAccess { get; set; }
	}

	public enum Modules
	{
		[Display(Name = "Home")]
		Home = 0,
		[Display(Name = "Event")]
		Event = 1,
		[Display(Name = "Research & Publication")]
		RnP = 2,
		[Display(Name = "Learning")]
		Learning = 3,
		[Display(Name = "User & Role")]
		Admin = 4,
		[Display(Name = "System Setting")]
		Setting = 5,
		[Display(Name = "Report")]
		Report = 6,

	}


	public enum UserAccess
	{
		//home 0 - 1000
		[Display(Name = "Dashboard 1")]
		HomeDashboard1 = 1,
		[Display(Name = "Dashboard 2")]
		HomeDashboard2 = 2,
		[Display(Name = "Dashboard 3")]
		HomeDashboard3 = 3,

		//e-Event 1001 - 2000
		[Display(Name = "Event Management")]
		EventMenu = 1001,
		[Display(Name = "Internal Event Menu")]
		EventIntenalEventMenu = 1002,
		[Display(Name = "Event Administrator")]
		EventAdministrator = 1003,

		//----------------------Media Interview Button--------------------//
		[Display(Name = "Media Interview - Recipient Email After Submit Media Interview")]
		Recipient_Submit_MediaInterview = 1011,

		[Display(Name = "Media Interview - Recipient Email After Verify Media Interview")]
		Recipient_Verify_MediaInterview = 1012,

		[Display(Name = "Media Interview - Recipient Email After Approval 1")] 
		Recipient_Approver1_MediaInterview = 1013,

		[Display(Name = "Media Interview - Recipient Email After Approval 2")]
		Recipient_Approver2_MediaInterview = 1014,

		[Display(Name = "Media Interview - Recipient Email After Approval 3")]
		Recipient_Approver3_MediaInterview = 1015,

		[Display(Name = "Media Interview - Recipient Email After Reject")]
		Recipient_Reject_MediaInterview = 1016,


		//----------------------Exhibition RoadShow Button--------------------//
		[Display(Name = "Exhibition RoadShow - Submit")]
		Recipient_Submit_ExhibitionRoadShow = 1017,

		[Display(Name = "Exhibition RoadShow - Verify")]
		Recipient_Verify_ExhibitionRoadShow = 1018,

		[Display(Name = "Exhibition RoadShow - Approve (Level 1)")]
		Recipient_Approver1_ExhibitionRoadShow = 1019,

		[Display(Name = "Exhibition RoadShow - Approve (Level 2)")]
		Recipient_Approver2_ExhibitionRoadShow = 1020,

		[Display(Name = "Exhibition RoadShow - Approve (Level 3)")]
		Recipient_Approver3_ExhibitionRoadShow = 1021,

		[Display(Name = "Exhibition RoadShow - Reject")]
		Recipient_Reject_ExhibitionRoadShow = 1022,



		//-----------------------------------------------------------//

		[Display(Name = "Public Event - Submit")]
		Recipient_Submit_PublicEvent = 1029,

		[Display(Name = "Public Event - Verify")]
		Recipient_Verify_PublicEvent = 1030,

		[Display(Name = "Public Event - Approve (Level 1)")]
		Recipient_Approver1_PublicEvent = 1031,

		[Display(Name = "Public Event - Approve (Level 2)")]
		Recipient_Approver2_PublicEvent = 1032,

		[Display(Name = "Public Event - Approve (Level 3)")]
		Recipient_Approver3_PublicEvent = 1033,

		[Display(Name = "Public Event - Published")]
		Recipient_Published_PublicEvent = 1034,

		[Display(Name = "Public Event - Require Amendment")]
		Recipient_Reject_PublicEvent = 1035,

		[Display(Name = "Public Event - Cancel")]
		Recipient_Cancel_PublicEvent = 1036,


		//research 2001 - 3000
		[Display(Name = "R&P Management Menu")]
		RnPMenu = 2001,
		[Display(Name = "Publication Menu")]
		RnPPublicationMenu = 2002,
		[Display(Name = "Survey Menu")]
		RnPSurveyMenu = 2003,

		[Display(Name = "Publication - List/View")]
		RnPPublicationView = 2101,
		[Display(Name = "Publication - Edit")]                  // add/edit/delete/submit/cancel
		RnPPublicationEdit = 2102,
		[Display(Name = "Publication - Withdraw")]              // withdraw/cancel withdrawal
		RnPPublicationWithdraw = 2103,
		[Display(Name = "Publication - Publish")]               // publish/unpublish
		RnPPublicationPublish = 2104,

		[Display(Name = "Publication - Verify")]
		RnPPublicationVerify = 2201,
		[Display(Name = "Publication - Approve (Level 1)")]
		RnPPublicationApprove1 = 2202,
		[Display(Name = "Publication - Approve (Level 2)")]
		RnPPublicationApprove2 = 2203,
		[Display(Name = "Publication - Approve (Level 3)")]
		RnPPublicationApprove3 = 2204,

		[Display(Name = "Survey - List/View")]
		RnPSurveyView = 2301,
		[Display(Name = "Survey - Edit")]                       // add/edit/build/delete/submit/cancel
		RnPSurveyEdit = 2302,
		[Display(Name = "Survey - Withdraw")]                   // withdraw/cancel withdrawal
		RnPSurveyWithdraw = 2303,
		[Display(Name = "Survey - Publish")]                    // publish/unpublish
		RnPSurveyPublish = 2304,

		[Display(Name = "Survey - Verify")]
		RnPSurveyVerify = 2401,
		[Display(Name = "Survey - Approve (Level 1)")]
		RnPSurveyApprove1 = 2402,
		[Display(Name = "Survey - Approve (Level 2)")]
		RnPSurveyApprove2 = 2403,
		[Display(Name = "Survey - Approve (Level 3)")]
		RnPSurveyApprove3 = 2404,



        //elearning 3001 - 4000
        [Display(Name = "Learning Management Menu")]
        LearningMenu = 3001,
        
        //elearnig Admin
        [Display(Name = "Course - Create")]
        CourseCreate,
        [Display(Name = "Course - View")]
        CourseView,
        [Display(Name = "Course - Edit")]
        CourseEdit,
        [Display(Name = "Course - Publish")]
        CoursePublish,
        [Display(Name = "Course - Assign Trainer")]
        CourseAssignTrainer,

        // elearning Verifier
        [Display(Name = "Course - Verify")]
        CourseVerify,

        // elearning Approvers
        [Display(Name = "Course - Approve (Level 1)")]
        CourseApproval1,
        [Display(Name = "Course - Approve (Level 2)")]
        CourseApproval2,
        [Display(Name = "Course - Approve (Level 3)")]
        CourseApproval3,
        [Display(Name = "Course - Withdraw")]

        // elearning Learner
        CourseEnroll,

        // elearning Trainer
        CourseGroupCreate,
        CourseDiscussionCreate,
        CourseDiscussionGroupCreate,        
        CourseAddDocument,
        
 
        

		//administration 4001 - 5000
		[Display(Name = "User & Role Management Menu")]
		AdminMenu = 4001,
		[Display(Name = "Individual Menu")]
		AdminIndividualMenu = 4002,
		[Display(Name = "Agency Menu")]
		AdminCompanyMenu = 4003,
		[Display(Name = "Staff Menu")]
		AdminStaffMenu = 4004,
		[Display(Name = "Individual List")]
		AdminIndividualList = 4005,
		[Display(Name = "Individual Edit")]
		AdminIndividualEdit = 4006,



		//setting 5001 - 6000
		[Display(Name = "System Settings Menu")]
		SettingMenu = 5001,







		//report 6001 - 7000
		[Display(Name = "Reports Menu")]
		ReportMenu = 6001,







	}
}
