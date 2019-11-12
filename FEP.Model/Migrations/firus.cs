using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

            //target groups
            /*
            if (!db.TargetedGroups.Any())
            {
                db.TargetedGroups.Add(new TargetedGroups
                {
                    Name = "Group 1",
                    Description = "Male (18-25) in JB",
                    MinAge = 18,
                    MaxAge = 25,
                    Gender = MemberGender.Male,
                    MinSalary = 1500,
                    MaxSalary = 3500,
                    Status = 1,
                    PaymentStatus = 1,
                    Delinquent = 0,
                    EmploymentType = 2,
                    State = 1,
                    City = "Johor Bahru",
                    DateCreated = DateTime.Now,
                    CreatorId = 1,
                    Active = true
                });
                db.TargetedGroups.Add(new TargetedGroups
                {
                    Name = "Group 2",
                    Description = "Any (40-55)",
                    MinAge = 40,
                    MaxAge = 55,
                    Gender = MemberGender.Any,
                    MinSalary = null,
                    MaxSalary = null,
                    Status = null,
                    PaymentStatus = null,
                    Delinquent = null,
                    EmploymentType = null,
                    State = null,
                    City = "",
                    DateCreated = DateTime.Now,
                    CreatorId = 1,
                    Active = true
                });
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
            /*
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
            */

            //publication approval
            /*
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
            */

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



            //DefaultSLAReminder(db);
            //DefaultParameterGroup(db);
            //DefaultTemplate(db);
        }

        public static void DefaultSLAReminder(DbEntities db)
        {
            db.SLAReminder.AddOrUpdate(s => s.NotificationType,

                // survey
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.SubmitSurvey, NotificationType = NotificationType.Submit_Survey_Creation, ETCode = "ET101RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelSurvey, NotificationType = NotificationType.Submit_Survey_Cancellation, ETCode = "ET102RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.PublishSurvey, NotificationType = NotificationType.Submit_Survey_Publication, ETCode = "ET103RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifySurvey, NotificationType = NotificationType.Verify_Survey_Creation, ETCode = "ET111RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_1, ETCode = "ET121RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_2, ETCode = "ET122RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_3, ETCode = "ET123RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_Final, ETCode = "ET124RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.DistributeSurvey, NotificationType = NotificationType.Submit_Survey_Distribution, ETCode = "ET131RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.AnswerSurvey, NotificationType = NotificationType.Submit_Survey_Response, ETCode = "ET132RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                // publication
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.SubmitPublication, NotificationType = NotificationType.Submit_Publication_Creation, ETCode = "ET201RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelPublication, NotificationType = NotificationType.Submit_Publication_Cancellation, ETCode = "ET202RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.PublishPublication, NotificationType = NotificationType.Submit_Publication_Publication, ETCode = "ET203RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification, ETCode = "ET204RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.WithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal, ETCode = "ET205RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification_Cancellation, ETCode = "ET206RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelWithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal_Cancellation, ETCode = "ET207RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublication, NotificationType = NotificationType.Verify_Publication_Creation, ETCode = "ET211RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublicationModification, NotificationType = NotificationType.Verify_Publication_Modification, ETCode = "ET212RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublicationWithdrawal, NotificationType = NotificationType.Verify_Publication_Withdrawal, ETCode = "ET213RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_1, ETCode = "ET221RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_2, ETCode = "ET222RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_3, ETCode = "ET223RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_Final, ETCode = "ET224RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_1, ETCode = "ET225RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_2, ETCode = "ET226RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_3, ETCode = "ET227RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_Final, ETCode = "ET228RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_1, ETCode = "ET229RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_2, ETCode = "ET230RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_3, ETCode = "ET231RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_Final, ETCode = "ET232RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                // refunds (will be superseded by payment below in future)
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.RefundPublication, NotificationType = NotificationType.Submit_Publication_Refund, ETCode = "ET241RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.RefundPublication, NotificationType = NotificationType.Approve_Publication_Refund_Incomplete, ETCode = "ET242RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.RefundPublication, NotificationType = NotificationType.Approve_Publication_Refund_Complete, ETCode = "ET243RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                // payment
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_GL, ETCode = "ET001PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_GL, ETCode = "ET002PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Payment, ETCode = "ET011PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Payment, ETCode = "ET012PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Submit_Refund_Request, ETCode = "ET021PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Refund_Request, ETCode = "ET022PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Approve_Refund_Request, ETCode = "ET023PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Refund, ETCode = "ET024PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Refund_Incomplete, ETCode = "ET025PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Refund_Complete, ETCode = "ET026PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }

            );
        }

        public static void DefaultParameterGroup(DbEntities db)
        {

            foreach (TemplateParameterType paramType in Enum.GetValues(typeof(TemplateParameterType)))
            {
                SLAEventType EventType;

                int pType = (int)paramType;

                if (pType >= 101 && pType <= 120)
                {
                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.SubmitSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.PublishSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifySurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApproveSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.DistributeSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.AnswerSurvey, TemplateParameterType = paramType });

                    continue;

                }

                if (pType >= 121 && pType <= 130)
                {
                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.SubmitPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.PublishPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ModifyPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.WithdrawPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelModifyPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelWithdrawPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationModification, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationWithdrawal, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApprovePublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationModification, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, TemplateParameterType = paramType });

                    continue;

                }

                if (pType >= 131 && pType <= 140)
                {
                    // will be superseded by below
                    db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                    new ParameterGroup { SLAEventType = SLAEventType.RefundPublication, TemplateParameterType = paramType });

                    // other refunds can add here
                    db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                    new ParameterGroup { SLAEventType = SLAEventType.Refund, TemplateParameterType = paramType });

                    continue;
                }
            }


        }

        public static void DefaultTemplate(DbEntities db)
        {

            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            //db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
            //    new NotificationTemplate
            //    {
            //        NotificationType = NotificationType.ResetPassword,
            //        NotificationCategory = NotificationCategory.System,
            //        TemplateName = NotificationType.ResetPassword.DisplayName(),
            //        TemplateRefNo = "T" + ((int)NotificationType.ResetPassword).ToString(),
            //        enableEmail = true,
            //        TemplateSubject = "New FE Portal Account Created",
            //        TemplateMessage = "&lt;p&gt;Dear&amp;nbsp;&lt;span style=&quot;font-size: 1rem;&quot;&gt;[#UserFullName],&lt;/span&gt;&lt;/p&gt;&lt;p&gt;You can activate your account [#Link].&amp;nbsp;&lt;/p&gt;&lt;p&gt;Your login details:&lt;/p&gt;&lt;p&gt;[#LoginDetail]&lt;br&gt;&lt;/p&gt;&lt;p&gt;&lt;span style=&quot;color: rgb(255, 255, 255); font-size: 12px; text-align: center; white-space: nowrap; background-color: rgb(41, 182, 246);&quot;&gt;&lt;br&gt;&lt;/span&gt;&lt;/p&gt;",
            //        enableSMSMessage = false,
            //        SMSMessage = "SMS Message Template",
            //        enableWebMessage = false,
            //        WebMessage = "Web Message Template",
            //        CreatedDate = DateTime.Now,
            //        CreatedBy = user.Id,
            //        User = user,
            //        Display = true
            //    });

        }

    }

}
