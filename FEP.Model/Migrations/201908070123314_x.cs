namespace FEP.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RoleAccesses", newName: "RoleAccess");
            DropForeignKey("dbo.CourseFile", "FileId", "dbo.File");
            DropForeignKey("dbo.Speaker", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.Speaker", "EventId", "dbo.PublicEvent");
            DropForeignKey("dbo.Speaker", "UserId", "dbo.User");
            DropIndex("dbo.Speaker", new[] { "EventId" });
            DropIndex("dbo.Speaker", new[] { "UserId" });
            DropIndex("dbo.Speaker", new[] { "CreatedBy" });
            CreateTable(
                "dbo.AccountSetting",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsPasswordExpiry = c.Boolean(nullable: false),
                        IsLimitLoginAttempt = c.Boolean(nullable: false),
                        PasswordExpiryDuration = c.Int(),
                        LoginAttemptLimit = c.Int(),
                        InactiveDuration = c.Int(nullable: false),
                        IsContainLowerCase = c.Boolean(nullable: false),
                        IsContainUpperCase = c.Boolean(nullable: false),
                        IsContainNumeric = c.Boolean(nullable: false),
                        IsContainSymbol = c.Boolean(nullable: false),
                        IsLengthLimit = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SecretKey = c.String(),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        FileSize = c.Int(nullable: false),
                        FileType = c.String(),
                        FileTag = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.EmailToSend",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        SendDate = c.DateTime(nullable: false),
                        IsSent = c.Boolean(nullable: false),
                        SentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailToSendAddress",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmailToSendId = c.Long(nullable: false),
                        IsCC = c.Boolean(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailToSend", t => t.EmailToSendId)
                .Index(t => t.EmailToSendId);
            
            CreateTable(
                "dbo.EventExternalExhibitor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.EventObjective",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ObjectiveTitle = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.EventSpeaker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpeakerName = c.String(),
                        Remark = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NotificationType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Message = c.String(),
                        Link = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.NotificationSetting",
                c => new
                    {
                        NotificationType = c.Int(nullable: false),
                        IsSendEmail = c.Boolean(nullable: false),
                        IsSendNotification = c.Boolean(nullable: false),
                        NotificationMessage = c.String(),
                        EmailSubject = c.String(),
                        EmailMessage = c.String(),
                    })
                .PrimaryKey(t => t.NotificationType);
            
            CreateTable(
                "dbo.NotificationToSend",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        Link = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        SendDate = c.DateTime(nullable: false),
                        IsSent = c.Boolean(nullable: false),
                        SentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationToSendRecipient",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NotificationToSendId = c.Long(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotificationToSend", t => t.NotificationToSendId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.NotificationToSendId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SystemSetting",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SystemTitle = c.String(),
                        ShortTitle = c.String(),
                        SystemFooter = c.String(),
                        SystemVersion = c.String(),
                        EmailFooter = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            AddColumn("dbo.User", "CreatedBy", c => c.Int());
            AddColumn("dbo.User", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.PublicEvent", "EventObjectiveId", c => c.Int());
            AddColumn("dbo.PublicEvent", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PublicEvent", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PublicEvent", "ExternalExhibitorId", c => c.Int());
            AddColumn("dbo.PublicEvent", "EventSpeakerId", c => c.Int());
            AddColumn("dbo.Role", "Name", c => c.String());
            AddColumn("dbo.Role", "Description", c => c.String());
            AlterColumn("dbo.PublicEvent", "TargetedGroup", c => c.Int(nullable: false));
            CreateIndex("dbo.PublicEvent", "EventObjectiveId");
            CreateIndex("dbo.PublicEvent", "ExternalExhibitorId");
            CreateIndex("dbo.PublicEvent", "EventSpeakerId");
            AddForeignKey("dbo.CourseFile", "FileId", "dbo.FileDocument", "Id");
            AddForeignKey("dbo.PublicEvent", "ExternalExhibitorId", "dbo.EventExternalExhibitor", "Id");
            AddForeignKey("dbo.PublicEvent", "EventObjectiveId", "dbo.EventObjective", "Id");
            AddForeignKey("dbo.PublicEvent", "EventSpeakerId", "dbo.EventSpeaker", "Id");
            DropColumn("dbo.User", "LoginId");
            DropColumn("dbo.UserAccount", "CreatedDate");
            DropColumn("dbo.UserAccount", "CreatedBy");
            DropColumn("dbo.UserAccount", "Display");
            DropColumn("dbo.PublicEvent", "EventObjective");
            DropColumn("dbo.PublicEvent", "Date");
            DropColumn("dbo.PublicEvent", "ExternalExhibitor");
            DropColumn("dbo.Role", "RoleName");
            DropColumn("dbo.Role", "RoleDescription");
            DropTable("dbo.File");
            DropTable("dbo.Speaker");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Speaker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SpeakerName = c.String(),
                        EventId = c.Int(),
                        UserId = c.Int(),
                        Remark = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            AddColumn("dbo.Role", "RoleDescription", c => c.String());
            AddColumn("dbo.Role", "RoleName", c => c.String());
            AddColumn("dbo.PublicEvent", "ExternalExhibitor", c => c.String());
            AddColumn("dbo.PublicEvent", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.PublicEvent", "EventObjective", c => c.String());
            AddColumn("dbo.UserAccount", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserAccount", "CreatedBy", c => c.Int());
            AddColumn("dbo.UserAccount", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.User", "LoginId", c => c.String());
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.NotificationToSendRecipient", "UserId", "dbo.User");
            DropForeignKey("dbo.NotificationToSendRecipient", "NotificationToSendId", "dbo.NotificationToSend");
            DropForeignKey("dbo.Notification", "UserId", "dbo.User");
            DropForeignKey("dbo.PublicEvent", "EventSpeakerId", "dbo.EventSpeaker");
            DropForeignKey("dbo.EventSpeaker", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.PublicEvent", "EventObjectiveId", "dbo.EventObjective");
            DropForeignKey("dbo.EventObjective", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.PublicEvent", "ExternalExhibitorId", "dbo.EventExternalExhibitor");
            DropForeignKey("dbo.EventExternalExhibitor", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EmailToSendAddress", "EmailToSendId", "dbo.EmailToSend");
            DropForeignKey("dbo.CourseFile", "FileId", "dbo.FileDocument");
            DropForeignKey("dbo.FileDocument", "CreatedBy", "dbo.User");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.NotificationToSendRecipient", new[] { "UserId" });
            DropIndex("dbo.NotificationToSendRecipient", new[] { "NotificationToSendId" });
            DropIndex("dbo.Notification", new[] { "UserId" });
            DropIndex("dbo.EventSpeaker", new[] { "CreatedBy" });
            DropIndex("dbo.EventObjective", new[] { "CreatedBy" });
            DropIndex("dbo.EventExternalExhibitor", new[] { "CreatedBy" });
            DropIndex("dbo.PublicEvent", new[] { "EventSpeakerId" });
            DropIndex("dbo.PublicEvent", new[] { "ExternalExhibitorId" });
            DropIndex("dbo.PublicEvent", new[] { "EventObjectiveId" });
            DropIndex("dbo.EmailToSendAddress", new[] { "EmailToSendId" });
            DropIndex("dbo.FileDocument", new[] { "CreatedBy" });
            AlterColumn("dbo.PublicEvent", "TargetedGroup", c => c.Int());
            DropColumn("dbo.Role", "Description");
            DropColumn("dbo.Role", "Name");
            DropColumn("dbo.PublicEvent", "EventSpeakerId");
            DropColumn("dbo.PublicEvent", "ExternalExhibitorId");
            DropColumn("dbo.PublicEvent", "EndDate");
            DropColumn("dbo.PublicEvent", "StartDate");
            DropColumn("dbo.PublicEvent", "EventObjectiveId");
            DropColumn("dbo.User", "CreatedDate");
            DropColumn("dbo.User", "CreatedBy");
            DropTable("dbo.UserRole");
            DropTable("dbo.SystemSetting");
            DropTable("dbo.NotificationToSendRecipient");
            DropTable("dbo.NotificationToSend");
            DropTable("dbo.NotificationSetting");
            DropTable("dbo.Notification");
            DropTable("dbo.EventSpeaker");
            DropTable("dbo.EventObjective");
            DropTable("dbo.EventExternalExhibitor");
            DropTable("dbo.EmailToSendAddress");
            DropTable("dbo.EmailToSend");
            DropTable("dbo.FileDocument");
            DropTable("dbo.Client");
            DropTable("dbo.AccountSetting");
            CreateIndex("dbo.Speaker", "CreatedBy");
            CreateIndex("dbo.Speaker", "UserId");
            CreateIndex("dbo.Speaker", "EventId");
            AddForeignKey("dbo.Speaker", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Speaker", "EventId", "dbo.PublicEvent", "Id");
            AddForeignKey("dbo.Speaker", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.CourseFile", "FileId", "dbo.File", "Id");
            RenameTable(name: "dbo.RoleAccess", newName: "RoleAccesses");
        }
    }
}
