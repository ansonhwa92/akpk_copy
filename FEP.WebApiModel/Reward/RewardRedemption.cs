using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Reward
{
    public class RewardRedemptionModel
    {
        [Display(Name = "Reward Code")]
        public string RewardCode { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Discount Value")]
        public int DiscountValue { get; set; }
        [Display(Name = "Points To Redeem")]
        public int PointsToRedeem { get; set; }
        [Display(Name = "Valid Duration (Days)")]
        public int ValidDuration { get; set; }
        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
    }

    public class CreateRewardRedemptionModel : RewardRedemptionModel
    {
        public CreateRewardRedemptionModel() { }
    }
    public class DetailRewardRedemptionModel : RewardRedemptionModel
    {
        public DetailRewardRedemptionModel() { }
        public bool IsClaimed { get; set; }

        [Required]
        public int Id { get; set; }
    }
    public class EditRewardRedemptionModel : RewardRedemptionModel
    {
        public EditRewardRedemptionModel() { }
        [Required]
        public int Id { get; set; }
    }
    public class DeleteRewardRedemptionModel : DetailRewardRedemptionModel
    {
        public DeleteRewardRedemptionModel() { }
    }
    public class ListRewardRedemptionModel
    {
        public FilterRewardRedemptionModel filter { get; set; }
        public List<DetailRewardRedemptionModel> RewardRedemptionList { get; set; }
        public ListRewardRedemptionModel() { }
        public ListRewardRedemptionModel(List<DetailRewardRedemptionModel> ListRewardRedemption)
        {
            this.RewardRedemptionList = ListRewardRedemption;
        }
    }
    public class FilterRewardRedemptionModel : DataTableModel
    {
        [Display(Name = "Reward Code")]
        public string RewardCode { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Discount Value")] 
        public int DiscountValue { get; set; }
        [Display(Name = "Points To Redeem")]
        public int? PointsToRedeem { get; set; }
        [Display(Name = "Valid Duration (Days)")]
        public int? ValidDuration { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
    }

}
