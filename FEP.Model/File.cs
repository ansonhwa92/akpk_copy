using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("FileDocument")]
    public class FileDocument
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int FileSize { get; set; } //Byte

        public string FileType { get; set; } 

        public string FileTag { get; set; }

        public string Directory { get; set; }
        
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Display { get; set; }

        [ForeignKey("CreatedBy")]
        public User User { get; set; }
    }

    public enum FileType
    {
        Document,
        Audio,
        Video,
        Image,
        Flash,
        Unknown
    }
}
