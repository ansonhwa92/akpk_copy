using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.Migrations
{
	public static class firus
	{
		public static void Seed(DbEntities db)
		{
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

			//rnp verifier
			//var user1 = db.User.Local.Where(r => r.Name.Contains("R&P Verifier")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Verifier")).FirstOrDefault();

			//if (user1 == null)
			//{

			//        //var role1 = db.Role.Local.Where(r => r.Name.Contains("Verifier R&P")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("Verifier R&P")).FirstOrDefault();

			//        //List<UserRole> userroles = new List<UserRole>();

			//        //userroles.Add(new UserRole { Role = role1 });

			//        db.User.Add(
			//            new User
			//            {
			//                Name = "R&P Verifier",
			//                Email = "rnpverifier@fep.com",
			//                UserType = UserType.Staff,
			//                CreatedDate = DateTime.Now,
			//                Display = true,
			//                UserAccount = new UserAccount
			//                {
			//                    LoginId = "rnpverifier@fep.com",
			//                    HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
			//                    Salt = "/ZCqmg==",
			//                    IsEnable = true,
			//                    LoginAttempt = 0,
			//                    LastPasswordChange = DateTime.Now,
			//                    LastLogin = DateTime.Now,
			//                    //UserRoles = userroles
			//                },
			//            }
			//        );

			//    }

			//    //rnp approver 1
			//    var user2 = db.User.Local.Where(r => r.Name.Contains("R&P Approver 1")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Approver 1")).FirstOrDefault();

			//    if (user2 == null)
			//    {

			//        //var role2 = db.Role.Local.Where(r => r.Name.Contains("Approver R&P 1")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("Approver R&P 1")).FirstOrDefault();

			//        //List<UserRole> userroles = new List<UserRole>();

			//        //userroles.Add(new UserRole { Role = role2 });

			//        db.User.Add(
			//            new User
			//            {
			//                Name = "R&P Approver 1",
			//                Email = "rnpapprover1@fep.com",
			//                UserType = UserType.Staff,
			//                CreatedDate = DateTime.Now,
			//                Display = true,
			//                UserAccount = new UserAccount
			//                {
			//                    LoginId = "rnpapprover1@fep.com",
			//                    HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
			//                    Salt = "/ZCqmg==",
			//                    IsEnable = true,
			//                    LoginAttempt = 0,
			//                    LastPasswordChange = DateTime.Now,
			//                    LastLogin = DateTime.Now,
			//                    //UserRoles = userroles
			//                },
			//            }
			//        );

			//    }

			//    //rnp approver 2
			//    var user3 = db.User.Local.Where(r => r.Name.Contains("R&P Approver 2")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Approver 2")).FirstOrDefault();

			//    if (user3 == null)
			//    {

			//        //var role3 = db.Role.Local.Where(r => r.Name.Contains("Approver R&P 2")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("Approver R&P 2")).FirstOrDefault();

			//        //List<UserRole> userroles = new List<UserRole>();

			//        //userroles.Add(new UserRole { Role = role3 });

			//        db.User.Add(
			//            new User
			//            {
			//                Name = "R&P Approver 2",
			//                Email = "rnpapprover2@fep.com",
			//                UserType = UserType.Staff,
			//                CreatedDate = DateTime.Now,
			//                Display = true,
			//                UserAccount = new UserAccount
			//                {
			//                    LoginId = "rnpapprover2@fep.com",
			//                    HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
			//                    Salt = "/ZCqmg==",
			//                    IsEnable = true,
			//                    LoginAttempt = 0,
			//                    LastPasswordChange = DateTime.Now,
			//                    LastLogin = DateTime.Now,
			//                    //UserRoles = userroles
			//                },
			//            }
			//        );

			//    }

			//    //rnp approver 3
			//    var user4 = db.User.Local.Where(r => r.Name.Contains("R&P Approver 3")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Approver 3")).FirstOrDefault();

			//    if (user4 == null)
			//    {

			//        //var role4 = db.Role.Local.Where(r => r.Name.Contains("Approver R&P 3")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("Approver R&P 3")).FirstOrDefault();

			//        //List<UserRole> userroles = new List<UserRole>();

			//        //userroles.Add(new UserRole { Role = role4 });

			//        db.User.Add(
			//            new User
			//            {
			//                Name = "R&P Approver 3",
			//                Email = "rnpapprover3@fep.com",
			//                UserType = UserType.Staff,
			//                CreatedDate = DateTime.Now,
			//                Display = true,
			//                UserAccount = new UserAccount
			//                {
			//                    LoginId = "rnpapprover3@fep.com",
			//                    HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
			//                    Salt = "/ZCqmg==",
			//                    IsEnable = true,
			//                    LoginAttempt = 0,
			//                    LastPasswordChange = DateTime.Now,
			//                    LastLogin = DateTime.Now,
			//                    //UserRoles = userroles
			//                },
			//            }
			//        );

			//    }

		}

	}
}
