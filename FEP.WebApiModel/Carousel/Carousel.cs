using FEP.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using System.Web;
using System.Web.Mvc;

namespace FEP.WebApiModel.Carousel
{
    public class CarouselModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name ="Carousel Image")]
        public string CarouselImage { get; set; }
        public bool Display { get; set; }
        [Display(Name = "Display Start Date")]
        public DateTime? DisplayDate { get; set; }
        public int Sequence { get; set; }
        [AllowHtml]
        [Display(Name = "Free Text Area")]
        public string FreeTextArea { get; set; }
        [Display(Name = "Free Text Location")]
        public FreeTextLocation TextLocation { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }

    public class CarouselImagesModel
    {
        public int ID { get; set; }
        public int CarouselID { get; set; }
        public string CoverPicture { get; set; }
    }
    // for creating
    public class CreateCarouselModel : CarouselModel
    {
        public CreateCarouselModel()
        {
            CoverPictures = new List<Attachment>();
            CoverPictureFiles = new List<HttpPostedFileBase>();
            CoverFilesId = new List<int>();
        }

        [Display(Name = "CoverPictures")]
        public IEnumerable<Attachment> CoverPictures { get; set; }
        public IEnumerable<HttpPostedFileBase> CoverPictureFiles { get; set; }

        public List<int> CoverFilesId { get; set; }
    }

    // for editing
    public class EditCarouselModel : CarouselModel
    {
        public EditCarouselModel()
        {
            CoverPictures = new List<Attachment>();
            CoverPictureFiles = new List<HttpPostedFileBase>();
            CoverFilesId = new List<int>();
        }

        [Required]
        public int ID { get; set; }

        [Display(Name = "CoverPictures")]
        public IEnumerable<Attachment> CoverPictures { get; set; }
        public IEnumerable<HttpPostedFileBase> CoverPictureFiles { get; set; }

        public List<int> CoverFilesId { get; set; }
    }

    public class CarouselModelNoFile
    {
        [Required(ErrorMessageResourceName = "ValidRequiredTitle", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "ValidRequiredDescription", ErrorMessageResourceType = typeof(Language.RnPForm))]
        public string Description { get; set; }

        public string CarouselImage { get; set; }
        public bool Display { get; set; }
        public DateTime? DisplayDate { get; set; }
        public int Sequence { get; set; }
        public string FreeTextArea { get; set; }
        public FreeTextLocation TextLocation { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }

    // for details

    // class for updating of publication information by client app
    // used to create and edit publication information
    public class DetailsCarouselModel : CarouselModelNoFile
    {
        public DetailsCarouselModel() { }

        [Required]
        public int ID { get; set; }

        [Display(Name = "CarouselPictures", ResourceType = typeof(Language.RnPForm))]
        public IEnumerable<Attachment> CoverPictures { get; set; }
        
        [Display(Name = "CarouselPictures", ResourceType = typeof(Language.RnPForm))]
        public string CoverPicture { get; set; }
    }

    // for creating
    public class CreateCarouselModelNoFile : CarouselModelNoFile
    {
        public CreateCarouselModelNoFile()
        {
            CoverPictures = new List<Attachment>();
        }

        public IEnumerable<Attachment> CoverPictures { get; set; }

        public List<int> CoverFilesId { get; set; }
    }
    //for editing

    public class EditCarouselModelNoFile : CarouselModelNoFile
    {
        public EditCarouselModelNoFile()
        {
            CoverPictures = new List<Attachment>();
        }

        [Required]
        public int Id { get; set; }

        public IEnumerable<Attachment> CoverPictures { get; set; }

        public List<int> CoverFilesId { get; set; }
    }
    public class ListCarouselModel
    {
        public FilterCarouselModel Filter { get; set; }

        public CarouselModel List { get; set; }
    }

    // class for setting and returning filters for the datatable list of publications
    public class FilterCarouselModel : DataTableModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Carousel Image")]
        public string CarouselImage { get; set; }
    }
}
