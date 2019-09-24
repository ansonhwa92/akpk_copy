﻿using FEP.Model.eLearning;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FEP.Helper;
using Language;

namespace FEP.WebApiModel.eLearning
{
    /// <summary>
    /// Model for Home/Index page - listing of all Courses
    /// </summary>
    public class ReturnBriefCourseModel : BaseModel
    {

        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }


        [Display(Name = "Code", ResourceType = typeof(Language.eLearning.Course))]
        public string Code { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Language.eLearning.Course))]
        public int CategoryId { get; set; }

        [Display(Name = "Price", ResourceType = typeof(Language.eLearning.Course))]
        public string Price { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Language.eLearning.Course))]
        public CourseStatus Status { get; set; }
       
    }
    
    public class ReturnListCourseModel
    {
        public FilterCourseModel Filters { get; set; }
        public ReturnBriefCourseModel Courses { get; set; }
    }

 
    // class for setting and returning filters for the datatable list of publications
    public class FilterCourseModel : DataTableModel
    {

        [Display(Name = "Category", ResourceType = typeof(Language.eLearning.Course))]
        public int CategoryId { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Language.eLearning.Course))]
        public string Code { get; set; }

    }

    public class CreateOrEditCourseModel : BaseModel
    {
        [Required]
        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Course))]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description", ResourceType = typeof(Language.eLearning.Course))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(Language.eLearning.Course))]
        public int CategoryId { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Language.eLearning.Course))]
        public string Code { get; set; }


        [Display(Name = "Objectives", ResourceType = typeof(Language.eLearning.Course))]
        public string Objectives { get; set; }

        [Display(Name = "Medium", ResourceType = typeof(Language.eLearning.Course))]
        public CourseMedium Medium { get; set; }


        [Display(Name = "ScheduleType", ResourceType = typeof(Language.eLearning.Course))]
        public CourseScheduleType ScheduleType { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Language.eLearning.Course))]
        public CourseLanguage Language { get; set; }

        [Display(Name = "Duration", ResourceType = typeof(Language.eLearning.Course))]
        public int Duration { get; set; }

        [Display(Name = "DurationType", ResourceType = typeof(Language.eLearning.Course))]
        public DurationType DurationType { get; set; }

        [Display(Name = "IsFree", ResourceType = typeof(Language.eLearning.Course))]
        public bool IsFree { get; set; }
        [Display(Name = "Price", ResourceType = typeof(Language.eLearning.Course))]
        public decimal Price { get; set; }
        [Display(Name = "ViewCategory", ResourceType = typeof(Language.eLearning.Course))]
        public ViewCategory ViewCategory { get; set; }
    }

}