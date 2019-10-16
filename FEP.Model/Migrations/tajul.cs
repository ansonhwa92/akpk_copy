using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.Migrations
{
	public static class tajulSeed
	{
		public static void Seed(DbEntities db)
		{
			DefaultSLAReminder(db);
			DefaultParameterGroup(db);
			//DefaultTemplate(db);
		}

		public static void DefaultSLAReminder(DbEntities db)
		{
			//if (!db.SLAReminder.Any())

			//{
			db.SLAReminder.AddOrUpdate(s => s.NotificationType,
				new SLAReminder { SLAEventType = SLAEventType.SubmitPublicEvent, NotificationType = NotificationType.Submit_Public_Event_For_Verification, ETCode = "ET001PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyPublicEvent, NotificationType = NotificationType.Verify_Public_Event_After_Submit_For_Verification, ETCode = "ET002PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_1, ETCode = "ET003PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_2, ETCode = "ET004PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_3, ETCode = "ET005PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.CancelPublicEvent, NotificationType = NotificationType.Cancel_Public_Event, ETCode = "ET006PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.RejectPublicEvent, NotificationType = NotificationType.Reject_Public_Event, ETCode = "ET007PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.PublishPublicEvent, NotificationType = NotificationType.Publish_Public_Event, ETCode = "ET008PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				//EVENT EXTERNAL - MEDIA INTERVIEW
				new SLAReminder { SLAEventType = SLAEventType.SubmitMediaInterview, NotificationType = NotificationType.Submit_Media_Interview_For_Verification, ETCode = "ET001EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyMediaInterview, NotificationType = NotificationType.Verify_Media_Interview_After_Submit_For_Verification, ETCode = "ET002EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_1, ETCode = "ET003EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_2, ETCode = "ET004EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_3, ETCode = "ET005EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.RejectMediaInterview, NotificationType = NotificationType.Reject_Media_Interview, ETCode = "ET006EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				//EVENT EXTERNAL - EXHIBITION ROADSHOW
				new SLAReminder { SLAEventType = SLAEventType.SubmitExhibitionRoadshow, NotificationType = NotificationType.Submit_Exhibition_RoadShow_For_Verification, ETCode = "ET007EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyExhibitionRoadshow, NotificationType = NotificationType.Verify_Exhibition_RoadShow_After_Submit_For_Verification, ETCode = "ET008EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_1, ETCode = "ET009EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_2, ETCode = "ET010EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_3, ETCode = "ET011EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.RejectExhibitionRoadshow, NotificationType = NotificationType.Reject_Exhibition_RoadShow, ETCode = "ET012EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },


				new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_GL, ETCode = "ET003PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Payment, ETCode = "ET004PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_GL, ETCode = "ET005PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Payment, ETCode = "ET006PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Refund_Request, ETCode = "ET007PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Approve_Refund_Request, ETCode = "ET008PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Refund, ETCode = "ET009PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },


				//EVENT EXTERNAL - 
				//new SLAReminder { SLAEventType = SLAEventType.VerifyExternalRequest, NotificationType = NotificationType.Verify_External_Request_Exhibition_ESS, ETCode = "ET011EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				//new SLAReminder { SLAEventType = SLAEventType.VerifyExternalRequest, NotificationType = NotificationType.Verify_External_Request_Duty_Roster, ETCode = "ET012EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				//new SLAReminder { SLAEventType = SLAEventType.ApproveExternalRequest, NotificationType = NotificationType.Approve_External_Request_Exhibition_Participation, ETCode = "ET013EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				//new SLAReminder { SLAEventType = SLAEventType.ApproveExternalRequest, NotificationType = NotificationType.Approve_External_Request_Duty_Roster, ETCode = "ET014EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				new SLAReminder { SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Creation, ETCode = "ET016EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Published_Change, ETCode = "ET017EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Published_Withdraw, ETCode = "ET018EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Participant_Withdraw, ETCode = "ET019EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				new SLAReminder { SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Creation, ETCode = "ET020EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Published_Change, ETCode = "ET021EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Published_Withdraw, ETCode = "ET022EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Participant_Withdraw, ETCode = "ET023EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                    // survey
                    new SLAReminder { SLAEventType = SLAEventType.SubmitSurvey, NotificationType = NotificationType.Submit_Survey_Creation, ETCode = "ET101RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.CancelSurvey, NotificationType = NotificationType.Submit_Survey_Cancellation, ETCode = "ET102RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.PublishSurvey, NotificationType = NotificationType.Submit_Survey_Publication, ETCode = "ET103RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.VerifySurvey, NotificationType = NotificationType.Verify_Survey_Creation, ETCode = "ET111RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_1, ETCode = "ET121RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_2, ETCode = "ET122RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_3, ETCode = "ET123RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_Final, ETCode = "ET124RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.DistributeSurvey, NotificationType = NotificationType.Submit_Survey_Distribution, ETCode = "ET131RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.AnswerSurvey, NotificationType = NotificationType.Submit_Survey_Response, ETCode = "ET132RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				// publication
				new SLAReminder { SLAEventType = SLAEventType.SubmitPublication, NotificationType = NotificationType.Submit_Publication_Creation, ETCode = "ET201RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.CancelPublication, NotificationType = NotificationType.Submit_Publication_Cancellation, ETCode = "ET202RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.PublishPublication, NotificationType = NotificationType.Submit_Publication_Publication, ETCode = "ET203RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification, ETCode = "ET204RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.WithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal, ETCode = "ET205RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.CancelModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification_Cancellation, ETCode = "ET206RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.CancelWithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal_Cancellation, ETCode = "ET207RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyPublication, NotificationType = NotificationType.Verify_Publication_Creation, ETCode = "ET211RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyPublicationModification, NotificationType = NotificationType.Verify_Publication_Modification, ETCode = "ET212RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.VerifyPublicationWithdrawal, NotificationType = NotificationType.Verify_Publication_Withdrawal, ETCode = "ET213RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_1, ETCode = "ET221RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_2, ETCode = "ET222RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_3, ETCode = "ET223RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_Final, ETCode = "ET224RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_1, ETCode = "ET225RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_2, ETCode = "ET226RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_3, ETCode = "ET227RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_Final, ETCode = "ET228RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_1, ETCode = "ET229RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_2, ETCode = "ET230RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_3, ETCode = "ET231RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_Final, ETCode = "ET232RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				//system
				new SLAReminder { SLAEventType = SLAEventType.ActivateAccount, NotificationType = NotificationType.ActivateAccount, ETCode = "ET001SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.ResetPassword, NotificationType = NotificationType.ResetPassword, ETCode = "ET002SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days },
				new SLAReminder { SLAEventType = SLAEventType.SystemError, NotificationType = NotificationType.SystemError, ETCode = "ET003SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days }
				);
			//}
		}

		public static void DefaultParameterGroup(DbEntities db)
		{
			//if (!db.ParameterGroup.Any())
			//{
			foreach (TemplateParameterType paramType in Enum.GetValues(typeof(TemplateParameterType)))
			{
				SLAEventType EventType;

				int pType = (int)paramType;

				if (pType >= 1 && pType <= 20)
				{

					if (paramType == TemplateParameterType.UserFullName)
					{
						db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.System, TemplateParameterType = paramType });

						db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = paramType });

						db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.ResetPassword, TemplateParameterType = paramType });
					}

					if (paramType == TemplateParameterType.Link)
					{
						db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.System, TemplateParameterType = paramType });

						db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = paramType });

						db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.ResetPassword, TemplateParameterType = paramType });
					}

					if (paramType == TemplateParameterType.LoginDetail)
					{
						db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = paramType });
					}

					if (paramType == TemplateParameterType.ErrorDetail)
					{
						EventType = SLAEventType.Payment;
					}
                    if (pType >= 101 && pType <= 120)
                    {
                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.SubmitSurvey, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.CancelSurvey, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.PublishSurvey, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.VerifySurvey, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.ApproveSurvey, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.DistributeSurvey, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.AnswerSurvey, TemplateParameterType = paramType });

                        continue;

                    }
                    if (pType >= 121 && pType <= 140)
                    {
                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.SubmitPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.CancelPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.PublishPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.ModifyPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.WithdrawPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.CancelModifyPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.CancelWithdrawPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.VerifyPublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationModification, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationWithdrawal, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.ApprovePublication, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationModification, TemplateParameterType = paramType });

                        db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                        new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, TemplateParameterType = paramType });

                        continue;

                    }

				}


			}
			//}

		}

		public static void DefaultTemplate(DbEntities db)
		{
			//if (!db.NotificationTemplates.Any())
			//{
			var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

			foreach (NotificationType notifyType in Enum.GetValues(typeof(NotificationType)))
			{

				switch (notifyType)
				{

					case NotificationType.ActivateAccount:

						db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
						new NotificationTemplate
						{
							NotificationType = notifyType,
							TemplateName = notifyType.DisplayName(),
							TemplateRefNo = "T" + ((int)notifyType).ToString(),
							enableEmail = true,
							TemplateSubject = "New FE Portal Account Created",
							TemplateMessage = "&lt;p&gt;Dear&amp;nbsp;&lt;span style=&quot;font-size: 1rem;&quot;&gt;[#UserFullName],&lt;/span&gt;&lt;/p&gt;&lt;p&gt;You can activate your account [#Link].&amp;nbsp;&lt;/p&gt;&lt;p&gt;Your login details:&lt;/p&gt;&lt;p&gt;[#LoginDetail]&lt;br&gt;&lt;/p&gt;&lt;p&gt;&lt;span style=&quot;color: rgb(255, 255, 255); font-size: 12px; text-align: center; white-space: nowrap; background-color: rgb(41, 182, 246);&quot;&gt;&lt;br&gt;&lt;/span&gt;&lt;/p&gt;",
							enableSMSMessage = false,
							SMSMessage = "SMS Message Template",
							enableWebMessage = false,
							WebMessage = "Web Message Template",
							CreatedDate = DateTime.Now,
							CreatedBy = user.Id,
							Display = true
						});

						break;

					case NotificationType.ResetPassword:

						db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
						new NotificationTemplate
						{
							NotificationType = notifyType,
							TemplateName = notifyType.DisplayName(),
							TemplateRefNo = "T" + ((int)notifyType).ToString(),
							enableEmail = true,
							TemplateSubject = "FE Portal Password Reset",
							TemplateMessage = "&lt;p style=&quot;font-size: 16px;&quot;&gt;Dear&amp;nbsp;&lt;span style=&quot;font-size: 1rem;&quot;&gt;[#UserFullName],&lt;/span&gt;&lt;/p&gt;&lt;p style=&quot;font-size: 16px;&quot;&gt;You can reset your password [#Link].&amp;nbsp;&lt;/p&gt;&lt;p style=&quot;font-size: 16px;&quot;&gt;&lt;br&gt;&lt;/p&gt;",
							enableSMSMessage = false,
							SMSMessage = "SMS Message Template",
							enableWebMessage = false,
							WebMessage = "Web Message Template",
							CreatedDate = DateTime.Now,
							CreatedBy = user.Id,
							Display = true
						});

						break;

					default:

						db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
							new NotificationTemplate
							{
								NotificationType = notifyType,
								TemplateName = notifyType.DisplayName(),
								TemplateRefNo = "T" + ((int)notifyType).ToString(),
								enableEmail = true,
								TemplateSubject = "Subject: " + notifyType.DisplayName(),
								TemplateMessage = "Email Body Template",
								enableSMSMessage = true,
								SMSMessage = "SMS Message Template",
								enableWebMessage = true,
								WebMessage = "Web Message Template",
								CreatedDate = DateTime.Now,
								CreatedBy = user.Id,
								Display = true
							});

						break;

				}
			}
			//}

		}
	}
}
