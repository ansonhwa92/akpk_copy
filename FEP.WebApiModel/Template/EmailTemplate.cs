using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.Template
{
    public class ListEmailTemplateModel
    {
        public FilterEmailTemplateModel Filter { get; set; }
        public EmailTemplateModel List { get; set; }
    }

    public class FilterEmailTemplateModel : DataTableModel
    {
        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modified")]
        public DateTime? LastModified { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
    }
    public class EmailTemplateModel
    {
        public int Id { get; set; }

        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Template Message")]
        public string TemplateMessage { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modified")]
        public DateTime? LastModified { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        public bool Display { get; set; }
    }

    public class CreateEmailTemplateModel
    {
        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Template Message")]
        public string TemplateMessage { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

    }
}
