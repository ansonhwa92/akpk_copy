using FEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eLearning
{
    public class FileDocumentModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int FileSize { get; set; } //Byte

        public string FileType { get; set; }

        public string FileTag { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public string FileNameOnStorage { get; set; }

        public User User { get; set; }

        public int CourseId { get; set; }
        public FileType ContentFileType { get; set; }
        public int ContentId { get; set; }
    }


}
