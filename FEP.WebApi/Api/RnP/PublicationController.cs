using FEP.Model;
using FEP.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


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

        // GET: api/RnP/Publication (all)
        public List<PublicationApiModel> Get()
        {
            var publications = db.Publication.OrderBy(p => p.Title).Select(s => new PublicationApiModel
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
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                Status = s.Status,
                WStatus = s.WStatus,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount
            }).ToList();

            return publications;
        }

        // GET: api/RnP/Publication/{status} (published/etc.)
        public List<PublicationApiModel> Get(PublicationStatus status)
        {
            var publications = db.Publication.Where(p => p.Status == status).OrderBy(p => p.Title).Select(s => new PublicationApiModel
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
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                Status = s.Status,
                WStatus = s.WStatus,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount
            }).ToList();

            return publications;
        }

        // GET: api/RnP/Publication/5
        public PublicationApiModel Get(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).Select(s => new PublicationApiModel
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
                ProofOfApproval = s.ProofOfApproval,
                StockBalance = s.StockBalance,
                WithdrawalReason = s.WithdrawalReason,
                ProofOfWithdrawal = s.ProofOfWithdrawal,
                DateAdded = s.DateAdded,
                Status = s.Status,
                WStatus = s.WStatus,
                ViewCount = s.ViewCount,
                PurchaseCount = s.PurchaseCount
            }).FirstOrDefault();

            //approvals & withdrawal approvals
            var approvals = db.PublicationApproval.Where(pa => pa.PublicationID == publication.ID).ToList();
            var withdrawals = db.PublicationWithdrawal.Where(pw => pw.PublicationID == publication.ID).ToList();

            publication.Approvals = approvals;
            publication.Withdrawals = withdrawals;

            return publication;
        }

        // POST: api/RnP/Publication
        public HttpResponseMessage Post([FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // PUT: api/RnP/Publication/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // DELETE: api/RnP/Publication/5
        public bool Delete(int id)
        {
            var publication = db.Publication.Where(p => p.ID == id).FirstOrDefault();

            if (publication != null)
            {
                db.Publication.Remove(publication);
                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return true;
            }

            return false;

        }
    }
}
