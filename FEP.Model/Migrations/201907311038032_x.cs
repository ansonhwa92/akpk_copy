namespace FEP.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
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
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginId = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        UserType = c.Int(nullable: false),
                        Display = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
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
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
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
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleAccess",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserAccess = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RoleAccess", "RoleId", "dbo.Role");
            DropForeignKey("dbo.NotificationToSendRecipient", "UserId", "dbo.User");
            DropForeignKey("dbo.NotificationToSendRecipient", "NotificationToSendId", "dbo.NotificationToSend");
            DropForeignKey("dbo.Notification", "UserId", "dbo.User");
            DropForeignKey("dbo.FileDocument", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.UserAccount", "UserId", "dbo.User");
            DropForeignKey("dbo.EmailToSendAddress", "EmailToSendId", "dbo.EmailToSend");
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.RoleAccess", new[] { "RoleId" });
            DropIndex("dbo.NotificationToSendRecipient", new[] { "UserId" });
            DropIndex("dbo.NotificationToSendRecipient", new[] { "NotificationToSendId" });
            DropIndex("dbo.Notification", new[] { "UserId" });
            DropIndex("dbo.UserAccount", new[] { "UserId" });
            DropIndex("dbo.FileDocument", new[] { "CreatedBy" });
            DropIndex("dbo.EmailToSendAddress", new[] { "EmailToSendId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.SystemSetting");
            DropTable("dbo.RoleAccess");
            DropTable("dbo.Role");
            DropTable("dbo.NotificationToSendRecipient");
            DropTable("dbo.NotificationToSend");
            DropTable("dbo.NotificationSetting");
            DropTable("dbo.Notification");
            DropTable("dbo.UserAccount");
            DropTable("dbo.User");
            DropTable("dbo.FileDocument");
            DropTable("dbo.EmailToSendAddress");
            DropTable("dbo.EmailToSend");
            DropTable("dbo.Client");
            DropTable("dbo.AccountSetting");
        }
    }
}
