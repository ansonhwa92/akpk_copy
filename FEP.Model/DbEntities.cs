using FEP.Model.eLearning;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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

        //user
        public DbSet<UserAccount> UserAccount { get; set; }

        public DbSet<ActivateAccount> ActivateAccount { get; set; }
        public DbSet<PasswordReset> PasswordReset { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<CompanyProfile> CompanyProfile { get; set; }
        public DbSet<StaffProfile> StaffProfile { get; set; }

        public DbSet<Sector> Sector { get; set; }
        public DbSet<State> State { get; set; }

        public DbSet<Department> Department { get; set; }
        public DbSet<Branch> Branch { get; set; }

        //access
        public DbSet<Access> Access { get; set; }

        //role
        public DbSet<Role> Role { get; set; }

        public DbSet<RoleAccess> RoleAccess { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<RoleDefault> RoleDefault { get; set; }

        //setting
        public DbSet<SystemSetting> SystemSetting { get; set; }

        public DbSet<AccountSetting> AccountSetting { get; set; }

        //file
        public DbSet<FileDocument> FileDocument { get; set; }

        //notification
        public DbSet<Notification> Notification { get; set; }

        public DbSet<NotificationToSend> NotificationToSend { get; set; }
        public DbSet<NotificationToSendRecipient> NotificationToSendRecipient { get; set; }

        //public DbSet<NotificationSetting> NotificationSetting { get; set; }

        //email
        public DbSet<EmailToSend> EmailToSend { get; set; }

        public DbSet<EmailToSendAddress> EmailToSendAddress { get; set; }

        //web api
        public DbSet<Client> Client { get; set; }

        //payment
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }

        public DbSet<PurchaseOrderItem> PurchaseOrderItem { get; set; }

        //logs
        public DbSet<UserLog> UserLog { get; set; }

        public DbSet<ErrorLog> ErrorLog { get; set; }

        //elearning
        //      public DbSet<LearningCourse> LearningCourse { get; set; }
        //public DbSet<LearningCourseCertificate> LearningCourseCertificate { get; set; }
        //public DbSet<LearningCourseCategory> LearningCourseCategory { get; set; }
        //public DbSet<Learner> Learner { get; set; }
        //public DbSet<CourseAdministrator> CourseAdministrator { get; set; }
        //public DbSet<CourseLearner> CourseLearner { get; set; }
        //public DbSet<LearnerBadge> LearnerBadge { get; set; }
        //public DbSet<CourseInstructor> CourseInstructor { get; set; }
        //public DbSet<CourseContent> CourseContent { get; set; }
        //public DbSet<CourseFile> CourseFile { get; set; }
        //public DbSet<CourseApproval> CourseApproval { get; set; }
        //public DbSet<CourseWithdraw> CourseWithdraw { get; set; }
        //public DbSet<GamificationPoint> GamificationPoint { get; set; }
        //public DbSet<GamificationLevel> GamificationLevel { get; set; }
        //public DbSet<GamificationBadge> GamificationBadge { get; set; }
        //public DbSet<LearningGroup> LearningGroup { get; set; }
        //public DbSet<LearningGroupMember> LearningGroupMember { get; set; }
        //public DbSet<LearningDiscussion> LearningDiscussion { get; set; }
        //public DbSet<LearningDiscussionView> LearningDiscussionView { get; set; }
        //public DbSet<LearningDiscussionReply> LearningDiscussionReply { get; set; }
        //public DbSet<LearningDiscussionReplyUpvote> LearningDiscussionReplyUpvote { get; set; }
        //public DbSet<LearningDiscussionAttachment> LearningDiscussionAttachment { get; set; }
        //public DbSet<LearningDiscussionComment> LearningDiscussionComment { get; set; }
        //public DbSet<LearningEvent> LearningEvent { get; set; }
        //public DbSet<LearningEventAudience> LearningEventAudience { get; set; }

        //publication
        public DbSet<Publication> Publication { get; set; }

        public DbSet<PublicationCategory> PublicationCategory { get; set; }
        public DbSet<PublicationApproval> PublicationApproval { get; set; }
        public DbSet<PublicationWithdrawal> PublicationWithdrawal { get; set; }
        public DbSet<PublicationPurchase> PublicationPurchase { get; set; }
        public DbSet<PublicationPurchaseItem> PublicationPurchaseItem { get; set; }
        public DbSet<PublicationRefund> PublicationRefund { get; set; }

        //research
        public DbSet<Survey> Survey { get; set; }

        public DbSet<SurveyApproval> SurveyApproval { get; set; }
        public DbSet<SurveyResponse> SurveyResponse { get; set; }

        //eEvent
        public DbSet<PublicEvent> PublicEvent { get; set; }

        public DbSet<EventCalendar> EventCalendar { get; set; }
        public DbSet<EventAgenda> EventAgenda { get; set; }
        public DbSet<EventSpeaker> EventSpeaker { get; set; }
        public DbSet<EventBooking> EventBooking { get; set; }
        public DbSet<InvitationEvent> InvitationEvent { get; set; }
        public DbSet<EventMediaInterviewRequest> EventMediaInterviewRequest { get; set; }
        public DbSet<EventAttendance> EventAttendance { get; set; }
        public DbSet<ManuscriptSubmission> ManuscriptSubmission { get; set; }
        public DbSet<ParticipantFeedback> ParticipantFeedback { get; set; }
        public DbSet<EventMember> EventMember { get; set; }
        public DbSet<EventApproval> EventApproval { get; set; }
        public DbSet<EventVerifier> EventVerifier { get; set; }

        //public DbSet<EventCancellation> EventCancellation { get; set; }
        public DbSet<EventExhibitionRequest> EventExhibitionRequest { get; set; }

        public DbSet<EventFile> EventFile { get; set; }
        public DbSet<EventExternalExhibitor> EventExternalExhibitor { get; set; }
        public DbSet<MediaFile> MediaFile { get; set; }
        public DbSet<EventCategory> EventCategory { get; set; }
        public DbSet<SpeakerFile> SpeakerFile { get; set; }

        // Elearning
        public DbSet<Course> Courses { get; set; }

        public DbSet<ContentFile> ContentFiles { get; set; }

        public DbSet<CourseApprovalLog> CourseApprovals { get; set; }

        public DbSet<CourseCertificate> CourseCertificates { get; set; }

        public DbSet<CourseCertificateTemplate> CourseCertificateTemplates { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }

        public DbSet<CourseEvent> CourseEvents { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<GamificationCriteria> GamificationCriteria { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerCourse> TrainerCourses { get; set; }
        public DbSet<TrainerGroup> TrainerGroups { get; set; }
        public DbSet<CourseProgress> CourseProgress { get; set; }

        // Quiz, questions
        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
        public DbSet<OrderAnswer> OrderAnswers { get; set; }
        public DbSet<FreeTextAnswer> FreeTextAnswers { get; set; }
        public DbSet<ContentQuestion> ContentQuestions { get; set; }


        // Elearning Discussion
        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<DiscussionPost> DiscussionPosts { get; set; }
        //public DbSet<PostQueue> PostQueue { get; set; }

        // Elearning Lookup
        public DbSet<RefCourseCategory> RefCourseCategories { get; set; }

        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<TemplateParameters> TemplateParameters { get; set; }
        public DbSet<SLAReminder> SLAReminder { get; set; }
        public DbSet<SLAReminderStatus> SLAReminderStatus { get; set; }
        public DbSet<BulkNotification> BulkNotification { get; set; }
        public DbSet<ParameterGroup> ParameterGroup { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }


        public virtual void SetModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetDeleted(object entity)
        {
            this.Entry(entity).State = EntityState.Deleted;
        }
    }


}