using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.Migrations
{
	public static class aiman
	{

		public static void Seed(DbEntities db)
		{
			if (!db.EventCategory.Any())
			{
				db.EventCategory.AddOrUpdate(c => c.CategoryName,
					new EventCategory { CategoryName = "Workshops", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Seminars", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Dialogues", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Conferences", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Symposium", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Convention", CreatedDate = DateTime.Now, Display = true }
					);
			}

			AddAdministrator(db, "Admin FEP", "012345678913", "fep.akpk@gmail.com", "0123456789");
			AddAdministrator(db, "Verifier R&P", "012345678913", "verifierrnp@yahoo.com", "0123456789");
			AddAdministrator(db, "Verifier Event", "012345678914", "verifierevent@yahoo.com", "0123456789");
			AddAdministrator(db, "Approver 1", "012345678915", "approverfirst@yahoo.com", "0123456789");
			AddAdministrator(db, "Approver 2", "012345678916", "approversecond@yahoo.com", "0123456789");
			AddAdministrator(db, "Approver 3", "012345678917", "approverthird@yahoo.com", "0123456789");

			AddExternalExhibitor(db, "External Exhibitor 1", "0123456789", "ExternalExhibitor1@yahoo.com", "PrimusCore Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 2", "0123456789", "ExternalExhibitor2@yahoo.com", "GrabFood Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 3", "0123456789", "ExternalExhibitor3@yahoo.com", "DH Robotic Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 4", "0123456789", "ExternalExhibitor4@yahoo.com", "DahMakan Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 5", "0123456789", "ExternalExhibitor5@yahoo.com", "SilverFern Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 6", "0123456789", "ExternalExhibitor6@yahoo.com", "FoodPanda Sdn Bhd");

			//public static void DefaultPublicEvent(DbEntities db, string EventTitle, string Category, string RefNo)
			//DefaultPublicEvent(db, "Financial Management Excellence Certificate", "Workshops", "EVP/0611/0001");
			//DefaultPublicEvent(db, "AKPK International Conference & Expo", "Dialogues", "EVP/0611/0002");
			//DefaultPublicEvent(db, "Sales Enablement Leadership & Learning", "Conferences", "EVP/0611/0003");


			DefaultSLAReminder(db);
			DefaultParameterGroup(db);
			DefaultTemplate(db);
			DefaultRole(db);

		}

		public static void AddAdministrator(DbEntities db, string Name, string ICNo, string Email, string MobileNo)
		{
			var user = db.User.Local.Where(r => r.Email == Email).FirstOrDefault() ?? db.User.Where(r => r.Email == Email).FirstOrDefault();

			if (user == null)
			{

				var useraccount = new UserAccount
				{
					LoginId = Email,
					IsEnable = true,
					LoginAttempt = 0,
					HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
					Salt = "/ZCqmg=="
				};

				var staff = new StaffProfile
				{
					BranchId = null,
					DepartmentId = null,
					DesignationId = null
				};

				db.User.Add(
					new User
					{
						Name = Name,
						ICNo = ICNo,
						Email = Email,
						MobileNo = MobileNo,
						UserType = UserType.Staff,
						Display = true,
						CreatedDate = DateTime.Now,
						UserAccount = useraccount,
						StaffProfile = staff
					});
			}
		}

		public static void AddExternalExhibitor(DbEntities db, string Name, string PhoneNo, string Email, string CompanyName)
		{
			var externalexhibitor = db.EventExternalExhibitor.Local.Where(r => r.Email == Email).FirstOrDefault() ?? db.EventExternalExhibitor.Where(r => r.Email == Email).FirstOrDefault();

			if (externalexhibitor == null)
			{
				db.EventExternalExhibitor.Add(new EventExternalExhibitor
				{
					Name = Name,
					Email = Email,
					PhoneNo = PhoneNo,
					CompanyName = CompanyName,
					Display = true,
					CreatedDate = DateTime.Now,
				});
			}
		}

		public static void DefaultSLAReminder(DbEntities db)
		{
			db.SLAReminder.AddOrUpdate(s => s.NotificationType,

				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitPublicEvent, NotificationType = NotificationType.Submit_Public_Event_For_Verification, ETCode = "ET001PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyPublicEvent, NotificationType = NotificationType.Verify_Public_Event_After_Submit_For_Verification, ETCode = "ET002PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_1, ETCode = "ET003PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_2, ETCode = "ET004PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_ByApprover_3, ETCode = "ET005PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.CancelPublicEvent, NotificationType = NotificationType.Cancel_Public_Event, ETCode = "ET006PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectPublicEvent, NotificationType = NotificationType.Reject_Public_Event, ETCode = "ET007PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.PublishPublicEvent, NotificationType = NotificationType.Publish_Public_Event, ETCode = "ET008PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitCancellationModificationRequest, NotificationType = NotificationType.Submit_CancellationModification_For_Verification, ETCode = "ET009PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyCancellationModificationRequest, NotificationType = NotificationType.Verify_CancellationModification, ETCode = "ET010PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveCancellationModificationRequest, NotificationType = NotificationType.Approve_CancellationModification_ByApprover_1, ETCode = "ET011PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveCancellationModificationRequest, NotificationType = NotificationType.Approve_CancellationModification_ByApprover_2, ETCode = "ET012PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveCancellationModificationRequest, NotificationType = NotificationType.Approve_CancellationModification_ByApprover_3, ETCode = "ET013PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.AmendmentCancellationModificationRequest, NotificationType = NotificationType.RequireAmendment_CancellationModification, ETCode = "ET014PE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },



				//EVENT EXTERNAL - MEDIA INTERVIEW
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitMediaInterview, NotificationType = NotificationType.Submit_Media_Interview_For_Verification, ETCode = "ET001EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyMediaInterview, NotificationType = NotificationType.Verify_Media_Interview_After_Submit_For_Verification, ETCode = "ET002EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_1, ETCode = "ET003EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_2, ETCode = "ET004EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_3, ETCode = "ET005EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectMediaInterview, NotificationType = NotificationType.Reject_Media_Interview, ETCode = "ET006EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },


				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RepAvailable_MediaInterview, NotificationType = NotificationType.RepAvailable_MediaInterview, ETCode = "ET007EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RepNotAvailable_MediaInterview, NotificationType = NotificationType.RepNotAvailable_MediaInterview, ETCode = "ET008EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },




				//EVENT EXTERNAL - EXHIBITION ROADSHOW
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitExhibitionRoadshow, NotificationType = NotificationType.Submit_Exhibition_RoadShow_For_Verification, ETCode = "ET007EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyExhibitionRoadshow, NotificationType = NotificationType.Verify_Exhibition_RoadShow_After_Submit_For_Verification, ETCode = "ET008EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_1, ETCode = "ET009EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_2, ETCode = "ET010EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_3, ETCode = "ET011EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectExhibitionRoadshow, NotificationType = NotificationType.Reject_Exhibition_RoadShow, ETCode = "ET012EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

				//new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Submit_DutyRoster_For_Verification, ETCode = "ET013EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				//new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Verify_DutyRoster, ETCode = "ET014EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				//new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.NotVerify_DutyRoster, ETCode = "ET015EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				//new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Approve_DutyRoster, ETCode = "ET016EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.AcceptParticipation_Exhibition_RoadShow, ETCode = "ET017EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
				new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectExhibitionRoadshowDutyRoster, NotificationType = NotificationType.DeclineParticipation_Exhibition_RoadShow, ETCode = "ET018EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }

			//new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SendInvitationToNominees, NotificationType = NotificationType.SendInvitationToNominees, ETCode = "ET019EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }
			);
		}

		public static void DefaultParameterGroup(DbEntities db)
		{

			foreach (TemplateParameterType paramType in Enum.GetValues(typeof(TemplateParameterType)))
			{
				SLAEventType EventType;

				int pType = (int)paramType;

				if (pType >= 21 && pType <= 40) //Verify & Approval
				{
					db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
						new ParameterGroup { SLAEventType = SLAEventType.SubmitPublicEvent, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicEvent, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicEvent, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.CancelPublicEvent, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.RejectPublicEvent, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.PublishPublicEvent, TemplateParameterType = paramType },

						new ParameterGroup { SLAEventType = SLAEventType.SubmitMediaInterview, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.VerifyMediaInterview, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.ApproveMediaInterview, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.RejectMediaInterview, TemplateParameterType = paramType },

						new ParameterGroup { SLAEventType = SLAEventType.SubmitExhibitionRoadshow, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.VerifyExhibitionRoadshow, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.ApproveExhibitionRoadshow, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.RejectExhibitionRoadshow, TemplateParameterType = paramType },

						new ParameterGroup { SLAEventType = SLAEventType.SubmitExhibitionRoadshowDutyRoster, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.VerifyExhibitionRoadshowDutyRoster, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.RejectExhibitionRoadshowDutyRoster, TemplateParameterType = paramType },

						new ParameterGroup { SLAEventType = SLAEventType.SubmitCancellationModificationRequest, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.VerifyCancellationModificationRequest, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.ApproveCancellationModificationRequest, TemplateParameterType = paramType },
						new ParameterGroup { SLAEventType = SLAEventType.AmendmentCancellationModificationRequest, TemplateParameterType = paramType }
					);

					continue;

				}

			}

		}

		public static void DefaultTemplate(DbEntities db)
		{

			var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

			//db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
			//    new NotificationTemplate
			//    {
			//        NotificationType = NotificationType.ResetPassword,
			//        NotificationCategory = NotificationCategory.System,
			//        TemplateName = NotificationType.ResetPassword.DisplayName(),
			//        TemplateRefNo = "T" + ((int)NotificationType.ResetPassword).ToString(),
			//        enableEmail = true,
			//        TemplateSubject = "New FE Portal Account Created",
			//        TemplateMessage = "&lt;p&gt;Dear&amp;nbsp;&lt;span style=&quot;font-size: 1rem;&quot;&gt;[#UserFullName],&lt;/span&gt;&lt;/p&gt;&lt;p&gt;You can activate your account [#Link].&amp;nbsp;&lt;/p&gt;&lt;p&gt;Your login details:&lt;/p&gt;&lt;p&gt;[#LoginDetail]&lt;br&gt;&lt;/p&gt;&lt;p&gt;&lt;span style=&quot;color: rgb(255, 255, 255); font-size: 12px; text-align: center; white-space: nowrap; background-color: rgb(41, 182, 246);&quot;&gt;&lt;br&gt;&lt;/span&gt;&lt;/p&gt;",
			//        enableSMSMessage = false,
			//        SMSMessage = "SMS Message Template",
			//        enableWebMessage = false,
			//        WebMessage = "Web Message Template",
			//        CreatedDate = DateTime.Now,
			//        CreatedBy = user.Id,
			//        User = user,
			//        Display = true
			//    });

		}

		public static void DefaultRole(DbEntities db)
		{
			List<RoleAccess> AdminEventFED = new List<RoleAccess>();
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.EventAdministratorFED });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Submit_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Verify_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Approver1_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Approver2_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Approver3_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Published_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Cancelled_PublicEvent });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Submit_CancellationModificationRequest });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Verifier_CancellationModificationRequest });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Approver1_CancellationModificationRequest });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Approver2_CancellationModificationRequest });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Approver3_CancellationModificationRequest });
			AdminEventFED.Add(new RoleAccess { UserAccess = UserAccess.Amendment_CancellationModificationRequest });

			mhafeez.AddRole(db, "Admin - Internal Event", "Admin - FED", AdminEventFED);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> AdminEventCCD = new List<RoleAccess>();
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.EventAdministratorCCD });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Submit_MediaInterview });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_MediaInterview });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Verify_MediaInterview });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver1_MediaInterview });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver2_MediaInterview });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver3_MediaInterview });

			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Submit_ExhibitionRoadShow });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_ExhibitionRoadShow });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Verify_ExhibitionRoadShow });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver1_ExhibitionRoadShow });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver2_ExhibitionRoadShow });
			AdminEventCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver3_ExhibitionRoadShow });

			mhafeez.AddRole(db, "Admin - External Event", "Admin - CCD", AdminEventCCD);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> VerifierEventFED = new List<RoleAccess>();
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.VerifierPublicEventFED });
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.Submit_PublicEvent });
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.Submit_CancellationModificationRequest });
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_PublicEvent });
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.Amendment_CancellationModificationRequest });
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.Verify_PublicEvent });
			VerifierEventFED.Add(new RoleAccess { UserAccess = UserAccess.Verifier_CancellationModificationRequest });

			mhafeez.AddRole(db, "Verifier Event - FED", "Verifier Event", VerifierEventFED);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> VerifierMediaCCD = new List<RoleAccess>();
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.VerifierMediaInterviewCCD });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.Submit_ExhibitionRoadShow });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.Submit_MediaInterview });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_ExhibitionRoadShow });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_MediaInterview });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.Verify_ExhibitionRoadShow });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.Verify_MediaInterview });
			VerifierMediaCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver3_MediaInterview });

			mhafeez.AddRole(db, "Verifier Event - CCD", "Verifier Event", VerifierMediaCCD);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> VerifierExhibitionCCD = new List<RoleAccess>();
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.VerifierExhibitionCCD });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.Submit_ExhibitionRoadShow });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.Submit_MediaInterview });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_ExhibitionRoadShow });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.RequireAmendment_MediaInterview });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.Verify_ExhibitionRoadShow });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.Verify_MediaInterview });
			VerifierExhibitionCCD.Add(new RoleAccess { UserAccess = UserAccess.Approver3_ExhibitionRoadShow });

			mhafeez.AddRole(db, "Verifier Event - CCD", "Verifier Event", VerifierExhibitionCCD);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> PublicEventApprover1 = new List<RoleAccess>();
			PublicEventApprover1.Add(new RoleAccess { UserAccess = UserAccess.Approver1PublicEvent });
			PublicEventApprover1.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			PublicEventApprover1.Add(new RoleAccess { UserAccess = UserAccess.Verify_PublicEvent });
			PublicEventApprover1.Add(new RoleAccess { UserAccess = UserAccess.Verifier_CancellationModificationRequest });
			PublicEventApprover1.Add(new RoleAccess { UserAccess = UserAccess.Approver1_PublicEvent });
			PublicEventApprover1.Add(new RoleAccess { UserAccess = UserAccess.Approver1_CancellationModificationRequest });

			mhafeez.AddRole(db, "Public Event - Approver 1", "Approver 1", PublicEventApprover1);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> MediaInterviewApprover1 = new List<RoleAccess>();
			MediaInterviewApprover1.Add(new RoleAccess { UserAccess = UserAccess.Approver1MediaInterview });
			MediaInterviewApprover1.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			MediaInterviewApprover1.Add(new RoleAccess { UserAccess = UserAccess.Verify_MediaInterview });
			MediaInterviewApprover1.Add(new RoleAccess { UserAccess = UserAccess.Approver1_MediaInterview });

			mhafeez.AddRole(db, "Media Interview - Approver 1", "Approver 1", MediaInterviewApprover1);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> ExhibitionApprover1 = new List<RoleAccess>();
			ExhibitionApprover1.Add(new RoleAccess { UserAccess = UserAccess.Approver1Exhibition });
			ExhibitionApprover1.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			ExhibitionApprover1.Add(new RoleAccess { UserAccess = UserAccess.Verify_ExhibitionRoadShow });
			ExhibitionApprover1.Add(new RoleAccess { UserAccess = UserAccess.Approver1_ExhibitionRoadShow });

			mhafeez.AddRole(db, "Exhibition - Approver 1", "Approver 1", ExhibitionApprover1);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> PublicEventApprover2 = new List<RoleAccess>();
			PublicEventApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver2PublicEvent });
			PublicEventApprover2.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			PublicEventApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver1_CancellationModificationRequest });
			PublicEventApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver1_PublicEvent });
			PublicEventApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver2_CancellationModificationRequest });
			PublicEventApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver2_PublicEvent });

			mhafeez.AddRole(db, "Public Event - Approver 2", "Approver 2", PublicEventApprover2);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> MediaInterviewApprover2 = new List<RoleAccess>();
			MediaInterviewApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver2MediaInterview });
			MediaInterviewApprover2.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			MediaInterviewApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver1_MediaInterview });
			MediaInterviewApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver2_MediaInterview });

			mhafeez.AddRole(db, "Media Interview - Approver 2", "Approver 2", MediaInterviewApprover2);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> ExhibitionApprover2 = new List<RoleAccess>();
			ExhibitionApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver2Exhibition });
			ExhibitionApprover2.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			ExhibitionApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver1_ExhibitionRoadShow });
			ExhibitionApprover2.Add(new RoleAccess { UserAccess = UserAccess.Approver2_ExhibitionRoadShow });

			mhafeez.AddRole(db, "Exhibition - Approver 2", "Approver 2", ExhibitionApprover2);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> PublicEventApprover3 = new List<RoleAccess>();
			PublicEventApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver3PublicEvent });
			PublicEventApprover3.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			PublicEventApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver2_CancellationModificationRequest });
			PublicEventApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver2_PublicEvent });
			PublicEventApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver3_CancellationModificationRequest });
			PublicEventApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver3_PublicEvent });

			mhafeez.AddRole(db, "Public Event - Approver 3", "Approver 3", PublicEventApprover3);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> MediaInterviewApprover3 = new List<RoleAccess>();
			MediaInterviewApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver3MediaInterview });
			MediaInterviewApprover3.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			MediaInterviewApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver2_MediaInterview });
			MediaInterviewApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver3_MediaInterview });

			mhafeez.AddRole(db, "Media Interview - Approver 3", "Approver 3", MediaInterviewApprover3);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> ExhibitionApprover3 = new List<RoleAccess>();
			ExhibitionApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver3Exhibition });
			ExhibitionApprover3.Add(new RoleAccess { UserAccess = UserAccess.EventMenu });
			ExhibitionApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver2_ExhibitionRoadShow });
			ExhibitionApprover3.Add(new RoleAccess { UserAccess = UserAccess.Approver3_ExhibitionRoadShow });

			mhafeez.AddRole(db, "Exhibition - Approver 3", "Approver 3", ExhibitionApprover3);

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> EventReception = new List<RoleAccess>();
			EventReception.Add(new RoleAccess { UserAccess = UserAccess.EventReception });
			mhafeez.AddRole(db, "Event Reception", "Event Reception");

			//-------------------------------------------------------------------------------------------------//

			List<RoleAccess> EventModerator = new List<RoleAccess>();
			EventModerator.Add(new RoleAccess { UserAccess = UserAccess.EventReception });
			mhafeez.AddRole(db, "Event Moderator", "Event Moderator");
			//-------------------------------------------------------------------------------------------------//

		}


		//public static void DefaultPublicEvent(DbEntities db, string EventTitle, string Category, string RefNo)
		//{
		//	var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

		//	var getpublicevent = db.PublicEvent.Where(p => p.RefNo == RefNo).FirstOrDefault() ?? db.PublicEvent.Where(p => p.RefNo == RefNo).FirstOrDefault();

		//	if (getpublicevent == null)
		//	{
		//		var category = new EventCategory
		//		{
		//			CategoryName = Category,
		//			Display = true,
		//			CreatedDate = DateTime.Now,
		//			CreatedBy = user.Id
		//		};

		//		db.PublicEvent.Add(new PublicEvent
		//		{
		//			EventTitle = EventTitle,
		//			EventCategoryId = category.Id,
		//			EventCategory = category,
		//			StartDate = DateTime.Now,
		//			EndDate = DateTime.Now,
		//			RefNo = RefNo,
		//			EventStatus = EventStatus.Published,
		//			Fee = 100,
		//			EventObjective = "Event Objective",
		//			TargetedGroup = EventTargetGroup.Goverment,
		//			Venue = "AKPK Kuala Lumpur",
		//			ParticipantAllowed = 200,
		//			Display = true,
		//			CreatedDate = DateTime.Now,
		//			CreatedBy = user.Id,
		//			CreatedByUser = user
		//		});
		//	}
		//}






	}
}