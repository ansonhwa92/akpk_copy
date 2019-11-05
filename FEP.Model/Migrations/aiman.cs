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


            DefaultSLAReminder(db);
            DefaultParameterGroup(db);
            DefaultTemplate(db);


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

                //EVENT EXTERNAL - MEDIA INTERVIEW
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitMediaInterview, NotificationType = NotificationType.Submit_Media_Interview_For_Verification, ETCode = "ET001EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyMediaInterview, NotificationType = NotificationType.Verify_Media_Interview_After_Submit_For_Verification, ETCode = "ET002EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_1, ETCode = "ET003EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_2, ETCode = "ET004EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveMediaInterview, NotificationType = NotificationType.Approve_Media_Interview_ByApprover_3, ETCode = "ET005EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectMediaInterview, NotificationType = NotificationType.Reject_Media_Interview, ETCode = "ET006EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                //EVENT EXTERNAL - EXHIBITION ROADSHOW
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.SubmitExhibitionRoadshow, NotificationType = NotificationType.Submit_Exhibition_RoadShow_For_Verification, ETCode = "ET007EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.VerifyExhibitionRoadshow, NotificationType = NotificationType.Verify_Exhibition_RoadShow_After_Submit_For_Verification, ETCode = "ET008EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_1, ETCode = "ET009EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_2, ETCode = "ET010EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.ApproveExhibitionRoadshow, NotificationType = NotificationType.Approve_Exhibition_RoadShow_ByApprover_3, ETCode = "ET011EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Event, SLAEventType = SLAEventType.RejectExhibitionRoadshow, NotificationType = NotificationType.Reject_Exhibition_RoadShow, ETCode = "ET012EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                new SLAReminder { SLAEventType = SLAEventType.SubmitExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Submit_DutyRoster_For_Verification, ETCode = "ET013EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { SLAEventType = SLAEventType.VerifyExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Verify_DutyRoster, ETCode = "ET014EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.NotVerify_DutyRoster, ETCode = "ET015EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.Approve_DutyRoster, ETCode = "ET016EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { SLAEventType = SLAEventType.ApproveExhibitionRoadshowDutyRoster, NotificationType = NotificationType.AcceptParticipation_Exhibition_RoadShow, ETCode = "ET017EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { SLAEventType = SLAEventType.RejectExhibitionRoadshowDutyRoster, NotificationType = NotificationType.DeclineParticipation_Exhibition_RoadShow, ETCode = "ET018EE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }

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
                    new ParameterGroup { SLAEventType = SLAEventType.SubmitPublicEvent, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicEvent, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicEvent, TemplateParameterType = paramType });

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



    }
}
