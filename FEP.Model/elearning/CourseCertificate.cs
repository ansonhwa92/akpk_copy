using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Language;

namespace FEP.Model.eLearning
{
    public class CourseCertificate : BaseEntity
    {        
        public string Description { get; set; }
        public string BackgroundImageFilename { get; set; }
        public int FileUploadId { get; set; }
        public virtual FileUpload FileUpload { get; set; }
        public TypePageOrientation TypePageOrientation { get; set; }
    }

    public class CourseCertificateTemplate : BaseEntity
    {
        public string Description { get; set; }
        public string Template { get; set; }
        public TypePageOrientation TypePageOrientation { get; set; }

    }

    public enum TypePageOrientation
    {
        Portrait,
        Landscape
    }
}
