namespace FEP.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.eEvent", newName: "PublicEvent");
            CreateTable(
                "dbo.EventApproval",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApproverId = c.Int(nullable: false),
                        ApprovedDate = c.DateTime(),
                        Remark = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApproverId)
                .Index(t => t.ApproverId);
            
            AddColumn("dbo.Agenda", "CreatedBy", c => c.Int());
            AddColumn("dbo.Agenda", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Agenda", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.PublicEvent", "CreatedBy", c => c.Int());
            AddColumn("dbo.PublicEvent", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.PublicEvent", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventAttendance", "CreatedBy", c => c.Int());
            AddColumn("dbo.EventAttendance", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.EventAttendance", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventBooking", "CreatedBy", c => c.Int());
            AddColumn("dbo.EventBooking", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.EventBooking", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventInterviewRequest", "CreatedBy", c => c.Int());
            AddColumn("dbo.EventInterviewRequest", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.EventInterviewRequest", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventMember", "EventId", c => c.Int());
            AddColumn("dbo.EventMember", "CreatedBy", c => c.Int());
            AddColumn("dbo.EventMember", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.EventMember", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.InvitationEvent", "CreatedBy", c => c.Int());
            AddColumn("dbo.InvitationEvent", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.InvitationEvent", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.ManuscriptSubmission", "CreatedBy", c => c.Int());
            AddColumn("dbo.ManuscriptSubmission", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ManuscriptSubmission", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.ParticipantFeedback", "CreatedBy", c => c.Int());
            AddColumn("dbo.ParticipantFeedback", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ParticipantFeedback", "Display", c => c.Boolean(nullable: false));
            AddColumn("dbo.Speaker", "CreatedBy", c => c.Int());
            AddColumn("dbo.Speaker", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Speaker", "Display", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PublicEvent", "Fee", c => c.Single());
            AlterColumn("dbo.EventBooking", "Price", c => c.Single(nullable: false));
            AlterColumn("dbo.EventBooking", "Total", c => c.Single(nullable: false));
            CreateIndex("dbo.Agenda", "CreatedBy");
            CreateIndex("dbo.EventAttendance", "CreatedBy");
            CreateIndex("dbo.PublicEvent", "CreatedBy");
            CreateIndex("dbo.EventBooking", "CreatedBy");
            CreateIndex("dbo.EventInterviewRequest", "CreatedBy");
            CreateIndex("dbo.EventMember", "EventId");
            CreateIndex("dbo.EventMember", "CreatedBy");
            CreateIndex("dbo.InvitationEvent", "CreatedBy");
            CreateIndex("dbo.ManuscriptSubmission", "CreatedBy");
            CreateIndex("dbo.ParticipantFeedback", "CreatedBy");
            CreateIndex("dbo.Speaker", "CreatedBy");
            AddForeignKey("dbo.Agenda", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.EventAttendance", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.PublicEvent", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.EventBooking", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.EventInterviewRequest", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.EventMember", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.EventMember", "EventId", "dbo.PublicEvent", "Id");
            AddForeignKey("dbo.InvitationEvent", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.ManuscriptSubmission", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.ParticipantFeedback", "CreatedBy", "dbo.User", "Id");
            AddForeignKey("dbo.Speaker", "CreatedBy", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Speaker", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.ParticipantFeedback", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.ManuscriptSubmission", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.InvitationEvent", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EventMember", "EventId", "dbo.PublicEvent");
            DropForeignKey("dbo.EventMember", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EventInterviewRequest", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EventBooking", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.PublicEvent", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EventAttendance", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.EventApproval", "ApproverId", "dbo.User");
            DropForeignKey("dbo.Agenda", "CreatedBy", "dbo.User");
            DropIndex("dbo.Speaker", new[] { "CreatedBy" });
            DropIndex("dbo.ParticipantFeedback", new[] { "CreatedBy" });
            DropIndex("dbo.ManuscriptSubmission", new[] { "CreatedBy" });
            DropIndex("dbo.InvitationEvent", new[] { "CreatedBy" });
            DropIndex("dbo.EventMember", new[] { "CreatedBy" });
            DropIndex("dbo.EventMember", new[] { "EventId" });
            DropIndex("dbo.EventInterviewRequest", new[] { "CreatedBy" });
            DropIndex("dbo.EventBooking", new[] { "CreatedBy" });
            DropIndex("dbo.PublicEvent", new[] { "CreatedBy" });
            DropIndex("dbo.EventAttendance", new[] { "CreatedBy" });
            DropIndex("dbo.EventApproval", new[] { "ApproverId" });
            DropIndex("dbo.Agenda", new[] { "CreatedBy" });
            AlterColumn("dbo.EventBooking", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EventBooking", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PublicEvent", "Fee", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Speaker", "Display");
            DropColumn("dbo.Speaker", "CreatedDate");
            DropColumn("dbo.Speaker", "CreatedBy");
            DropColumn("dbo.ParticipantFeedback", "Display");
            DropColumn("dbo.ParticipantFeedback", "CreatedDate");
            DropColumn("dbo.ParticipantFeedback", "CreatedBy");
            DropColumn("dbo.ManuscriptSubmission", "Display");
            DropColumn("dbo.ManuscriptSubmission", "CreatedDate");
            DropColumn("dbo.ManuscriptSubmission", "CreatedBy");
            DropColumn("dbo.InvitationEvent", "Display");
            DropColumn("dbo.InvitationEvent", "CreatedDate");
            DropColumn("dbo.InvitationEvent", "CreatedBy");
            DropColumn("dbo.EventMember", "Display");
            DropColumn("dbo.EventMember", "CreatedDate");
            DropColumn("dbo.EventMember", "CreatedBy");
            DropColumn("dbo.EventMember", "EventId");
            DropColumn("dbo.EventInterviewRequest", "Display");
            DropColumn("dbo.EventInterviewRequest", "CreatedDate");
            DropColumn("dbo.EventInterviewRequest", "CreatedBy");
            DropColumn("dbo.EventBooking", "Display");
            DropColumn("dbo.EventBooking", "CreatedDate");
            DropColumn("dbo.EventBooking", "CreatedBy");
            DropColumn("dbo.EventAttendance", "Display");
            DropColumn("dbo.EventAttendance", "CreatedDate");
            DropColumn("dbo.EventAttendance", "CreatedBy");
            DropColumn("dbo.PublicEvent", "Display");
            DropColumn("dbo.PublicEvent", "CreatedDate");
            DropColumn("dbo.PublicEvent", "CreatedBy");
            DropColumn("dbo.Agenda", "Display");
            DropColumn("dbo.Agenda", "CreatedDate");
            DropColumn("dbo.Agenda", "CreatedBy");
            DropTable("dbo.EventApproval");
            RenameTable(name: "dbo.PublicEvent", newName: "eEvent");
        }
    }
}
