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
			DefaultTemplate(db);
		}

		public static void DefaultSLAReminder(DbEntities db)
		{
			if (!db.SLAReminder.Any())
			{
				db.SLAReminder.AddOrUpdate(s => s.NotificationType,
                    new SLAReminder { SLAEventType = SLAEventType.VerifyPublicEvent, NotificationType = NotificationType.Verify_Public_Event_Creation, ETCode = "ET001APE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.VerifyPublicEvent, NotificationType = NotificationType.Verify_Public_Event_Published_Changed, ETCode = "ET001BPE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.VerifyPublicEvent, NotificationType = NotificationType.Verify_Public_Event_Published_Cancelled, ETCode = "ET001CPE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

					new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_Creation1, ETCode = "ET002APE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_Creation2, ETCode = "ET002APE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_Creation3, ETCode = "ET002APE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

					new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_Published_Changed, ETCode = "ET002BPE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_Published_Cancelled, ETCode = "ET002CPE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

					new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_GL, ETCode = "ET003PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Payment, ETCode = "ET004PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_GL, ETCode = "ET005PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Payment, ETCode = "ET006PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Refund_Request, ETCode = "ET007PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Approve_Refund_Request, ETCode = "ET008PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Refund, ETCode = "ET009PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

					new SLAReminder { SLAEventType = SLAEventType.VerifyExternalRequest, NotificationType = NotificationType.Verify_External_Request_Media_Interview, ETCode = "ET010EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.VerifyExternalRequest, NotificationType = NotificationType.Verify_External_Request_Exhibition_ESS, ETCode = "ET011EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.VerifyExternalRequest, NotificationType = NotificationType.Verify_External_Request_Duty_Roster, ETCode = "ET012EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

					new SLAReminder { SLAEventType = SLAEventType.ApproveExternalRequest, NotificationType = NotificationType.Approve_External_Request_Media_Interview, ETCode = "ET013EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.ApproveExternalRequest, NotificationType = NotificationType.Approve_External_Request_Exhibition_Participation, ETCode = "ET014EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
					new SLAReminder { SLAEventType = SLAEventType.ApproveExternalRequest, NotificationType = NotificationType.Approve_External_Request_Duty_Roster, ETCode = "ET015EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

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

                    new SLAReminder { SLAEventType = SLAEventType.System, NotificationType = NotificationType.ActivateAccount, ETCode = "ET001SY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.System, NotificationType = NotificationType.ResetPassword, ETCode = "ET002SY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }

                    );
			}
		}

		public static void DefaultParameterGroup(DbEntities db)
		{
			if (!db.ParameterGroup.Any())
			{
				foreach (TemplateParameterType paramType in Enum.GetValues(typeof(TemplateParameterType)))
				{
					SLAEventType EventType;

					int pType = (int)paramType;

					if (pType >= 1 && pType <= 20)
					{
						EventType = SLAEventType.System;
					}
					if (pType >= 21 && pType <= 40) //Verify & Approval
					{
						db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
						new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicEvent, TemplateParameterType = paramType });

						db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
						new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicEvent, TemplateParameterType = paramType });

						continue;

					}
					if (pType >= 41 && pType <= 60)
					{
						EventType = SLAEventType.Payment;
					}
					else
					{
						EventType = SLAEventType.System;
					}

					db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
						new ParameterGroup { SLAEventType = EventType, TemplateParameterType = paramType });
				}
			}

		}

		public static void DefaultTemplate(DbEntities db)
		{
			if (!db.NotificationTemplates.Any())
			{
				foreach (NotificationType notifyType in Enum.GetValues(typeof(NotificationType)))
				{
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
							CreatedBy = db.User.Where(u => u.Name == "System Admin").FirstOrDefault().Id,
							Display = true
						});
				}
			}

		}
	}
}
