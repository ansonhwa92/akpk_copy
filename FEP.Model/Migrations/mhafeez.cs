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
            //if (!db.NotificationSetting.Any())
            //{
            //    foreach (NotificationType type in (NotificationType[])Enum.GetValues(typeof(NotificationType)))
            //    {
            //        db.NotificationSetting.AddOrUpdate(
            //            t => new { t.NotificationType },
            //            new NotificationSetting { NotificationType = type }
            //        );
            //    }
            //}
            
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
            /*else
            {
                var role = db.Role.Local.Where(r => r.Name.Contains("All Access")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("All Access")).FirstOrDefault();

                List<UserRole> userroles = new List<UserRole>();

                userroles.Add(new UserRole { Role = role });
                db.User.Add(
                    new User
                    {
                        Name = "Tajul Admin",
                        Email = "tajulzaid@gmail.com",
                        UserType = UserType.SystemAdmin,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "tajulzaid@gmail.com",
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
            }*/

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
                db.State.AddOrUpdate(s => s.Name,
                    new State { Name = "Johor", Display = true },
                    new State { Name = "Kedah", Display = true },
                    new State { Name = "Kelantan", Display = true },
                    new State { Name = "Melaka", Display = true },
                    new State { Name = "Negeri Sembilan", Display = true },
                    new State { Name = "Pahang", Display = true },
                    new State { Name = "Pulau Pinang", Display = true },
                    new State { Name = "Perak", Display = true },
                    new State { Name = "Perlis", Display = true },
                    new State { Name = "Selangor", Display = true },
                    new State { Name = "Terengganu", Display = true },
                    new State { Name = "Sabah", Display = true },
                    new State { Name = "Sarawak", Display = true },
                    new State { Name = "Wilayah Persekutuan Kuala Lumpur", Display = true },
                    new State { Name = "Wilayah Persekutuan Labuan", Display = true },
                    new State { Name = "Wilayah Persekutuan Putrajaya", Display = true }
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

            if (!db.Country.Any())
            {
                db.Country.AddOrUpdate(s => s.Name,
                    new Country { Name = "Afghanistan", Display = true },
                    new Country { Name = "Albania", Display = true },
                    new Country { Name = "Algeria", Display = true },
                    new Country { Name = "Andorra", Display = true },
                    new Country { Name = "Angola", Display = true },
                    new Country { Name = "Anguilla", Display = true },
                    new Country { Name = "Antigua & Barbuda", Display = true },
                    new Country { Name = "Argentina", Display = true },
                    new Country { Name = "Armenia", Display = true },
                    new Country { Name = "Australia", Display = true },
                    new Country { Name = "Austria", Display = true },
                    new Country { Name = "Azerbaijan", Display = true },
                    new Country { Name = "Bahamas", Display = true },
                    new Country { Name = "Bahrain", Display = true },
                    new Country { Name = "Bangladesh", Display = true },
                    new Country { Name = "Barbados", Display = true },
                    new Country { Name = "Belarus", Display = true },
                    new Country { Name = "Belgium", Display = true },
                    new Country { Name = "Belize", Display = true },
                    new Country { Name = "Benin", Display = true },
                    new Country { Name = "Bermuda", Display = true },
                    new Country { Name = "Bhutan", Display = true },
                    new Country { Name = "Bolivia", Display = true },
                    new Country { Name = "Bosnia & Herzegovina", Display = true },
                    new Country { Name = "Botswana", Display = true },
                    new Country { Name = "Brazil", Display = true },
                    new Country { Name = "Brunei Darussalam", Display = true },
                    new Country { Name = "Bulgaria", Display = true },
                    new Country { Name = "Burkina Faso", Display = true },
                    new Country { Name = "Burundi", Display = true },
                    new Country { Name = "Cambodia", Display = true },
                    new Country { Name = "Cameroon", Display = true },
                    new Country { Name = "Canada", Display = true },
                    new Country { Name = "Cape Verde", Display = true },
                    new Country { Name = "Cayman Islands", Display = true },
                    new Country { Name = "Central African Republic", Display = true },
                    new Country { Name = "Chad", Display = true },
                    new Country { Name = "Chile", Display = true },
                    new Country { Name = "China", Display = true },
                    new Country { Name = "China - Hong Kong / Macau", Display = true },
                    new Country { Name = "Colombia", Display = true },
                    new Country { Name = "Comoros", Display = true },
                    new Country { Name = "Congo", Display = true },
                    new Country { Name = "Congo, Democratic Republic of (DRC)", Display = true },
                    new Country { Name = "Costa Rica", Display = true },
                    new Country { Name = "Croatia", Display = true },
                    new Country { Name = "Cuba", Display = true },
                    new Country { Name = "Cyprus", Display = true },
                    new Country { Name = "Czech Republic", Display = true },
                    new Country { Name = "Denmark", Display = true },
                    new Country { Name = "Djibouti", Display = true },
                    new Country { Name = "Dominica", Display = true },
                    new Country { Name = "Dominican Republic", Display = true },
                    new Country { Name = "Ecuador", Display = true },
                    new Country { Name = "Egypt", Display = true },
                    new Country { Name = "El Salvador", Display = true },
                    new Country { Name = "Equatorial Guinea", Display = true },
                    new Country { Name = "Eritrea", Display = true },
                    new Country { Name = "Estonia", Display = true },
                    new Country { Name = "Eswatini", Display = true },
                    new Country { Name = "Ethiopia", Display = true },
                    new Country { Name = "Fiji", Display = true },
                    new Country { Name = "Finland", Display = true },
                    new Country { Name = "France", Display = true },
                    new Country { Name = "French Guiana", Display = true },
                    new Country { Name = "Gabon", Display = true },
                    new Country { Name = "Gambia, Republic of The", Display = true },
                    new Country { Name = "Georgia", Display = true },
                    new Country { Name = "Germany", Display = true },
                    new Country { Name = "Ghana", Display = true },
                    new Country { Name = "Great Britain", Display = true },
                    new Country { Name = "Greece", Display = true },
                    new Country { Name = "Grenada", Display = true },
                    new Country { Name = "Guadeloupe", Display = true },
                    new Country { Name = "Guatemala", Display = true },
                    new Country { Name = "Guinea", Display = true },
                    new Country { Name = "Guinea-Bissau", Display = true },
                    new Country { Name = "Guyana", Display = true },
                    new Country { Name = "Haiti", Display = true },
                    new Country { Name = "Honduras", Display = true },
                    new Country { Name = "Hungary", Display = true },
                    new Country { Name = "Iceland", Display = true },
                    new Country { Name = "India", Display = true },
                    new Country { Name = "Indonesia", Display = true },
                    new Country { Name = "Iran", Display = true },
                    new Country { Name = "Iraq", Display = true },
                    new Country { Name = "Israel and the Occupied Territories", Display = true },
                    new Country { Name = "Italy", Display = true },
                    new Country { Name = "Ivory Coast (Cote d'Ivoire)", Display = true },
                    new Country { Name = "Jamaica", Display = true },
                    new Country { Name = "Japan", Display = true },
                    new Country { Name = "Jordan", Display = true },
                    new Country { Name = "Kazakhstan", Display = true },
                    new Country { Name = "Kenya", Display = true },
                    new Country { Name = "Korea, Democratic Republic of (North Korea)", Display = true },
                    new Country { Name = "Korea, Republic of (South Korea)", Display = true },
                    new Country { Name = "Kosovo", Display = true },
                    new Country { Name = "Kuwait", Display = true },
                    new Country { Name = "Kyrgyz Republic (Kyrgyzstan)", Display = true },
                    new Country { Name = "Laos", Display = true },
                    new Country { Name = "Latvia", Display = true },
                    new Country { Name = "Lebanon", Display = true },
                    new Country { Name = "Lesotho", Display = true },
                    new Country { Name = "Liberia", Display = true },
                    new Country { Name = "Libya", Display = true },
                    new Country { Name = "Liechtenstein", Display = true },
                    new Country { Name = "Lithuania", Display = true },
                    new Country { Name = "Luxembourg", Display = true },
                    new Country { Name = "Madagascar", Display = true },
                    new Country { Name = "Malawi", Display = true },
                    new Country { Name = "Malaysia", Display = true },
                    new Country { Name = "Maldives", Display = true },
                    new Country { Name = "Mali", Display = true },
                    new Country { Name = "Malta", Display = true },
                    new Country { Name = "Martinique", Display = true },
                    new Country { Name = "Mauritania", Display = true },
                    new Country { Name = "Mauritius", Display = true },
                    new Country { Name = "Mayotte", Display = true },
                    new Country { Name = "Mexico", Display = true },
                    new Country { Name = "Moldova, Republic of", Display = true },
                    new Country { Name = "Monaco", Display = true },
                    new Country { Name = "Mongolia", Display = true },
                    new Country { Name = "Montenegro", Display = true },
                    new Country { Name = "Montserrat", Display = true },
                    new Country { Name = "Morocco", Display = true },
                    new Country { Name = "Mozambique", Display = true },
                    new Country { Name = "Myanmar/Burma", Display = true },
                    new Country { Name = "Namibia", Display = true },
                    new Country { Name = "Nepal", Display = true },
                    new Country { Name = "New Zealand", Display = true },
                    new Country { Name = "Nicaragua", Display = true },
                    new Country { Name = "Niger", Display = true },
                    new Country { Name = "Nigeria", Display = true },
                    new Country { Name = "North Macedonia, Republic of", Display = true },
                    new Country { Name = "Norway", Display = true },
                    new Country { Name = "Oman", Display = true },
                    new Country { Name = "Pacific Islands", Display = true },
                    new Country { Name = "Pakistan", Display = true },
                    new Country { Name = "Panama", Display = true },
                    new Country { Name = "Papua New Guinea", Display = true },
                    new Country { Name = "Paraguay", Display = true },
                    new Country { Name = "Peru", Display = true },
                    new Country { Name = "Philippines", Display = true },
                    new Country { Name = "Poland", Display = true },
                    new Country { Name = "Portugal", Display = true },
                    new Country { Name = "Puerto Rico", Display = true },
                    new Country { Name = "Qatar", Display = true },
                    new Country { Name = "Reunion", Display = true },
                    new Country { Name = "Romania", Display = true },
                    new Country { Name = "Russian Federation", Display = true },
                    new Country { Name = "Rwanda", Display = true },
                    new Country { Name = "Saint Kitts and Nevis", Display = true },
                    new Country { Name = "Saint Lucia", Display = true },
                    new Country { Name = "Saint Vincent and the Grenadines", Display = true },
                    new Country { Name = "Samoa", Display = true },
                    new Country { Name = "Sao Tome and Principe", Display = true },
                    new Country { Name = "Saudi Arabia", Display = true },
                    new Country { Name = "Senegal", Display = true },
                    new Country { Name = "Serbia", Display = true },
                    new Country { Name = "Seychelles", Display = true },
                    new Country { Name = "Sierra Leone", Display = true },
                    new Country { Name = "Singapore", Display = true },
                    new Country { Name = "Slovak Republic (Slovakia)", Display = true },
                    new Country { Name = "Slovenia", Display = true },
                    new Country { Name = "Solomon Islands", Display = true },
                    new Country { Name = "Somalia", Display = true },
                    new Country { Name = "South Africa", Display = true },
                    new Country { Name = "South Sudan", Display = true },
                    new Country { Name = "Spain", Display = true },
                    new Country { Name = "Sri Lanka", Display = true },
                    new Country { Name = "Sudan", Display = true },
                    new Country { Name = "Suriname", Display = true },
                    new Country { Name = "Sweden", Display = true },
                    new Country { Name = "Switzerland", Display = true },
                    new Country { Name = "Syria", Display = true },
                    new Country { Name = "Tajikistan", Display = true },
                    new Country { Name = "Tanzania", Display = true },
                    new Country { Name = "Thailand", Display = true },
                    new Country { Name = "Netherlands", Display = true },
                    new Country { Name = "Timor Leste", Display = true },
                    new Country { Name = "Togo", Display = true },
                    new Country { Name = "Trinidad & Tobago", Display = true },
                    new Country { Name = "Tunisia", Display = true },
                    new Country { Name = "Turkey", Display = true },
                    new Country { Name = "Turkmenistan", Display = true },
                    new Country { Name = "Turks & Caicos Islands", Display = true },
                    new Country { Name = "Uganda", Display = true },
                    new Country { Name = "Ukraine", Display = true },
                    new Country { Name = "United Arab Emirates", Display = true },
                    new Country { Name = "United States of America (USA)", Display = true },
                    new Country { Name = "Uruguay", Display = true },
                    new Country { Name = "Uzbekistan", Display = true },
                    new Country { Name = "Venezuela", Display = true },
                    new Country { Name = "Vietnam", Display = true },
                    new Country { Name = "Virgin Islands (UK)", Display = true },
                    new Country { Name = "Virgin Islands (US)", Display = true },
                    new Country { Name = "Yemen", Display = true },
                    new Country { Name = "Zambia", Display = true },
                    new Country { Name = "Zimbabwe", Display = true }
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
