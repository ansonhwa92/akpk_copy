using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.Reward
{
    public class RewardActivityPointModel
    {
        [Display(Name = "Activity")]
        public int ActivityId { get; set; }
        [Display(Name = "Activity")]
        public string ActivityName { get; set; }
        [Display(Name = "Value")]
        public int Value { get; set; }
        
        [DataType(DataType.DateTime)]
        [UIHint("DateTime")]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
    }

    public class ActivityDummyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateRewardActivityPointModel : RewardActivityPointModel
    {
        public IEnumerable<SelectListItem> ActivityDummyList { get; set; }
        public CreateRewardActivityPointModel() { }
    }

    public class DetailRewardActivityPointModel : RewardActivityPointModel
    {
        public DetailRewardActivityPointModel() { }

        [Required]
        public int Id { get; set; }
    }

    public class EditRewardActivityPointModel : RewardActivityPointModel
    {
        public IEnumerable<SelectListItem> ActivityDummyList { get; set; }
        public EditRewardActivityPointModel() { }

        [Required]
        public int Id { get; set; }
    }

    public class DeleteRewardActivityPointModel : DetailRewardActivityPointModel
    {
        public DeleteRewardActivityPointModel() { }
    }

    public class ListRewardActivityPointModel
    {
        public FilterRewardActivityPointModel filter { get; set; }
        public List<DetailRewardActivityPointModel> RewardActivityPointList { get; set; }
        public ListRewardActivityPointModel() { }
        public ListRewardActivityPointModel(List<DetailRewardActivityPointModel> ListRewardActivityPoint)
        {
            this.RewardActivityPointList = ListRewardActivityPoint;
        }
    }

    public class FilterRewardActivityPointModel : DataTableModel
    {
        [Display(Name = "Activity")]
        public string ActivityName { get; set; }
        [Display(Name = "Value")]
        public int Value { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Created By")]
        public string CreatedByName { get; set; }
    }
}
