using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
    [Table("File")]
    public class File
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int FileSize { get; set; } //Byte

        public FileType FileType { get; set; } 

        public string FileTag { get; set; }

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
