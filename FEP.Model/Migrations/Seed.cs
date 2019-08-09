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
    public static class seed
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

        public static void Default(DbEntities db)
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

            //publication category
            if (!db.PublicationCategory.Any())
            {
                db.PublicationCategory.Add(new PublicationCategory { Name = "Articles" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Books" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Facts Sheet" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Journals" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Literature Reviews" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Reports" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Research Papers" });
            }
        }

    }
}
