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

                db.User.Add(
                    new User
                    {
                        Name = "System Admin",
                        Email = "mhafeez@primuscore.com",                       
                        UserType = UserType.SystemAdmin,                        
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "admin",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,                           
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,                                                      
                        },
                    }
                );

            }

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
                    new Sector { Name = "Aerospace" },
                    new Sector { Name = "Transport" },
                    new Sector { Name = "Computer" },
                    new Sector { Name = "Telecommunication" },
                    new Sector { Name = "Agriculture" },
                    new Sector { Name = "Construction" },
                    new Sector { Name = "Education" },
                    new Sector { Name = "Pharmaceutical" },
                    new Sector { Name = "Food" },
                    new Sector { Name = "HealthCare" },
                    new Sector { Name = "Hospitality" },
                    new Sector { Name = "Entertainment" },
                    new Sector { Name = "News Media" },
                    new Sector { Name = "Energy" },
                    new Sector { Name = "Manufacturing" },
                    new Sector { Name = "Music" },
                    new Sector { Name = "Mining" },
                    new Sector { Name = "Worldwide Web" },
                    new Sector { Name = "Electronic" }
                );
            }

        }

    }
}
