using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Reward
{
    public class UserRewardRedemptionModel
    {
        [Display(Name = "User")]
        public int UserId { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Description")]
        public int RewardRedemptionId { get; set; }
        [Display(Name = "Description")]
        public string RewardDescription { get; set; }

        //-----------------------------------------------------
        //  Rewarded By Admin
        //-----------------------------------------------------
        [Display(Name = "Reward Type")]
        public RewardType RewardType { get; set; }
        [Display(Name = "Rewarded By")]
        public int? RewardedBy { get; set; }
        [Display(Name = "Rewarded By")]
        public string RewardedByName { get; set; }
        //-----------------------------------------------------
        [Display(Name = "RedeemDate")]
        public DateTime RedeemDate { get; set; }
        [Display(Name = "Reward Status")]
        public RewardStatus RewardStatus { get; set; }
    }
    public class CreateUserRewardRedemptionModel : UserRewardRedemptionModel
    {
        public CreateUserRewardRedemptionModel() { }
    }
    public class DetailUserRewardRedemptionModel : UserRewardRedemptionModel
    {
        public DetailUserRewardRedemptionModel() { }
        [Required]
        public int Id { get; set; }
    }
    public class EditUserRewardRedemptionModel : UserRewardRedemptionModel
    {
        public EditUserRewardRedemptionModel() { }
        [Required]
        public int Id { get; set; }
    }
    public class DeleteUserRewardRedemptionModel : DetailUserRewardRedemptionModel
    {
        public DeleteUserRewardRedemptionModel() { }
    }
    public class ListUserRewardRedemptionModel
    {
        public FilterUserRewardRedemptionModel filter { get; set; }
        public List<DetailUserRewardRedemptionModel> UserRewardRedemptionList { get; set; }
        public ListUserRewardRedemptionModel() { }
        public ListUserRewardRedemptionModel(List<DetailUserRewardRedemptionModel> ListUserRewardRedemption)
        {
            this.UserRewardRedemptionList = ListUserRewardRedemption;
        }
    }
    public class FilterUserRewardRedemptionModel
    {
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Description")]
        public string RewardDescription { get; set; }

        //-----------------------------------------------------
        //  Rewarded By Admin
        //-----------------------------------------------------
        [Display(Name = "Reward Type")]
        public RewardType RewardType { get; set; }
        [Display(Name = "Rewarded By")]
        public string RewardedByName { get; set; }
        //-----------------------------------------------------
        [Display(Name = "RedeemDate")]
        public DateTime RedeemDate { get; set; }
        [Display(Name = "Reward Status")]
        public RewardStatus RewardStatus { get; set; }
    }
}
