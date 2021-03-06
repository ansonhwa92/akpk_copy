﻿using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FEP.Model.Migrations
{
    public static class tajulSeed
    {
        public static void Seed(DbEntities db)
        {
            //DefaultSLAReminder(db);
            //DefaultParameterGroup(db);
            DefaultTemplate(db);
        }

        public static void DefaultSLAReminder(DbEntities db)
        {
            //if (!db.SLAReminder.Any())

            //{
            //db.SLAReminder.AddOrUpdate(s => s.NotificationType,
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitPublicEvent, NotificationType = NotificationType.Submit_Public_Event_For_Verification, ETCode = "ET001PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyPublicEvent, NotificationType = NotificationType.Verify_Public_Event_After_Submit_For_Verification, ETCode = "ET002PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_1, ETCode = "ET003PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_2, ETCode = "ET004PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_3, ETCode = "ET005PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.CancelPublicEvent, NotificationType = NotificationType.Cancel_Public_Event, ETCode = "ET006PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectPublicEvent, NotificationType = NotificationType.Reject_Public_Event, ETCode = "ET007PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.PublishPublicEvent, NotificationType = NotificationType.Publish_Public_Event, ETCode = "ET008PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //    //EVENT EXTERNAL - MEDIA INTERVIEW
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitMediaInterview, NotificationType = NotificationType.Submit_Media_Interview_For_Verification, ETCode = "ET001EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyMediaInterview, NotificationType = NotificationType.Verify_Media_Interview_After_Submit_For_Verification, ETCode = "ET002EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_1, ETCode = "ET003EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_2, ETCode = "ET004EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_3, ETCode = "ET005EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectMediaInterview, NotificationType = NotificationType.Reject_Media_Interview, ETCode = "ET006EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //    //EVENT EXTERNAL - EXHIBITION ROADSHOW
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitExhibitionRoadshow, NotificationType = NotificationType.Submit_Exhibition_RoadShow_For_Verification, ETCode = "ET007EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyExhibitionRoadshow, NotificationType = NotificationType.Verify_Exhibition_RoadShow_After_Submit_For_Verification, ETCode = "ET008EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_1, ETCode = "ET009EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_2, ETCode = "ET010EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_3, ETCode = "ET011EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectExhibitionRoadshow, NotificationType = NotificationType.Reject_Exhibition_RoadShow, ETCode = "ET012EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //    new SLAReminder { SLAEventType = SLAEventType.SubmitExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Submit_DutyRoster_For_Verification, ETCode = "ET013EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { SLAEventType = SLAEventType.VerifyExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Verify_DutyRoster, ETCode = "ET014EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.NotVerify_DutyRoster, ETCode = "ET015EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Approve_DutyRoster, ETCode = "ET016EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.AcceptParticipation_Exhibition_RoadShow, ETCode = "ET017EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { SLAEventType = SLAEventType.RejectExhibitionRoadshowDutyRoster, NotificationType = NotificationType.DeclineParticipation_Exhibition_RoadShow, ETCode = "ET018EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },


            //    new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_GL, ETCode = "ET003PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_GL, ETCode = "ET005PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Payment, ETCode = "ET006PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Refund_Request, ETCode = "ET007PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Approve_Refund_Request, ETCode = "ET008PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Payment, ETCode = "ET004PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Refund, ETCode = "ET009PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },


            //    //EVENT EXTERNAL - 
            //    //new SLAReminder { SLAEventType = SLAEventType.VerifyExternalRequest, NotificationType = NotificationType.Verify_External_Request_Exhibition_ESS, ETCode = "ET011EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    //new SLAReminder { SLAEventType = SLAEventType.VerifyExternalRequest, NotificationType = NotificationType.Verify_External_Request_Duty_Roster, ETCode = "ET012EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    //new SLAReminder { SLAEventType = SLAEventType.ApproveExternalRequest, NotificationType = NotificationType.Approve_External_Request_Exhibition_Participation, ETCode = "ET013EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    //new SLAReminder { SLAEventType = SLAEventType.ApproveExternalRequest, NotificationType = NotificationType.Approve_External_Request_Duty_Roster, ETCode = "ET014EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //    new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Creation, ETCode = "ET016EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Published_Change, ETCode = "ET017EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Published_Withdraw, ETCode = "ET018EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Participant_Withdraw, ETCode = "ET019EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //    new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Published_Change, ETCode = "ET021EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Published_Withdraw, ETCode = "ET022EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Participant_Withdraw, ETCode = "ET023EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //        // survey
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.SubmitSurvey, NotificationType = NotificationType.Submit_Survey_Creation, ETCode = "ET101RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.CancelSurvey, NotificationType = NotificationType.Submit_Survey_Cancellation, ETCode = "ET102RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.PublishSurvey, NotificationType = NotificationType.Submit_Survey_Publication, ETCode = "ET103RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifySurvey, NotificationType = NotificationType.Verify_Survey_Creation, ETCode = "ET111RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_1, ETCode = "ET121RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_2, ETCode = "ET122RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_3, ETCode = "ET123RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_Final, ETCode = "ET124RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.DistributeSurvey, NotificationType = NotificationType.Submit_Survey_Distribution, ETCode = "ET131RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //        new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.AnswerSurvey, NotificationType = NotificationType.Submit_Survey_Response, ETCode = "ET132RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //    // publication
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.SubmitPublication, NotificationType = NotificationType.Submit_Publication_Creation, ETCode = "ET201RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelPublication, NotificationType = NotificationType.Submit_Publication_Cancellation, ETCode = "ET202RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.PublishPublication, NotificationType = NotificationType.Submit_Publication_Publication, ETCode = "ET203RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification, ETCode = "ET204RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.WithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal, ETCode = "ET205RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification_Cancellation, ETCode = "ET206RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelWithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal_Cancellation, ETCode = "ET207RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublication, NotificationType = NotificationType.Verify_Publication_Creation, ETCode = "ET211RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublicationModification, NotificationType = NotificationType.Verify_Publication_Modification, ETCode = "ET212RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublicationWithdrawal, NotificationType = NotificationType.Verify_Publication_Withdrawal, ETCode = "ET213RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_1, ETCode = "ET221RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_2, ETCode = "ET222RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_3, ETCode = "ET223RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_Final, ETCode = "ET224RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_1, ETCode = "ET225RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_2, ETCode = "ET226RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_3, ETCode = "ET227RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_Final, ETCode = "ET228RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_1, ETCode = "ET229RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_2, ETCode = "ET230RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_3, ETCode = "ET231RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_Final, ETCode = "ET232RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

            //    //system
            //    new SLAReminder { NotificationCategory = NotificationCategory.System, SLAEventType = SLAEventType.ActivateAccount, NotificationType = NotificationType.ActivateAccount, ETCode = "ET001SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.System, SLAEventType = SLAEventType.ResetPassword, NotificationType = NotificationType.ResetPassword, ETCode = "ET002SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days },
            //    new SLAReminder { NotificationCategory = NotificationCategory.System, SLAEventType = SLAEventType.SystemError, NotificationType = NotificationType.SystemError, ETCode = "ET003SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days }
            //    );
            //}
        }

        public static void DefaultParameterGroup(DbEntities db)
        {
            //if (!db.ParameterGroup.Any())
            //{
            //foreach (TemplateParameterType paramType in Enum.GetValues(typeof(TemplateParameterType)))
            //{
            //    SLAEventType EventType;

            //    int pType = (int)paramType;

            //    if (pType >= 1 && pType <= 20)
            //    {

            //        if (paramType == TemplateParameterType.UserFullName)
            //        {
            //            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            //            new ParameterGroup { SLAEventType = SLAEventType.System, TemplateParameterType = paramType });

            //            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            //            new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = paramType });

            //            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            //            new ParameterGroup { SLAEventType = SLAEventType.ResetPassword, TemplateParameterType = paramType });
            //        }

            //        if (paramType == TemplateParameterType.Link)
            //        {
            //            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            //            new ParameterGroup { SLAEventType = SLAEventType.System, TemplateParameterType = paramType });

            //            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            //            new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = paramType });

            //            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            //            new ParameterGroup { SLAEventType = SLAEventType.ResetPassword, TemplateParameterType = paramType });
            //        }

            //        if (paramType == TemplateParameterType.LoginDetail)
            //        {
            //            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            //            new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = paramType });
            //        }

            //        if (paramType == TemplateParameterType.ErrorDetail)
            //        {
            //            EventType = SLAEventType.Payment;
            //        }


            //    }
            //    if (pType >= 21 && pType <= 40) //Verify & Approval
            //    {
            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.SubmitPublicEvent, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicEvent, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicEvent, TemplateParameterType = paramType });

            //        continue;

            //    }
            //    if (pType >= 101 && pType <= 120)
            //    {
            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.SubmitSurvey, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.CancelSurvey, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.PublishSurvey, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.VerifySurvey, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.ApproveSurvey, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.DistributeSurvey, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.AnswerSurvey, TemplateParameterType = paramType });

            //        continue;

            //    }
            //    if (pType >= 121 && pType <= 140)
            //    {
            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.SubmitPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.CancelPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.PublishPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.ModifyPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.WithdrawPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.CancelModifyPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.CancelWithdrawPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.VerifyPublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationModification, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationWithdrawal, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.ApprovePublication, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationModification, TemplateParameterType = paramType });

            //        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
            //        new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, TemplateParameterType = paramType });

            //        continue;

            //    }


            //}
            //}

        }

        public static void DefaultTemplate(DbEntities db)
        {

            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            foreach (NotificationType notifyType in Enum.GetValues(typeof(NotificationType)))
            {
                var tempSLA = db.SLAReminder.Where(s => s.NotificationType == notifyType).FirstOrDefault();
                
                NotificationCategory tempCategory = (int)0;
                
                if (tempSLA != null) { tempCategory = tempSLA.NotificationCategory; }

                var template = db.NotificationTemplates.Local.Where(r => r.NotificationType == notifyType).FirstOrDefault() ?? db.NotificationTemplates.Where(r => r.NotificationType == notifyType).FirstOrDefault();

                if (template == null)
                {

                    db.NotificationTemplates.Add(
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            NotificationCategory = tempCategory,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Subject: " + notifyType.DisplayName(),
                            TemplateMessage = "Email Body Template",
                            enableSMSMessage = true,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = true,
                            WebMessage = "Web Message Template",
                            WebNotifyLink = "",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            User = user,
                            Display = true
                        });

                }



            }
        }

    }
}

