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

        /*
         * The following API calls are for Survey record retrieval operations, including:
         * 1. Admin views/filters list of surveys
         * 2. Admin retrieves single survey for viewing (includes history)
         * 3. Admin retrieves single survey for review before submission
         * 4. Verifier/Approvers evaluate a survey in order to approve/reject it
         */

        // Main DataTable function for listing and filtering
        // POST: api/RnP/Survey/GetAll (DataTable)
        [Route("api/RnP/Survey/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterSurveyModel request)
        {

            var query = db.Survey.Where(p => p.Status != SurveyStatus.Trashed);   //TODO: all!!

            var totalCount = query.Count();

            //advance search
            //bool isconvertible = false;
            //int myType = 0;
            //isconvertible = int.TryParse(request.Type, out myType);

            /*
            query = query.Where(p => (request.Type == null || p.Category.Name.Contains(request.Type))
               && (request.Title == null || p.Author.Contains(request.Title))
               && (request.StartDate == null || p.Title.Contains(request.StartDate))
               && (request.EndDate == null || p.ISBN.Contains(request.EndDate))
               );
            */
            query = query.Where(p => (request.Title == null || p.Title.Contains(request.Title)));

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

                    case "Type":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Type);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Type);
                        }

                        break;

                    case "Duration":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.StartDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.StartDate);
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
                        query = query.OrderBy(o => o.Title);
                        break;
                }

            }
            else
            {
                query = query.OrderBy(o => o.Title);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new ReturnBriefSurveyModel
                {
                    ID = s.ID,
                    Title = s.Title,
                    Type = s.Type,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Duration = "",
                    InviteCount = s.InviteCount,
                    SubmitCount = s.SubmitCount,
                    Progress = "",
                    Status = s.Status
                }).ToList();
                //Duration = s.StartDate.ToString("dd/MM/yyyy") + " - " + s.EndDate.ToString("dd/MM/yyyy"),
                //Progress = s.InviteCount.ToString() + " / " + s.SubmitCount.ToString(),

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }

        // Alternative function for listing (all)
        // GET: api/RnP/Survey (list) - CURRENTLY NOT USED
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
                RequireLogin = s.RequireLogin,
                Contents = s.Contents,
                Active = s.Active,
                ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status
            }).ToList();

            return surveys;
        }

        // Function to get a single survey
        // NOTE: THE WAY INFO IS RETURNED HERE MAY NEED TO BE USED FOR ALL GET FUNCTIONS IN THE FUTURE
        // GET: api/RnP/Survey/5
        //public ReturnSurveyModel Get(int id)
        public IHttpActionResult Get(int id)
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
                RequireLogin = s.RequireLogin,
                Contents = s.Contents,
                Active = s.Active,
                ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status
            }).FirstOrDefault();

            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
            //return survey;
        }

        // Function to get a single survey
        // NOTE: THE WAY INFO IS RETURNED HERE MAY NEED TO BE USED FOR ALL GET FUNCTIONS IN THE FUTURE
        // GET: api/RnP/Survey/5
        //public ReturnSurveyModel Get(int id)
        [Route("api/RnP/Survey/GetSingle")]
        public IHttpActionResult GetSingle(int id)
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
                RequireLogin = s.RequireLogin,
                Contents = s.Contents,
                Active = s.Active,
                ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status
            }).FirstOrDefault();

            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
            //return survey;
        }

        // Function to get survey templates
        // GET: api/RnP/Survey/GetTemplates
        public List<UpdateSurveyTemplateModel> GetTemplates()
        {
            var surveys = db.Survey.OrderBy(v => v.Title).Select(s => new UpdateSurveyTemplateModel
            {
                ID = s.ID,
                TemplateName = s.TemplateName,
                TemplateDescription = s.TemplateDescription
            }).ToList();

            return surveys;
        }

        // Function to get json of a single survey template
        // GET: api/RnP/Survey/GetTemplate/5
        public string GetTemplate(int id)
        {
            var survey = db.Survey.Where(v => v.ID == id).Select(s => new ReturnSurveyModel
            {
                ID = s.ID,
                TemplateName = s.TemplateName,
                TemplateDescription = s.TemplateDescription,
                Contents = s.Contents
            }).FirstOrDefault();

            return survey.Contents;
        }

        // Function to get survey details for review before submission. The details retrieved include action
        // log history. This function is called for the fourth page of survey creation/editing.
        // GET: api/RnP/Survey/GetForReview/5
        [Route("api/RnP/Survey/GetForReview")]
        public ReturnSurveyModel GetForReview(int id)
        {
            var survey = db.Survey.Where(p => p.ID == id).Select(s => new ReturnSurveyModel
            {
                ID = s.ID,
                Type = s.Type,
                Category = s.Category,
                Title = s.Title,
                Description = s.Description,
                TargetGroup = s.TargetGroup,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                RequireLogin = s.RequireLogin,
                Contents = s.Contents,
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount,
                DmsPath = s.DmsPath                
            }).FirstOrDefault();

            return survey;
        }

        // Function to get survey info for viewing by admin.
        // This function has to handle submission, deletion (direct) or cancellation by admin
        // Admin can either submit from here or from create/edit review page. Admin can also delete survey from here
        // if still at draft stage or cancel it if at rejected stage (TBC - can admin cancel during review?)
        // GET: api/RnP/Survey/GetForView/5
        [Route("api/RnP/Survey/GetForView")]
        public ReturnSurveyModel GetForView(int id)
        {
            var survey = db.Survey.Where(p => p.ID == id).Select(s => new ReturnSurveyModel
            {
                ID = s.ID,
                Type = s.Type,
                Category = s.Category,
                Title = s.Title,
                Description = s.Description,
                TargetGroup = s.TargetGroup,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                RequireLogin = s.RequireLogin,
                Contents = s.Contents,
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount,
                DmsPath = s.DmsPath
            }).FirstOrDefault();

            return survey;
        }

        // Function to get survey info for evaluation (approve/reject) by verifier or approver.
        // Verifier/approver can approve (submit for approval) or reject (require amendment).
        // GET: api/RnP/Survey/GetForEvaluation/5
        [Route("api/RnP/Survey/GetForEvaluation")]
        public ReturnSurveyApprovalModel GetForEvaluation(int id)
        {
            var survey = db.Survey.Where(p => p.ID == id).Select(s => new ReturnSurveyModel
            {
                ID = s.ID,
                Type = s.Type,
                Category = s.Category,
                Title = s.Title,
                Description = s.Description,
                TargetGroup = s.TargetGroup,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                RequireLogin = s.RequireLogin,
                Contents = s.Contents,
                Pictures = s.Pictures,
                ProofOfApproval = s.ProofOfApproval,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount,
                DmsPath = s.DmsPath
            }).FirstOrDefault();

            var sapproval = db.SurveyApproval.Where(pa => pa.SurveyID == id && pa.Status == SurveyApprovalStatus.None).Select(s => new ReturnUpdateSurveyApprovalModel
            {
                ID = s.ID,
                SurveyID = s.SurveyID,
                Level = s.Level,
                ApproverId = 0,
                Status = SurveyApprovalStatus.None,
                Remarks = "",
                RequireNext = false
            }).FirstOrDefault();

            var sevaluation = new ReturnSurveyApprovalModel
            {
                Survey = survey,
                Approval = sapproval
            };

            return sevaluation;
        }

        // Function to get survey creation/approval history (action log).
        // Only completed approvals are listed. Usually called as the second call after retrieval of survey information.
        // GET: api/RnP/Survey/GetHistory/5
        [Route("api/RnP/Survey/GetHistory")]
        public List<SurveyApprovalHistoryModel> GetHistory(int id)
        {
            //var phistory = db.PublicationApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { tapproval = pa, tuser = u }).Where(pau => pau.tapproval.PublicationID == id && pau.tapproval.Status != PublicationApprovalStatus.None).OrderByDescending(pau => pau.tapproval.ApprovalDate).Select(s => new PublicationApprovalHistoryModel
            var shistory = db.SurveyApproval.Where(pa => pa.SurveyID == id && pa.Status != SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).Select(s => new SurveyApprovalHistoryModel
            {
                Level = s.Level,
                ApproverId = s.ApproverId,
                Status = s.Status,
                Remarks = s.Remarks
            }).ToList();

            return shistory;
        }

        /*
         * The following API calls are for Survey record modification operations, including:
         * 1. Admin clicks next (autosaves survey as draft) after creating
         * 2. Admin clicks next (autosaves survey as draft) after editing
         * 3. Admin saves survey builder as draft after creating
         * 4. Admin saves survey builder as draft after editing
         * 5. Admin submits survey for approval after reviewing
         * 6. Admin deletes survey
         * 7. Verifier/Approvers approve/reject the survey after evaluation
         * 8. Admin cancels survey after verifier/approver rejects
         */

        /*
        // POST: api/RnP/Survey
        public HttpResponseMessage Post([FromBody]string value)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }
        */

        // Function to save a survey (as draft) after creating a new one.
        // POST: api/RnP/Survey/Create
        [Route("api/RnP/Survey/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public string Create([FromBody] UpdateSurveyModel model)
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
                    RequireLogin = model.RequireLogin,
                    TemplateName = "",
                    TemplateDescription = "",
                    Contents = "",
                    Active = false,
                    Pictures = model.Pictures,
                    ProofOfApproval = model.ProofOfApproval,
                    CancelRemark = "",
                    DateAdded = DateTime.Now,
                    CreatorId = 0,
                    RefNo = "",
                    Status = SurveyStatus.New,
                    InviteCount = 0,
                    SubmitCount = 0,
                    DmsPath = ""
                };

                db.Survey.Add(survey);
                db.SaveChanges();
                //if (db.SaveChanges() > 0) {

                // modify survey by adding ref no based on year, month and new ID
                var refno = "";
                if (model.Type == SurveyType.Public)
                {
                    refno = "SVP/" + DateTime.Now.ToString("yyMM");
                }
                else
                {
                    refno = "SVT/" + DateTime.Now.ToString("yyMM");
                }
                refno += "/" + survey.ID.ToString("D4");
                survey.RefNo = refno;

                db.Entry(survey).State = EntityState.Modified;
                db.SaveChanges();

                return survey.ID.ToString() + "|" + model.Title;
            }
            return "";
        }

        /*
        // PUT: api/RnP/Survey/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { isSuccess = true });
            return response;
        }
        */

        // Function to save a survey (as draft) after editing an existing one.
        // POST: api/RnP/Survey/Edit
        [Route("api/RnP/Survey/Edit")]
        [HttpPost]
        [ValidationActionFilter]
        public string Edit([FromBody] UpdateSurveyModel model)
        {

            if (ModelState.IsValid)
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
                    survey.RequireLogin = model.RequireLogin;
                    survey.Pictures = model.Pictures;
                    survey.ProofOfApproval = model.ProofOfApproval;

                    db.Entry(survey).State = EntityState.Modified;
                    db.SaveChanges();

                    return model.Title;
                }
            }

            return "";
        }

        // Function to save survey build (as draft) after editing an existing one.
        // This is called automatically every time Survey Creator autosaves changes, and is also called by
        // the other controller for final save before review
        // POST: api/RnP/Survey/Build
        [Route("api/RnP/Survey/Build")]
        [HttpPost]
        [ValidationActionFilter]
        public string Build([FromBody] UpdateSurveyContentsModel model)
        {

            if (ModelState.IsValid)
            {
                var survey = db.Survey.Where(v => v.ID == model.ID).FirstOrDefault();

                if (survey != null)
                {
                    survey.Contents = model.Contents;

                    db.Entry(survey).State = EntityState.Modified;
                    db.SaveChanges();

                    return survey.Title;
                }
            }

            return "";
        }

        // Function to save survey template after creating/editing a survey build.
        // POST: api/RnP/Survey/SaveTemmplate
        [Route("api/RnP/Survey/SaveTemplate")]
        [HttpPost]
        [ValidationActionFilter]
        public string SaveTemplate([FromBody] UpdateSurveyTemplateModel model)
        {

            if (ModelState.IsValid)
            {
                var survey = db.Survey.Where(v => v.ID == model.ID).FirstOrDefault();

                if (survey != null)
                {
                    survey.TemplateName = model.TemplateName;
                    survey.TemplateDescription = model.TemplateDescription;

                    db.Entry(survey).State = EntityState.Modified;
                    db.SaveChanges();

                    return survey.Title;
                }
            }

            return "";
        }

        // Function to submit a survey for verification after reviewing.
        // POST: api/RnP/Survey/Submit
        [Route("api/RnP/Survey/Submit")]
        [HttpPost]
        [ValidationActionFilter]
        public string Submit([FromBody] UpdateSurveyModel model)
        {

            if (ModelState.IsValid)
            {
                var survey = db.Survey.Where(p => p.ID == model.ID).FirstOrDefault();

                if (survey != null)
                {
                    survey.Status = SurveyStatus.Submitted;

                    db.Entry(survey).State = EntityState.Modified;

                    // create first approval record (using existing ID)
                    var sapproval = new SurveyApproval
                    {
                        SurveyID = survey.ID,
                        Level = SurveyApprovalLevels.Verifier,
                        ApproverId = 0,
                        Status = SurveyApprovalStatus.None,
                        ApprovalDate = DateTime.Now,
                        Remarks = "",
                        RequireNext = false
                    };

                    db.SurveyApproval.Add(sapproval);
                    db.SaveChanges();

                    //return model.Title;
                    return survey.Title;
                }
            }
            return "";
        }

        // Function to submit a survey for verification from details page.
        // GET: api/RnP/Survey/SubmitByID
        [Route("api/RnP/Survey/SubmitByID")]
        public string SubmitByID(int id)
        {

            var survey = db.Survey.Where(p => p.ID == id).FirstOrDefault();

            if (survey != null)
            {
                survey.Status = SurveyStatus.Submitted;

                db.Entry(survey).State = EntityState.Modified;

                // create first approval record (using existing ID)
                var sapproval = new SurveyApproval
                {
                    SurveyID = survey.ID,
                    Level = SurveyApprovalLevels.Verifier,
                    ApproverId = 0,
                    Status = SurveyApprovalStatus.None,
                    ApprovalDate = DateTime.Now,
                    Remarks = "",
                    RequireNext = false
                };

                db.SurveyApproval.Add(sapproval);
                db.SaveChanges();

                //return model.Title;
                return survey.Title;
            }
            return "";
        }

        // Function to delete an unsubmitted survey after viewing on delete screen.
        // Can also be called from the Discard button at the review screen (in which case confirmation is via prompt).
        // GET: api/RnP/Survey/Delete/5
        [Route("api/RnP/Survey/Delete")]
        //[HttpPost]
        //[ValidationActionFilter]
        public string Delete(int id)
        {
            var survey = db.Survey.Where(v => v.ID == id).FirstOrDefault();

            if (survey != null)
            {
                string stitle = survey.Title;

                // delete record
                db.Survey.Remove(survey);

                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return stitle;
            }

            return "";
        }

        // Function for when Verifier/Approver approves/rejects survey
        // POST: api/RnP/Survey/Evaluate
        [Route("api/RnP/Survey/Evaluate")]
        [HttpPost]
        [ValidationActionFilter]
        public string Evaluate([FromBody] ReturnSurveyApprovalModel model)
        {

            if (ModelState.IsValid)
            {
                var sapproval = db.SurveyApproval.Where(pa => pa.ID == model.Approval.ID).FirstOrDefault();

                if (sapproval != null)
                {
                    sapproval.ApproverId = model.Approval.ApproverId;
                    sapproval.Status = model.Approval.Status;
                    sapproval.ApprovalDate = DateTime.Now;
                    sapproval.Remarks = model.Approval.Remarks;
                    sapproval.RequireNext = model.Approval.RequireNext;
                    // requirenext is always set to true when coming from verifier approval, and always false from approver3

                    db.Entry(sapproval).State = EntityState.Modified;
                    db.SaveChanges();

                    var survey = db.Survey.Where(p => p.ID == sapproval.SurveyID).FirstOrDefault();
                    if (survey != null)
                    {
                        // proceed depending on status (assuming user can only pick approve and reject)
                        if (model.Approval.Status == SurveyApprovalStatus.Rejected)
                        {
                            if (survey.Status == SurveyStatus.Submitted)
                            {
                                survey.Status = SurveyStatus.VerifierRejected;
                                db.Entry(survey).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else if (survey.Status == SurveyStatus.Verified)
                            {
                                survey.Status = SurveyStatus.ApproverRejected;
                                db.Entry(survey).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            // proceed depending on requirenext
                            if (model.Approval.RequireNext == false)
                            {
                                // no more approvals necessary (assumes verifier will never get here)
                                survey.Status = SurveyStatus.Approved;
                                db.Entry(survey).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                SurveyApprovalLevels nextlevel;
                                switch (sapproval.Level)
                                {
                                    case SurveyApprovalLevels.Verifier:
                                        nextlevel = SurveyApprovalLevels.Approver1;
                                        break;
                                    case SurveyApprovalLevels.Approver1:
                                        nextlevel = SurveyApprovalLevels.Approver1;
                                        break;
                                    case SurveyApprovalLevels.Approver2:
                                        nextlevel = SurveyApprovalLevels.Approver1;
                                        break;
                                    default:
                                        nextlevel = SurveyApprovalLevels.Approver1;
                                        break;
                                }

                                // create next approval record
                                var snewapproval = new SurveyApproval
                                {
                                    SurveyID = sapproval.SurveyID,
                                    Level = nextlevel,
                                    ApproverId = 0,
                                    Status = SurveyApprovalStatus.None,
                                    ApprovalDate = DateTime.Now,
                                    Remarks = "",
                                    RequireNext = false
                                };

                                db.SurveyApproval.Add(snewapproval);
                                db.SaveChanges();
                            }

                        }

                        return survey.Title;
                    }
                }
            }

            return "";
        }

        // Function to cancel a survey after rejection by verifier or approver.
        // POST: api/RnP/Survey/Cancel
        [Route("api/RnP/Survey/Cancel")]
        [HttpPost]
        [ValidationActionFilter]
        public string Cancel([FromBody] UpdateSurveyCancellationModel model)
        {

            if (ModelState.IsValid)
            {
                var survey = db.Survey.Where(p => p.ID == model.ID).FirstOrDefault();

                if (survey != null)
                {
                    survey.Status = SurveyStatus.Trashed;
                    survey.CancelRemark = model.CancelRemark;
                    survey.DateCancelled = DateTime.Now;

                    db.Entry(survey).State = EntityState.Modified;

                    db.SaveChanges();

                    return survey.Title;
                }
            }
            return "";
        }

    }
}