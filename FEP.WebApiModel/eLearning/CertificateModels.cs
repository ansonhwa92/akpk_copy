using FEP.Model;
using FEP.Model.eLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.WebApiModel.eLearning
{
    //BACKGROUND & TEMPLATE
    public class CertificatesModel
    {
        public ICollection<CourseCertificateTemplate> Template { get; set; }
        public ICollection<CourseCertificate> Background { get; set; }
    }

    //BACKGROUND
    public class CreateBackgroundModel
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public HttpPostedFileBase File { get; set; }
        public string FileName { get; set; }
        public FileUpload FileUpload { get; set; }
        public int? FileUploadId { get; set; }

        [Required]
        [Display(Name = "Page Orientation")]
        public TypePageOrientation TypePageOrientation { get; set; }
    }

    //TEMPLATE
    public class CreateTemplateModel
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [AllowHtml]
        public string Template { get; set; }

        [Required]
        [Display(Name = "Page Orientation")]
        public TypePageOrientation TypePageOrientation { get; set; }

    }
}
