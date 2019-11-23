using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEP.Model
{
    [Table("KMCs")]
    public class KMCs
    {
        [Key]
        public int Id { get; set; }

        public int KMCCategoryId { get; set; }
        
        public string Thumbnail { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public bool IsShow { get; set; }

        public bool IsEditor { get; set; }

        public KMCType? KMCType { get; set; }

        public string EditorCode { get; set; }

        public int? FileId { get; set; }

        public string FileType { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("KMCCategoryId")]
        public virtual KMCCategory Category { get; set; }
        
        [ForeignKey("FileId")]
        public virtual FileDocument FileDoc { get; set; }

    }

    [Table("KMCUser")]
    public class KMCUser
    {
        [Key]
        public int Id { get; set; }

        public int KMCId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("KMCId")]
        public virtual KMCs KMC { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }


    [Table("KMCCategory")]
    public class KMCCategory
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        public bool Display { get; set; }
    }

    public enum KMCType
    {
        [Display(Name = "Image")]
        Image,
        [Display(Name = "Video")]        
        Video,
        [Display(Name = "Audio")]
        Audio,
        [Display(Name = "Document")]
        Document,        
        [Display(Name = "Others")]
        Others
    }

}
