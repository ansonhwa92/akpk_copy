using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace FEP.Model
{
    [Table("TargetedGroupCities")]
    public class TargetedGroupCities
    {
        [Key]
        public int ID { get; set; }
        public int StateID { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }

    [Table("TargetedGroups")]
    public class TargetedGroups
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public MemberGender? Gender { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public MemberDMPStatus? DMPStatus { get; set; }
        public MemberDelinquent? Delinquent { get; set; }
        public MemberEmploymentType? EmploymentType { get; set; }
        public MemberState? State { get; set; }
        public int? CityCode { get; set; }
        // auto-filled in data...............................................................................................
        public DateTime DateCreated { get; set; }
        public int CreatorId { get; set; }
        public bool Active { get; set; }
        // foreign key.......................................................................................................
        [ForeignKey("CityCode")]
        public virtual TargetedGroupCities City { get; set; }
    }

    [Table("TargetedGroupMembers")]
    public class TargetedGroupMembers
    {
        [Key]
        public int ID { get; set; }
        public int TargetedGroupID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public SourceSystem Source { get; set; }
        // foreign keys......................................................................................................
        [ForeignKey("TargetedGroupID")]
        public virtual TargetedGroups Group { get; set; }
    }

    public enum MemberGender
    {
        [Display(Name = "Any")]
        Any,
        [Display(Name = "Female")]          //F
        Female,
        [Display(Name = "Male")]            //M
        Male,
        [Display(Name = "Transgender")]     //T
        Transgender
    }

    public enum MemberDMPStatus
    {
        [Display(Name = "Any")]
        Any = 0,
        [Display(Name = "New")]
        New = 2954,
        [Display(Name = "Draft DMP Restructuring Plan")]
        DraftDMP = 2955,
        [Display(Name = "Interim DMP Restructuring Plan")]
        InterimDMP = 2956,
        [Display(Name = "Pending First Payment")]
        PendingFirst = 2957,
        [Display(Name = "Active")]
        Active = 2958,
        [Display(Name = "Withdrawal")]
        Withdrawal = 2959,
        [Display(Name = "Rescheduled")]
        Rescheduled = 2960,
        [Display(Name = "Settled")]
        Settled = 2961,
        [Display(Name = "Lapsed")]
        Lapsed = 3008,
        [Display(Name = "Terminated")]
        Terminated = 3009,
        [Display(Name = "Full Settlement")]
        FullSettlement = 3010,
        [Display(Name = "Draft")]
        Draft = 3045,
        [Display(Name = "Rejected")]
        Rejected = 3219,
        [Display(Name = "Finalized DMP Restructuring Plan")]
        FinalizedDMP = 3279,
        [Display(Name = "Postponement")]
        Postponement = 3332,
        [Display(Name = "Counselling Cancelled")]
        CounsellingCancelled = 5608,
        [Display(Name = "Reschedule Cancelled")]
        RescheduleCancelled = 5609
    }

    public enum MemberDelinquent
    {
        [Display(Name = "Any")]
        Any = 0,
        [Display(Name = "3 months in advance paid")]            //-3
        Advanced3 = -3,
        [Display(Name = "2 months in advance paid")]            //-2
        Advanced2 = -2,
        [Display(Name = "1 month in advance paid")]             //-1
        Advanced1 = -1,
        [Display(Name = "No outstanding payment")]              //0
        NoneOutstanding = 0,
        [Display(Name = "1 month outstanding payment")]         //1
        Outstanding1 = 1,
        [Display(Name = "2 months outstanding payment")]        //2
        Outstanding2 = 2,
        [Display(Name = "3 months outstanding payment")]        //3
        Outstanding3 = 3
    }

    public enum MemberEmploymentType
    {
        [Display(Name = "Any")]
        Any,
        [Display(Name = "Employed")]
        Employed = 481,
        [Display(Name = "Retiree")]
        Retiree = 5584,
        [Display(Name = "Self-Employed")]
        SelfEmployed = 482,
        [Display(Name = "Unemployed")]
        Unemployed = 607
    }

    public enum MemberState
    {
        [Display(Name = "Johor")]
        Johor = 94,
        [Display(Name = "Kedah")]
        Kedah = 95,
        [Display(Name = "Kelantan")]
        Kelantan = 96,
        [Display(Name = "Melaka")]
        Melaka = 370,
        [Display(Name = "Negeri Sembilan")]
        NegeriSembilan = 371,
        [Display(Name = "Pahang")]
        Pahang = 372,
        [Display(Name = "Perak")]
        Perak = 373,
        [Display(Name = "Perlis")]
        Perlis = 374,
        [Display(Name = "Pulau Pinang")]
        PulauPinang = 375,
        [Display(Name = "Sabah")]
        Sabah = 376,
        [Display(Name = "Sarawak")]
        Sarawak = 377,
        [Display(Name = "Selangor")]
        Selangor = 378,
        [Display(Name = "Terengganu")]
        Terengganu = 379,
        [Display(Name = "WP Kuala Lumpur")]
        WPKualaLumpur = 380,
        [Display(Name = "WP Labuan")]
        WPLabuan = 381,
        [Display(Name = "WP Putrajaya")]
        WPPutrajaya = 382
    }

    public enum SourceSystem
    {
        [Display(Name = "HRMS")]
        HRMS,
        [Display(Name = "OBS")]
        OBS,
        [Display(Name = "CBS")]
        CBS
    }

}
