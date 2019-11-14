using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FEP.Model
{
    [Table("KMC")]
    public class KMC
    {
        [Key]
        public int Id { get; set; }

        public int KMCCategoryId { get; set; }
        
        public int? ThumbnailId { get; set; }

        public string Title { get; set; }

        public KMCType KMCType { get; set; }

        public string EditorCode { get; set; }

        public int? FileId { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("KMCCategoryId")]
        public virtual KMCCategory Category { get; set; }

        [ForeignKey("ThumbnailId")]
        public virtual FileDocument Thumbnail { get; set; }

        [ForeignKey("FileId")]
        public virtual FileDocument FileDoc { get; set; }

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
        [Display(Name = "Editor")]
        HtmlEditor,
        [Display(Name = "Others")]
        Others
    }

}
