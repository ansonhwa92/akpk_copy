﻿using System;
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

                    new SLAReminder { SLAEventType = SLAEventType.ApprovePublicEvent, NotificationType = NotificationType.Approve_Public_Event_Creation, ETCode = "ET002APE", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
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

                    new SLAReminder { SLAEventType = SLAEventType.VerifySurvey, NotificationType = NotificationType.Verify_Survey_Creation, ETCode = "ET024RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.VerifySurvey, NotificationType = NotificationType.Verify_Survey_Published_Cancelled, ETCode = "ET025RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                    new SLAReminder { SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation, ETCode = "ET026RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation, ETCode = "ET027RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                    new SLAReminder { SLAEventType = SLAEventType.VerifyPublication, NotificationType = NotificationType.Verify_Publication_Creation, ETCode = "ET028RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.VerifyPublication, NotificationType = NotificationType.Verify_Publication_Published_Change, ETCode = "ET029RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.VerifyPublication, NotificationType = NotificationType.Verify_Publication_Published_Withdraw, ETCode = "ET030RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                    new SLAReminder { SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation, ETCode = "ET031RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Published_Change, ETCode = "ET032RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                    new SLAReminder { SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Published_Withdraw, ETCode = "ET033RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }

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
                        new NotificationTemplate {
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
                        } );
                }
            }
            
        }
    }
}
