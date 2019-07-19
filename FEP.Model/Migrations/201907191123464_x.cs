namespace FEP.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseAdministrator",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginId = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        UserType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAccount",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        LoginId = c.String(),
                        HashPassword = c.String(),
                        Salt = c.String(),
                        IsEnable = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        LastPasswordChange = c.DateTime(),
                        LoginAttempt = c.Int(nullable: false),
                        ValidFrom = c.DateTime(),
                        ValidTo = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LearningCourse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        URL = c.String(),
                        Objective = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        DurationtoComplete = c.Int(nullable: false),
                        IntroVideoPath = c.String(),
                        LevelRequired = c.Int(nullable: false),
                        IsHide = c.Boolean(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        Price = c.Single(nullable: false),
                        CertificateId = c.Int(),
                        LastUpdate = c.DateTime(),
                        ApprovalId1 = c.Int(),
                        ApprovalId2 = c.Int(),
                        ApprovalId3 = c.Int(),
                        ApprovalId4 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId1)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId2)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId3)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId4)
                .ForeignKey("dbo.LearningCourseCategory", t => t.CategoryId)
                .ForeignKey("dbo.LearningCourseCertificates", t => t.CertificateId)
                .Index(t => t.CategoryId)
                .Index(t => t.CertificateId)
                .Index(t => t.ApprovalId1)
                .Index(t => t.ApprovalId2)
                .Index(t => t.ApprovalId3)
                .Index(t => t.ApprovalId4);
            
            CreateTable(
                "dbo.CourseApproval",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApproverId = c.Int(nullable: false),
                        ApprovedDate = c.DateTime(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LearningCourseCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LearningCourseCertificates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BackgroundImage = c.String(),
                        Template = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseContent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Title = c.String(),
                        CompletionType = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        QuestionType = c.Int(),
                        Timer = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseModule", t => t.ModuleId)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.CourseModule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        FileId = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .ForeignKey("dbo.File", t => t.FileId)
                .Index(t => t.CourseId)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        FileSize = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        FileTag = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseInstructor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseLearner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        EnrollDate = c.DateTime(),
                        CompleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseWithdraw",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        Reason = c.String(),
                        ApprovalId1 = c.Int(),
                        ApprovalId2 = c.Int(),
                        ApprovalId3 = c.Int(),
                        ApprovalId4 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId1)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId2)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId3)
                .ForeignKey("dbo.CourseApproval", t => t.ApprovalId4)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.ApprovalId1)
                .Index(t => t.ApprovalId2)
                .Index(t => t.ApprovalId3)
                .Index(t => t.ApprovalId4);
            
            CreateTable(
                "dbo.GamificationBadge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Title = c.String(),
                        Value = c.Int(nullable: false),
                        IconPath = c.String(),
                        IsEnable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GamificationLevel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        IsEnable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GamificationPoint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        IsEnable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Learner",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Point = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LearnerBadge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BadgeType = c.Int(nullable: false),
                        AchieveDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LearningDiscussions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Topic = c.String(),
                        ViewCategory = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LatestReply = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.LearningDiscussionAttachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplyId = c.Int(nullable: false),
                        AttachmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.File", t => t.AttachmentId)
                .ForeignKey("dbo.LearningDiscussionReplies", t => t.ReplyId)
                .Index(t => t.ReplyId)
                .Index(t => t.AttachmentId);
            
            CreateTable(
                "dbo.LearningDiscussionReplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        Message = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpvoteNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningDiscussions", t => t.TopicId)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.TopicId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.LearningDiscussionComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplyId = c.Int(nullable: false),
                        Message = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningDiscussionReplies", t => t.ReplyId)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.ReplyId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.LearningDiscussionReplyUpvotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplyId = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningDiscussionReplies", t => t.ReplyId)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.ReplyId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.LearningDiscussionViews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        GroupId = c.Int(),
                        CourseId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .ForeignKey("dbo.LearningGroups", t => t.GroupId)
                .ForeignKey("dbo.LearningDiscussions", t => t.TopicId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.TopicId)
                .Index(t => t.GroupId)
                .Index(t => t.CourseId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LearningGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SharedKey = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LearningEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        ViewCategory = c.Int(nullable: false),
                        ColorCode = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LearningEventAudiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LearningEventId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        GroupId = c.Int(),
                        CourseId = c.Int(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningCourse", t => t.CourseId)
                .ForeignKey("dbo.LearningEvents", t => t.LearningEventId)
                .ForeignKey("dbo.LearningGroups", t => t.GroupId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.LearningEventId)
                .Index(t => t.GroupId)
                .Index(t => t.CourseId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LearningGroupMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearningGroups", t => t.GroupId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ProformaInvoiceNo = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        TotalPrice = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseOrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        Description = c.String(),
                        PurchaseType = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId)
                .Index(t => t.PurchaseOrderId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        RoleDescription = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserAccess = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleAccesses", "RoleId", "dbo.Role");
            DropForeignKey("dbo.PurchaseOrderItems", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.LearningGroupMembers", "UserId", "dbo.User");
            DropForeignKey("dbo.LearningGroupMembers", "GroupId", "dbo.LearningGroups");
            DropForeignKey("dbo.LearningEventAudiences", "UserId", "dbo.User");
            DropForeignKey("dbo.LearningEventAudiences", "GroupId", "dbo.LearningGroups");
            DropForeignKey("dbo.LearningEventAudiences", "LearningEventId", "dbo.LearningEvents");
            DropForeignKey("dbo.LearningEventAudiences", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.LearningDiscussionViews", "UserId", "dbo.User");
            DropForeignKey("dbo.LearningDiscussionViews", "TopicId", "dbo.LearningDiscussions");
            DropForeignKey("dbo.LearningDiscussionViews", "GroupId", "dbo.LearningGroups");
            DropForeignKey("dbo.LearningDiscussionViews", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.LearningDiscussionReplyUpvotes", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.LearningDiscussionReplyUpvotes", "ReplyId", "dbo.LearningDiscussionReplies");
            DropForeignKey("dbo.LearningDiscussionComments", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.LearningDiscussionComments", "ReplyId", "dbo.LearningDiscussionReplies");
            DropForeignKey("dbo.LearningDiscussionAttachments", "ReplyId", "dbo.LearningDiscussionReplies");
            DropForeignKey("dbo.LearningDiscussionReplies", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.LearningDiscussionReplies", "TopicId", "dbo.LearningDiscussions");
            DropForeignKey("dbo.LearningDiscussionAttachments", "AttachmentId", "dbo.File");
            DropForeignKey("dbo.LearningDiscussions", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.LearnerBadge", "UserId", "dbo.User");
            DropForeignKey("dbo.Learner", "UserId", "dbo.User");
            DropForeignKey("dbo.CourseWithdraw", "UserId", "dbo.User");
            DropForeignKey("dbo.CourseWithdraw", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.CourseWithdraw", "ApprovalId4", "dbo.CourseApproval");
            DropForeignKey("dbo.CourseWithdraw", "ApprovalId3", "dbo.CourseApproval");
            DropForeignKey("dbo.CourseWithdraw", "ApprovalId2", "dbo.CourseApproval");
            DropForeignKey("dbo.CourseWithdraw", "ApprovalId1", "dbo.CourseApproval");
            DropForeignKey("dbo.CourseLearner", "UserId", "dbo.User");
            DropForeignKey("dbo.CourseLearner", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.CourseInstructor", "UserId", "dbo.User");
            DropForeignKey("dbo.CourseInstructor", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.CourseFile", "FileId", "dbo.File");
            DropForeignKey("dbo.CourseFile", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.CourseContent", "ModuleId", "dbo.CourseModule");
            DropForeignKey("dbo.CourseModule", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.CourseAdministrator", "CourseId", "dbo.LearningCourse");
            DropForeignKey("dbo.LearningCourse", "CertificateId", "dbo.LearningCourseCertificates");
            DropForeignKey("dbo.LearningCourse", "CategoryId", "dbo.LearningCourseCategory");
            DropForeignKey("dbo.LearningCourse", "ApprovalId4", "dbo.CourseApproval");
            DropForeignKey("dbo.LearningCourse", "ApprovalId3", "dbo.CourseApproval");
            DropForeignKey("dbo.LearningCourse", "ApprovalId2", "dbo.CourseApproval");
            DropForeignKey("dbo.LearningCourse", "ApprovalId1", "dbo.CourseApproval");
            DropForeignKey("dbo.CourseAdministrator", "UserId", "dbo.User");
            DropForeignKey("dbo.UserAccount", "UserId", "dbo.User");
            DropIndex("dbo.RoleAccesses", new[] { "RoleId" });
            DropIndex("dbo.PurchaseOrderItems", new[] { "PurchaseOrderId" });
            DropIndex("dbo.LearningGroupMembers", new[] { "GroupId" });
            DropIndex("dbo.LearningGroupMembers", new[] { "UserId" });
            DropIndex("dbo.LearningEventAudiences", new[] { "UserId" });
            DropIndex("dbo.LearningEventAudiences", new[] { "CourseId" });
            DropIndex("dbo.LearningEventAudiences", new[] { "GroupId" });
            DropIndex("dbo.LearningEventAudiences", new[] { "LearningEventId" });
            DropIndex("dbo.LearningDiscussionViews", new[] { "UserId" });
            DropIndex("dbo.LearningDiscussionViews", new[] { "CourseId" });
            DropIndex("dbo.LearningDiscussionViews", new[] { "GroupId" });
            DropIndex("dbo.LearningDiscussionViews", new[] { "TopicId" });
            DropIndex("dbo.LearningDiscussionReplyUpvotes", new[] { "CreatedBy" });
            DropIndex("dbo.LearningDiscussionReplyUpvotes", new[] { "ReplyId" });
            DropIndex("dbo.LearningDiscussionComments", new[] { "CreatedBy" });
            DropIndex("dbo.LearningDiscussionComments", new[] { "ReplyId" });
            DropIndex("dbo.LearningDiscussionReplies", new[] { "CreatedBy" });
            DropIndex("dbo.LearningDiscussionReplies", new[] { "TopicId" });
            DropIndex("dbo.LearningDiscussionAttachments", new[] { "AttachmentId" });
            DropIndex("dbo.LearningDiscussionAttachments", new[] { "ReplyId" });
            DropIndex("dbo.LearningDiscussions", new[] { "CreatedBy" });
            DropIndex("dbo.LearnerBadge", new[] { "UserId" });
            DropIndex("dbo.Learner", new[] { "UserId" });
            DropIndex("dbo.CourseWithdraw", new[] { "ApprovalId4" });
            DropIndex("dbo.CourseWithdraw", new[] { "ApprovalId3" });
            DropIndex("dbo.CourseWithdraw", new[] { "ApprovalId2" });
            DropIndex("dbo.CourseWithdraw", new[] { "ApprovalId1" });
            DropIndex("dbo.CourseWithdraw", new[] { "CourseId" });
            DropIndex("dbo.CourseWithdraw", new[] { "UserId" });
            DropIndex("dbo.CourseLearner", new[] { "CourseId" });
            DropIndex("dbo.CourseLearner", new[] { "UserId" });
            DropIndex("dbo.CourseInstructor", new[] { "CourseId" });
            DropIndex("dbo.CourseInstructor", new[] { "UserId" });
            DropIndex("dbo.CourseFile", new[] { "FileId" });
            DropIndex("dbo.CourseFile", new[] { "CourseId" });
            DropIndex("dbo.CourseModule", new[] { "CourseId" });
            DropIndex("dbo.CourseContent", new[] { "ModuleId" });
            DropIndex("dbo.LearningCourse", new[] { "ApprovalId4" });
            DropIndex("dbo.LearningCourse", new[] { "ApprovalId3" });
            DropIndex("dbo.LearningCourse", new[] { "ApprovalId2" });
            DropIndex("dbo.LearningCourse", new[] { "ApprovalId1" });
            DropIndex("dbo.LearningCourse", new[] { "CertificateId" });
            DropIndex("dbo.LearningCourse", new[] { "CategoryId" });
            DropIndex("dbo.UserAccount", new[] { "UserId" });
            DropIndex("dbo.CourseAdministrator", new[] { "CourseId" });
            DropIndex("dbo.CourseAdministrator", new[] { "UserId" });
            DropTable("dbo.RoleAccesses");
            DropTable("dbo.Role");
            DropTable("dbo.PurchaseOrderItems");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.LearningGroupMembers");
            DropTable("dbo.LearningEventAudiences");
            DropTable("dbo.LearningEvents");
            DropTable("dbo.LearningGroups");
            DropTable("dbo.LearningDiscussionViews");
            DropTable("dbo.LearningDiscussionReplyUpvotes");
            DropTable("dbo.LearningDiscussionComments");
            DropTable("dbo.LearningDiscussionReplies");
            DropTable("dbo.LearningDiscussionAttachments");
            DropTable("dbo.LearningDiscussions");
            DropTable("dbo.LearnerBadge");
            DropTable("dbo.Learner");
            DropTable("dbo.GamificationPoint");
            DropTable("dbo.GamificationLevel");
            DropTable("dbo.GamificationBadge");
            DropTable("dbo.CourseWithdraw");
            DropTable("dbo.CourseLearner");
            DropTable("dbo.CourseInstructor");
            DropTable("dbo.File");
            DropTable("dbo.CourseFile");
            DropTable("dbo.CourseModule");
            DropTable("dbo.CourseContent");
            DropTable("dbo.LearningCourseCertificates");
            DropTable("dbo.LearningCourseCategory");
            DropTable("dbo.CourseApproval");
            DropTable("dbo.LearningCourse");
            DropTable("dbo.UserAccount");
            DropTable("dbo.User");
            DropTable("dbo.CourseAdministrator");
        }
    }
}
