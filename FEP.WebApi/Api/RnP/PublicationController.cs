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
                    Category = s.Category.Name
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
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).ToList();

            return publications;
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
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            if (publication == null)
            {
                return NotFound();
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
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

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
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                WithdrawalDate = s.WithdrawalDate,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

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
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                WithdrawalDate = s.WithdrawalDate,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

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
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                WithdrawalDate = s.WithdrawalDate,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                DmsPath = s.DmsPath,
                Category = s.Category.Name
            }).FirstOrDefault();

            var puser = db.User.Where(u => u.Id == publication.CreatorId).FirstOrDefault();
            if (puser != null)
            {
                publication.CreatorName = puser.Name;
            }

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
        public string Create([FromBody] UpdatePublicationModel model)
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
                    Pictures = model.Pictures,
                    ProofOfApproval = model.ProofOfApproval,
                    StockBalance = model.StockBalance,
                    WithdrawalReason = "",
                    ProofOfWithdrawal = "",
                    CancelRemark = "",
                    DateAdded = DateTime.Now,
                    CreatorId = model.CreatorId,
                    RefNo = "",
                    Status = PublicationStatus.New,
                    ViewCount = 0,
                    PurchaseCount = 0,
                    DmsPath = ""
                };

                db.Publication.Add(publication);
                db.SaveChanges();
                //if (db.SaveChanges() > 0) {

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
        public string Edit([FromBody] UpdatePublicationModel model)
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
                    publication.Pictures = model.Pictures;
                    publication.ProofOfApproval = model.ProofOfApproval;
                    publication.StockBalance = model.StockBalance;
                    publication.DmsPath = "";
                    //publication.CreatorId = model.CreatorId;

                    db.Entry(publication).State = EntityState.Modified;
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
        public string Submit([FromBody] UpdatePublicationModel model)
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

                    //return model.Title;
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

                //return model.Title;
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

                //return publication.Title;
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

                        //return publication.Title;
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

                    //return publication.Title;
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
        public string Withdraw([FromBody] UpdatePublicationWithdrawalModel model)
        {

            if (ModelState.IsValid)
            {
                var publication = db.Publication.Where(p => p.ID == model.ID).FirstOrDefault();

                if (publication != null)
                {
                    publication.WithdrawalReason = model.WithdrawalReason;
                    publication.WithdrawalDate = DateTime.Now;
                    publication.ProofOfWithdrawal = model.ProofOfWithdrawal;
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
                    db.SaveChanges();

                    //return publication.Title;
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
                    publication.ProofOfWithdrawal = "";
                    publication.Status = PublicationStatus.Published;

                    db.Entry(publication).State = EntityState.Modified;

                    //db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    //return publication.Title;
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

                        //return publication.Title;
                        return publication.ID + "|" + publication.Title + "|" + publication.Author + "|" + publication.RefNo;
                    }
                }
            }

            return "";
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
                toverifier = true;
            }
            else if ((type == NotificationType.Submit_Publication_Cancellation) || (type == NotificationType.Submit_Publication_Modification_Cancellation) || (type == NotificationType.Submit_Publication_Withdrawal_Cancellation))
            {
                toverifier = true;
                toapprover1 = true;
                toapprover2 = true;
                toapprover3 = true;
            }
            else if (type == NotificationType.Submit_Publication_Publication)
            {
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
                }
                else
                {
                    toapprover1 = true;
                }
            }
            else if ((type == NotificationType.Approve_Publication_Creation_1) || (type == NotificationType.Approve_Publication_Modification_1) || (type == NotificationType.Approve_Publication_Withdrawal_1))
            {
                if (status == PublicationApprovalStatus.Rejected)
                {
                    toadmin = true;
                    toverifier = true;
                }
                else
                {
                    if (forward)
                    {
                        toapprover2 = true;
                    }
                    else
                    {
                        toadmin = true;
                        toverifier = true;
                    }
                }
            }
            else if ((type == NotificationType.Approve_Publication_Creation_2) || (type == NotificationType.Approve_Publication_Modification_2) || (type == NotificationType.Approve_Publication_Withdrawal_2))
            {
                if (status == PublicationApprovalStatus.Rejected)
                {
                    toadmin = true;
                    toverifier = true;
                    toapprover1 = true;
                }
                else
                {
                    if (forward)
                    {
                        toapprover3 = true;
                    }
                    else
                    {
                        toadmin = true;
                        toverifier = true;
                        toapprover1 = true;
                    }
                }
            }
            else if ((type == NotificationType.Approve_Publication_Creation_3) || (type == NotificationType.Approve_Publication_Modification_3) || (type == NotificationType.Approve_Publication_Withdrawal_3))
            {
                if (status == PublicationApprovalStatus.Rejected)
                {
                    toadmin = true;
                    toverifier = true;
                    toapprover1 = true;
                    toapprover2 = true;
                }
                else
                {
                    toadmin = true;
                    toverifier = true;
                    toapprover1 = true;
                    toapprover2 = true;
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
    }
}
