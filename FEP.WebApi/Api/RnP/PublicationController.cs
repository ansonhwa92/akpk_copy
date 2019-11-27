using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.RnP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web;
using FEP.WebApiModel.SLAReminder;
using FEP.WebApiModel.FileDocuments;


namespace FEP.WebApi.Api.RnP
{
    [Route("api/RnP/Publication")]
    public class PublicationController : ApiController
    {
        private DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*
         * The following API calls are for Publication record retrieval operations, including:
         * 1. Admin views/filters list of publications
         * 2. Admin retrieves list of publication types for selection
         * 3. Admin retrieves single publication for viewing (includes history)
         * 4. Admin retrieves single publication for review before submission
         * 5. Verifier/Approvers evaluate a publication in order to approve/reject it
         */

        // Main DataTable function for listing and filtering
        // POST: api/RnP/Publication.GetAll (DataTable)
        [Route("api/RnP/Publication/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterPublicationModel request)
        {

            var query = db.Publication.Where(p => p.Status <= PublicationStatus.WithdrawalTrashed);   //TODO: all!!

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
            query = query.Where(p => (request.Author == null || p.Author.Contains(request.Author))
               && (request.Title == null || p.Title.Contains(request.Title))
               && (request.ISBN == null || p.ISBN.Contains(request.ISBN))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Author.Contains(value)
                || p.Title.Contains(value)
                || p.ISBN.Contains(value)
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
                    case "RefNo":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.RefNo);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.RefNo);
                        }

                        break;

                    case "Category":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Category.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Category.Name);
                        }

                        break;

                    case "Author":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Author);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Author);
                        }

                        break;

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

                    case "ISBN":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.ISBN);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.ISBN);
                        }

                        break;

                    case "Status":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Status);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Status);
                        }

                        break;

                    default:
                        query = query.OrderBy(o => o.Category.Name).OrderBy(o => o.Title);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(o => o.Category.Name).OrderBy(o => o.Title);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new ReturnBriefPublicationModel
                {
                    ID = s.ID,
                    RefNo = s.RefNo,
                    Author = s.Author,
                    Title = s.Title,
                    ISBN = s.ISBN,
                    Status = s.Status,
                    Category = s.Category.Name,
                    ApprovalLevel = db.PublicationApproval.Where(pa => pa.PublicationID == s.ID && pa.Status == PublicationApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level,
                    WithdrawalLevel = db.PublicationWithdrawal.Where(pw => pw.PublicationID == s.ID && pw.Status == PublicationApprovalStatus.None).OrderByDescending(pw => pw.ApprovalDate).FirstOrDefault().Level
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        // Alternative function for listing (all)
        // GET: api/RnP/Publication (misc list) - CURRENTLY USED FOR ANONYMOUS BROWSING
        public List<ReturnPublicationModel> Get()
        {
            var publications = db.Publication.Where(p => p.Status == PublicationStatus.Published).OrderByDescending(p => p.DateAdded).OrderBy(p => p.Title).Select(s => new ReturnPublicationModel
            {
                ID = s.ID,
                CategoryID = s.CategoryID,
                Author = s.Author,
                Coauthor = s.Coauthor,
                Title = s.Title,
                Year = s.Year,
                Description = s.Description,
                Language = s.Language,
                ISBN = s.ISBN,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                FreeHCopy = s.FreeHCopy,
                FreeDCopy = s.FreeDCopy,
                FreeHDCopy = s.FreeHDCopy,
                HDcopy = s.HDcopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                //ProofOfWithdrawal = s.ProofOfWithdrawal,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DownloadCount = s.DownloadCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).ToList();

            return publications;
        }

        // GET: api/RnP/Publication/GetPublications (list) - CURRENTLY USED FOR ANONYMOUS BROWSING
        [Route("api/RnP/Publication/GetPublications")]
        [HttpGet]
        public BrowsePublicationModel GetPublications(string keyword, string sorting, bool articles, bool books, bool factssheet, bool journals, bool reviews, bool reports, bool researchpapers, bool digital, bool hardcopy, bool bahasamalaysia, bool english)
        {
            // active (for now = published) only

            var query = db.Publication.Where(p => p.Status == PublicationStatus.Published);

            var totalCount = query.Count();

            query = query.Where(p => (keyword == null || keyword == ""
                || p.Title.Contains(keyword)
                || p.Author.Contains(keyword) || p.Coauthor.Contains(keyword)
                || p.ISBN.Contains(keyword)));

            if (!articles) { query = query.Where(p => p.CategoryID != 1); }
            if (!books) { query = query.Where(p => p.CategoryID != 2); }
            if (!factssheet) { query = query.Where(p => p.CategoryID != 3); }
            if (!journals) { query = query.Where(p => p.CategoryID != 4); }
            if (!reviews) { query = query.Where(p => p.CategoryID != 5); }
            if (!reports) { query = query.Where(p => p.CategoryID != 6); }
            if (!researchpapers) { query = query.Where(p => p.CategoryID != 7); }

            if (digital) { query = query.Where(p => p.Digitalcopy == true); }
            if (hardcopy) { query = query.Where(p => p.Hardcopy == true); }

            if (bahasamalaysia) { query = query.Where(p => p.Language.Contains("Bahasa Malaysia")); }
            if (english) { query = query.Where(p => p.Language.Contains("English")); }

            var filteredCount = query.Count();

            if (sorting == "title")
            {
                query = query.OrderBy(o => o.Title).OrderByDescending(o => o.Year);
            }
            else if (sorting == "year")
            {
                query = query.OrderByDescending(o => o.Year).OrderBy(o => o.Title);
            }
            else if (sorting == "added")
            {
                query = query.OrderByDescending(o => o.DateAdded).OrderBy(o => o.Title);
            }
            else
            {
                query = query.OrderBy(o => o.Title).OrderByDescending(o => o.Year);
            }

            var data = query.Skip(0).Take(filteredCount).Select(s => new ReturnPublicationModel
            {
                ID = s.ID,
                CategoryID = s.CategoryID,
                Author = s.Author,
                Coauthor = s.Coauthor,
                Title = s.Title,
                Year = s.Year,
                Description = s.Description,
                Language = s.Language,
                ISBN = s.ISBN,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                FreeHCopy = s.FreeHCopy,
                FreeDCopy = s.FreeDCopy,
                FreeHDCopy = s.FreeHDCopy,
                HDcopy = s.HDcopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                //ProofOfWithdrawal = s.ProofOfWithdrawal,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DownloadCount = s.DownloadCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).ToList();

            //var di = -1;
            foreach (var publication in data)
            {
                //di++;
                var pubimages = db.PublicationImages.Where(i => i.PublicationID == publication.ID).Select(s => new PublicationImagesModel
                {
                    ID = s.ID,
                    PublicationID = s.PublicationID,
                    CoverPicture = s.CoverPicture,
                    AuthorPicture = s.AuthorPicture
                }).FirstOrDefault();

                if (pubimages != null)
                {
                    if ((pubimages.CoverPicture != null) && (pubimages.CoverPicture != ""))
                    {
                        publication.CoverPicture = pubimages.CoverPicture.Substring(pubimages.CoverPicture.LastIndexOf('\\') + 1);
                    }
                    if ((pubimages.AuthorPicture != null) && (pubimages.AuthorPicture != ""))
                    {
                        publication.AuthorPicture = pubimages.AuthorPicture.Substring(pubimages.AuthorPicture.LastIndexOf('\\') + 1);
                    }
                }
            }

            var browser = new BrowsePublicationModel
            {
                Keyword = keyword,
                Sorting = sorting,
                LastIndex = filteredCount,
                ItemCount = totalCount,
                Publications = data
            };

            return browser;
        }

        // Function to get a single publication
        // NOTE: THE WAY INFO IS RETURNED HERE MAY NEED TO BE USED FOR ALL GET FUNCTIONS IN THE FUTURE
        // GET: api/RnP/Publication/5
        //public ReturnPublicationModel Get(int id)
        public IHttpActionResult Get(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).Select(s => new ReturnPublicationModel
            {
                ID = s.ID,
                CategoryID = s.CategoryID,
                Author = s.Author,
                Coauthor = s.Coauthor,
                Title = s.Title,
                Year = s.Year,
                Description = s.Description,
                Language = s.Language,
                ISBN = s.ISBN,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                FreeHCopy = s.FreeHCopy,
                FreeDCopy = s.FreeDCopy,
                FreeHDCopy = s.FreeHDCopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                //ProofOfWithdrawal = s.ProofOfWithdrawal,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DownloadCount = s.DownloadCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            if (publication == null)
            {
                return NotFound();
            }

            publication.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.DigitalPublications = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.DigitalPublication && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            var pubimages = db.PublicationImages.Where(i => i.PublicationID == id).Select(s => new PublicationImagesModel
            {
                ID = s.ID,
                PublicationID = s.PublicationID,
                CoverPicture = s.CoverPicture,
                AuthorPicture = s.AuthorPicture
            }).FirstOrDefault();

            if (pubimages != null)
            {
                if ((pubimages.CoverPicture != null) && (pubimages.CoverPicture != ""))
                {
                    publication.CoverPicture = pubimages.CoverPicture.Substring(pubimages.CoverPicture.LastIndexOf('\\') + 1);
                }
                if ((pubimages.AuthorPicture != null) && (pubimages.AuthorPicture != ""))
                {
                    publication.AuthorPicture = pubimages.AuthorPicture.Substring(pubimages.AuthorPicture.LastIndexOf('\\') + 1);
                }
            }

            return Ok(publication);
            //return publication;
        }

        // Function to get list of publication categories for user selection.
        // This function is called at the very start whenever the Admin is creating a new publiction. The category
        // selected by the admin will be passed to the next page which is the actual publication creation page.
        // GET: api/RnP/Publication/GetCategories
        [Route("api/RnP/Publication/GetCategories")]
        public List<ReturnPublicationCategory> GetCategories()
        {
            var pubcats = db.PublicationCategory.OrderBy(pc => pc.ID).Select(s => new ReturnPublicationCategory
            {
                ID = s.ID,
                Name = s.Name
            }).ToList();

            return pubcats;
        }

        // Function to get publication details for review before submission. The details retrieved include action
        // log history. This function is called for the third page of publication creation/editing.
        // GET: api/RnP/Publication/GetForReview/5
        [Route("api/RnP/Publication/GetForReview")]
        public ReturnPublicationModel GetForReview(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).Select(s => new ReturnPublicationModel
            {
                ID = s.ID,
                CategoryID = s.CategoryID,
                Author = s.Author,
                Coauthor = s.Coauthor,
                Title = s.Title,
                Year = s.Year,
                Description = s.Description,
                Language = s.Language,
                ISBN = s.ISBN,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                FreeHCopy = s.FreeHCopy,
                FreeDCopy = s.FreeDCopy,
                FreeHDCopy = s.FreeHDCopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                //ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DownloadCount = s.DownloadCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

            publication.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.DigitalPublications = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.DigitalPublication && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            return publication;
        }

        // Function to get publication info for viewing by admin.
        // This function has to handle submission, deletion (direct) or cancellation by admin
        // Admin can either submit from here or from create/edit review page. Admin can also delete publication from here
        // if still at draft stage or cancel it if at rejected stage (TBC - can admin cancel during review?)
        // GET: api/RnP/Publication/GetForView/5
        [Route("api/RnP/Publication/GetForView")]
        public ReturnPublicationModel GetForView(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).Select(s => new ReturnPublicationModel
            {
                ID = s.ID,
                CategoryID = s.CategoryID,
                Author = s.Author,
                Coauthor = s.Coauthor,
                Title = s.Title,
                Year = s.Year,
                Description = s.Description,
                Language = s.Language,
                ISBN = s.ISBN,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                FreeHCopy = s.FreeHCopy,
                FreeDCopy = s.FreeDCopy,
                FreeHDCopy = s.FreeHDCopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                //ProofOfWithdrawal = s.ProofOfWithdrawal,
                WithdrawalDate = s.WithdrawalDate,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DownloadCount = s.DownloadCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

            publication.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.ProofOfWithdrawal = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.ProofOfWithdrawal && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.DigitalPublications = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.DigitalPublication && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            return publication;
        }

        // Function to get publication info for evaluation (approve/reject) by verifier or approver.
        // Verifier/approver can approve (submit for approval) or reject (require amendment).
        // GET: api/RnP/Publication/GetForEvaluation/5
        [Route("api/RnP/Publication/GetForEvaluation")]
        public ReturnPublicationApprovalModel GetForEvaluation(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).Select(s => new ReturnPublicationModel
            {
                ID = s.ID,
                CategoryID = s.CategoryID,
                Author = s.Author,
                Coauthor = s.Coauthor,
                Title = s.Title,
                Year = s.Year,
                Description = s.Description,
                Language = s.Language,
                ISBN = s.ISBN,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                FreeHCopy = s.FreeHCopy,
                FreeDCopy = s.FreeDCopy,
                FreeHDCopy = s.FreeHDCopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                //ProofOfWithdrawal = s.ProofOfWithdrawal,
                WithdrawalDate = s.WithdrawalDate,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DownloadCount = s.DownloadCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

            publication.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.DigitalPublications = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.DigitalPublication && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            var papproval = db.PublicationApproval.Where(pa => pa.PublicationID == id && pa.Status == PublicationApprovalStatus.None).Select(s => new ReturnUpdatePublicationApprovalModel
            {
                ID = s.ID,
                PublicationID = s.PublicationID,
                Level = s.Level,
                ApproverId = 0,
                Status = PublicationApprovalStatus.None,
                Remarks = "",
                RequireNext = s.RequireNext
            }).FirstOrDefault();

            var pevaluation = new ReturnPublicationApprovalModel
            {
                Pub = publication,
                Approval = papproval
            };

            return pevaluation;
        }

        // Function to get publication info for withdrawal evaluation (approve/reject) by verifier or approver.
        // Verifier/approver can approve (submit for approval) or reject (require amendment).
        // GET: api/RnP/Publication/GetForWithdrawalEvaluation/5
        [Route("api/RnP/Publication/GetForWithdrawalEvaluation")]
        public ReturnPublicationWithdrawalModel GetForWithdrawalEvaluation(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).Select(s => new ReturnPublicationModel
            {
                ID = s.ID,
                CategoryID = s.CategoryID,
                Author = s.Author,
                Coauthor = s.Coauthor,
                Title = s.Title,
                Year = s.Year,
                Description = s.Description,
                Language = s.Language,
                ISBN = s.ISBN,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                FreeHCopy = s.FreeHCopy,
                FreeDCopy = s.FreeDCopy,
                FreeHDCopy = s.FreeHDCopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                //ProofOfWithdrawal = s.ProofOfWithdrawal,
                WithdrawalDate = s.WithdrawalDate,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DownloadCount = s.DownloadCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

            publication.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.ProofOfWithdrawal = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.ProofOfWithdrawal && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            publication.DigitalPublications = db.FileDocument.Where(f => f.Display).Join(db.PublicationFile.Where(p => p.FileCategory == PublicationFileCategory.DigitalPublication && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            var pwithdrawal = new UpdatePublicationWithdrawalModel
            {
                ID = publication.ID,
                WithdrawalReason = publication.WithdrawalReason,
                ProofOfWithdrawal = publication.ProofOfWithdrawal
            };

            var papproval = db.PublicationWithdrawal.Where(pa => pa.PublicationID == id && pa.Status == PublicationApprovalStatus.None).Select(s => new ReturnUpdatePublicationApprovalModel
            {
                ID = s.ID,
                PublicationID = s.PublicationID,
                Level = s.Level,
                ApproverId = 0,
                Status = PublicationApprovalStatus.None,
                Remarks = "",
                RequireNext = s.RequireNext
            }).FirstOrDefault();

            var pevaluation = new ReturnPublicationWithdrawalModel
            {
                Pub = publication,
                Withdrawal = pwithdrawal,
                Approval = papproval
            };

            return pevaluation;
        }

        // Function to get publication creation/approval history (action log).
        // Only completed approvals are listed. Usually called as the second call after retrieval of publication information.
        // GET: api/RnP/Publication/GetHistory/5
        [Route("api/RnP/Publication/GetHistory")]
        public List<PublicationApprovalHistoryModel> GetHistory(int id)
        {
            var phistory = db.PublicationApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { pa.PublicationID, pa.Level, pa.ApproverId, pa.ApprovalDate, pa.Status, pa.Remarks, UserName = u.Name }).Where(pa => pa.PublicationID == id && pa.Status != PublicationApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).Select(s => new PublicationApprovalHistoryModel
            {
                Level = s.Level,
                ApproverId = s.ApproverId,
                ApprovalDate = s.ApprovalDate,
                UserName = s.UserName,
                Status = s.Status,
                Remarks = s.Remarks
            }).ToList();

            return phistory;
        }

        // Function to get publication withdrawal approval history (action log part 2).
        // Only completed approvals are listed. Usually called as the second call after retrieval of publication information.
        // GET: api/RnP/Publication/GetWithdrawalHistory/5
        [Route("api/RnP/Publication/GetWithdrawalHistory")]
        public List<PublicationWithdrawalHistoryModel> GetWithdrawalHistory(int id)
        {
            //var phistory = db.PublicationApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { tapproval = pa, tuser = u }).Where(pau => pau.tapproval.PublicationID == id && pau.tapproval.Status != PublicationApprovalStatus.None).OrderByDescending(pau => pau.tapproval.ApprovalDate).Select(s => new PublicationApprovalHistoryModel
            var phistory = db.PublicationWithdrawal.Join(db.User, pw => pw.ApproverId, u => u.Id, (pw, u) => new { pw.PublicationID, pw.Level, pw.ApproverId, pw.ApprovalDate, pw.Status, pw.Remarks, UserName = u.Name }).Where(pw => pw.PublicationID == id && pw.Status != PublicationApprovalStatus.None).OrderByDescending(pw => pw.ApprovalDate).Select(s => new PublicationWithdrawalHistoryModel
            {
                Level = s.Level,
                ApproverId = s.ApproverId,
                ApprovalDate = s.ApprovalDate,
                UserName = s.UserName,
                Status = s.Status,
                Remarks = s.Remarks
            }).ToList();

            return phistory;
        }

        // Function to get the next pending approval record
        // This is used to check for who's in charge of the next step in approval
        // GET: api/RnP/Publication/GetNextApproval/5
        [Route("api/RnP/Publication/GetNextApproval")]
        public PublicationApprovalHistoryModel GetNextApproval(int id)
        {
            //var phistory = db.PublicationApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { tapproval = pa, tuser = u }).Where(pau => pau.tapproval.PublicationID == id && pau.tapproval.Status != PublicationApprovalStatus.None).OrderByDescending(pau => pau.tapproval.ApprovalDate).Select(s => new PublicationApprovalHistoryModel
            var phistory = db.PublicationApproval.Where(pa => pa.PublicationID == id && pa.Status == PublicationApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).Select(s => new PublicationApprovalHistoryModel
            {
                Level = s.Level,
                ApproverId = s.ApproverId,
                ApprovalDate = s.ApprovalDate,
                Status = s.Status,
                Remarks = s.Remarks
            }).FirstOrDefault();

            return phistory;
        }

        // Function to get the next pending withdrawal approval record
        // This is used to check for who's in charge of the next step in approval
        // GET: api/RnP/Publication/GetNextWithdrawalApproval/5
        [Route("api/RnP/Publication/GetNextWithdrawalApproval")]
        public PublicationWithdrawalHistoryModel GetNextWithdrawalApproval(int id)
        {
            //var phistory = db.PublicationApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { tapproval = pa, tuser = u }).Where(pau => pau.tapproval.PublicationID == id && pau.tapproval.Status != PublicationApprovalStatus.None).OrderByDescending(pau => pau.tapproval.ApprovalDate).Select(s => new PublicationApprovalHistoryModel
            var phistory = db.PublicationWithdrawal.Where(pw => pw.PublicationID == id && pw.Status == PublicationApprovalStatus.None).OrderByDescending(pw => pw.ApprovalDate).Select(s => new PublicationWithdrawalHistoryModel
            {
                Level = s.Level,
                ApproverId = s.ApproverId,
                ApprovalDate = s.ApprovalDate,
                Status = s.Status,
                Remarks = s.Remarks
            }).FirstOrDefault();

            return phistory;
        }

        // Function to check if publication title and author combo exists
        [Route("api/RnP/Publication/TitleExists")]
        [HttpGet]
        public IHttpActionResult TitleExists(int? id, string title, string author)
        {

            if (id == null)
            {
                if (db.Publication.Any(p => p.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && p.Author.Equals(author, StringComparison.CurrentCultureIgnoreCase) && p.Status != PublicationStatus.Trashed))
                    return Ok(true);
            }
            else
            {
                if (db.Publication.Any(p => p.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && p.Author.Equals(author, StringComparison.CurrentCultureIgnoreCase) && p.ID != id && p.Status != PublicationStatus.Trashed))
                    return Ok(true);
            }

            return NotFound();
        }

        // Function to check if publication ISBN/ISSN/DOI exists
        [Route("api/RnP/Publication/ISBNExists")]
        [HttpGet]
        public IHttpActionResult ISBNExists(int? id, string isbn)
        {

            if (id == null)
            {
                if (db.Publication.Any(p => p.ISBN.Equals(isbn, StringComparison.CurrentCultureIgnoreCase) && p.Status != PublicationStatus.Trashed))
                    return Ok(true);
            }
            else
            {
                if (db.Publication.Any(p => p.ISBN.Equals(isbn, StringComparison.CurrentCultureIgnoreCase) && p.ID != id && p.Status != PublicationStatus.Trashed))
                    return Ok(true);
            }

            return NotFound();
        }

        /*
         * The following API calls are for Publication record modification operations, including:
         * Note: Selection of publication category is handled by Intranet controller
         * 1. Admin saves publication as draft after creating
         * 2. Admin saves publication as draft after editing (auto when clicking next?)
         * 3. Admin submits publication for approval after reviewing
         * 4. Admin deletes publication
         * 5. Verifier/Approvers approve/reject the publication after evaluation
         * 6. Admin cancels publication after verifier/approver rejects
         */

        /*
        // POST: api/RnP/Publication
        public HttpResponseMessage Post([FromBody]string value)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }
        */

        // Function to save a publication (as draft) after creating a new one.
        // POST: api/RnP/Publication/Create
        [Route("api/RnP/Publication/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public string Create([FromBody] CreatePublicationModelNoFile model)
        {

            if (ModelState.IsValid)
            {
                var publication = new Publication
                {
                    CategoryID = model.CategoryID,
                    Author = model.Author,
                    Coauthor = model.Coauthor,
                    Title = model.Title,
                    Year = model.Year,
                    Description = model.Description,
                    Language = model.Language,
                    ISBN = model.ISBN,
                    Hardcopy = model.Hardcopy,
                    Digitalcopy = model.Digitalcopy,
                    HDcopy = model.HDcopy,
                    FreeHCopy = model.FreeHCopy,
                    FreeDCopy = model.FreeDCopy,
                    FreeHDCopy = model.FreeHDCopy,
                    HPrice = model.HPrice,
                    DPrice = model.DPrice,
                    HDPrice = model.HDPrice,
                    StockBalance = model.StockBalance,
                    WithdrawalReason = "",
                    //ProofOfWithdrawal = "",
                    CancelRemark = "",
                    DateAdded = DateTime.Now,
                    CreatorId = model.CreatorId,
                    RefNo = "",
                    Status = PublicationStatus.New,
                    ViewCount = 0,
                    PurchaseCount = 0,
                    DownloadCount = 0,
                    DmsPath = ""
                };

                db.Publication.Add(publication);
                db.SaveChanges();

                //files 1
                foreach (var fileid in model.CoverFilesId)
                {
                    var coverfile = new PublicationFile
                    {
                        FileCategory = PublicationFileCategory.CoverImage,
                        FileId = fileid,
                        ParentId = publication.ID
                    };

                    db.PublicationFile.Add(coverfile);
                }

                //files 2
                foreach (var fileid in model.AuthorFilesId)
                {
                    var authorfile = new PublicationFile
                    {
                        FileCategory = PublicationFileCategory.AuthorImage,
                        FileId = fileid,
                        ParentId = publication.ID
                    };

                    db.PublicationFile.Add(authorfile);
                }

                //files 3
                foreach (var fileid in model.ProofFilesId)
                {
                    var prooffile = new PublicationFile
                    {
                        FileCategory = PublicationFileCategory.ProofOfApproval,
                        FileId = fileid,
                        ParentId = publication.ID
                    };

                    db.PublicationFile.Add(prooffile);
                }

                //files 4
                foreach (var fileid in model.DigitalFilesId)
                {
                    var digitalfile = new PublicationFile
                    {
                        FileCategory = PublicationFileCategory.DigitalPublication,
                        FileId = fileid,
                        ParentId = publication.ID
                    };

                    db.PublicationFile.Add(digitalfile);
                }

                // modify publication by adding ref no based on year, month and new ID (Survey= SVP & SVT)
                var refno = "PUB/" + DateTime.Now.ToString("yyMM");
                refno += "/" + publication.ID.ToString("D4");
                publication.RefNo = refno;

                db.Entry(publication).State = EntityState.Modified;
                db.SaveChanges();

                return publication.ID.ToString() + "|" + model.Title;
            }
            return "";
        }

        /*
        // PUT: api/RnP/Publication/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }
        */

        // Function to save a publication (as draft) after editing an existing one.
        // POST: api/RnP/Publication/Edit
        [Route("api/RnP/Publication/Edit")]
        [HttpPost]
        [ValidationActionFilter]
        public string Edit([FromBody] EditPublicationModelNoFile model)
        {

            if (ModelState.IsValid)
            {
                var publication = db.Publication.Where(p => p.ID == model.ID).FirstOrDefault();

                if (publication != null)
                {
                    publication.CategoryID = model.CategoryID;
                    publication.Author = model.Author;
                    publication.Coauthor = model.Coauthor;
                    publication.Title = model.Title;
                    publication.Year = model.Year;
                    publication.Description = model.Description;
                    publication.Language = model.Language;
                    publication.ISBN = model.ISBN;
                    publication.Hardcopy = model.Hardcopy;
                    publication.Digitalcopy = model.Digitalcopy;
                    publication.HDcopy = model.HDcopy;
                    publication.FreeHCopy = model.FreeHCopy;
                    publication.FreeDCopy = model.FreeDCopy;
                    publication.FreeHDCopy = model.FreeHDCopy;
                    publication.HPrice = model.HPrice;
                    publication.DPrice = model.DPrice;
                    publication.HDPrice = model.HDPrice;
                    publication.StockBalance = model.StockBalance;
                    publication.DmsPath = "";
                    //publication.CreatorId = model.CreatorId;

                    db.Entry(publication).State = EntityState.Modified;

                    //files 1

                    var attachments1 = db.PublicationFile.Where(s => s.FileCategory == PublicationFileCategory.CoverImage && s.ParentId == model.ID).ToList();

                    if (attachments1 != null)
                    {
                        if (model.CoverPictures == null)
                        {
                            foreach (var attachment in attachments1)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.PublicationFile.Remove(attachment);
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
                                    db.PublicationFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.CoverFilesId)
                    {
                        var coverfile = new PublicationFile
                        {
                            FileCategory = PublicationFileCategory.CoverImage,
                            FileId = fileid,
                            ParentId = publication.ID
                        };

                        db.PublicationFile.Add(coverfile);
                    }

                    //files 2

                    var attachments2 = db.PublicationFile.Where(s => s.FileCategory == PublicationFileCategory.AuthorImage && s.ParentId == model.ID).ToList();

                    if (attachments2 != null)
                    {
                        if (model.AuthorPictures == null)
                        {
                            foreach (var attachment in attachments2)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.PublicationFile.Remove(attachment);
                            }
                        }
                        else
                        {
                            foreach (var attachment in attachments2)
                            {
                                if (!model.AuthorPictures.Any(u => u.Id == attachment.FileDocument.Id))
                                {
                                    attachment.FileDocument.Display = false;
                                    db.FileDocument.Attach(attachment.FileDocument);
                                    db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                    db.PublicationFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.AuthorFilesId)
                    {
                        var authorfile = new PublicationFile
                        {
                            FileCategory = PublicationFileCategory.AuthorImage,
                            FileId = fileid,
                            ParentId = publication.ID
                        };

                        db.PublicationFile.Add(authorfile);
                    }

                    //files 3

                    var attachments3 = db.PublicationFile.Where(s => s.FileCategory == PublicationFileCategory.ProofOfApproval && s.ParentId == model.ID).ToList();

                    if (attachments3 != null)
                    {
                        if (model.ProofOfApproval == null)
                        {
                            foreach (var attachment in attachments3)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.PublicationFile.Remove(attachment);
                            }
                        }
                        else
                        {
                            foreach (var attachment in attachments3)
                            {
                                if (!model.ProofOfApproval.Any(u => u.Id == attachment.FileDocument.Id))
                                {
                                    attachment.FileDocument.Display = false;
                                    db.FileDocument.Attach(attachment.FileDocument);
                                    db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                    db.PublicationFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.ProofFilesId)
                    {
                        var prooffile = new PublicationFile
                        {
                            FileCategory = PublicationFileCategory.ProofOfApproval,
                            FileId = fileid,
                            ParentId = publication.ID
                        };

                        db.PublicationFile.Add(prooffile);
                    }

                    //files 4

                    var attachments4 = db.PublicationFile.Where(s => s.FileCategory == PublicationFileCategory.DigitalPublication && s.ParentId == model.ID).ToList();

                    if (attachments4 != null)
                    {
                        if (model.DigitalPublications == null)
                        {
                            foreach (var attachment in attachments4)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.PublicationFile.Remove(attachment);
                            }
                        }
                        else
                        {
                            foreach (var attachment in attachments4)
                            {
                                if (!model.DigitalPublications.Any(u => u.Id == attachment.FileDocument.Id))
                                {
                                    attachment.FileDocument.Display = false;
                                    db.FileDocument.Attach(attachment.FileDocument);
                                    db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                    db.PublicationFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.DigitalFilesId)
                    {
                        var digitalfile = new PublicationFile
                        {
                            FileCategory = PublicationFileCategory.DigitalPublication,
                            FileId = fileid,
                            ParentId = publication.ID
                        };

                        db.PublicationFile.Add(digitalfile);
                    }

                    db.SaveChanges();

                    return model.Title;
                }
            }
            return "";
        }

        // Function to submit a publication for verification after reviewing.
        // POST: api/RnP/Publication/Submit
        [Route("api/RnP/Publication/Submit")]
        [HttpPost]
        [ValidationActionFilter]
        public string Submit([FromBody] DetailsPublicationModel model)
        {

            if (ModelState.IsValid)
            {
                var publication = db.Publication.Where(p => p.ID == model.ID).FirstOrDefault();

                if (publication != null)
                {
                    publication.Status = PublicationStatus.Submitted;

                    db.Entry(publication).State = EntityState.Modified;

                    // create first approval record (using existing ID)
                    var papproval = new PublicationApproval
                    {
                        PublicationID = publication.ID,
                        Level = PublicationApprovalLevels.Verifier,
                        ApproverId = 0,
                        Status = PublicationApprovalStatus.None,
                        ApprovalDate = DateTime.Now,
                        Remarks = "",
                        RequireNext = false
                    };

                    db.PublicationApproval.Add(papproval);
                    db.SaveChanges();

                    return publication.Title + "|" + publication.RefNo;
                }
            }
            return "";
        }

        // Function to submit a publication for verification from details page.
        // GET: api/RnP/Publication/SubmitByID
        [Route("api/RnP/Publication/SubmitByID")]
        public string SubmitByID(int id)
        {

            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                publication.Status = PublicationStatus.Submitted;

                db.Entry(publication).State = EntityState.Modified;

                // create first approval record (using existing ID)
                var papproval = new PublicationApproval
                {
                    PublicationID = publication.ID,
                    Level = PublicationApprovalLevels.Verifier,
                    ApproverId = 0,
                    Status = PublicationApprovalStatus.None,
                    ApprovalDate = DateTime.Now,
                    Remarks = "",
                    RequireNext = false
                };

                db.PublicationApproval.Add(papproval);
                db.SaveChanges();

                return publication.Title + "|" + publication.Author + "|" + publication.RefNo;
            }
            return "";
        }

        // Function to publish a publication from details page.
        // GET: api/RnP/Publication/PublishByID
        [Route("api/RnP/Publication/PublishByID")]
        public string PublishByID(int id)
        {

            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                publication.Status = PublicationStatus.Published;

                db.Entry(publication).State = EntityState.Modified;

                db.SaveChanges();

                return publication.Title + "|" + publication.Author + "|" + publication.RefNo;
            }
            return "";
        }

        // Function to delete an unsubmitted publication after viewing on delete screen.
        // Can also be called from the Discard button at the review screen (in which case confirmation is via prompt).
        // GET: api/RnP/Publication/Delete/5
        [Route("api/RnP/Publication/Delete")]
        //[HttpPost]
        public string Delete(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                string ptitle = publication.Title;

                // delete record
                db.Publication.Remove(publication);

                //db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return ptitle;
            }

            return "";
        }

        // Function for when Verifier/Approver approves/rejects publication
        // POST: api/RnP/Publication/Evaluate
        [Route("api/RnP/Publication/Evaluate")]
        [HttpPost]
        [ValidationActionFilter]
        public string Evaluate([FromBody] ReturnPublicationApprovalModel model)
        {

            if (ModelState.IsValid)
            {
                var papproval = db.PublicationApproval.Where(pa => pa.ID == model.Approval.ID).FirstOrDefault();

                if (papproval != null)
                {
                    papproval.ApproverId = model.Approval.ApproverId;
                    papproval.Status = model.Approval.Status;
                    papproval.ApprovalDate = DateTime.Now;
                    papproval.Remarks = model.Approval.Remarks;
                    papproval.RequireNext = model.Approval.RequireNext;
                    // requirenext is always set to true when coming from verifier approval, and always false from approver3

                    db.Entry(papproval).State = EntityState.Modified;
                    // HERE
                    db.SaveChanges();

                    var publication = db.Publication.Where(p => p.ID == papproval.PublicationID).FirstOrDefault();
                    if (publication != null)
                    {
                        // proceed depending on status (assuming user can only pick approve and reject)
                        if (model.Approval.Status == PublicationApprovalStatus.Rejected)
                        {
                            if (publication.Status == PublicationStatus.Submitted)
                            {
                                publication.Status = PublicationStatus.VerifierRejected;
                                db.Entry(publication).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else if (publication.Status == PublicationStatus.Verified)
                            {
                                publication.Status = PublicationStatus.ApproverRejected;
                                db.Entry(publication).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            // proceed depending on requirenext
                            if (model.Approval.RequireNext == false)
                            {
                                // no more approvals necessary (assumes verifier will never get here)
                                publication.Status = PublicationStatus.Approved;
                                db.Entry(publication).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                PublicationApprovalLevels nextlevel;
                                switch (papproval.Level)
                                {
                                    case PublicationApprovalLevels.Verifier:
                                        nextlevel = PublicationApprovalLevels.Approver1;
                                        publication.Status = PublicationStatus.Verified;
                                        db.Entry(publication).State = EntityState.Modified;
                                        break;
                                    case PublicationApprovalLevels.Approver1:
                                        nextlevel = PublicationApprovalLevels.Approver2;
                                        break;
                                    case PublicationApprovalLevels.Approver2:
                                        nextlevel = PublicationApprovalLevels.Approver3;
                                        break;
                                    default:
                                        nextlevel = PublicationApprovalLevels.Approver1;
                                        break;
                                }

                                // create next approval record
                                var pnewapproval = new PublicationApproval
                                {
                                    PublicationID = papproval.PublicationID,
                                    Level = nextlevel,
                                    ApproverId = 0,
                                    Status = PublicationApprovalStatus.None,
                                    ApprovalDate = DateTime.Now,
                                    Remarks = "",
                                    RequireNext = false
                                };

                                db.PublicationApproval.Add(pnewapproval);
                                // HERE
                                db.SaveChanges();
                            }

                        }

                        return publication.ID + "|" + publication.Title + "|" + publication.Author + "|" + publication.RefNo;
                    }
                }
            }

            return "";
        }

        // Function to cancel a publication after rejection by verifier or approver.
        // (from details page)
        // POST: api/RnP/Publication/Cancel
        [Route("api/RnP/Publication/Cancel")]
        [HttpPost]
        [ValidationActionFilter]
        public string Cancel([FromBody] UpdatePublicationCancellationModel model)
        {

            if (ModelState.IsValid)
            {
                var publication = db.Publication.Where(p => p.ID == model.ID).FirstOrDefault();

                if (publication != null)
                {
                    publication.Status = PublicationStatus.Trashed;
                    publication.CancelRemark = model.CancelRemark;
                    publication.DateCancelled = DateTime.Now;

                    db.Entry(publication).State = EntityState.Modified;

                    db.SaveChanges();

                    return publication.Title + "|" + publication.Author + "|" + publication.RefNo;
                }
            }
            return "";
        }

        /*
         * The following API calls are for Publication Withdrawal operations, including:
         * 1. Admin confirms withdrawal of a previously selected Publication (from the Published list)
         * 2. Admin confirms cancellation of withdrawal
         * 3. Approvers review and approve/reject the withdrawal
         */

        // Function for when Admin confirms to withdraw a published publication
        // (from details page)
        // POST: api/RnP/Publication/Withdraw
        [Route("api/RnP/Publication/Withdraw")]
        [HttpPost]
        [ValidationActionFilter]
        public string Withdraw([FromBody] UpdatePublicationWithdrawalModelNoFile model)
        {

            if (ModelState.IsValid)
            {
                var publication = db.Publication.Where(p => p.ID == model.ID).FirstOrDefault();

                if (publication != null)
                {
                    publication.WithdrawalReason = model.WithdrawalReason;
                    publication.WithdrawalDate = DateTime.Now;
                    //publication.ProofOfWithdrawal = model.ProofOfWithdrawal;
                    publication.Status = PublicationStatus.WithdrawalSubmitted;

                    db.Entry(publication).State = EntityState.Modified;

                    // create first approval record (using existing ID)
                    var pwithdrawal = new PublicationWithdrawal
                    {
                        PublicationID = publication.ID,
                        Level = PublicationApprovalLevels.Verifier,
                        ApproverId = 0,
                        Status = PublicationApprovalStatus.None,
                        ApprovalDate = DateTime.Now,
                        Remarks = "",
                        RequireNext = false
                    };

                    db.PublicationWithdrawal.Add(pwithdrawal);

                    //files

                    var attachments1 = db.PublicationFile.Where(s => s.FileCategory == PublicationFileCategory.ProofOfWithdrawal && s.ParentId == model.ID).ToList();

                    if (attachments1 != null)
                    {
                        if (model.ProofOfWithdrawal == null)
                        {
                            foreach (var attachment in attachments1)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.PublicationFile.Remove(attachment);
                            }
                        }
                        else
                        {
                            foreach (var attachment in attachments1)
                            {
                                if (!model.ProofOfWithdrawal.Any(u => u.Id == attachment.FileDocument.Id))
                                {
                                    attachment.FileDocument.Display = false;
                                    db.FileDocument.Attach(attachment.FileDocument);
                                    db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                    db.PublicationFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.ProofFilesId)
                    {
                        var prooffile = new PublicationFile
                        {
                            FileCategory = PublicationFileCategory.ProofOfWithdrawal,
                            FileId = fileid,
                            ParentId = publication.ID
                        };

                        db.PublicationFile.Add(prooffile);
                    }

                    db.SaveChanges();

                    return publication.Title + "|" + publication.Author + "|" + publication.RefNo;
                }
            }

            return "";
        }

        // Function for when admin confirms cancellation of publication withdrawal
        // TODO: where to store remarks??
        // POST: api/RnP/Publication/CancelWithdrawal
        [Route("api/RnP/Publication/CancelWithdrawal")]
        [HttpPost]
        [ValidationActionFilter]
        public string CancelWithdrawal([FromBody] UpdatePublicationCancellationModel model)
        {
            if (ModelState.IsValid)
            {
                var publication = db.Publication.Where(p => p.ID == model.ID).FirstOrDefault();

                if (publication != null)
                {
                    publication.WithdrawalReason = "";
                    //publication.ProofOfWithdrawal = "";
                    publication.Status = PublicationStatus.Published;

                    db.Entry(publication).State = EntityState.Modified;

                    //db.Configuration.ValidateOnSaveEnabled = false;

                    //remove files

                    var attachments1 = db.PublicationFile.Where(s => s.FileCategory == PublicationFileCategory.ProofOfWithdrawal && s.ParentId == model.ID).ToList();

                    if (attachments1 != null)
                    {
                        foreach (var attachment in attachments1)
                        {
                            attachment.FileDocument.Display = false;
                            db.FileDocument.Attach(attachment.FileDocument);
                            db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                            db.PublicationFile.Remove(attachment);
                        }
                    }

                    db.SaveChanges();

                    return publication.Title + "|" + publication.Author + "|" + publication.RefNo;
                }
            }
            return "";
        }

        // Function for when Approver approves/rejects publication withdrawal
        // POST: api/RnP/Publication/EvaluateWithdrawal
        [Route("api/RnP/Publication/EvaluateWithdrawal")]
        [HttpPost]
        [ValidationActionFilter]
        public string EvaluateWithdrawal([FromBody] ReturnPublicationWithdrawalModel model)
        {

            if (ModelState.IsValid)
            {
                var papproval = db.PublicationWithdrawal.Where(pa => pa.ID == model.Approval.ID).FirstOrDefault();

                if (papproval != null)
                {
                    papproval.ApproverId = model.Approval.ApproverId;
                    papproval.Status = model.Approval.Status;
                    papproval.ApprovalDate = DateTime.Now;
                    papproval.Remarks = model.Approval.Remarks;
                    papproval.RequireNext = model.Approval.RequireNext;
                    // requirenext is always set to true when coming from verifier approval, and always false from approver3

                    db.Entry(papproval).State = EntityState.Modified;
                    db.SaveChanges();

                    var publication = db.Publication.Where(p => p.ID == papproval.PublicationID).FirstOrDefault();
                    if (publication != null)
                    {
                        // proceed depending on status (assuming user can only pick approve and reject)
                        if (model.Approval.Status == PublicationApprovalStatus.Rejected)
                        {
                            if (publication.Status == PublicationStatus.WithdrawalSubmitted)
                            {
                                publication.Status = PublicationStatus.WithdrawalVerifierRejected;
                                db.Entry(publication).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else if (publication.Status == PublicationStatus.WithdrawalVerified)
                            {
                                publication.Status = PublicationStatus.WithdrawalApproverRejected;
                                db.Entry(publication).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            // proceed depending on requirenext
                            if (model.Approval.RequireNext == false)
                            {
                                // no more approvals necessary
                                publication.Status = PublicationStatus.Withdrawn;       // as opposed to Approved coz admin don't need to "Publish" withdrawals
                                db.Entry(publication).State = EntityState.Modified;
                                db.SaveChanges();

                                var emailres = SendEmailNotificationWithdrawal(publication);
                            }
                            else
                            {
                                PublicationApprovalLevels nextlevel;
                                switch (papproval.Level)
                                {
                                    case PublicationApprovalLevels.Verifier:
                                        nextlevel = PublicationApprovalLevels.Approver1;
                                        publication.Status = PublicationStatus.WithdrawalVerified;
                                        db.Entry(publication).State = EntityState.Modified;
                                        break;
                                    case PublicationApprovalLevels.Approver1:
                                        nextlevel = PublicationApprovalLevels.Approver2;
                                        break;
                                    case PublicationApprovalLevels.Approver2:
                                        nextlevel = PublicationApprovalLevels.Approver3;
                                        break;
                                    default:
                                        nextlevel = PublicationApprovalLevels.Approver1;
                                        break;
                                }

                                // create next approval record
                                var pnewapproval = new PublicationWithdrawal
                                {
                                    PublicationID = papproval.PublicationID,
                                    Level = nextlevel,
                                    ApproverId = 0,
                                    Status = PublicationApprovalStatus.None,
                                    ApprovalDate = DateTime.Now,
                                    Remarks = "",
                                    RequireNext = false
                                };

                                db.PublicationWithdrawal.Add(pnewapproval);
                                db.SaveChanges();
                            }

                        }

                        return publication.ID + "|" + publication.Title + "|" + publication.Author + "|" + publication.RefNo;
                    }
                }
            }

            return "";
        }

        /*
         * The following API calls are for Image uploads
         * 1. 
         * 2. 
         */

        // GET: api/RnP/Publication/UploadImages
        [Route("api/RnP/Publication/UploadImages")]
        [HttpGet]
        public int UploadImages(int pubid, string coverpic, string authorpic)
        {
            var pubimages = new PublicationImages
            {
                PublicationID = pubid,
                CoverPicture = coverpic,
                AuthorPicture = authorpic
            };

            db.PublicationImages.Add(pubimages);
            db.SaveChanges();

            return pubimages.ID;
        }

        // GET: api/RnP/Publication/UpdateImagesCover
        [Route("api/RnP/Publication/UpdateImagesCover")]
        [HttpGet]
        public int UpdateImagesCover(int pubid, string coverpic)
        {
            var pubimages = db.PublicationImages.Where(pi => pi.PublicationID == pubid).FirstOrDefault();

            if (pubimages != null)
            {
                pubimages.CoverPicture = coverpic;
                db.Entry(pubimages).State = EntityState.Modified;
                db.SaveChanges();

                return pubimages.ID;
            }

            return 0;
        }

        // GET: api/RnP/Publication/UpdateImagesAuthor
        [Route("api/RnP/Publication/UpdateImagesAuthor")]
        [HttpGet]
        public int UpdateImagesAuthor(int pubid, string authorpic)
        {
            var pubimages = db.PublicationImages.Where(pi => pi.PublicationID == pubid).FirstOrDefault();

            if (pubimages != null)
            {
                pubimages.AuthorPicture = authorpic;
                db.Entry(pubimages).State = EntityState.Modified;
                db.SaveChanges();

                return pubimages.ID;
            }

            return 0;
        }

        // GET: api/RnP/Publication/UpdateImagePublicationID
        [Route("api/RnP/Publication/UpdateImagePublicationID")]
        [HttpGet]
        public int UpdateImagePublicationID(int id, int pubid)
        {
            var pubimages = db.PublicationImages.Where(pi => pi.ID == id).FirstOrDefault();

            if (pubimages != null)
            {
                pubimages.PublicationID = pubid;
                db.Entry(pubimages).State = EntityState.Modified;
                db.SaveChanges();

                return pubimages.ID;
            }

            return 0;
        }

        /*
         * The following API calls are for Publication purchasing operations (prior to cart), including:
         * 1. 
         * 2. 
         */

        // Function for getting existing delivery address
        // GET: api/RnP/Publication/GetDeliveryInfo/5
        [Route("api/RnP/Publication/GetDeliveryInfo")]
        public PublicationDeliveryModel GetDeliveryInfo(int userid)
        {
            var info = db.PublicationDelivery.Where(d => d.UserId == userid).FirstOrDefault();

            if (info != null)
            {
                var newinfo = new PublicationDeliveryModel
                {
                    ID = info.ID,
                    UserId = info.UserId,
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    Address1 = info.Address1,
                    Address2 = info.Address2,
                    Postcode = info.Postcode,
                    City = info.City,
                    State = info.State,
                    PhoneNumber = info.PhoneNumber
                };
                return newinfo;
            }
            else
            {
                var newinfo = new PublicationDeliveryModel
                {
                    UserId = userid,
                    FirstName = "",
                    LastName = "",
                    Address1 = "",
                    Address2 = "",
                    Postcode = "",
                    City = "",
                    State = DeliveryStates.Johor,
                    PhoneNumber = ""
                };
                return newinfo;
            }

        }

        // Function for creating/upating delivery address for publication purchases
        // POST: api/RnP/Publication/UpdateDeliveryInfo
        [Route("api/RnP/Publication/UpdateDeliveryInfo")]
        [HttpPost]
        [ValidationActionFilter]
        public bool UpdateDeliveryInfo([FromBody] PublicationDeliveryModel model)
        {
            if (ModelState.IsValid)
            {
                var info = db.PublicationDelivery.Where(d => d.UserId == model.UserId).FirstOrDefault();

                if (info != null)
                {
                    info.UserId = model.UserId;
                    info.FirstName = model.FirstName;
                    info.LastName = model.LastName;
                    info.Address1 = model.Address1;
                    info.Address2 = model.Address2;
                    info.Postcode = model.Postcode;
                    info.City = model.City;
                    info.State = model.State;
                    info.PhoneNumber = model.PhoneNumber;

                    db.Entry(info).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    var newinfo = new PublicationDelivery
                    {
                        UserId = model.UserId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        Postcode = model.Postcode,
                        City = model.City,
                        State = model.State,
                        PhoneNumber = model.PhoneNumber
                    };

                    db.PublicationDelivery.Add(newinfo);
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        // Function for adding order item when publication purchase selection is finalised
        // called by Add to Cart button at PurchasePublication page
        // the caller must also call api/Commerce/Cart/AddItem
        // POST: api/RnP/Publication/AddOrderItem
        [Route("api/RnP/Publication/AddOrderItem")]
        [HttpPost]
        [ValidationActionFilter]
        public bool AddOrderItem([FromBody] PublicationPurchaseItemModel model)
        {

            if (ModelState.IsValid)
            {
                var pitem = new PublicationPurchaseItem
                {
                    PurchaseOrderId = model.PurchaseOrderId,
                    UserId = model.UserId,
                    PublicationID = model.PublicationID,
                    Format = model.Format,
                    Price = model.Price,
                    Quantity = model.Quantity
                };

                db.PublicationPurchaseItem.Add(pitem);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        // Remove order item
        // POST: api/RnP/Publication/RemoveOrderItem
        [Route("api/RnP/Publication/RemoveOrderItem")]
        public bool RemoveOrderItem(int itemid)
        {
            var item = db.PublicationPurchaseItem.Where(pi => pi.ID == itemid).FirstOrDefault();

            if (item != null)
            {
                db.PublicationPurchaseItem.Remove(item);
                db.SaveChanges();
            }

            return true;
        }

        /*
         * Tracking functions
         */

        // Function to increment view count.
        // GET: api/RnP/Publication/IncrementView
        [Route("api/RnP/Publication/IncrementView")]
        [HttpGet]
        public bool IncrementView(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                publication.ViewCount = publication.ViewCount + 1;

                db.Entry(publication).State = EntityState.Modified;

                db.SaveChanges();

                return true;
            }

            return false;
        }

        // Function to increment purchase count.
        // GET: api/RnP/Publication/IncrementPurchase
        [Route("api/RnP/Publication/IncrementPurchase")]
        [HttpGet]
        public bool IncrementPurchase(int? id, int incrementvalue = 1)
        {
            if (id == null)
            {
                return false;
            }

            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                publication.PurchaseCount = publication.PurchaseCount + incrementvalue;

                db.Entry(publication).State = EntityState.Modified;

                db.SaveChanges();

                return true;
            }

            return false;
        }

        // Function to increment download count.
        // GET: api/RnP/Publication/IncrementDownload
        [Route("api/RnP/Publication/IncrementDownload")]
        [HttpGet]
        public bool IncrementDownload(int? id, int? userid)
        {
            if (id == null)
            {
                return false;
            }

            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                publication.DownloadCount = publication.DownloadCount + 1;

                db.Entry(publication).State = EntityState.Modified;

                var pdownloads = db.PublicationDownloads.Where(d => d.PublicationID == id && d.UserId == userid).FirstOrDefault();

                if (pdownloads == null)
                {
                    var pdownload = new PublicationDownloads
                    {
                        PublicationID = id.Value,
                        UserId = userid.Value
                    };

                    db.PublicationDownloads.Add(pdownload);
                }

                db.SaveChanges();

                return true;
            }

            return false;
        }

        // Function to check if digital publication is refundable (not yet downloaded)
        // GET: api/RnP/Publication/IsDownloaded
        [Route("api/RnP/Publication/IsDownloaded")]
        [HttpGet]
        public bool IsDownloaded(int? userid, int? pubid)
        {
            if ((userid == null) || (pubid == null))
            {
                return true;
            }

            var pdownloads = db.PublicationDownloads.Where(d => d.PublicationID == pubid && d.UserId == userid).FirstOrDefault();

            if (pdownloads != null)
            {
                return true;
            }

            return false;
        }

        // Function to check if publication is refundable (hard copy or undownloaded softcopy)
        // GET: api/RnP/Publication/IsRefundable
        /*
        [Route("api/RnP/Publication/IsRefundable")]
        [HttpGet]
        public bool IsRefundable(int? userid, int? cartid, int? itemid)
        {
            if ((userid == null) || (cartid == null) || (itemid == null))
            {
                return false;
            }

            var pubitem = db.PublicationPurchaseItem.Where(pi => pi.UserId == userid && pi.PurchaseOrderId == cartid && pi.PublicationID == itemid).FirstOrDefault();

            if (publication != null)
            {
                publication.PurchaseCount = publication.PurchaseCount + incrementvalue;

                db.Entry(publication).State = EntityState.Modified;

                db.SaveChanges();

                return true;
            }

            return false;
        }
        */

        /*
         * Settings
        */

        // Load Settings
        // GET: api/RnP/Publication/LoadSettings
        [Route("api/RnP/Publication/LoadSettings")]
        [HttpGet]
        public PublicationSettingsModel LoadSettings()
        {
            var settings = db.PublicationSettings.Select(s => new PublicationSettingsModel
            {
                HardcopyReturnPeriod = s.HardcopyReturnPeriod,
                MinimumPublishedYear = s.MinimumPublishedYear
            }).FirstOrDefault();

            return settings;
        }

        // Get specific setting: HardcopyReturnPeriod
        // GET: api/RnP/Publication/GetSettingsHardcopyReturnPeriod
        [Route("api/RnP/Publication/GetSettingsHardcopyReturnPeriod")]
        [HttpGet]
        public int GetSettingsHardcopyReturnPeriod()
        {
            var settings = db.PublicationSettings.Select(s => new PublicationSettingsModel
            {
                HardcopyReturnPeriod = s.HardcopyReturnPeriod
            }).FirstOrDefault();

            return settings.HardcopyReturnPeriod;
        }

        // Get specific setting: MinimumPublishedYear
        // GET: api/RnP/Publication/GetSettingsMinimumPublishedYear
        [Route("api/RnP/Publication/GetSettingsMinimumPublishedYear")]
        [HttpGet]
        public int GetSettingsMinimumPublishedYear()
        {
            var settings = db.PublicationSettings.Select(s => new PublicationSettingsModel
            {
                MinimumPublishedYear = s.MinimumPublishedYear
            }).FirstOrDefault();

            return settings.MinimumPublishedYear;
        }

        // Save Settings
        // POST: api/RnP/Publication/SaveSettings
        [Route("api/RnP/Publication/SaveSettings")]
        [HttpPost]
        public bool SaveSettings(PublicationSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                var settings = db.PublicationSettings.FirstOrDefault();

                if (settings != null)
                {
                    settings.HardcopyReturnPeriod = model.HardcopyReturnPeriod;
                    settings.MinimumPublishedYear = model.MinimumPublishedYear;
                    db.Entry(settings).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        /*
         * The following API calls are for SLAReminder/Notifications including:
         * 1. Get notification receiver IDs
         * 2. Save notification ID (to publication table)
         * The functions are called by SendNotification function from Intranet
         */

        // Function to get notification receivers based on notification category and type.
        // Called when sending notifications
        // TO CONSIDER: Currently cancellations notify all approvers.
        // GET: api/RnP/Publication/GetNotificationReceivers/
        [Route("api/RnP/Publication/GetNotificationReceivers")]
        public List<int> GetNotificationReceivers(NotificationCategory cat, NotificationType type, PublicationApprovalStatus status, bool forward)
        {
            List<int> receivers = new List<int> { };

            // prepare
            bool toadmin = false;
            bool toverifier = false;
            bool toapprover1 = false;
            bool toapprover2 = false;
            bool toapprover3 = false;

            if ((type == NotificationType.Submit_Publication_Creation) || (type == NotificationType.Submit_Publication_Modification) || (type == NotificationType.Submit_Publication_Withdrawal))
            {
                toadmin = true;
                toverifier = true;
            }
            else if ((type == NotificationType.Submit_Publication_Cancellation) || (type == NotificationType.Submit_Publication_Modification_Cancellation) || (type == NotificationType.Submit_Publication_Withdrawal_Cancellation))
            {
                toadmin = true;
                toverifier = true;
                toapprover1 = true;
                toapprover2 = true;
                toapprover3 = true;
            }
            else if (type == NotificationType.Submit_Publication_Publication)
            {
                toadmin = true;
                toverifier = true;
                toapprover1 = true;
                toapprover2 = true;
                toapprover3 = true;
            }
            else if ((type == NotificationType.Verify_Publication_Creation) || (type == NotificationType.Verify_Publication_Modification) || (type == NotificationType.Verify_Publication_Withdrawal))
            {
                if (status == PublicationApprovalStatus.Rejected)
                {
                    toadmin = true;
                    toverifier = true;
                }
                else
                {
                    toverifier = true;
                    toapprover1 = true;
                }
            }
            else if ((type == NotificationType.Approve_Publication_Creation_1) || (type == NotificationType.Approve_Publication_Modification_1) || (type == NotificationType.Approve_Publication_Withdrawal_1))
            {
                if (status == PublicationApprovalStatus.Rejected)
                {
                    toadmin = true;
                    toapprover1 = true;
                }
                else
                {
                    if (forward)
                    {
                        toapprover1 = true;
                        toapprover2 = true;
                    }
                    else
                    {
                        toadmin = true;
                        toverifier = true;
                        toapprover1 = true;
                    }
                }
            }
            else if ((type == NotificationType.Approve_Publication_Creation_2) || (type == NotificationType.Approve_Publication_Modification_2) || (type == NotificationType.Approve_Publication_Withdrawal_2))
            {
                if (status == PublicationApprovalStatus.Rejected)
                {
                    toadmin = true;
                    toapprover2 = true;
                }
                else
                {
                    if (forward)
                    {
                        toapprover2 = true;
                        toapprover3 = true;
                    }
                    else
                    {
                        toadmin = true;
                        toverifier = true;
                        toapprover2 = true;
                    }
                }
            }
            else if ((type == NotificationType.Approve_Publication_Creation_3) || (type == NotificationType.Approve_Publication_Modification_3) || (type == NotificationType.Approve_Publication_Withdrawal_3))
            {
                if (status == PublicationApprovalStatus.Rejected)
                {
                    toadmin = true;
                    toapprover3 = true;
                }
                else
                {
                    toadmin = true;
                    toverifier = true;
                    toapprover3 = true;
                }
            }

            // get list of users
            var allusers = db.User.Where(u => u.Display).ToList();

            foreach (FEP.Model.User myuser in allusers)
            {
                if (myuser.UserAccount.IsEnable)
                {
                    // get list of roles
                    var myroles = myuser.UserAccount.UserRoles;
                    foreach (UserRole myrole in myroles)
                    {
                        var myroleid = myrole.RoleId;
                        // get list of access
                        var myaccesses = db.RoleAccess.Where(ra => ra.RoleId == myroleid).ToList();
                        foreach (RoleAccess myaccess in myaccesses)
                        {
                            UserAccess myfunction = myaccess.UserAccess;
                            if (myfunction == UserAccess.RnPPublicationEdit)
                            {
                                if (toadmin)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPPublicationWithdraw)
                            {
                                if (toadmin)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPPublicationVerify)
                            {
                                if (toverifier)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPPublicationApprove1)
                            {
                                if (toapprover1)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPPublicationApprove2)
                            {
                                if (toapprover2)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPPublicationApprove3)
                            {
                                if (toapprover3)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                        }
                    }
                }
            }

            // return unique ids
            if (receivers.Count > 0)
            {
                List<int> uniquereceivers = receivers.Distinct().ToList();
                receivers = uniquereceivers;
            }
            return receivers;
        }

        // Function to save notification ID.
        // Called when sending notifications
        // GET: api/RnP/Publication/SaveNotificationID/
        [Route("api/RnP/Publication/SaveNotificationID")]
        public bool SaveNotificationID(int id, int notificationid)
        {
            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                publication.NotificationID = notificationid;

                db.Entry(publication).State = EntityState.Modified;
                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        // BULK EMAIL

        [NonAction]
        public bool SendEmailNotificationWithdrawal(Publication model)
        {
            ParameterListToSend paramToSend = new ParameterListToSend();
            paramToSend.PublicationTitle = model.Title;
            paramToSend.PublicationAuthor = model.Author;
            paramToSend.PublicationCode = model.RefNo;
            paramToSend.PublicationApproval = "";

            var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType.Approve_Publication_Withdrawal_Final).FirstOrDefault();
            string Subject = generateBodyMessage("Publication Withdrawal Notice", NotificationType.Approve_Publication_Withdrawal_Final, paramToSend);
            string Body = generateBodyMessage(template.TemplateMessage, NotificationType.Approve_Publication_Withdrawal_Final, paramToSend);

            List<string> Email = new List<string> { };
            var nonstaff = db.User.Where(u => (u.UserType == UserType.Individual || u.UserType == UserType.Company)).ToList();
            foreach (User myuser in nonstaff)
            {
                Email.Add(myuser.Email);
            }

            var sendresult = SendBulkEmail(NotificationType.Approve_Publication_Withdrawal_Final, NotificationCategory.ResearchAndPublication, Email, paramToSend, Subject, Body);
            return true;
        }

        [NonAction]
        public string GetPropertyValues(Object obj, string propertyName)
        {
            Type t = obj.GetType();
            System.Reflection.PropertyInfo[] props = t.GetProperties();
            string value = "";
            foreach (var prop in props)
                if (prop.Name == propertyName)
                {
                    value = (prop.GetValue(obj))?.ToString();
                    break;
                }
                else
                    value = "";

            return value;
        }

        [NonAction]
        public string generateBodyMessage(string TemplateText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WholeText = TemplateText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WholeText = WholeText.Replace(textToReplace, theValue);
            }

            return WholeText;
        }

        [NonAction]
        public string generateSubjectMessage(string TemplateText, NotificationType NotificationType, ParameterListToSend paramToSend)
        {
            var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
            string WholeText = TemplateText;
            foreach (var item in ParamList)
            {
                string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
                string textToReplace = "[#" + item.TemplateParameterType + "]";
                WholeText = WholeText.Replace(textToReplace, theValue);
            }

            return WholeText;
        }

        [NonAction]
        public async System.Threading.Tasks.Task<IHttpActionResult> SendBulkEmail(NotificationType NotificationType, NotificationCategory NotificationCategory, List<string> Emails, ParameterListToSend ParameterListToSend, string emailSubject, string emailBody)
        {
            bool success = true;
            foreach (string receiverEmailAddress in Emails)
            {
                int counter = 1;
                var response = await sendEmailUsingAPIAsync(DateTime.Now, (int)NotificationCategory, (int)NotificationType, receiverEmailAddress, emailSubject, emailBody, counter);
                if (response == null)
                {
                    success = false;
                }
            }

            return Ok(success);
        }

        [NonAction]
        public async System.Threading.Tasks.Task<EmailClass> sendEmailUsingAPIAsync(DateTime emailDate, int notifyCategory, int notifyType, string emailAddress, string emailSubject, string emailBody, int counter)
        {
            DateTime myTimeNow = DateTime.Now;
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            EmailClass emailObj = new EmailClass
            {
                datID = emailAddress + "-email" + counter + "-" + epoch.ToString(),
                datType = notifyCategory,
                datNotify = notifyType,
                dtInsert = myTimeNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtSchedule = emailDate.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                dtExpired = emailDate.AddYears(1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                emailTo = emailAddress,
                subject = HttpUtility.HtmlDecode(emailSubject),
                body = HttpUtility.HtmlDecode(emailBody)
            };
            var response = await FEP.Intranet.WepApiMethod.SendApiAsync<EmailClass>(System.Web.Mvc.HttpVerbs.Post, $"BulkEmail", emailObj, FEP.Intranet.WepApiMethod.APIEngine.EmailSMSAPI);

            if (response.isSuccess)
                return response.Data;
            else
                return null;
        }

    }
}
