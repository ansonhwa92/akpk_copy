namespace FEP.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventInterviewRequest", newName: "EventMediaInterviewRequest");
            CreateTable(
                "dbo.EventFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        FileSize = c.Long(nullable: false),
                        FileDescription = c.String(),
                        Category = c.Int(),
                        UploadedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Display = c.Boolean(nullable: false),
                        EventId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PublicEvent", t => t.EventId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.EventVerifier",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VerifierId = c.Int(nullable: false),
                        EventId = c.Int(),
                        VerifiedDate = c.DateTime(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PublicEvent", t => t.EventId)
                .ForeignKey("dbo.User", t => t.VerifierId)
                .Index(t => t.VerifierId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.EventCancellation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        EventId = c.Int(),
                        Reasons = c.String(),
                        ApprovalId1 = c.Int(),
                        ApprovalId2 = c.Int(),
                        ApprovalId3 = c.Int(),
                        ApprovalId4 = c.Int(),
                        VerifyId = c.Int(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventApproval", t => t.ApprovalId1)
                .ForeignKey("dbo.EventApproval", t => t.ApprovalId2)
                .ForeignKey("dbo.EventApproval", t => t.ApprovalId3)
                .ForeignKey("dbo.EventApproval", t => t.ApprovalId4)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .ForeignKey("dbo.PublicEvent", t => t.EventId)
                .ForeignKey("dbo.User", t => t.UserId)
                .ForeignKey("dbo.EventVerifier", t => t.VerifyId)
                .Index(t => t.UserId)
                .Index(t => t.EventId)
                .Index(t => t.ApprovalId1)
                .Index(t => t.ApprovalId2)
                .Index(t => t.ApprovalId3)
                .Index(t => t.ApprovalId4)
                .Index(t => t.VerifyId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.EventExhibitionRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReceivedBy = c.Int(),
                        ReceivedDate = c.DateTime(nullable: false),
                        Receive_Via = c.String(),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        Display = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .Index(t => t.CreatedBy);
            
            AddColumn("dbo.User", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventApproval", "EventId", c => c.Int());
            AddColumn("dbo.EventMediaInterviewRequest", "MediaName", c => c.String());
            AddColumn("dbo.EventMediaInterviewRequest", "MediaType", c => c.String());
            AddColumn("dbo.EventMediaInterviewRequest", "ContactPerson", c => c.String());
            AddColumn("dbo.EventMediaInterviewRequest", "ContactNo", c => c.Int(nullable: false));
            AddColumn("dbo.EventMediaInterviewRequest", "Address", c => c.String());
            AddColumn("dbo.EventMediaInterviewRequest", "Email", c => c.String());
            CreateIndex("dbo.EventApproval", "EventId");
            AddForeignKey("dbo.EventApproval", "EventId", "dbo.PublicEvent", "Id");
            DropColumn("dbo.EventMediaInterviewRequest", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventMediaInterviewRequest", "Name", c => c.String());
            DropForeignKey("dbo.EventExhibitionRequest", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EventCancellation", "VerifyId", "dbo.EventVerifier");
            DropForeignKey("dbo.EventCancellation", "UserId", "dbo.User");
            DropForeignKey("dbo.EventCancellation", "EventId", "dbo.PublicEvent");
            DropForeignKey("dbo.EventCancellation", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EventCancellation", "ApprovalId4", "dbo.EventApproval");
            DropForeignKey("dbo.EventCancellation", "ApprovalId3", "dbo.EventApproval");
            DropForeignKey("dbo.EventCancellation", "ApprovalId2", "dbo.EventApproval");
            DropForeignKey("dbo.EventCancellation", "ApprovalId1", "dbo.EventApproval");
            DropForeignKey("dbo.EventApproval", "EventId", "dbo.PublicEvent");
            DropForeignKey("dbo.EventVerifier", "VerifierId", "dbo.User");
            DropForeignKey("dbo.EventVerifier", "EventId", "dbo.PublicEvent");
            DropForeignKey("dbo.EventFile", "EventId", "dbo.PublicEvent");
            DropIndex("dbo.EventExhibitionRequest", new[] { "CreatedBy" });
            DropIndex("dbo.EventCancellation", new[] { "CreatedBy" });
            DropIndex("dbo.EventCancellation", new[] { "VerifyId" });
            DropIndex("dbo.EventCancellation", new[] { "ApprovalId4" });
            DropIndex("dbo.EventCancellation", new[] { "ApprovalId3" });
            DropIndex("dbo.EventCancellation", new[] { "ApprovalId2" });
            DropIndex("dbo.EventCancellation", new[] { "ApprovalId1" });
            DropIndex("dbo.EventCancellation", new[] { "EventId" });
            DropIndex("dbo.EventCancellation", new[] { "UserId" });
            DropIndex("dbo.EventVerifier", new[] { "EventId" });
            DropIndex("dbo.EventVerifier", new[] { "VerifierId" });
            DropIndex("dbo.EventFile", new[] { "EventId" });
            DropIndex("dbo.EventApproval", new[] { "EventId" });
            DropColumn("dbo.EventMediaInterviewRequest", "Email");
            DropColumn("dbo.EventMediaInterviewRequest", "Address");
            DropColumn("dbo.EventMediaInterviewRequest", "ContactNo");
            DropColumn("dbo.EventMediaInterviewRequest", "ContactPerson");
            DropColumn("dbo.EventMediaInterviewRequest", "MediaType");
            DropColumn("dbo.EventMediaInterviewRequest", "MediaName");
            DropColumn("dbo.EventApproval", "EventId");
            DropColumn("dbo.User", "Display");
            DropTable("dbo.EventExhibitionRequest");
            DropTable("dbo.EventCancellation");
            DropTable("dbo.EventVerifier");
            DropTable("dbo.EventFile");
            RenameTable(name: "dbo.EventMediaInterviewRequest", newName: "EventInterviewRequest");
        }
    }
}
