﻿using FEP.Model;
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
        public int courseId { get; set; }
        public int CourseCertificateId { get; set; }
        public int CourseCertificateTemplateId { get; set; }
        public int selectedBackground { get; set; }
        public int selectedTemplate { get; set; }
        public ICollection<CourseCertificateTemplate> Template { get; set; }
        public ICollection<CourseCertificate> Background { get; set; }

        //public Course Course { get; set; }
    }

    //BACKGROUND
    public class CreateBackgroundModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
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
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Page Orientation")]
        public TypePageOrientation TypePageOrientation { get; set; }
        [Required]
        [AllowHtml]
        public string Template { get; set; }

        public CreateTemplateModel()
        {
            this.Template = "";
        }

        public CreateTemplateModel(string template)
        {
            this.Template = template;
        }
    }

    public class ReviewCertificateModel
    {
        public int CourseId { get; set; }
        public CourseCertificate Background { get; set; }
        public CourseCertificateTemplate Template { get; set; }

    }


    public class ViewCertificateModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
        public string DateCompleted { get; set; }
        public string StudentName { get; set; }
        public CourseCertificate Background { get; set; }
        public CourseCertificateTemplate Template { get; set; }

    }
}
