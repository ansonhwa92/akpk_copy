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
    [Route("api/RnP/Research")]
    public class ResearchController : ApiController
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

        // GET: api/RnP/Research (all)
        public List<ResearchApiModel> Get()
        {
            var surveys = db.Survey.OrderBy(v => v.Title).Select(s => new ResearchApiModel
            {
                ID = s.ID,
                Type = s.Type,
                Category = s.Category,
                Title = s.Title,
                Description = s.Description,
                TargetGroup = s.TargetGroup,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Contents = s.Contents,
                Active = s.Active,
                ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status
            }).ToList();

            return surveys;
        }

        // GET: api/RnP/Research/{status} (published/etc.)
        public List<ResearchApiModel> Get(SurveyStatus status)
        {
            var surveys = db.Survey.Where(v => v.Status == status).OrderBy(v => v.Title).Select(s => new ResearchApiModel
            {
                ID = s.ID,
                Type = s.Type,
                Category = s.Category,
                Title = s.Title,
                Description = s.Description,
                TargetGroup = s.TargetGroup,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Contents = s.Contents,
                Active = s.Active,
                ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status
            }).ToList();

            return surveys;
        }

        // GET: api/RnP/Research/5
        public ResearchApiModel Get(int id)
        {
            var survey = db.Survey.Where(v => v.ID == id).Select(s => new ResearchApiModel
            {
                ID = s.ID,
                Type = s.Type,
                Category = s.Category,
                Title = s.Title,
                Description = s.Description,
                TargetGroup = s.TargetGroup,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Contents = s.Contents,
                Active = s.Active,
                ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status
            }).FirstOrDefault();

            //approvals
            var approvals = db.SurveyApproval.Where(va => va.SurveyID == survey.ID).ToList();

            survey.Approvals = approvals;

            return survey;
        }

        // POST: api/RnP/Research
        public HttpResponseMessage Post([FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // PUT: api/RnP/Research/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // DELETE: api/RnP/Research/5
        public bool Delete(int id)
        {
            var survey = db.Survey.Where(v => v.ID == id).FirstOrDefault();

            if (survey != null)
            {
                db.Survey.Remove(survey);
                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return true;
            }

            return false;

        }
    }
}
