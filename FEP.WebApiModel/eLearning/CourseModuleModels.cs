﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Helper;
using FEP.Model.eLearning;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FEP.WebApiModel.eLearning
{
    public class CreateOrEditModuleModel : BaseModel
    {
        [Required]
        [AllowHtml]

        [Display(Name = "Title", ResourceType = typeof(Language.eLearning.Module))]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Description", ResourceType = typeof(Language.eLearning.Module))]
        public string Description { get; set; }

        public int Order { get; set; }

        [AllowHtml]
        [Display(Name = "Objectives", ResourceType = typeof(Language.eLearning.Module))]
        public string Objectives { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; }

        public virtual ICollection<CourseContent> ModuleContents { get; set; }

        // for use with completion criteria
        public List<CourseContent> Questions { get; set; }

    }   


    //public class ContentCompletionModel : BaseModel
    //{

    //    [Display(Name = "CompletionCriteria", ResourceType = typeof(Language.eLearning.Content))]
    //    public ContentCompletionType CompletionType { get; set; }

    //    [Display(Name = "CompleteATimer", ResourceType = typeof(Language.eLearning.Course))]
    //    public int? Timer { get; set; } //completiontype timer in sec

    //    [Display(Name = "CompleteAQuestion", ResourceType = typeof(Language.eLearning.Course))]
    //    public int? QuestionId { get; set; }



    //}
}
