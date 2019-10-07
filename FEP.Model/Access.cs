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
		EventAdministrator = 1002,

		//----------------------Media Interview Button--------------------//
		[Display(Name = "Media Interview - Submit Media Interview For Verification")]
		Submit_MediaInterview_ForVerification = 1011,

		[Display(Name = "Media Interview - Reject Media Interview")]
		Reject_MediaInterview_ForVerification = 1012,

		[Display(Name = "Media Interview - Verified Media Interview")]
		Verified_MediaInterview_ForVerification = 1013,

		[Display(Name = "Media Interview - Approve Media Interview Approver 1")]
		Approved_MediaInterview_ByApprover1 = 1014,

		[Display(Name = "Media Interview - Approve Media Interview Approver 2")]
		Approved_MediaInterview_ByApprover2 = 1015,

		[Display(Name = "Media Interview - Approve Media Interview Approver 3")]
		Approved_MediaInterview_ByApprover3 = 1016,

		[Display(Name = "Media Interview - Update Representative Available")]
		Submit_MediaInterview_RepAvailable = 1017,

		[Display(Name = "Media Interview - Update Representative Not Available")]
		Submit_MediaInterview_RepNotAvailable = 1018,

		[Display(Name = "Media Interview - Verifier")]
		MediaInterview_Verifier = 1019,

		[Display(Name = "Media Interview - Approver 1")]
		MediaInterview_Approver1 = 1020,

		[Display(Name = "Media Interview - Approver 1")]
		MediaInterview_Approver2 = 1021,

		[Display(Name = "Media Interview - Approver 1")]
		MediaInterview_Approver3 = 1022,
		//-----------------------------------------------------------//

		[Display(Name = "Public Event - Submit Public Event for verification")]
		Submit_PublicEvent_ForVerification = 1023,

		[Display(Name = "Public Event - Verify Public Event")]
		Verified_PublicEvent_ForVerification = 1024,

		[Display(Name = "Public Event - Reject Public Event")]
		Reject_PublicEvent_ForVerification = 1025,

		[Display(Name = "Public Event - Approve Public Event by Approver 1")]
		Approved_PublicEvent_ByApprover1 = 1026,

		[Display(Name = "Public Event - Approve Public Event by Approver 2")]
		Approved_PublicEvent_ByApprover2 = 1027,

		[Display(Name = "Public Event - Approve Public Event by Approver 3")]
		Approved_PublicEvent_ByApprover3 = 1028,

		[Display(Name = "Public Event - Verifier")]
		PublicEvent_Verifier = 1029,

		[Display(Name = "Public Event - Approver 1")]
		PublicEvent_Approver1 = 1030,

		[Display(Name = "Public Event - Approver 2")]
		PublicEvent_Approver2 = 1031,

		[Display(Name = "Public Event - Approver 3")]
		PublicEVent_Approver3 = 1032,


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
