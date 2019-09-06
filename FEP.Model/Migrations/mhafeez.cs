using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.Migrations
{
    public static class mhafeez
    {

        public static string DisplayName(this Enum val)
        {
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }

        public static void Seed(DbEntities db)
        {
            //notification
            if (!db.NotificationSetting.Any())
            {
                foreach (NotificationType type in (NotificationType[])Enum.GetValues(typeof(NotificationType)))
                {
                    db.NotificationSetting.AddOrUpdate(
                        t => new { t.NotificationType },
                        new NotificationSetting { NotificationType = type }
                    );
                }
            }
            
            //default setting
            if (!db.SystemSetting.Any(m => m.Id == 0))
            {
                db.SystemSetting.Add(new SystemSetting { Id = 0, SystemTitle = "Financial Education Portal", ShortTitle = "FEP", SystemVersion = "1.0", SystemFooter = "<b>Copyright © 2017-2020</b> All rights reserved." });
            }

            if (!db.AccountSetting.Any(m => m.Id == 0))
            {
                db.AccountSetting.Add(new AccountSetting { Id = 0, IsPasswordExpiry = false, InactiveDuration = 30, IsLimitLoginAttempt = false, LoginAttemptLimit = null, PasswordExpiryDuration = null });
            }

            //default access
            DefaultAccess(db);
            
            //default role
            List<RoleAccess> roleaccess = new List<RoleAccess>();

            foreach (UserAccess useraccess in Enum.GetValues(typeof(UserAccess)))
            {
                roleaccess.Add(new RoleAccess { UserAccess = useraccess });
            }


            //role n roleaccess default for system admin
            var roleallaccess = db.Role.Local.Where(r => r.Name.Contains("All Access")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("All Access")).FirstOrDefault();

            if (roleallaccess == null)
            {
                db.Role.Add(
                    new Role
                    {
                        Name = "All Access",
                        Description = "All Access",
                        Display = true,
                        CreatedDate = DateTime.Now,
                        RoleAccess = roleaccess
                    });
            }
            
            //default user
            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            if (user == null)
            {
                                
                var role = db.Role.Local.Where(r => r.Name.Contains("All Access")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("All Access")).FirstOrDefault();

                List<UserRole> userroles = new List<UserRole>();

                userroles.Add(new UserRole { Role = role });

                db.User.Add(
                    new User
                    {
                        Name = "System Admin",
                        Email = "admin@fep.com",                       
                        UserType = UserType.SystemAdmin,                        
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "admin@fep.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,                           
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,   
                            UserRoles = userroles
                        },
                    }
                );

            }

            AddRole(db, "Individual", "Default Individual");
            AddRole(db, "Individual with paper", "Individual with paper");
            AddRole(db, "Individual with paper to present", "Individual with paper to present");

            AddRole(db, "Agency", "Default Agency");
            AddRole(db, "Organizer", "Default Organizer");

            AddRole(db, "Trainer", "Default Trainer");
            AddRole(db, "Facilitator", "Default Facilitator");
            AddRole(db, "Speaker", "Default Speaker");

            AddRole(db, "Chief Editor", "Chief Editor");

            AddRole(db, "Staff", "Staff");

            AddRole(db, "Admin Event", "Admin Event");
            AddRole(db, "Admin R&P", "Admin R&P");
            AddRole(db, "Admin eLearning", "Admin eLearning");
            
            AddRole(db, "Event Reception", "Event Reception");
            AddRole(db, "Event Moderator", "Event Moderator");
            
            AddRole(db, "Verifier Event", "Verifier Event");
            AddRole(db, "Verifier R&P", "Verifier R&P");
            AddRole(db, "Verifier eLearning", "Verifier eLearning");
           
            AddRole(db, "Approver Event Level 1", "Approver Event Level 1");
            AddRole(db, "Approver R&P 1", "Approver R&P 1");
            AddRole(db, "Approver eLearning 1", "Approver eLearning 1");

            AddRole(db, "Approver Event 2", "Approver Event 2");
            AddRole(db, "Approver R&P 2", "Approver R&P 2");
            AddRole(db, "Approver eLearning 2", "Approver eLearning 2");

            AddRole(db, "Approver Event 3", "Approver Event 3");
            AddRole(db, "Approver R&P 3", "Approver R&P 3");
            AddRole(db, "Approver eLearning 3", "Approver eLearning 3");


            if (!db.State.Any())
            {
                db.State.AddOrUpdate(s => s.Code,
                    new State { Code = "01", Name = "Johor" },
                    new State { Code = "02", Name = "Kedah" },
                    new State { Code = "03", Name = "Kelantan" },
                    new State { Code = "04", Name = "Melaka" },
                    new State { Code = "05", Name = "Negeri Sembilan" },
                    new State { Code = "06", Name = "Pahang" },
                    new State { Code = "07", Name = "Pulau Pinang" },
                    new State { Code = "08", Name = "Perak" },
                    new State { Code = "09", Name = "Perlis" },
                    new State { Code = "10", Name = "Selangor" },
                    new State { Code = "11", Name = "Terengganu" },
                    new State { Code = "12", Name = "Sabah" },
                    new State { Code = "13", Name = "Sarawak" },
                    new State { Code = "14", Name = "Wilayah Persekutuan Kuala Lumpur" },
                    new State { Code = "15", Name = "Wilayah Persekutuan Labuan" },
                    new State { Code = "16", Name = "Wilayah Persekutuan Putrajaya" }
                );
            }

            if (!db.Sector.Any())
            {
                db.Sector.AddOrUpdate(s => s.Name,
                    new Sector { Name = "Aerospace", Display = true },
                    new Sector { Name = "Transport", Display = true },
                    new Sector { Name = "Computer", Display = true },
                    new Sector { Name = "Telecommunication", Display = true },
                    new Sector { Name = "Agriculture", Display = true },
                    new Sector { Name = "Construction", Display = true },
                    new Sector { Name = "Education", Display = true },
                    new Sector { Name = "Pharmaceutical", Display = true },
                    new Sector { Name = "Food", Display = true },
                    new Sector { Name = "HealthCare", Display = true },
                    new Sector { Name = "Hospitality", Display = true },
                    new Sector { Name = "Entertainment", Display = true },
                    new Sector { Name = "News Media", Display = true },
                    new Sector { Name = "Energy", Display = true },
                    new Sector { Name = "Manufacturing", Display = true },
                    new Sector { Name = "Music", Display = true },
                    new Sector { Name = "Mining", Display = true },
                    new Sector { Name = "Worldwide Web", Display = true },
                    new Sector { Name = "Electronic", Display = true }
                );
            }

        }


        public static void DefaultAccess(DbEntities db)
        {
            //access
            foreach (UserAccess useraccess in Enum.GetValues(typeof(UserAccess)))
            {

                Modules module;

                int access = (int)useraccess;

                if (access >= 0 && access <= 1000)
                {
                    module = Modules.Home;
                }
                else if (access >= 1001 && access <= 2000)
                {
                    module = Modules.Event;
                }
                else if (access >= 2001 && access <= 3000)
                {
                    module = Modules.RnP;
                }
                else if (access >= 3001 && access <= 4000)
                {
                    module = Modules.Learning;
                }
                else if (access >= 4001 && access <= 5000)
                {
                    module = Modules.Admin;
                }
                else if (access >= 5001 && access <= 6000)
                {
                    module = Modules.Setting;
                }
                else //if (access >= 6001 && access <= 7000)
                {
                    module = Modules.Report;
                }
                        
                db.Access.AddOrUpdate(a => a.UserAccess, new Access { UserAccess = useraccess, Module = module, Description = useraccess.DisplayName() });

            }
        }

        public static void AddRole(DbEntities db, string RoleName, string Description)
        {

            var role = db.Role.Local.Where(r => r.Name.Contains(RoleName)).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains(RoleName)).FirstOrDefault();

            if (role == null)
            {
                db.Role.Add(
                    new Role
                    {
                        Name = RoleName,
                        Description = Description,
                        Display = true,
                        CreatedDate = DateTime.Now                       
                    });
            }

        }

    }
}
