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
               NotificationType.Verify_Courses_Creation,
               NotificationType.Approve_Courses_Creation_Approver1,
               NotificationType.Approve_Courses_Creation_Approver2,
               NotificationType.Approve_Courses_Creation_Approver3,
               NotificationType.Course_Approved,
               NotificationType.Course_Amendment,
               NotificationType.Course_Invitation,
               NotificationType.Approve_Courses_Published_Change,
               NotificationType.Approve_Courses_Published_Withdraw
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
                    case NotificationType.Verify_Courses_Creation:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Verify A New Course : [#CourseTitle]",
                            TemplateMessage = @"Dear [#],<br /> <br />
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
                            TemplateMessage = @"Dear [#UserFullName],<br /> <br />
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

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Course  [#CourseTitle] has been Approved",
                            TemplateMessage = @"Dear [#UserFullName],<br /> <br />
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

                    case NotificationType.Course_Amendment:

                        db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                        new NotificationTemplate
                        {
                            NotificationType = notifyType,
                            TemplateName = notifyType.DisplayName(),
                            TemplateRefNo = "T" + ((int)notifyType).ToString(),
                            enableEmail = true,
                            TemplateSubject = "Course [#CourseTitle] Require Amendment",
                            TemplateMessage = @"Dear [#UserFullName],<br /> <br />
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
                            TemplateMessage = @"Dear [#UserFullName],<br /> <br />
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
                }
            }
        }
    }
}