using System.ComponentModel.DataAnnotations.Schema;

namespace FEP.Model.eLearning
{
    [Table("FileUpload")]
    public class FileUpload: BaseEntity
    {
        public int Order { get; set; }

        public FileType FileType { get; set; }
        public string FileName { get; set; }

        public string FilePath { get; set; }
        public int FileSize { get; set; } //Byte
        public string FileTag { get; set; }
        
        public string FileNameOnStorage { get; set; }

        [ForeignKey("CreatedBy")]
        public User User { get; set; }
    }
}
