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

            /*
            // rnp admin

            var user0 = db.User.Local.Where(r => r.Name.Contains("R&P Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Admin")).FirstOrDefault();

            if (user0 == null)
            {

                db.User.Add(
                    new User
                    {
                        Name = "R&P Admin",
                        Email = "adminrnp@yahoo.com",
                        UserType = UserType.Staff,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "adminrnp@yahoo.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,
                            //UserRoles = userroles
                        },
                    }
                );

            }
            */

            /*
            // rnp verifier

            var user1 = db.User.Local.Where(r => r.Name.Contains("R&P Verifier")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Verifier")).FirstOrDefault();

            if (user1 == null)
            {

                db.User.Add(
                    new User
                    {
                        Name = "R&P Verifier",
                        Email = "verifierrnp@yahoo.com",
                        UserType = UserType.Staff,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "verifierrnp@yahoo.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,
                            //UserRoles = userroles
                            },
                    }
                );

            }
            */

            /*
            //rnp approver 1

            var user2 = db.User.Local.Where(r => r.Name.Contains("R&P Approver 1")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Approver 1")).FirstOrDefault();

            if (user2 == null)
            {

                db.User.Add(
                    new User
                    {
                        Name = "R&P Approver 1",
                        Email = "ApproverRNP1@fep.com",
                        UserType = UserType.Staff,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "ApproverRNP1@fep.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,
                            //UserRoles = userroles
                            },
                    }
                );

            }

            //rnp approver 2

            var user3 = db.User.Local.Where(r => r.Name.Contains("R&P Approver 2")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Approver 2")).FirstOrDefault();

            if (user3 == null)
            {

                db.User.Add(
                    new User
                    {
                        Name = "R&P Approver 2",
                        Email = "ApproverRNP2@fep.com",
                        UserType = UserType.Staff,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "ApproverRNP2@fep.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,
                            //UserRoles = userroles
                            },
                    }
                );

            }

            //rnp approver 3

            var user4 = db.User.Local.Where(r => r.Name.Contains("R&P Approver 3")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("R&P Approver 3")).FirstOrDefault();

            if (user4 == null)
            {

                db.User.Add(
                    new User
                    {
                        Name = "R&P Approver 3",
                        Email = "ApproverRNP3@fep.com",
                        UserType = UserType.Staff,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "ApproverRNP3@fep.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,
                            //UserRoles = userroles
                            },
                    }
                );

            }
            */

            //promotion codes
            if (!db.PromotionCode.Any())
            {
                db.PromotionCode.Add(new PromotionCode {
                    Code = "MFM1234",
                    MoneyValue = 10,
                    PercentageValue = 0,
                    ExpiryDate = DateTime.Parse("31-12-2019 23:59:59"),
                    Used = false
                });
                db.PromotionCode.Add(new PromotionCode
                {
                    Code = "MFM5678",
                    MoneyValue = 20,
                    PercentageValue = 0,
                    ExpiryDate = DateTime.Parse("30-09-2019 23:59:59"),
                    Used = false
                });
            }

        }

    }
}
