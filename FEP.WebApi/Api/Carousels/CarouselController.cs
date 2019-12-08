using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Carousel;
using FEP.WebApiModel.FileDocuments;

namespace FEP.WebApi.Api.Carousels
{

    [Route("api/Carousels/Carousel")]
    public class CarouselController : ApiController
    {
        private DbEntities db = new DbEntities();

        public IHttpActionResult Get()
        {
            var carousels = db.Carousel.Where(u => u.Display && !u.IsDeleted).OrderBy( x => x.Sequence).Select(s => new CarouselModel
            {
                Id = s.Id,
                TextLocation = (FreeTextLocation)s.TextLocation,
                FreeTextArea = s.FreeTextArea
            }).ToList();

            foreach (var d in carousels)
            {

                var crsimages = db.CarouselImages.Where(i => i.CarouselID == d.Id).Select(s => new CarouselImagesModel
                {
                    ID = s.ID,
                    CoverPicture = s.CoverPicture
                }).FirstOrDefault();


                if (crsimages != null)
                {
                    if ((crsimages.CoverPicture != null) && (crsimages.CoverPicture != ""))
                    {
                        d.CarouselImage = crsimages.CoverPicture.Substring(crsimages.CoverPicture.LastIndexOf('\\') + 1);
                    }
                }

            }
            return Ok(carousels);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var carousel = db.Carousel.Where(u => u.Id == id).Select(s => new CreateCarouselModel
            {
                Title = s.Title,
                Description = s.Description, 
                Display = s.Display,
                DisplayDate = s.DisplayDate,
                Sequence = s.Sequence,
                TextLocation = (FreeTextLocation)s.TextLocation,
                FreeTextArea = s.FreeTextArea
            }).FirstOrDefault();

            if (carousel == null)
            {
                return NotFound();
            }

            carousel.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.CarouselFile.Where(p => p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            
            var crsimages = db.CarouselImages.Where(i => i.CarouselID == id).Select(s => new CarouselImagesModel
            {
                ID = s.ID,
                CarouselID = s.CarouselID,
                CoverPicture = s.CoverPicture
            }).FirstOrDefault();

            if (crsimages != null)
            {
                if ((crsimages.CoverPicture != null) && (crsimages.CoverPicture != ""))
                {
                    carousel.CarouselImage = crsimages.CoverPicture.Substring(crsimages.CoverPicture.LastIndexOf('\\') + 1);
                }
            }

            return Ok(carousel);
        }

        // Main DataTable function for listing and filtering
        // POST: api/Carousels/Carousel.GetAll (DataTable)
        [Route("api/Carousels/Carousel/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterCarouselModel request)
        {

            var query = db.Carousel.Where(p => !p.IsDeleted);   //TODO: all!!

            var totalCount = query.Count();

            //advance search
            //bool isconvertible = false;
            //int myType = 0;
            //isconvertible = int.TryParse(request.Type, out myType);

            /*
            query = query.Where(p => (request.Type == null || p.Category.Name.Contains(request.Type))
               && (request.Author == null || p.Author.Contains(request.Author))
               && (request.Title == null || p.Title.Contains(request.Title))
               && (request.ISBN == null || p.ISBN.Contains(request.ISBN))
               );
            */
            query = query.Where(p => (request.Title == null || p.Title.Contains(request.Title))
               && (request.Description == null || p.Description.Contains(request.Description))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Title.Contains(value)
                || p.Description.Contains(value)
                );
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "Title":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Title);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Title);
                        }

                        break;

                    case "Description":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Description);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Description);
                        }

                        break;

                    default:
                        query = query.OrderBy(o => o.Id).OrderBy(o => o.Title);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(o => o.Id).OrderBy(o => o.Title);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new CreateCarouselModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Sequence = s.Sequence
                }).ToList();

            foreach (var d in data)
            {

                var crsimages = db.CarouselImages.Where(i => i.CarouselID == d.Id).Select(s => new CarouselImagesModel
                {
                    ID = s.ID,
                    CarouselID = s.CarouselID,
                    CoverPicture = s.CoverPicture
                }).FirstOrDefault();


                if (crsimages != null)
                {
                    if ((crsimages.CoverPicture != null) && (crsimages.CoverPicture != ""))
                    {
                        d.CarouselImage = crsimages.CoverPicture.Substring(crsimages.CoverPicture.LastIndexOf('\\') + 1);
                    }
                }

            }
            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        // Function to save a publication (as draft) after creating a new one.
        // POST: api/Carousels/Carousel/Create
        [Route("api/Carousels/Carousel/Create")]
        [HttpPost]
        //[ValidationActionFilter]
        public string Create([FromBody] CreateCarouselModelNoFile model)
        {

            if (ModelState.IsValid)
            {
                int seq = 0;
                if ( model.Sequence == 0)
                {
                    var record = db.Carousel.FirstOrDefault(c => !c.IsDeleted);
                    
                    if(record != null)
                    {
                        seq = db.Carousel.Where(c => !c.IsDeleted).Max(x => x.Sequence);
                    }
                }

                seq++;

                var carousel = new Carousel
                {
                    Title = model.Title,
                    Description = model.Description,
                    Sequence = seq,
                    Display = (model.Display) ? true : false,
                    DisplayDate = model.DisplayDate,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = model.CreatedBy,
                    TextLocation = (int)model.TextLocation,
                    FreeTextArea = model.FreeTextArea
                };

                db.Carousel.Add(carousel);
                db.SaveChanges();

                //files 1
                foreach (var fileid in model.CoverFilesId)
                {
                    var coverfile = new CarouselFile
                    {
                        FileId = fileid,
                        ParentId = carousel.Id
                    };

                    db.CarouselFile.Add(coverfile);
                }

                // modify carousel by adding ref no based on year, month and new ID (Survey= SVP & SVT)
                var refno = "CRS/" + DateTime.Now.ToString("yyMM");
                refno += "/" + carousel.Id.ToString("D4");
                carousel.RefNo = refno;

                db.Entry(carousel).State = EntityState.Modified;
                db.SaveChanges();

                return carousel.Id.ToString() + "|" + model.Title;
            }
            return "";
        }

        [Route("api/Carousels/Carousel/Edit")]
        [HttpPost]
        [ValidationActionFilter]
        public string Edit([FromBody] CreateCarouselModel model)
        {

            if (ModelState.IsValid)
            {
                var carousel = db.Carousel.Where(p => p.Id == model.Id).FirstOrDefault();

                if (carousel != null)
                {
                    carousel.Title = model.Title;
                    carousel.Description = model.Description;
                    carousel.Display = model.Display;
                    carousel.DisplayDate = model.DisplayDate;
                    carousel.TextLocation = (int)model.TextLocation;
                    carousel.FreeTextArea = model.FreeTextArea;
                    carousel.LastModifiedBy = model.LastModifiedBy;
                    carousel.LastModifiedDate = DateTime.Now;

                    db.Entry(carousel).State = EntityState.Modified;

                    //files 1

                    var attachments1 = db.CarouselFile.Where(s => s.ParentId == model.Id).ToList();

                    if (attachments1 != null)
                    {
                        if (model.CoverPictures == null)
                        {
                            foreach (var attachment in attachments1)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.CarouselFile.Remove(attachment);
                            }
                        }
                        else
                        {
                            foreach (var attachment in attachments1)
                            {
                                if (!model.CoverPictures.Any(u => u.Id == attachment.FileDocument.Id))
                                {
                                    attachment.FileDocument.Display = false;
                                    db.FileDocument.Attach(attachment.FileDocument);
                                    db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                    db.CarouselFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.CoverFilesId)
                    {
                        var coverfile = new CarouselFile
                        {
                            FileId = fileid,
                            ParentId = carousel.Id
                        };

                        db.CarouselFile.Add(coverfile);
                    }

                    db.SaveChanges();

                    return model.Title;
                }
            }
            return "";
        }

        [Route("api/Carousels/Carousel/UpdateSequence")]
        [HttpPost]
        public string UpdateSequence([FromBody] CarouselModel model)
        {            
            var carousel = db.Carousel.Where(p => p.Id == model.Id).FirstOrDefault();
            string ptitle = string.Empty;
            if (carousel != null)
            {
                int currentSeq = carousel.Sequence;

                ptitle = carousel.Title;

                var beforeCurrentCarousel = db.Carousel.Where(p => p.Sequence < currentSeq).FirstOrDefault();
                if (beforeCurrentCarousel == null)
                {
                    int carouselSeq = carousel.Sequence;

                    var carouselList = db.Carousel.Where(p => (p.Sequence > currentSeq && p.Sequence <= model.Sequence));

                    foreach (var cl in carouselList)
                    {
                        cl.Sequence = carouselSeq;
                        carouselSeq++;
                    }
                }
                else
                {
                    int carouselSeq = carousel.Sequence;

                    var carouselList = db.Carousel.Where(p => (p.Sequence >= model.Sequence && p.Sequence < currentSeq ));

                    foreach (var cl in carouselList)
                    {
                        cl.Sequence = carouselSeq;
                        carouselSeq++;
                    }
                }

                carousel.Sequence = model.Sequence;

                db.Entry(carousel).State = EntityState.Modified;

                db.SaveChanges();

                return model.Id.ToString();
            }
            return "";
        }
        // GET: api/Carousels/Carousel/UploadImages
        [Route("api/Carousels/Carousel/UploadImages")]
        [HttpGet]
        public int UploadImages(int pubid, string coverpic)
        {
            var crsimages = new CarouselImages
            {
                CarouselID = pubid,
                CoverPicture = coverpic
            };

            db.CarouselImages.Add(crsimages);
            db.SaveChanges();

            return crsimages.ID;
        }

        // GET: api/Carousels/Carousel/UpdateImages
        [Route("api/Carousels/Carousel/UpdateImages")]
        [HttpGet]
        public int UpdateImages(int pubid, string coverpic)
        {
            var crsimages = db.CarouselImages.Where(pi => pi.CarouselID == pubid).FirstOrDefault();

            if (crsimages != null)
            {
                crsimages.CoverPicture = coverpic;
                db.Entry(crsimages).State = EntityState.Modified;
                db.SaveChanges();

                return crsimages.ID;
            }

            return 0;
        }

        // GET: api/Carousels/Carousel/UpdateImagePublicationID
        [Route("api/Carousels/Carousel/UpdateImageCarouselID")]
        [HttpGet]
        public int UpdateImageCarouselID(int id, int pubid)
        {
            var crsimages = db.CarouselImages.Where(pi => pi.ID == id).FirstOrDefault();

            if (crsimages != null)
            {
                crsimages.CarouselID = pubid;
                db.Entry(crsimages).State = EntityState.Modified;
                db.SaveChanges();

                return crsimages.ID;
            }

            return 0;
        }

        [ValidationActionFilter]
        public IHttpActionResult Post(CarouselModel model)
        {
            int seq = 0;
            var record = db.Carousel.Where(r => !r.IsDeleted).OrderByDescending(x => x.Sequence).FirstOrDefault();

            if (record != null)
                seq = record.Sequence;

            seq++;

            var carousel = new Carousel
            {
                CarouselImage = model.CarouselImage,
                Title = model.Title,
                Description = model.Description,
                DeletedBy = model.DeletedBy,
                DeletedDate = (model.DeletedBy != null) ? model.DeletedDate : null,
                Display = (model.Display) ? true : false,
                FreeTextArea = model.FreeTextArea,
                Sequence = seq,
                TextLocation = (int)model.TextLocation,
                DisplayDate = (model.DisplayDate != null) ? model.DisplayDate : DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedBy = model.CreatedBy,
                LastModifiedBy = model.CreatedBy
            };

            db.Carousel.Add(carousel);

            db.SaveChanges();

            return Ok(carousel.Id);
        }

        [ValidationActionFilter]
        public IHttpActionResult Put(CarouselModel model)
        {
            var carousel = db.Carousel.Where(r => r.Id == model.Id && !r.IsDeleted).FirstOrDefault();

            if (carousel == null)
            {
                return NotFound();
            }

            carousel.Title = model.Title;
            carousel.Description = model.Description;
            carousel.FreeTextArea = model.FreeTextArea;

            db.Entry(carousel).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(true);
        }

        [Route("api/Carousels/Carousel/Delete")]
        //[HttpPost]
        public string Delete(int id)
        {
            var carousel = db.Carousel.Where(r => r.Id == id && !r.IsDeleted).FirstOrDefault();

            if (carousel != null)
            {
                int carouselSeq = carousel.Sequence;
                string ptitle = carousel.Title;

                var carouselFiles = db.CarouselFile.Where(s => s.ParentId == carousel.Id);

                foreach (var cf in carouselFiles)
                {
                    db.CarouselFile.Remove(cf);
                }

                var carouselImages = db.CarouselImages.Where(s => s.CarouselID == carousel.Id);

                foreach (var ci in carouselImages)
                {
                    db.CarouselImages.Remove(ci);
                }

                var carouselList = db.Carousel.Where(p => p.Sequence > carouselSeq);

                foreach (var cl in carouselList)
                {
                    cl.Sequence = carouselSeq;
                    carouselSeq++;
                }

                // delete record
                db.Carousel.Remove(carousel);
                db.SaveChanges();

                return ptitle;
            }

            return "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarouselExists(int id)
        {
            return db.Carousel.Count(e => e.Id == id) > 0;
        }
    }
}