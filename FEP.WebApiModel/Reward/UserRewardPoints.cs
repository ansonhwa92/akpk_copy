using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Reward
{
    public class UserRewardPointsModel
    {
        [Display(Name = "Course")]
        public int? CourseId { get; set; }
        [Display(Name = "Course")]
        public string CourseName { get; set; }
        [Display(Name = "Points Received")]
        public int PointsReceived { get; set; }
        [Display(Name = "User")]
        public int UserId { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }

        [Display(Name = "Reward Type")]
        public RewardType RewardType { get; set; }
        [Display(Name = "Reward Type")]
        public string RewardTypeName { get; set; }
        [Display(Name = "Award Reason")]
        public string AwardReason { get; set; }
        [Display(Name = "Rewarded By")]
        public int? RewardedBy { get; set; }
        [Display(Name = "Rewarded By")]
        public string RewardedByName { get; set; }

        [Display(Name = "Date Received")]
        [UIHint("Date")]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy hh:mm tt}")]
        public DateTime DateReceived { get; set; }

    }
    public class CreateUserRewardPointsModel : UserRewardPointsModel
    {
        public CreateUserRewardPointsModel() { }
        public List<SelectListItem> userList { get; set; }
    }
    public class DetailUserRewardPointsModel : UserRewardPointsModel
    {
        public DetailUserRewardPointsModel() { }
        [Required]
        public int Id { get; set; }
    }
    public class EditUserRewardPointsModel : UserRewardPointsModel
    {
        public EditUserRewardPointsModel() { }
        [Required]
        public int Id { get; set; }
    }
    public class DeleteUserRewardPointsModel : DetailUserRewardPointsModel
    {
        public DeleteUserRewardPointsModel() { }
    }
    public class ListUserRewardPointsModel
    {
        public FilterUserRewardPointsModel filter { get; set; }
        public List<DetailUserRewardPointsModel> UserRewardPointsList { get; set; }
        public ListUserRewardPointsModel() { }
        public ListUserRewardPointsModel(List<DetailUserRewardPointsModel> ListUserRewardPoints)
        {
            this.UserRewardPointsList = ListUserRewardPoints;
        }

        public IEnumerable<SelectListItem> RewardTypeList { get; set; }
    }
    public class FilterUserRewardPointsModel : DataTableModel
    {
        [Display(Name = "Course")]
        public string CourseName { get; set; }
        [Display(Name = "Points Received")]
        public int? PointsReceived { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Reward Type")]
        public RewardType? RewardType { get; set; }
        [Display(Name = "Rewarded By")]
        public string RewardedBy { get; set; }
        [Display(Name = "Date Received")]
        public DateTime? DateReceived { get; set; }

    }

}
