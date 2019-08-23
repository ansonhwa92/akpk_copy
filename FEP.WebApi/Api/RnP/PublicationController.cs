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
        // POST: api/RnP/Publication.GetAll (DataTable)
        [Route("api/RnP/Publication/GetAll")]
        public IHttpActionResult Post(FilterPublicationModel request)
        {

            var query = db.Publication.Where(p => p.Status != PublicationStatus.Trashed);

            var totalCount = query.Count();

            //advance search
            //bool isconvertible = false;
            //int myType = 0;
            //isconvertible = int.TryParse(request.Type, out myType);

            query = query.Where(p => (request.Type == null || p.Category.Name.Contains(request.Type))
               && (request.Author == null || p.Author.Contains(request.Author))
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
                    case "Type":

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
                .Select(s => new ReturnPublicationModel
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
                    Free = s.Free,
                    Hardcopy = s.Hardcopy,
                    Digitalcopy = s.Digitalcopy,
                    HDcopy = s.HDcopy,
                    HPrice = s.HPrice,
                    DPrice = s.DPrice,
                    HDPrice = s.HDPrice,
                    Pictures = s.Pictures,
                    ProofOfApproval = s.ProofOfApproval,
                    StockBalance = s.StockBalance,
                    WithdrawalReason = s.WithdrawalReason,
                    ProofOfWithdrawal = s.ProofOfWithdrawal,
                    DateAdded = s.DateAdded,
                    Status = s.Status,
                    WStatus = s.WStatus,
                    ViewCount = s.ViewCount,
                    PurchaseCount = s.PurchaseCount,
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
        */

        // GET: api/RnP/Publication (list)
        public List<ReturnPublicationModel> Get()
        {
            var publications = db.Publication.OrderBy(p => p.Status).OrderBy(p => p.Title).Select(s => new ReturnPublicationModel
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
                Free = s.Free,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                Status = s.Status,
                WStatus = s.WStatus,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                Category = s.Category.Name
            }).ToList();

            return publications;
        }

        // GET: api/RnP/Publication/{filters}
        public List<ReturnPublicationModel> Get(PublicationStatus status)
        {
            var publications = db.Publication.Where(p => p.Status == status).OrderBy(p => p.Status).OrderBy(p => p.Title).Select(s => new ReturnPublicationModel
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
                Free = s.Free,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                Status = s.Status,
                WStatus = s.WStatus,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                Category = s.Category.Name
            }).ToList();

            return publications;
        }

        // GET: api/RnP/Publication/5
        public ReturnPublicationModel Get(int id)
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
                Free = s.Free,
                Hardcopy = s.Hardcopy,
                Digitalcopy = s.Digitalcopy,
                HDcopy = s.HDcopy,
                HPrice = s.HPrice,
                DPrice = s.DPrice,
                HDPrice = s.HDPrice,
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                Status = s.Status,
                WStatus = s.WStatus,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount,
                Category = s.Category.Name
            }).FirstOrDefault();

            //approvals & withdrawal approvals
            //var approvals = db.PublicationApproval.Where(pa => pa.PublicationID == publication.ID).ToList();
            //var withdrawals = db.PublicationWithdrawal.Where(pw => pw.PublicationID == publication.ID).ToList();

            //publication.Approvals = approvals;
            //publication.Withdrawals = withdrawals;

            return publication;
        }

        // POST: api/RnP/Publication
        public HttpResponseMessage Post([FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // Not sure to use this or above
        [Route("api/RnP/CreatePublication")]
        [HttpPost]
        [ValidationActionFilter]
        public string CreatePublication([FromBody] UpdatePublicationModel model)
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
                    Free = model.Free,
                    Hardcopy = model.Hardcopy,
                    Digitalcopy = model.Digitalcopy,
                    HDcopy = model.HDcopy,
                    HPrice = model.HPrice,
                    DPrice = model.DPrice,
                    HDPrice = model.HDPrice,
                    Pictures = model.Pictures,
                    ProofOfApproval = model.ProofOfApproval,
                    StockBalance = model.StockBalance,
                    WithdrawalReason = "",
                    ProofOfWithdrawal = "",
                    DateAdded = DateTime.Now,
                    WStatus = PublicationWithdrawalStatus.None,
                    ViewCount = 0,
                    PurchaseCount = 0
                };
                if (model.HiddenPubStatus == "save")
                {
                    publication.Status = PublicationStatus.New;
                }
                else
                {
                    publication.Status = PublicationStatus.Submitted;
                }

                db.Publication.Add(publication);

                db.SaveChanges();

                return model.Title;
            }
            return "";
        }

        // PUT: api/RnP/Publication/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // Not sure to use this or above
        [Route("api/RnP/EditPublication")]
        [HttpPost]
        [ValidationActionFilter]
        public string EditPublication([FromBody] UpdatePublicationModel model)
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
                publication.Free = model.Free;
                publication.Hardcopy = model.Hardcopy;
                publication.Digitalcopy = model.Digitalcopy;
                publication.HDcopy = model.HDcopy;
                publication.HPrice = model.HPrice;
                publication.DPrice = model.DPrice;
                publication.HDPrice = model.HDPrice;
                publication.Pictures = model.Pictures;
                publication.ProofOfApproval = model.ProofOfApproval;
                publication.StockBalance = model.StockBalance;
            }
            if (model.HiddenPubStatus == "save")
            {
                publication.Status = PublicationStatus.New;
            }
            else
            {
                publication.Status = PublicationStatus.Submitted;
            }

            db.Entry(publication).State = EntityState.Modified;
            db.SaveChanges();

            return model.Title;
        }

        // DELETE: api/RnP/Publication/5
        //public bool Delete(int id)
        [Route("api/RnP/DeletePublication")]
        [HttpPost]
        //[ValidationActionFilter]
        public string DeletePublication(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                db.Publication.Remove(publication);
                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return "true";
            }

            return "false";
        }

    }
}
