using FEP.Model.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FEP.Model.Migrations
{
    public static class SeedElearning
    {
        public const int courseTrialId = 99;

        public static void Seed(DbEntities db)
        {
            SeedRoles(db);
            SeedDefaultData(db);
            SeedSampleUsers(db);
            SeedSampleCategories(db);

            SeedSampleData(db);

            //   SeedSampleQuestions(db);

            SeedAssignTrainerToCourse(db);

            //SeedSampleCertificateAndTemplate(db);

            SeedAssignTrainerToGroup(db);

            // SeedAdditionalCourses(db);
            SeedRules(db);

            DefaultSLAReminder(db);
            DefaultParameterGroup(db);
            // DefaultTemplate(db);
        }

        private static void SeedDefaultData(DbEntities db)
        {
        }

        private static Course AddCourse(DbEntities db, string title, int courseId, CourseStatus courseStatus = CourseStatus.Draft)
        {
            int i = courseId;

            Course course = new Course
            {
                Id = courseId,
                CategoryId = 1,
                Code = "CODE : " + title,
                DefaultAllowablePercentageBeforeWithdraw = 25.0m,
                Description = "COURSE " + title,
                Duration = 5,
                DurationType = DurationType.Hour,
                IsFree = i % 2 == 1 ? true : false,
                Language = CourseLanguage.Malay,
                Medium = CourseMedium.Online,
                Objectives = "<strong>Objective</strong>",
                ScheduleType = CourseScheduleType.NoTimeLimit,
                Title = title,
                Status = courseStatus,
            };

            db.Courses.Add(course);

            db.SaveChanges();

            course.Modules = new List<CourseModule>
                {
                    new CourseModule { Title = "Sample Module : " + title,
                        CourseId =course.Id, Order=1, Description="Description",
                        ModuleContents = new List<CourseContent>
                        {
                            new CourseContent
                            {
                                Order = 1,
                                CourseId = course.Id,
                                ContentType = CourseContentType.RichText,
                                Text = "<h2>Sample Content for COURSE " + title + "</h2>",
                                CompletionType = ContentCompletionType.Timer,
                                Timer = 30, // in seconds
                                Description = "<p> This is Content 1 for COURSE " +title + "</p>",
                                Title = "<h2>Sample Content for COURSE " +title + "</h2>",
                            }
                        }
                    }
                };

            course.CourseApprovalLog = new List<CourseApprovalLog>
                {
                    new CourseApprovalLog
                    {
                        CreatedByName = "system",
                        ActionDate = DateTime.Now,
                        Remark = "Course " + course.Title + " created.",
                    },
                };

            db.Entry(course).State = EntityState.Modified;

            db.SaveChanges();

            return course;
        }

        private static void SeedAdditionalCourses(DbEntities db)
        {
            var courseExist = db.Courses.Find(2);

            if (courseExist != null)
                return;

            int i = 2;
            while (i <= 5)
            {
                // Add a course
                Course course = new Course
                {
                    Id = i,
                    CategoryId = 1,
                    Code = "COURSE " + i.ToString(),
                    DefaultAllowablePercentageBeforeWithdraw = 25.0m,
                    Description = "Sample Content : Code " + "COURSE " + i.ToString(),
                    Duration = 5,
                    DurationType = DurationType.Hour,
                    IsFree = i % 2 == 1 ? true : false,
                    Language = CourseLanguage.Malay,
                    Medium = CourseMedium.Online,
                    Objectives = "<strong>Objective</strong>",
                    ScheduleType = CourseScheduleType.NoTimeLimit,
                    Title = "Sample Content : Code " + "COURSE" + i.ToString(),
                    Status = CourseStatus.Draft,
                };

                db.Courses.Add(course);

                db.SaveChanges();

                CourseContent richText = new CourseContent
                {
                    Order = 1,
                    CourseId = course.Id,
                    ContentType = CourseContentType.RichText,
                    Text = "<h2>Sample Content for COURSE" + i.ToString() + "</h2>",
                    CompletionType = ContentCompletionType.Timer,
                    Timer = 30, // in seconds
                    Description = "<p> This is Content 1 for COURSE" + i.ToString() + "</p>",
                    Title = "<h2>Sample Content for COURSE" + i.ToString() + "</h2>",
                };

                course.Modules = new List<CourseModule>
                {
                    new CourseModule { Title = "Sample Module : Code " + "COURSE" + i.ToString(),
                        CourseId =course.Id, Order=1, Description="Description",
                        ModuleContents = new List<CourseContent>
                        {
                            new CourseContent
                            {
                                Order = 1,
                                CourseId = course.Id,
                                ContentType = CourseContentType.RichText,
                                Text = "<h2>Sample Content for COURSE" + i.ToString() + "</h2>",
                                CompletionType = ContentCompletionType.Timer,
                                Timer = 60, // in seconds
                                Description = "<p> This is Content 1 for COURSE" + i.ToString() + "</p>",
                                Title = "<h2>Sample Content for COURSE" + i.ToString() + "</h2>",
                            }
                        }
                    }
                };

                course.CourseApprovalLog = new List<CourseApprovalLog>
                {
                    new CourseApprovalLog
                    {
                        CreatedByName = "system",
                        ActionDate = DateTime.Now,
                        Remark = "Course " + course.Title + " created.",
                    },
                };

                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                i++;
            }
        }

        private static void SeedRules(DbEntities db)
        {
            var course = db.Courses.FirstOrDefault(x => x.Id == 1);

            course.TraversalRule = TraversalRule.Sequential;
            course.CompletionCriteriaType = CompletionCriteriaType.ActivityCompletion;
            course.ScoreCalculation = ScoreCalculation.Average;
            course.LearningPath = "{2,3,4}"; // course 2,3,4

            db.SaveChanges();
        }

        private static void SeedSampleCertificateAndTemplate(DbEntities db)
        {
            if (!db.CourseCertificates.Any())
            {
                db.CourseCertificates.Add(new CourseCertificate
                {
                    BackgroundImageFilename = "img1.jpg",
                    Description = "Image 1",
                });

                db.SaveChanges();

                db.CourseCertificates.Add(new CourseCertificate
                {
                    BackgroundImageFilename = "img2.jpg",
                    Description = "Image 2",
                });

                db.SaveChanges();
            }

            if (!db.CourseCertificateTemplates.Any())
            {
                db.CourseCertificateTemplates.Add(new CourseCertificateTemplate
                {
                    Description = "Template 1",
                    TypePageOrientation = TypePageOrientation.Portrait,
                    Template = "<h2>CERTIFICATE FOR </h2>"
                });

                db.SaveChanges();
            }
        }

        private static void AssignLearnersToCourseEvent(DbEntities db, int courseId)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            // Get the  course
            var courseEvent = db.CourseEvents.Where(x => x.CourseId == courseId)
                    .OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            if (courseEvent == null)
                return;

            var enrollment = db.Enrollments.Where(x => x.CourseEventId == courseEvent.Id);

            if (enrollment != null)
                return;

            var userRoles = db.UserRole.Where(x => x.Role.Name == RoleNames.eLearningLearner);

            foreach (var user in userRoles)
            {
                db.Enrollments.Add(new Enrollment
                {
                    CourseEventId = courseEvent.Id,
                    CourseId = courseId,
                    EnrolledDate = DateTime.Now,
                    LearnerId = user.Id,
                    Status = EnrollmentStatus.Enrolled,
                });
            }

            db.SaveChanges();
        }

        private static void SeedAssignTrainerToCourse(DbEntities db)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            // Get the  course
            var course = db.Courses.Include(x => x.Trainers).First();

            if (course.Trainers == null)
            {
                course.Trainers = new List<Trainer>();
            }

            var userRole = db.UserRole.FirstOrDefault(x => x.Role.Name == RoleNames.eLearningTrainer);

            var user = db.User.FirstOrDefault(x => x.Id == userRole.UserId);

            db.TrainerCourses.Add(new TrainerCourse
            {
                Trainer = new Trainer { User = user },
                Course = course
            });

            db.SaveChanges();
        }

        private static void SeedAssignTrainerToGroup(DbEntities db)
        {
            //  Get the  course
        }

        private static void SeedRoles(DbEntities db)
        {
            // Seed Role and Access
            AddRoleAndAccess(db, RoleNames.eLearningTrainer, "Default Trainer",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseGroupCreate, UserAccess.CourseAddDocument);
            AddRoleAndAccess(db, RoleNames.eLearningAdmin, "Admin eLearning",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView, 
                UserAccess.CourseCreate, UserAccess.CourseEdit,
                UserAccess.CourseGroupCreate, UserAccess.CourseGroupEdit, UserAccess.CourseGroupView,
                UserAccess.CourseDiscussionCreate,
                UserAccess.CoursePublish, UserAccess.CoursePublish);
            AddRoleAndAccess(db, RoleNames.eLearningVerifier, "Verifier eLearning",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseGroupView,
                UserAccess.CourseVerify);
            AddRoleAndAccess(db, RoleNames.eLearningApprover1, "Approver eLearning 1",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseGroupView,
                UserAccess.CourseApproval1);
            AddRoleAndAccess(db, RoleNames.eLearningApprover2, "Approver eLearning 2",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseGroupView,
                UserAccess.CourseApproval2);
            AddRoleAndAccess(db, RoleNames.eLearningApprover3, "Approver eLearning 3",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseGroupView,
                UserAccess.CourseApproval3);
            AddRoleAndAccess(db, RoleNames.eLearningLearner, "Learner",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseEnroll);
            AddRoleAndAccess(db, RoleNames.eLearningFacilitator, "Facilitator",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu,
                UserAccess.CourseGroupCreate, UserAccess.CourseGroupEdit, UserAccess.CourseGroupView, 
                UserAccess.CourseDiscussionCreate,
                UserAccess.CourseAddDocument, 
                UserAccess.CourseEnroll);
        }

        private static void SeedSampleUsers(DbEntities db)
        {
            // Seed User
            AddUser(db, "eladmin@fep.com", "eladmin@fep.com", UserType.Individual, RoleNames.eLearningTrainer, RoleNames.eLearningAdmin);
            AddUser(db, "elverifier@fep.com", "elverifier@fep.com", UserType.Individual, RoleNames.eLearningVerifier);
            AddUser(db, "elapprover1@fep.com", "elapprover1@fep.com", UserType.Individual, RoleNames.eLearningApprover1);
            AddUser(db, "elapprover2@fep.com", "elapprover2@fep.com", UserType.Individual, RoleNames.eLearningApprover2);
            AddUser(db, "elapprover3@fep.com", "elapprover3@fep.com", UserType.Individual, RoleNames.eLearningApprover3);

            AddUser(db, "min.elearn@yahoo.com", "min.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningTrainer, RoleNames.eLearningAdmin);
            AddUser(db, "v1.elearn@yahoo.com", "v1.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningVerifier);
            AddUser(db, "app1.elearn@yahoo.com", "app1.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningApprover1);
            AddUser(db, "app2.elearn@yahoo.com", "app2.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningApprover2);
            AddUser(db, "app3.elearn@yahoo.com", "app3.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningApprover3);

            AddUser(db, "faci1.elearn@yahoo.com", "faci1.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningFacilitator);
            AddUser(db, "faci2.elearn@yahoo.com", "faci2.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningFacilitator);

            AddUser(db, "stud1.elearn@yahoo.com", "stud1.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningLearner);
            AddUser(db, "stud2.elearn@yahoo.com", "stud2.elearn@yahoo.com", UserType.Individual, RoleNames.eLearningLearner);

            for (int i = 1; i <= 10; i++)
            {
                var facilitator = $"elInstructor{i}@fep.com";

                AddUser(db, facilitator, facilitator, UserType.Individual, RoleNames.eLearningFacilitator);
            }

            for (int i = 1; i <= 20; i++)
            {
                var trainer = $"eltrainer{i}@fep.com";

                AddUser(db, trainer, trainer, UserType.Individual, RoleNames.eLearningTrainer, RoleNames.eLearningFacilitator);
            }

            for (int i = 1; i <= 20; i++)
            {
                var student = $"elstudent{i}@fep.com";

                AddUser(db, student, student, UserType.Individual, RoleNames.eLearningLearner);
            }
        }

        private static void SeedSampleCategories(DbEntities db)
        {
            // Seed Category
            if (!db.RefCourseCategories.Any())
            {
                Func<string, string, RefCourseCategory> createCategory = (name, desc) =>
                {
                    var u = new RefCourseCategory
                    {
                        Name = name,
                        Description = desc,
                        IsDisplayed = true,
                    };

                    return u;
                };

                //db.RefCourseCategories.AddOrUpdate(
                //    x => x.Name,
                //    createCategory("General", "General Category"),
                //    createCategory("Youth", "Youth Personal Finance Management"),
                //    createCategory("Individual", "Individual Finance Management"),
                //    createCategory("Family", "Family Finance Management")

                //    );

                db.RefCourseCategories.AddOrUpdate(
                    x => x.Name,
                    createCategory("Cash Flow", "Cash Flow"),
                    createCategory("Car", "Car"),
                    createCategory("House", "House"),
                    createCategory("Investment", "Investment"),
                    createCategory("Protection", "Protection")

                    );

                db.SaveChanges();
            }
        }

        public static void SeedSampleData(DbEntities db)
        {
            if (!db.Courses.Any())
            {
                // add a front page video
                var fileDocument = new FileDocument
                {
                    FileName = "Test.mp4",
                    FileNameOnStorage = "Test.mp4",
                    FilePath = "D:\\FEPDoc",
                    FileType = FileType.Video.ToString(),
                    CreatedDate = DateTime.Now,
                    User = db.User.FirstOrDefault(x => x.Id == 1)
                };

                db.FileDocument.Add(fileDocument);

                db.SaveChanges();

                // Add a course
                Course course = new Course
                {
                    Id = 1,
                    CategoryId = 1,
                    Code = "GEN01",
                    DefaultAllowablePercentageBeforeWithdraw = 25.0m,
                    Description = "This course is aimed to educate the youth particularly aged between " +
                          "18 to 22 years to learn good and more practical personal financial management'",
                    Duration = 10,
                    DurationType = DurationType.Hour,
                    IsFree = false,
                    Language = CourseLanguage.Malay,
                    Medium = CourseMedium.Online,
                    Objectives = "This course is to expose and enpower students on good financial knowledge. By the end of the course," +
                             " student common should be able to: " +
                             "<ul> <li> Understand the terminology used in financial management </li>" +
                             "<li> Demonstrate the ability to conduct planning and budgeting activities towards " +
                             "realising a desired goal and </li>" +
                             "<li> Instill commitment towards loan - repayment to avoid future financial challenges.</li></ul>",
                    ScheduleType = CourseScheduleType.NoTimeLimit,
                    Title = "Kembara Bijak Wang",
                    Status = CourseStatus.Draft,
                    IntroMaterialId = fileDocument.Id,
                    IntroMaterial = fileDocument
                };

                db.Courses.Add(course);

                db.SaveChanges();

                // add modules
                List<CourseContent> moduleContents = new List<CourseContent>();

                CourseContent richText = new CourseContent
                {
                    Order = 2,
                    CourseId = course.Id,
                    ContentType = CourseContentType.RichText,
                    Text = "<div><h2>What is Lorem Ipsum?</h2><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing" +
                        " and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an" +
                        " unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only " +
                        "five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised " +
                        "in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop " +
                        "publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div>",
                    CompletionType = ContentCompletionType.Timer,
                    Timer = 120, // in seconds
                    Description = "<p> This is Content 1 </p>",
                    Title = "Content 1",
                };
                moduleContents.Add(richText);

                CourseContent video = new CourseContent
                {
                    Order = 4,
                    CourseId = course.Id,
                    ContentType = CourseContentType.Video,
                    Url = "https://www.youtube.com/watch?v=WEDIj9JBTC8",
                    CompletionType = ContentCompletionType.ClickButton,
                    Description = "<p> Watch the video</p>",
                    Title = "Content 2",
                };
                moduleContents.Add(video);

                CourseContent iframe = new CourseContent
                {
                    Order = 5,
                    CourseId = course.Id,
                    ContentType = CourseContentType.IFrame,
                    Url = "https://www.sinarharian.com.my/",
                    CompletionType = ContentCompletionType.ClickButton,
                    Description = "<p> This is Content 3 </p>",
                    Title = "Content 3",
                };
                moduleContents.Add(iframe);

                CourseContent doc = new CourseContent
                {
                    Order = 6,
                    CourseId = course.Id,
                    ContentType = CourseContentType.Document,
                    Url = "http://www.its.caltech.edu/~rosentha/courses/BEM103/Readings/JWCh01.pdf",
                    CompletionType = ContentCompletionType.ClickButton,
                    Description = "<p> Document </p>",
                    Title = "Read this document",
                };
                moduleContents.Add(doc);

                // add modules
                course.Modules = new List<CourseModule>
                {
                    new CourseModule
                    {
                        Order = 1,
                        CourseId = course.Id,
                        Description = "<p> Description Module 1</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 1",
                        ModuleContents = moduleContents,
                    },
                    new CourseModule
                    {
                        Order = 2,
                        CourseId = course.Id,
                        Description = "<p> Description Module 2</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 2",
                        ModuleContents = new List<CourseContent> {
                            new CourseContent
                            {
                                Order = 1,
                                CourseId = course.Id,
                                ContentType = CourseContentType.Video,
                                Url = "https://www.youtube.com/watch?v=WEDIj9JBTC8" ,
                                 CompletionType  = ContentCompletionType.ClickButton,
                                Description = "<p> Watch the video</p>",
                                Title = "Content 2",
                            }
                        } ,
                    },
                    new CourseModule
                    {
                        Order = 3,
                        CourseId = course.Id,
                        Description = "<p> Description Module 3</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 3",
                        ModuleContents = new List<CourseContent> {
                            new CourseContent
                            {
                                Order = 1,
                                CourseId = course.Id,
                                ContentType = CourseContentType.Video,
                                Url = "https://www.youtube.com/watch?v=WEDIj9JBTC8" ,
                                 CompletionType  = ContentCompletionType.ClickButton,
                                Description = "<p> Watch the video</p>",
                                Title = "Content 2",
                            }
                        },
                    },
                    new CourseModule
                    {
                        Order = 4,
                        CourseId = course.Id,
                        Description = "<p> Description Module 4</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 4",
                        ModuleContents = moduleContents.ToList().Select(x => new CourseContent {
                            Order = x.Order,
                            Text = x.Text,
                            Description = x.Description,
                            Timer  = x.Timer,
                            Title = x.Title,
                            CourseId = x.CourseId,
                            VideoType = x.VideoType,
                            AudioType = x.AudioType,
                            CompletionType = x.CompletionType,
                            Url = x.Url,
                            ShowIFrameAs  = x.ShowIFrameAs,
                            Question = x.Question,
                            QuestionType = x.QuestionType,
                            ContentType = x.ContentType
                        }).ToList(),
                    },
                };

                db.SaveChanges();

                course.CourseApprovalLog = new List<CourseApprovalLog>
                {
                    new CourseApprovalLog
                    {
                        CreatedByName = "system",
                        ActionDate = DateTime.Now,
                        Remark = "Course " + course.Title + " created.",
                        ApprovalStatus = ApprovalStatus.None
                    },
                };

                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var module in course.Modules)
                {
                    module.UpdateTotals();

                    db.SetModified(module);

                    db.SaveChanges();
                }
            }
        }

        public static void SeedSampleQuestions(DbEntities db)
        {
            if (!db.ContentQuestions.Any())
            {
                // Get the  course
                var courseId = db.Courses.Select(x => x.Id).First();

                var question = new Question
                {
                    CourseId = courseId,
                    QuestionType = QuestionType.MultipleChoice,
                    Name = "If a = 2 and b = 4, then what is a * b?",
                    MultipleChoiceAnswerId = 2,
                };

                question.MultipleChoiceAnswers = new List<MultipleChoiceAnswer>
                {
                    new MultipleChoiceAnswer
                    { Id= 1, Order = 1, Answer = "24"  },
                    new MultipleChoiceAnswer
                    { Id= 2, Order = 2, Answer = "8"  },
                    new MultipleChoiceAnswer
                    { Id= 3, Order = 3, Answer = "42"  },
                    new MultipleChoiceAnswer
                    { Id= 4, Order = 4, Answer = "4"  },
                };

                db.Questions.Add(question);

                db.SaveChanges();

                var question2 = new Question
                {
                    CourseId = courseId,
                    QuestionType = QuestionType.MultipleChoice,
                    Name = "What does RM stand for?",
                    MultipleChoiceAnswerId = 8,
                };

                question2.MultipleChoiceAnswers = new List<MultipleChoiceAnswer>
                {
                    new MultipleChoiceAnswer
                    { Id= 5, Order = 1, Answer = "Rumah Merah"  },
                    new MultipleChoiceAnswer
                    { Id= 6, Order = 2, Answer = "Risk Management"  },
                    new MultipleChoiceAnswer
                    { Id= 7, Order = 3, Answer = "Russian Mafia"  },
                    new MultipleChoiceAnswer
                    { Id= 8, Order = 4, Answer = "Ringgit Malaysia"  },
                };

                db.Questions.Add(question2);

                db.SaveChanges();

                var module = db.CourseModules
                    .Include(x => x.ModuleContents)
                    .FirstOrDefault(x => x.CourseId == courseId);

                var order = module.ModuleContents.Max(x => x.Order);

                var content = new CourseContent
                {
                    ContentType = CourseContentType.Test,
                    CourseId = courseId,
                    Order = ++order,
                    CompletionType = ContentCompletionType.AnswerQuestion,
                    QuestionType = question.QuestionType,
                    QuestionId = question.Id,
                    Title = "Question 1",
                    Description = "Question 1"
                };

                if (module.ModuleContents == null)
                    module.ModuleContents = new List<CourseContent>();

                module.ModuleContents.Add(content);
                module.UpdateTotals();

                db.SaveChanges();

                var content2 = new CourseContent
                {
                    ContentType = CourseContentType.Test,
                    CourseId = courseId,
                    Order = ++order,
                    CompletionType = ContentCompletionType.AnswerQuestion,
                    QuestionType = question2.QuestionType,
                    QuestionId = question2.Id,
                    Title = "Question 2",
                    Description = "Question 2"
                };

                content2.QuestionId = question2.Id;

                if (module.ModuleContents == null)
                    module.ModuleContents = new List<CourseContent>();

                module.ModuleContents.Add(content2);

                db.SaveChanges();

                module.UpdateTotals();

                db.SetModified(module);

                db.SaveChanges();
            }
        }

        public static void AddRoleAndAccess(DbEntities db, string RoleName, string Description, params UserAccess[] accesses)
        {
            var role = db.Role.Local.Where(r => r.Name.Contains(RoleName)).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains(RoleName)).FirstOrDefault();

            if (role == null)
            {
                List<RoleAccess> roleaccess = new List<RoleAccess>();

                foreach (var access in accesses)
                {
                    roleaccess.Add(new RoleAccess { UserAccess = access });
                }

                db.Role.Add(
                    new Role
                    {
                        Name = RoleName,
                        Description = Description,
                        Display = true,
                        CreatedDate = DateTime.Now,
                        RoleAccess = roleaccess
                    });

                db.SaveChanges();
            }
        }

        public static void AddUser(DbEntities db, string name, string email, UserType userType, params string[] roles)
        {
            var user = db.User.Local.Where(r => r.Name.Contains(name)).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains(name)).FirstOrDefault();

            if (user == null)
            {
                List<UserRole> userRoles = new List<UserRole>();

                foreach (var assignRole in roles)
                {
                    var role = db.Role.Local.Where(r => r.Name.Contains(assignRole)).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains(assignRole)).FirstOrDefault();

                    if (role != null)

                        userRoles.Add(new UserRole { Role = role });
                }

                db.User.AddOrUpdate(
                    x => x.Email,
                    new User
                    {
                        Name = name,
                        Email = email,
                        UserType = userType,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = email,
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,
                            UserRoles = userRoles
                        },
                    }
                );

                db.SaveChanges();
            }
        }

        public static void DefaultSLAReminder(DbEntities db)
        {
            db.SLAReminder.AddOrUpdate(s => s.NotificationType,

                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Creation, ETCode = "ET016EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Published_Change, ETCode = "ET017EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Published_Withdraw, ETCode = "ET018EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Verify_Courses_Participant_Withdraw, ETCode = "ET019EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Creation_Approver1, ETCode = "ET020EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Creation_Approver2, ETCode = "ET021EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Creation_Approver3, ETCode = "ET022EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Published_Change, ETCode = "ET023EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Published_Withdraw, ETCode = "ET024EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Approve_Courses_Participant_Withdraw, ETCode = "ET025EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.VerifyCourses, NotificationType = NotificationType.Course_Amendment, ETCode = "ET026EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Learning, SLAEventType = SLAEventType.ApproveCourses, NotificationType = NotificationType.Course_Approved, ETCode = "ET027EL", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }

            );
        }

        public static void DefaultParameterGroup(DbEntities db)
        {
            foreach (TemplateParameterType paramType in Enum.GetValues(typeof(TemplateParameterType)))
            {
                SLAEventType EventType;

                int pType = (int)paramType;

                if (pType >= 141 && pType <= 160)
                {
                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyCourses, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApproveCourses, TemplateParameterType = paramType });

                    continue;
                }
            }
        }

        public static void DefaultTemplate(DbEntities db)
        {
            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.Verify_Courses_Creation,
                    NotificationCategory = NotificationCategory.Learning,
                    TemplateName = NotificationType.Verify_Courses_Creation.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.Verify_Courses_Creation).ToString(),
                    enableEmail = true,
                    TemplateSubject = "Verify A New Course : [#CourseTitle]",
                    TemplateMessage = @"Hi,<br />
                                        <p>A course [#CourseTitle] requires verification.</p><br />
                                        Please click <a href='[#Link]'>here</a> to verify.<br />
                                        Thank you.<br />",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.Approve_Courses_Creation_Approver1,
                    NotificationCategory = NotificationCategory.Learning,
                    TemplateName = NotificationType.Approve_Courses_Creation_Approver1.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.Approve_Courses_Creation_Approver1).ToString(),
                    enableEmail = true,
                    TemplateSubject = "Approval needed for Course : [#CourseTitle]",
                    TemplateMessage = @"Hi, <br />
                                        A course [#CourseTitle] requires Approval.<br />
                                        Please  click <a href='[#Link]'>here</a> to approve.<br />
                                        Thank you.<br />",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.Approve_Courses_Creation_Approver2,
                    NotificationCategory = NotificationCategory.Learning,
                    TemplateName = NotificationType.Approve_Courses_Creation_Approver1.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.Approve_Courses_Creation_Approver1).ToString(),
                    enableEmail = true,
                    TemplateSubject = "Approval needed for Course : [#CourseTitle]",
                    TemplateMessage = @"Hi, <br />
                                        A course [#CourseTitle] requires Approval.<br />
                                        Please  click <a href='[#Link]'>here</a> to approve.<br />
                                        Thank you.<br />",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.Approve_Courses_Creation_Approver3,
                    NotificationCategory = NotificationCategory.Learning,
                    TemplateName = NotificationType.Approve_Courses_Creation_Approver1.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.Approve_Courses_Creation_Approver1).ToString(),
                    enableEmail = true,
                    TemplateSubject = "Approval needed for Course : [#CourseTitle]",
                    TemplateMessage = @"Hi, <br />
                                        A course [#CourseTitle] requires Approval.<br />
                                        Please  click <a href='[#Link]'>here</a> to approve.<br />
                                        Thank you.<br />",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.Course_Approved,
                    NotificationCategory = NotificationCategory.Learning,
                    TemplateName = NotificationType.Course_Approved.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.Course_Approved).ToString(),
                    enableEmail = true,
                    TemplateSubject = "Course  [#CourseTitle] has been Approved",
                    TemplateMessage = @"Hi, <br />
                                        The course [#CourseTitle] has been approved.<br />
                                        Please  click <a href='[#Link]'>here</a> to view.<br />
                                        Thank you.<br />",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.Course_Amendment,
                    NotificationCategory = NotificationCategory.Learning,
                    TemplateName = NotificationType.Course_Amendment.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.Course_Amendment).ToString(),
                    enableEmail = true,
                    TemplateSubject = "Course [#CourseTitle] Require Amendment",
                    TemplateMessage = @"Hi, <br />
                                        A course [#CourseTitle] requires ammendment.<br />
                                        Please  click <a href='[#Link]'>here</a> to view.<br />
                                        Thank you.<br />",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.Course_Invitation,
                    NotificationCategory = NotificationCategory.Learning,
                    TemplateName = NotificationType.Course_Invitation.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.Course_Invitation).ToString(),
                    enableEmail = true,
                    TemplateSubject = "Invitation to Enroll To Course [#CourseTitle]",
                    TemplateMessage = @"Hi, <br />
                                        You are invited to enroll to the course [#CourseTitle]<br />
                                        Please  click <a href='[#Link]'>here</a> to enroll.<br />
                                        Thank you.<br />",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
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