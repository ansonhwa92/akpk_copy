using FEP.Helper;
using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        [Display(Name = "Reward Code")]
        public string RewardCode { get; set; }
        [Display(Name = "Points Used")]
        public int? PointsUsed { get; set; }

        [Display(Name = "RedeemDate")]
        public DateTime RedeemDate { get; set; }
        [Display(Name = "Reward Status")]
        public RewardStatus RewardStatus { get; set; }
        public string RewardStatusName { get; set; }
    }

    public class CheckRewardValidity
    {
        [Required]
        public string RewardCode { get; set; }
        [Required]
        public int UserId { get; set; }
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

        public IEnumerable<SelectListItem> RewardStatusList { get; set; }
    }
    public class FilterUserRewardRedemptionModel : DataTableModel
    {
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Description")]
        public string RewardDescription { get; set; }

        [Display(Name = "Redeem Date")]
        [UIHint("Date")]
        public DateTime? RedeemDate { get; set; }
        [Display(Name = "Reward Status")]
        public RewardStatus? RewardStatus { get; set; }

        [Display(Name = "Reward Status")]
        public string RewardStatusName { get; set; }
        [Display(Name = "Points Used")]
        public int? PointsUsed { get; set; }
    }
}
