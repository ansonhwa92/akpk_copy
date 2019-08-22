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
    [Route("api/RnP/Survey")]
    public class SurveyController : ApiController
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

        // GET: api/RnP/Survey (list)
        public List<ReturnSurveyModel> Get()
        {
            var surveys = db.Survey.OrderBy(v => v.Title).Select(s => new ReturnSurveyModel
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

        // GET: api/RnP/Survey/{filters}
        public List<ReturnSurveyModel> Get(SurveyStatus status)
        {
            var surveys = db.Survey.Where(v => v.Status == status).OrderBy(v => v.Title).Select(s => new ReturnSurveyModel
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

        // GET: api/RnP/Survey/5
        public ReturnSurveyModel Get(int id)
        {
            var survey = db.Survey.Where(v => v.ID == id).Select(s => new ReturnSurveyModel
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
            //var approvals = db.SurveyApproval.Where(va => va.SurveyID == survey.ID).ToList();

            //survey.Approvals = approvals;

            return survey;
        }

        // POST: api/RnP/Survey
        public HttpResponseMessage Post([FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // Not sure to use this or above
        [Route("api/RnP/CreateSurvey")]
        [HttpPost]
        [ValidationActionFilter]
        public string CreateSurvey([FromBody] UpdateSurveyModel model)
        {

            if (ModelState.IsValid)
            {
                var survey = new Survey
                {
                    Type = model.Type,
                    Category = model.Category,
                    Title = model.Title,
                    Description = model.Description,
                    TargetGroup = model.TargetGroup,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Contents = "",
                    Active = false,
                    Pictures = model.Pictures,
                    ProofOfApproval = model.ProofOfApproval,
                    DateAdded = DateTime.Now,
                    Status = SurveyStatus.New
                };
                // Status = New because at this stage, contents (questions) are still not defined

                db.Survey.Add(survey);

                db.SaveChanges();

                return model.Title;
            }
            return "";
        }

        // PUT: api/RnP/Survey/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }

        // Not sure to use this or above
        [Route("api/RnP/EditSurvey")]
        [HttpPost]
        [ValidationActionFilter]
        public string EditSurvey([FromBody] UpdateSurveyModel model)
        {

            var survey = db.Survey.Where(v => v.ID == model.ID).FirstOrDefault();

            if (survey != null)
            {
                survey.Type = model.Type;
                survey.Category = model.Category;
                survey.Title = model.Title;
                survey.Description = model.Description;
                survey.TargetGroup = model.TargetGroup;
                survey.StartDate = model.StartDate;
                survey.EndDate = model.EndDate;
                survey.Pictures = model.Pictures;
                survey.ProofOfApproval = model.ProofOfApproval;
            }

            db.Entry(survey).State = EntityState.Modified;
            db.SaveChanges();

            return model.Title;
        }

        // DELETE: api/RnP/Survey/5
        //public bool Delete(int id)
        [Route("api/RnP/DeleteSurvey")]
        [HttpPost]
        //[ValidationActionFilter]
        public string DeleteSurvey(int id)
        {
            var survey = db.Survey.Where(v => v.ID == id).FirstOrDefault();

            if (survey != null)
            {
                db.Survey.Remove(survey);
                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return "true";
            }

            return "false";

        }
    }
}