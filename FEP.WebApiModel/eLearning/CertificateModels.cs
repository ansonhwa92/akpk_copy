using FEP.Model;
using FEP.Model.eLearning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FEP.WebApiModel.eLearning
{
    public class CertificatesModel
    {
        public int Id { get; set; }
        public string Description { get; set; } 

        public string Url { get; set; }
        public HttpPostedFileBase File { get; set; }
        public int? FileDocumentId { get; set; }
        public FileUpload FileUpload { get; set; }
        public string UploadFileName { get; set; }
        public FileType FileType { get; set; }
        public string FilePath { get; set; }
    }

    public class CreateBackgroundModel
    {
        //[Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.EventCategory))]
        public string Description { get; set; }
        public string Url { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string FileName { get; set; }
        public FileUpload FileUpload { get; set; }

        public int? FileUploadId { get; set; }
        //public FileDocument FileDocument { get; set; }
        //public string UploadFileName { get; set; }
        //public FileType FileType { get; set; }
        //public string FilePath { get; set; }

    }

    public class EditCertificateModel
    {
        public int Id { get; set; }
        public string No { get; set; }

        //[Required(ErrorMessageResourceName = "ValidRequiredName", ErrorMessageResourceType = typeof(Language.EventCategory))]
        public string Name { get; set; }
    }

    public class DeleteCertificateModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string Name { get; set; }
    }
}
