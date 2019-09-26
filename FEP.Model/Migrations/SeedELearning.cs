using FEP.Model.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FEP.Model.Migrations
{
    public static class SeedElearning
    {
        public static void Seed(DbEntities db)
        {
            // Seed Role and Access
            AddRoleAndAccess(db, RoleNames.eLearningTrainer, "Default Trainer",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseDiscussionGroupCreate, UserAccess.CourseGroupCreate,
                UserAccess.CourseEdit, UserAccess.CourseAddDocument);
            AddRoleAndAccess(db, RoleNames.eLearningAdmin, "Admin eLearning", 
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseCreate, UserAccess.CourseEdit, 
                UserAccess.CoursePublish, UserAccess.CoursePublish);
            AddRoleAndAccess(db, RoleNames.eLearningVerifier, "Verifier eLearning", 
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseVerify);
            AddRoleAndAccess(db, RoleNames.eLearningApprover1, "Approver eLearning 1", 
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseApproval1);
            AddRoleAndAccess(db, RoleNames.eLearningApprover2, "Approver eLearning 2",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseApproval2);
            AddRoleAndAccess(db, RoleNames.eLearningApprover3, "Approver eLearning 3",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView,
                UserAccess.CourseApproval3);
            AddRoleAndAccess(db, RoleNames.eLearningLearner, "Learner",
                UserAccess.HomeDashboard1, UserAccess.LearningMenu, UserAccess.CourseView, 
                UserAccess.CourseEnroll);

            // Seed User
            AddUser(db, "eladmin@fep.com", "eladmin@fep.com", UserType.Individual, RoleNames.eLearningTrainer, RoleNames.eLearningAdmin);
            AddUser(db, "eltrainer1@fep.com", "eltrainer1@fep.com", UserType.Individual, RoleNames.eLearningTrainer);
            AddUser(db, "eltrainer2@fep.com", "eltrainer2@fep.com", UserType.Individual, RoleNames.eLearningTrainer);
            AddUser(db, "elverifier@fep.com", "elverifier@fep.com", UserType.Individual, RoleNames.eLearningAdmin);
            AddUser(db, "elapprover1@fep.com", "elapprover1@fep.com", UserType.Individual, RoleNames.eLearningApprover1);
            AddUser(db, "elapprover2@fep.com", "elapprover2@fep.com", UserType.Individual, RoleNames.eLearningApprover2);
            AddUser(db, "elapprover3@fep.com", "elapprover3@fep.com", UserType.Individual, RoleNames.eLearningApprover3);
            AddUser(db, "ellearner1@fep.com", "learner1@fep.com", UserType.Individual, RoleNames.eLearningLearner);
            AddUser(db, "ellearner2@fep.com", "learner2@fep.com", UserType.Individual, RoleNames.eLearningLearner);

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

                db.RefCourseCategories.AddOrUpdate(
                    x => x.Name,
                    createCategory("General", "General Category"),
                    createCategory("Youth", "Youth Personal Finance Management"),
                    createCategory("Individual", "Individual Finance Management"),
                    createCategory("Family", "Family Finance Management")

                    );

                db.SaveChanges();
            }
        }

        public static void SeedSampleData(DbEntities db)
        {
            if (!db.Courses.Any())
            {
                // Add a course
                Course course = new Course
                {
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
                    Status = CourseStatus.Draft
                };

                db.Courses.Add(course);

                // add modules
                List<CourseContent> moduleContents = new List<CourseContent>();

                CourseContent richText = new CourseContent
                {
                    Order = 2,
                    ContentType = CourseContentType.RichText,
                    IsViewable = true,
                    Text = "<div><h2>What is Lorem Ipsum?</h2><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing" +
                        " and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an" +
                        " unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only " +
                        "five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised " +
                        "in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop " +
                        "publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div>",
                    CompletionType = ContentCompletionType.Timer,
                    Description = "<p> This is module content 1 </p>",
                    Title = "Module Content 1",
                };
                moduleContents.Add(richText);

                CourseContent video = new CourseContent
                {
                    Order = 4,
                    ContentType = CourseContentType.Video,
                    IsViewable = true,
                    Url = "https://www.youtube.com/watch?v=WEDIj9JBTC8",
                    CompletionType = ContentCompletionType.ClickButton,
                    Description = "<p> Watch the video</p>",
                    Title = "Module Content 2",
                };
                moduleContents.Add(video);

                CourseContent iframe = new CourseContent
                {
                    Order = 5,
                    ContentType = CourseContentType.IFrame,
                    IsViewable = true,
                    Url = "https://www.sinarharian.com.my/",
                    CompletionType = ContentCompletionType.ClickButton,
                    Description = "<p> This is module content 3 </p>",
                    Title = "Module 3",
                };
                moduleContents.Add(iframe);

                CourseContent doc = new CourseContent
                {
                    Order = 6,
                    ContentType = CourseContentType.Document,
                    IsViewable = true,
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
                        Description = "<p> Description Module</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 1>",
                        ModuleContents = moduleContents,
                    },
                    new CourseModule
                    {
                        Order = 2,
                        CourseId = course.Id,
                        Description = "<p> Description Module</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 2",
                        ModuleContents = new List<CourseContent> {
                            new CourseContent
                            {
                                Order = 1,
                                ContentType = CourseContentType.Video,
                                IsViewable = true,
                                Url = "https://www.youtube.com/watch?v=WEDIj9JBTC8" ,
                                CompletionType = ContentCompletionType.ClickButton,
                                Description = "<p> Watch the video</p>",
                                Title = "Module Content 2",
                            }
                        } ,
                    },
                    new CourseModule
                    {
                        Order = 3,
                        CourseId = course.Id,
                        Description = "<p> Description Module</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 3",
                        ModuleContents = new List<CourseContent> {
                            new CourseContent
                            {
                                Order = 1,
                                ContentType = CourseContentType.Video,
                                IsViewable = true,
                                Url = "https://www.youtube.com/watch?v=WEDIj9JBTC8" ,
                                CompletionType = ContentCompletionType.ClickButton,
                                Description = "<p> Watch the video</p>",
                                Title = "Module Content 2",
                            }
                        } ,
                    },
                    new CourseModule
                    {
                        Order = 4,
                        CourseId = course.Id,
                        Description = "<p> Description Module</p>",
                        Objectives = "In this module you will learn: <br /><ul><li>point 1</li><li>point 2</li><li>point 3</li></ul>",
                        Title = "Module 1",
                        ModuleContents = moduleContents.Where(x =>x.IsViewable).ToList().Select(x => new CourseContent()).ToList(),
                    },
                };

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
    }
}