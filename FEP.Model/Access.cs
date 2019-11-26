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

        [Display(Name = "Setting")]
        Setting = 4,

        [Display(Name = "Report")]
        Report = 5,

        [Display(Name = "Payment")]
        Payment = 6,

        [Display(Name = "KMC")]
        KMC = 7,

        [Display(Name = "System")]
        System = 8
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
		[Display(Name = "Event Management Menu")]
		EventMenu = 1001,
		[Display(Name = "Internal Event Menu")]
		EventIntenalEventMenu = 1002,



		//--------Untuk Button Sahaja----//
		[Display(Name = "Event Admin - FED")]
		EventAdministratorFED = 1003,
		[Display(Name = "Event Admin - CCD")]
		EventAdministratorCCD = 1004,
		[Display(Name = "Event Reception")]
		EventReception = 1005,
		[Display(Name = "Event Admin - FED")]
		EventVerifierFED = 1006,
		[Display(Name = "Event Admin - CCD")]
		EventVerifierCCD = 1007,
		[Display(Name = "Event Approver 1")]
		EventApprover1 = 1008,
		[Display(Name = "Event Approver 2")]
		EventApprover2 = 1009,
		[Display(Name = "Event Approver 3")]
		EventApprover3 = 1010,
		//--------/Untuk Button Sahaja----//


		//--------Untuk Notification Receiver-------//
		[Display(Name = "Media Interview - Submit")]
		Submit_MediaInterview = 1011,
		[Display(Name = "Media Interview - Verify")]
		Verify_MediaInterview = 1012,
		[Display(Name = "Media Interview -  Approve (Level 1)")]
		Approver1_MediaInterview = 1013,
		[Display(Name = "Media Interview -  Approve (Level 2)")]
		Approver2_MediaInterview = 1014,
		[Display(Name = "Media Interview -  Approve (Level 3)")]
		Approver3_MediaInterview = 1015,
		[Display(Name = "Media Interview - Require Amendment")]
		RequireAmendment_MediaInterview = 1016,


		[Display(Name = "Exhibition RoadShow - Submit")]
		Submit_ExhibitionRoadShow = 1017,
		[Display(Name = "Exhibition RoadShow - Verify")]
		Verify_ExhibitionRoadShow = 1018,
		[Display(Name = "Exhibition RoadShow - Approve (Level 1)")]
		Approver1_ExhibitionRoadShow = 1019,
		[Display(Name = "Exhibition RoadShow - Approve (Level 2)")]
		Approver2_ExhibitionRoadShow = 1020,
		[Display(Name = "Exhibition RoadShow - Approve (Level 3)")]
		Approver3_ExhibitionRoadShow = 1021,
		[Display(Name = "Exhibition RoadShow - Require Amendment")]
		RequireAmendment_ExhibitionRoadShow = 1022,


		[Display(Name = "Public Event - Submit")]
		Submit_PublicEvent = 1029,
		[Display(Name = "Public Event - Verify")]
		Verify_PublicEvent = 1030,
		[Display(Name = "Public Event - Approve (Level 1)")]
		Approver1_PublicEvent = 1031,
		[Display(Name = "Public Event - Approve (Level 2)")]
		Approver2_PublicEvent = 1032,
		[Display(Name = "Public Event - Approve (Level 3)")]
		Approver3_PublicEvent = 1033,
		[Display(Name = "Public Event - Published")]
		Published_PublicEvent = 1034,
		[Display(Name = "Public Event - Require Amendment")]
		RequireAmendment_PublicEvent = 1035,
		[Display(Name = "Public Event - Cancelled")]
		Cancelled_PublicEvent = 1036,

        [Display(Name = "Cancellation/Modification Public Event - Submit")]
        Submit_CancellationModificationRequest = 1037,

        [Display(Name = "Cancellation/Modification Public Event - Verify")]
        Verifier_CancellationModificationRequest = 1038,

        [Display(Name = "Cancellation/Modification Public Event - Approve (Level 1)")]
        Approver1_CancellationModificationRequest = 1039,

        [Display(Name = "Cancellation/Modification Public Event - Approve (Level 2)")]
        Approver2_CancellationModificationRequest = 1040,

        [Display(Name = "Cancellation/Modification Public Event - Approve (Level 3)")]
        Approver3_CancellationModificationRequest = 1041,

        [Display(Name = "Cancellation/Modification Public Event - Require Amendment")]
        Amendment_CancellationModificationRequest = 1042,

        //research & publication 2001 - 3000
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

        [Display(Name = "Course - Enroll/Withdraw")]

        // elearning Learner
        CourseEnroll,

        // elearning Facilitator
        CourseGroupCreate,

        CourseDiscussionCreate,
        CourseDiscussionGroupCreate,
        CourseAddDocument,

        //setting 5001 - 6000
        [Display(Name = "Settings Menu")]
        SettingMenu = 4001,

        [Display(Name = "Settings - User Management")]
        User = 4002,

        [Display(Name = "Settings - User Management - Individual")]
        UserIndividual = 4003,

        [Display(Name = "Settings - User Management - Individual - Create")]
        UserIndividualAdd = 4004,

        [Display(Name = "Settings - User Management - Individual - Edit")]
        UserIndividualEdit = 4005,

        [Display(Name = "Settings - User Management - Individual - Delete")]
        UserIndividualDelete = 4006,

        [Display(Name = "Settings - User Management - Individual - Enable/Disable Account")]
        UserIndividualAccount = 4007,

        [Display(Name = "Settings - User Management - Individual - Reset Password")]
        UserIndividualPassword = 4008,

        [Display(Name = "Settings - User Management - Company")]
        UserCompany = 4009,

        [Display(Name = "Settings - User Management - Company - Create")]
        UserCompanyAdd = 4010,

        [Display(Name = "Settings - User Management - Company - Edit")]
        UserCompanyEdit = 4011,

        [Display(Name = "Settings - User Management - Company - Delete")]
        UserCompanyDelete = 4012,

        [Display(Name = "Settings - User Management - Company - Enable/Disable Account")]
        UserCompanyAccount = 4013,

        [Display(Name = "Settings - User Management - Company - Reset Password")]
        UserCompanyPassword = 4014,

        [Display(Name = "Settings - User Management - Staff")]
        UserStaff = 4015,

        [Display(Name = "Settings - User Management - Staff - Edit")]
        UserStaffEdit = 4016,

        [Display(Name = "Settings - User Management - Staff - Enable/Disable Account")]
        UserStaffAccount = 4017,

        [Display(Name = "Settings - Role Management")]
        Role = 4018,

        [Display(Name = "Settings - Role Management - Create")]
        RoleAdd = 4019,

        [Display(Name = "Settings - Role Management - Update Access")]
        RoleAccess = 4020,

        [Display(Name = "Settings - Role Management - Update User")]
        RoleUser = 4021,

        [Display(Name = "Settings - Role Management - Edit")]
        RoleEdit = 4022,

        [Display(Name = "Settings - Role Management - Delete")]
        RoleDelete = 4023,

        [Display(Name = "Settings - Notification Template")]
        NotificationSetting = 4024,

        [Display(Name = "Settings - Notification Template - Edit")]
        NotificationSettingEdit = 4025,

        [Display(Name = "Settings - SLA Reminder")]
        SLA = 4026,

        [Display(Name = "Settings - Agency Sector")]
        SectorSetting = 4027,

        [Display(Name = "Settings - Agency Sector - Create")]
        SectorSettingCreate = 4028,

        [Display(Name = "Settings - Agency Sector - Edit")]
        SectorSettingEdit = 4029,

        [Display(Name = "Settings - Agency Sector - Delete")]
        SectorSettingDelete = 4030,

        [Display(Name = "Settings - Government Ministry")]
        MinistrySetting = 4031,

        [Display(Name = "Settings - Government Ministry - Create")]
        MinistrySettingCreate = 4032,

        [Display(Name = "Settings - Government Ministry - Edit")]
        MinistrySettingEdit = 4033,

        [Display(Name = "Settings - Government Ministry - Delete")]
        MinistrySettingDelete = 4034,

        [Display(Name = "Settings - Staff Branch")]
        BranchSetting = 4035,

        [Display(Name = "Settings - Staff Branch - Create")]
        BranchSettingAdd = 4036,

        [Display(Name = "Settings - Staff Branch - Edit")]
        BranchSettingEdit = 4037,

        [Display(Name = "Settings - Staff Branch - Delete")]
        BranchSettingDelete = 4038,

        [Display(Name = "Settings - User Logs")]
        UserLog = 4039,

        [Display(Name = "Settings - User Logs - Delete")]
        UserLogDelete = 4040,

        [Display(Name = "Settings - Error Logs")]
        ErrorLog = 4041,

        [Display(Name = "Settings - Error Logs - Delete")]
        ErrorLogDelete = 4042,
        [Display(Name = "Settings - User Account")]
        UserAccountSetting = 4043,

        //report 6001 - 7000
        [Display(Name = "Reports Menu")]
        ReportMenu = 6001,

        //payment 7001 - 8000
        [Display(Name = "Offline Payment")]
        OfflinePayment = 7001,

        [Display(Name = "Refunds")]
        Refunds = 7002,

        //KMC 8001 - 8100
        [Display(Name = "KMC")]
        KMCMenu = 8001,
    }
}