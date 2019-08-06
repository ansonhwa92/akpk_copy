﻿using FEP.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;

namespace FEP.Model
{
	public class DbEntities : DbContext
	{

		public DbEntities() : base("conStr")
		{

		}

		public DbEntities(string connString)
		{
			Database.Connection.ConnectionString = connString;
		}

		public DbSet<UserAccount> UserAccount { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Role> Role { get; set; }
		public DbSet<RoleAccess> RoleAccess { get; set; }

		//file
		public DbSet<File> File { get; set; }

		//payment
		public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
		public DbSet<PurchaseOrderItem> PurchaseOrderItem { get; set; }


		//elearning
		public DbSet<LearningCourse> LearningCourse { get; set; }
		public DbSet<LearningCourseCertificate> LearningCourseCertificate { get; set; }
		public DbSet<LearningCourseCategory> LearningCourseCategory { get; set; }
		public DbSet<Learner> Learner { get; set; }
		public DbSet<CourseAdministrator> CourseAdministrator { get; set; }
		public DbSet<CourseLearner> CourseLearner { get; set; }
		public DbSet<LearnerBadge> LearnerBadge { get; set; }
		public DbSet<CourseInstructor> CourseInstructor { get; set; }
		public DbSet<CourseContent> CourseContent { get; set; }
		public DbSet<CourseFile> CourseFile { get; set; }
		public DbSet<CourseApproval> CourseApproval { get; set; }
		public DbSet<CourseWithdraw> CourseWithdraw { get; set; }
		public DbSet<GamificationPoint> GamificationPoint { get; set; }
		public DbSet<GamificationLevel> GamificationLevel { get; set; }
		public DbSet<GamificationBadge> GamificationBadge { get; set; }
		public DbSet<LearningGroup> LearningGroup { get; set; }
		public DbSet<LearningGroupMember> LearningGroupMember { get; set; }
		public DbSet<LearningDiscussion> LearningDiscussion { get; set; }
		public DbSet<LearningDiscussionView> LearningDiscussionView { get; set; }
		public DbSet<LearningDiscussionReply> LearningDiscussionReply { get; set; }
		public DbSet<LearningDiscussionReplyUpvote> LearningDiscussionReplyUpvote { get; set; }
		public DbSet<LearningDiscussionAttachment> LearningDiscussionAttachment { get; set; }
		public DbSet<LearningDiscussionComment> LearningDiscussionComment { get; set; }
		public DbSet<LearningEvent> LearningEvent { get; set; }
		public DbSet<LearningEventAudience> LearningEventAudience { get; set; }

		//eEvent
		public DbSet<PublicEvent> PublicEvent { get; set; }
		public DbSet<EventCalendar> EventCalendar { get; set; }
		public DbSet<Agenda> Agenda { get; set; }
		public DbSet<Speaker> Speaker { get; set; }
		public DbSet<EventBooking> EventBooking { get; set; }
		public DbSet<InvitationEvent> InvitationEvent { get; set; }
		public DbSet<EventMediaInterviewRequest> EventMediaInterviewRequest { get; set; }
		public DbSet<EventAttendance> EventAttendance { get; set; }
		public DbSet<ManuscriptSubmission> ManuscriptSubmission { get; set; }
		public DbSet<ParticipantFeedback> ParticipantFeedback { get; set; }
		public DbSet<EventMember> EventMember { get; set; }
		public DbSet<EventApproval> EventApproval { get; set; }
		public DbSet<EventVerifier> EventVerifier { get; set; }
		public DbSet<EventCancellation> EventCancellation { get; set; }
		public DbSet<EventExhibitionRequest> EventExhibitionRequest { get; set; }
		public DbSet<EventFile> EventFile { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();


		}

	}
}
