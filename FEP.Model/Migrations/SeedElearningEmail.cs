using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FEP.Model.Migrations
{
    public static class SeedElearningEmail
    {
        public static void SeedTemplateParameter(DbEntities db)
        {
            var elearnTemplates = new List<NotificationType>
            {
               NotificationType.Verify_Courses_Creation,  // Email to Verifier
               NotificationType.Verify_Courses_Creation_Self, // Send Copy to Self

               NotificationType.Approve_Courses_Creation_Approver_Self,
               NotificationType.Approve_Courses_Creation_Approver1,
               NotificationType.Approve_Courses_Creation_Approver2,
               NotificationType.Approve_Courses_Creation_Approver3,
               NotificationType.Course_Approved,
               NotificationType.Course_Approved_Self,
               NotificationType.Course_Approved_Others,
               NotificationType.Course_Amendment,
               NotificationType.Course_Invitation,
               NotificationType.Course_Assigned_To_Facilitator,
               NotificationType.Course_Student_Enrolled,

               NotificationType.Notify_Admin_Participant_Withdraw,
               NotificationType.Notify_Self_Withdraw_From_Course,

               NotificationType.Course_Publish,
               NotificationType.Course_Cancelled,

               // 30-11-2019 - no template yet
               NotificationType.Approve_Courses_Participant_Withdraw,
               NotificationType.Approve_Courses_Published_Withdraw,
               NotificationType.Verify_Courses_Participant_Withdraw,
               NotificationType.Verify_Courses_Published_Withdraw,
               NotificationType.Verify_Courses_Published_Change,
               NotificationType.Approve_Courses_Published_Change
            };

            foreach (var item in elearnTemplates)
            {
                // seed template
                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "Link",
                });

                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "UserFullName",
                });

                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "CourseTitle",
                });
                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "CourseCode",
                });
                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "EnrollmentCode",
                });
                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "CourseAuthor",
                });
                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "CourseApproval",
                });
                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "LearnerName",
                });
                db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType }, new TemplateParameters
                {
                    NotificationType = item,
                    TemplateParameterType = "ReceiverFullName",
                });
            }
            db.SaveChanges();
        }

        public static void Seed(DbEntities db)
        {
            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            foreach (NotificationType notifyType in Enum.GetValues(typeof(NotificationType)))
            {
                switch (notifyType)
                {
                    case NotificationType.Notify_Admin_Participant_Withdraw: // Should send to verifier and admin

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "A Participant Withdraw From Course : [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>A participant has withdrawn from the course [#CourseTitle]</p><br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Notify_Self_Withdraw_From_Course: // Should send to verifier and admin

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "You have withdrawn From Course : [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>You have withdrawn from the course [#CourseTitle]</p><br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Verify_Courses_Creation: // Should send to verifier and admin

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Verify A New Course : [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>A course [#CourseTitle] requires verification.</p><br />
                                                Please click <a href='[#Link]'>here</a> to verify.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Verify_Courses_Creation_Self: // Should send to creator

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Request for  A New Course Verification : [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>You have sent a course [#CourseTitle] for verification.</p><br />
                                                Please click <a href='[#Link]'>here</a>.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Approve_Courses_Creation_Approver_Self:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Request For Approval needed for Course : [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>You have requested for appoval of the course course [#CourseTitle].</p><br />
                                                Please  click <a href='[#Link]'>here</a><br /><br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Approve_Courses_Creation_Approver1:
                    case NotificationType.Approve_Courses_Creation_Approver2:
                    case NotificationType.Approve_Courses_Creation_Approver3:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Approval needed for Course : [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>A course [#CourseTitle] requires Approval.</p><br />
                                                Please  click <a href='[#Link]'>here</a> to approve.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Approved:
                    case NotificationType.Course_Approved_Self:
                    case NotificationType.Course_Approved_Others:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Course  [#CourseTitle] has been Approved",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>The course [#CourseTitle] has been approved.</p><br />
                                                Please  click <a href='[#Link]'>here</a> to view.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Publish:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Course  [#CourseTitle] has been Published",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>The course [#CourseTitle] has been published.</p><br />
                                                Please  click <a href='[#Link]'>here</a> to view.<br /> <br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Cancelled:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Course  [#CourseTitle] has been Cancelled",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>The course [#CourseTitle] has been cancelled.</p><br />
                                                Please  click <a href='[#Link]'>here</a> to view.<br /> <br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Amendment_Self:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Revert Course [#CourseTitle] For Amendment",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>You have reverted the course [#CourseTitle] for ammendment.</p><br />
                                                Please  click <a href='[#Link]'>here</a> to view.<br /> <br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Amendment:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Course [#CourseTitle] Require Amendment",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>A course [#CourseTitle] requires ammendment.</p><br />
                                                Please  click <a href='[#Link]'>here</a> to view.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Invitation:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Invitation to Enroll To Course [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>You are invited to enroll to the course [#CourseTitle]</p><br />
                                                Please  click <a href='[#Link]'>here</a> to enroll.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Assigned_To_Facilitator:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Assigned to Course [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>As facilitator/trainer, you have been assigned to the course [#CourseTitle].</p><br />
                                                Please  click <a href='[#Link]'>here</a> to view the course.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;

                    case NotificationType.Course_Student_Enrolled:  // Send to Faclitator when a user enrolled

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "A new user enrolled to Course [#CourseTitle]",
                            TemplateMessage = @"Dear [#ReceiverFullName],<br /> <br />
                                                <p>A new user ([#LearnerName]) has enrolled to the course [#CourseTitle].</p><br />
                                                Please  click <a href='[#Link]'>here</a> to view the course.<br />
                                                Thank you.",
                            enableSMSMessage = false,
                            SMSMessage = "SMS Message Template",
                            enableWebMessage = false,
                            WebMessage = "Web Message Template",
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            Display = true
                        });

                        break;
                }
            }
        }
    }
}