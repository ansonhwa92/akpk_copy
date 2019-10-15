using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("RewardActivityPoint")]
    public class RewardActivityPoint
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        [ForeignKey("ActivityId")]
        public virtual ActivityDummy Activity { get; set; }
        public int Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
        public bool Display { get; set; }
    }

    [Table("RewardRedemption")]
    public class RewardRedemption
    {
        [Key]
        public int Id { get; set; }
        public string RewardCode { get; set; }
        public string Description { get; set; }
        public int PointsToRedeem { get; set; }
        public int ValidDuration { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Display { get; set; }
    }

    [Table("UserRewardPoints")]
    public class UserRewardPoints
    {
        [Key]
        public int Id { get; set; }
        public int? ActivityId { get; set; }//add virtual to the REAL Activity later
        [ForeignKey("ActivityId")]
        public virtual ActivityDummy Activity { get; set; }
        public int PointsReceived { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        //-----------------------------------------------------
        //  Rewarded can be from completed activity or awarded by admin
        //-----------------------------------------------------
        public RewardType RewardType { get; set; }
        public string AwardReason { get; set; }
        public int? RewardedBy { get; set; }
        [ForeignKey("RewardedBy")]
        public virtual User RewardSender { get; set; }
        //-----------------------------------------------------
        public DateTime DateReceived { get; set; }
        public bool Display { get; set; }
    }

    [Table("UserRewardRedemption")]
    public class UserRewardRedemption
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int RewardRedemptionId { get; set; }
        [ForeignKey("RewardRedemptionId")]
        public virtual RewardRedemption RewardRedemption { get; set; }

        public DateTime RedeemDate { get; set; }

        public RewardStatus RewardStatus { get; set; }


    }

    public enum RewardType
    {
        [Display(Name = "Activity")]
        ActivityReward,
        [Display(Name = "Admin Reward")]
        AdminReward
    }

    public enum RewardStatus
    {
        [Display(Name = "Open")]
        Open,
        [Display(Name = "Closed")]
        Closed
    }

    [Table("ActivityDummy")]
    public class ActivityDummy
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum ActivityListDummy
    {
        Activity1,
        Activity2,
        Activity3,
        Activity4,
        Activity5,
        Activity6,
        Activity7,
        Activity8,
        Activity9,
        Activity10
    }
}
