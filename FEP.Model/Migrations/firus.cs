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

            //banks
            if (!db.BankInformation.Any())
            {
                db.BankInformation.Add(new BankInformation { ShortName = "Maybank" });
                db.BankInformation.Add(new BankInformation { ShortName = "CIMB Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Public Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "RHB Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Hong Leong Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Ambank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Affin Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Alliance Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Bank Islam" });
                db.BankInformation.Add(new BankInformation { ShortName = "Bank Muamalat" });
                db.BankInformation.Add(new BankInformation { ShortName = "Bank Rakyat" });
                db.BankInformation.Add(new BankInformation { ShortName = "BSN" });
                db.BankInformation.Add(new BankInformation { ShortName = "HSBC Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Standard Chartered" });
                db.BankInformation.Add(new BankInformation { ShortName = "UOB Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Maybank2E" });
            }

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

            //publication
            if (!db.Publication.Any())
            {
                db.Publication.Add(new Publication
                {
                    CategoryID = 2,
                    Author = "Prof. Aiman",
                    Coauthor = null,
                    Title = "Debt Management 101",
                    Year = 2018,
                    Description = "A book about how to manage your debt effectively",
                    Language = "English",
                    ISBN = "ISBN 0001-0001-01",
                    Hardcopy = true,
                    Digitalcopy = true,
                    HDcopy = true,
                    FreeHCopy = false,
                    FreeDCopy = false,
                    FreeHDCopy = false,
                    HPrice = 39.9F,
                    DPrice = 10,
                    HDPrice = 45.1F,
                    Pictures = null,
                    ProofOfApproval = null,
                    StockBalance = 100,
                    CancelRemark = "",
                    WithdrawalReason = "",
                    WithdrawalDate = null,
                    ProofOfWithdrawal = "",
                    DateAdded = DateTime.Parse("2019-10-13 12:53:17"),
                    CreatorId = 1,
                    RefNo = "PUB/1910/0001",
                    Status = PublicationStatus.Published,
                    DateCancelled = null,
                    ViewCount = 0,
                    PurchaseCount = 0,
                    DmsPath = "",
                    NotificationID = 0
                });
                db.Publication.Add(new Publication
                {
                    CategoryID = 6,
                    Author = "Dr. Masyitah Ghaffar",
                    Coauthor = "Dr. Irwan",
                    Title = "Laporan Kredit 2017",
                    Year = 2018,
                    Description = "Laporan kredit bagi tahun 2017",
                    Language = "Bahasa Malaysia",
                    ISBN = "ISS 0002-0002",
                    Hardcopy = true,
                    Digitalcopy = true,
                    HDcopy = false,
                    FreeHCopy = false,
                    FreeDCopy = true,
                    FreeHDCopy = false,
                    HPrice = 12.8F,
                    DPrice = 0,
                    HDPrice = 0,
                    Pictures = null,
                    ProofOfApproval = null,
                    StockBalance = 50,
                    CancelRemark = "",
                    WithdrawalReason = "",
                    WithdrawalDate = null,
                    ProofOfWithdrawal = "",
                    DateAdded = DateTime.Parse("2019-10-13 14:12:58"),
                    CreatorId = 1,
                    RefNo = "PUB/1910/0002",
                    Status = PublicationStatus.Published,
                    DateCancelled = null,
                    ViewCount = 0,
                    PurchaseCount = 0,
                    DmsPath = "",
                    NotificationID = 0
                });
            }

            //publication approval
            if (!db.PublicationApproval.Any())
            {
                db.PublicationApproval.Add(new PublicationApproval
                {
                    PublicationID = 1,
                    Level = PublicationApprovalLevels.Verifier,
                    ApproverId = 1,
                    Status = PublicationApprovalStatus.Approved,
                    ApprovalDate = DateTime.Parse("2019-10-14 19:08:37"),
                    Remarks = "OK",
                    RequireNext = true
                });
                db.PublicationApproval.Add(new PublicationApproval
                {
                    PublicationID = 1,
                    Level = PublicationApprovalLevels.Approver1,
                    ApproverId = 1,
                    Status = PublicationApprovalStatus.Approved,
                    ApprovalDate = DateTime.Parse("2019-10-15 19:17:15"),
                    Remarks = "OK",
                    RequireNext = false
                });
                db.PublicationApproval.Add(new PublicationApproval
                {
                    PublicationID = 2,
                    Level = PublicationApprovalLevels.Verifier,
                    ApproverId = 1,
                    Status = PublicationApprovalStatus.Approved,
                    ApprovalDate = DateTime.Parse("2019-10-14 19:18:13"),
                    Remarks = "OK",
                    RequireNext = true
                });
                db.PublicationApproval.Add(new PublicationApproval
                {
                    PublicationID = 2,
                    Level = PublicationApprovalLevels.Approver1,
                    ApproverId = 1,
                    Status = PublicationApprovalStatus.Approved,
                    ApprovalDate = DateTime.Parse("2019-10-15 20:24:10"),
                    Remarks = "OK",
                    RequireNext = false
                });
            }

            /*
            //cart
            if (!db.PurchaseOrder.Any())
            {
                db.PurchaseOrder.Add(new PurchaseOrder
                {
                    UserId = 1,
                    DiscountCode = "",
                    ProformaInvoiceNo = "",
                    PaymentMode = PaymentModes.Online,
                    CreatedDate = DateTime.Parse("2019-10-14 13:45:42"),
                    TotalPrice = 0,
                    Status = CheckoutStatus.Paid,
                    ReceiptNo = "PB00301395",
                    PaymentDate = DateTime.Parse("2019-10-15 10:18:15"),
                    DeliveryStatus = DeliveryStatus.Shipped
                });
            }

            //cart items
            if (!db.PurchaseOrderItem.Any())
            {
                db.PurchaseOrderItem.Add(new PurchaseOrderItem
                {
                    PurchaseOrderId = 1,
                    Description = "Debt Management 101 (Digital)",
                    PurchaseType = PurchaseType.Publication,
                    ItemId = 1,
                    Price = 10,
                    Quantity = 1
                });
                db.PurchaseOrderItem.Add(new PurchaseOrderItem
                {
                    PurchaseOrderId = 1,
                    Description = "Debt Management 101 (Hard copy)",
                    PurchaseType = PurchaseType.Publication,
                    ItemId = 1,
                    Price = 39.9F,
                    Quantity = 5
                });
                db.PurchaseOrderItem.Add(new PurchaseOrderItem
                {
                    PurchaseOrderId = 1,
                    Description = "Debt Management 101 (Promotion 1+1)",
                    PurchaseType = PurchaseType.Publication,
                    ItemId = 1,
                    Price = 45.1F,
                    Quantity = 1
                });
                db.PurchaseOrderItem.Add(new PurchaseOrderItem
                {
                    PurchaseOrderId = 1,
                    Description = "Laporan Kredit 2017 (Hard copy)",
                    PurchaseType = PurchaseType.Publication,
                    ItemId = 2,
                    Price = 12.8F,
                    Quantity = 3
                });
            }
            */
        }

    }
}
