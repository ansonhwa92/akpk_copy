using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.RnP;
using FEP.WebApiModel.Integration;
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
    [Route("api/RnP/Survey")]
    public class SurveyController : ApiController
    {
        private DbEntities db = new DbEntities();

        private string HomeURL = "http://10.250.1.134";
        private string SubURL = "FEP_UAT_6";

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

            var query = db.Survey.Where(p => p.Status <= SurveyStatus.Trashed);   //TODO: all!!

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
                    RefNo = s.RefNo,
                    Title = s.Title,
                    Type = s.Type,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Duration = "",
                    InviteCount = s.InviteCount,
                    SubmitCount = s.SubmitCount,
                    Progress = "",
                    Status = s.Status,
                    ApprovalLevel = db.SurveyApproval.Where(sa => sa.SurveyID == s.ID && sa.Status == SurveyApprovalStatus.None).OrderByDescending(sa => sa.ApprovalDate).FirstOrDefault().Level
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

        // GET: api/RnP/Survey (list) - CURRENTLY NOT USED
        // (i.e. public/targeted surveys that don't require Login)
        public List<ReturnSurveyModel> Get()
        {
            // duration checked at browser view (so we can show EXPIRED for recent surveys and hide older ones)
            // NOPE!
            // If we wanna do that we'll do separate function for expired surveys
            // TODO: but startdate checking disabled for now 
            // active (for now = published) only
            // TODO: time portion may make duration checking fail
            var currdate = DateTime.Now;
            var tempsurveys = db.Survey.Where(v => v.Status == SurveyStatus.Published && v.RequireLogin == false && v.EndDate > currdate).OrderBy(v => v.StartDate).OrderBy(v => v.Title).Select(s => new ReturnSurveyModel
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
                //Pictures = s.Pictures,
                //ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status,
                CancelRemark = s.CancelRemark,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                DateCancelled = s.DateCancelled,
                DmsPath = s.DmsPath,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount
            }).ToList();

            // can do this in the select above but doing it here for clarity (compare with next function)

            List<ReturnSurveyModel> surveys = new List<ReturnSurveyModel> { };

            foreach (ReturnSurveyModel sitem in tempsurveys)
            {
                if (sitem.Type == SurveyType.Public)
                {
                    surveys.Add(sitem);
                }
            }

            return surveys;
        }

        // GET: api/RnP/Survey/GetAnonymousSurveys (list) - CURRENTLY USED FOR ANONYMOUS BROWSING
        // (i.e. public/targeted surveys that don't require Login)
        // This function is a synonym for the above function
        [Route("api/RnP/Survey/GetAnonymousSurveys")]
        [HttpGet]
        public BrowseSurveyModel GetAnonymousSurveys(string keyword, string sorting)
        {
            // duration checked at browser view (so we can show EXPIRED for recent surveys and hide older ones)
            // NOPE!
            // TODO: but startdate checking disabled for now 
            // active (for now = published) only
            // TODO: time portion may make duration checking fail
            // NOTE: survey will be included only if it's public mass and doesn't require login

            var currdate = DateTime.Now;

            var query = db.Survey.Where(v => v.Status == SurveyStatus.Published && v.RequireLogin == false && v.Type == SurveyType.Public && v.StartDate <= currdate && v.EndDate >= currdate);

            var totalCount = query.Count();

            query = query.Where(v => (keyword == null || keyword == "" || v.Title.Contains(keyword)));

            var filteredCount = query.Count();

            if (sorting == "expiry")
            {
                query = query.OrderBy(o => o.EndDate).OrderBy(o => o.Title);
            }
            else
            {
                query = query.OrderBy(o => o.Title).OrderBy(o => o.EndDate);
            }

            var data = query.Skip(0).Take(filteredCount).Select(s => new ReturnSurveyModel
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
                //Pictures = s.Pictures,
                //ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status,
                CancelRemark = s.CancelRemark,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                DateCancelled = s.DateCancelled,
                DmsPath = s.DmsPath,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount
            }).ToList();

            var browser = new BrowseSurveyModel
            {
                Keyword = keyword,
                Sorting = sorting,
                LastIndex = filteredCount,
                ItemCount = totalCount,
                Surveys = data
            };

            return browser;
        }

        // GET: api/RnP/Survey/GetLoginSurveys (list) - CURRENTLY USED FOR NON-ANONYMOUS BROWSING
        // (i.e. public/targeted surveys that require Login)
        [Route("api/RnP/Survey/GetLoginSurveys")]
        [HttpGet]
        public BrowseSurveyModel GetLoginSurveys(string useremail, string keyword, string sorting)
        {
            // duration checked at browser view (so we can show EXPIRED for recent surveys and hide older ones)
            // NOPE!
            // TODO: but startdate checking disabled for now 
            // active (for now = published) only
            // TODO: time portion may make duration checking fail
            // NOTE: survey included if it requires login (regardless of type: public or targeted)

            var currdate = DateTime.Now;

            var query = db.Survey.Where(v => v.Status == SurveyStatus.Published && v.RequireLogin == true && v.StartDate <= currdate && v.EndDate >= currdate);

            var totalCount = query.Count();

            query = query.Where(v => (keyword == null || keyword == "" || v.Title.Contains(keyword)));

            var filteredCount = query.Count();

            if (sorting == "expiry")
            {
                query = query.OrderBy(o => o.EndDate).OrderBy(o => o.Title);
            }
            else
            {
                query = query.OrderBy(o => o.Title).OrderBy(o => o.EndDate);
            }

            var data = query.Skip(0).Take(filteredCount).Select(s => new ReturnSurveyModel
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
                //Pictures = s.Pictures,
                //ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status,
                CancelRemark = s.CancelRemark,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                DateCancelled = s.DateCancelled,
                DmsPath = s.DmsPath,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount
            }).ToList();

            int newfilteredCount = 0;

            List<ReturnSurveyModel> surveys = new List<ReturnSurveyModel> { };

            foreach (ReturnSurveyModel sitem in data)
            {
                if (sitem.Type == SurveyType.Targeted)
                {
                    var groups = sitem.TargetGroup.Split(',');
                    foreach (string group in groups)
                    {
                        if (UserInGroup(useremail, group))
                        {
                            surveys.Add(sitem);
                            newfilteredCount++;
                        }
                    }
                }
                else
                {
                    surveys.Add(sitem);
                    newfilteredCount++;
                }
            }

            var browser = new BrowseSurveyModel
            {
                Keyword = keyword,
                Sorting = sorting,
                LastIndex = newfilteredCount,
                ItemCount = totalCount,
                Surveys = data
            };

            return browser;
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
                //Pictures = s.Pictures,
                //ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status,
                CancelRemark = s.CancelRemark,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                DateCancelled = s.DateCancelled,
                DmsPath = s.DmsPath,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount
            }).FirstOrDefault();

            if (survey == null)
            {
                return NotFound();
            }

            survey.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

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
                //ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status,
                CreatorName = ""
            }).FirstOrDefault();

            if (survey == null)
            {
                return NotFound();
            }

            var user = db.User.Where(u => u.Id == survey.CreatorId).FirstOrDefault();
            if (user != null)
            {
                survey.CreatorName = user.Name;
            }

            survey.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            return Ok(survey);
            //return survey;
        }

        // Function to get a single survey
        // GET: api/RnP/Survey/GetLinkedSurvey/
        [Route("api/RnP/Survey/GetLinkedSurvey")]
        public ReturnSurveyModel GetLinkedSurvey(string refno, string email)
        {
            var survey = db.Survey.Where(v => v.RefNo == refno).Select(s => new ReturnSurveyModel
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
                //ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                Status = s.Status,
                CreatorName = ""
            }).FirstOrDefault();

            if (survey == null)
            {
                return null;
            }

            var user = db.User.Where(u => u.Id == survey.CreatorId).FirstOrDefault();
            if (user != null)
            {
                survey.CreatorName = user.Name;
            }

            survey.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.CoverImage && p.ParentId == survey.ID), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.AuthorImage && p.ParentId == survey.ID), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.ProofOfApproval && p.ParentId == survey.ID), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            if (survey.Type == SurveyType.Targeted)
            {
                var groups = survey.TargetGroup.Split(',');
                foreach (string group in groups)
                {
                    if (UserInGroup(email, group))
                    {
                        return survey;
                    }
                }
            }

            return null;
        }

        // Function to get survey templates
        // GET: api/RnP/Survey/GetTemplates
        [Route("api/RnP/Survey/GetTemplates")]
        public List<UpdateSurveyTemplateModel> GetTemplates()
        {
            var surveys = db.Survey.Where(v => v.TemplateName != "").OrderBy(v => v.TemplateName).Select(s => new UpdateSurveyTemplateModel
            {
                ID = s.ID,
                TemplateName = s.TemplateName,
                TemplateDescription = s.TemplateDescription
            }).ToList();

            return surveys;
        }

        // Function to get json of a single survey template
        // GET: api/RnP/Survey/GetTemplate/5
        [Route("api/RnP/Survey/GetTemplate")]
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
                //Pictures = s.Pictures,
                //ProofOfApproval = s.ProofOfApproval,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount,
                DmsPath = s.DmsPath,
                CreatorName = ""
            }).FirstOrDefault();

            var user = db.User.Where(u => u.Id == survey.CreatorId).FirstOrDefault();
            if (user != null)
            {
                survey.CreatorName = user.Name;
            }

            survey.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

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
                //Pictures = s.Pictures,
                //ProofOfApproval = s.ProofOfApproval,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount,
                DmsPath = s.DmsPath,
                CreatorName = ""
            }).FirstOrDefault();

            var user = db.User.Where(u => u.Id == survey.CreatorId).FirstOrDefault();
            if (user != null)
            {
                survey.CreatorName = user.Name;
            }

            survey.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

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
                //Pictures = s.Pictures,
                //ProofOfApproval = s.ProofOfApproval,
                CancelRemark = s.CancelRemark,
                DateAdded = s.DateAdded,
                CreatorId = s.CreatorId,
                RefNo = s.RefNo,
                Status = s.Status,
                DateCancelled = s.DateCancelled,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount,
                DmsPath = s.DmsPath,
                CreatorName = ""
            }).FirstOrDefault();

            var user = db.User.Where(u => u.Id == survey.CreatorId).FirstOrDefault();
            if (user != null)
            {
                survey.CreatorName = user.Name;
            }

            survey.CoverPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.CoverImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.AuthorPictures = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.AuthorImage && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
            survey.ProofOfApproval = db.FileDocument.Where(f => f.Display).Join(db.SurveyFile.Where(p => p.FileCategory == SurveyFileCategory.ProofOfApproval && p.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

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
            var shistory = db.SurveyApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { pa.SurveyID, pa.Level, pa.ApproverId, pa.ApprovalDate, pa.Status, pa.Remarks, UserName = u.Name }).Where(pa => pa.SurveyID == id && pa.Status != SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).Select(s => new SurveyApprovalHistoryModel
            {
                Level = s.Level,
                ApproverId = s.ApproverId,
                ApprovalDate = s.ApprovalDate,
                UserName = s.UserName,
                Status = s.Status,
                Remarks = s.Remarks
            }).ToList();

            return shistory;
        }

        // Function to the next pending approval record
        // This is used to check for who's in charge of the next step in approval
        // GET: api/RnP/Survey/GetNextApproval/5
        [Route("api/RnP/Survey/GetNextApproval")]
        public SurveyApprovalHistoryModel GetNextApproval(int id)
        {
            //var phistory = db.PublicationApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { tapproval = pa, tuser = u }).Where(pau => pau.tapproval.PublicationID == id && pau.tapproval.Status != PublicationApprovalStatus.None).OrderByDescending(pau => pau.tapproval.ApprovalDate).Select(s => new PublicationApprovalHistoryModel
            var shistory = db.SurveyApproval.Where(sa => sa.SurveyID == id && sa.Status == SurveyApprovalStatus.None).OrderByDescending(sa => sa.ApprovalDate).Select(s => new SurveyApprovalHistoryModel
            {
                Level = s.Level,
                ApproverId = s.ApproverId,
                ApprovalDate = s.ApprovalDate,
                Status = s.Status,
                Remarks = s.Remarks
            }).FirstOrDefault();

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
        public string Create([FromBody] CreateSurveyModelNoFile model)
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
                    StartDate = DateTime.Parse(model.StartDate.ToString("yyyy/MM/dd") + " 00:00:00"),
                    EndDate = DateTime.Parse(model.EndDate.ToString("yyyy/MM/dd") + " 23:59:59"),
                    RequireLogin = model.RequireLogin,
                    TemplateName = "",
                    TemplateDescription = "",
                    Contents = "",
                    Active = false,
                    //Pictures = model.Pictures,
                    //ProofOfApproval = model.ProofOfApproval,
                    CancelRemark = "",
                    DateAdded = DateTime.Now,
                    CreatorId = model.CreatorId,
                    RefNo = "",
                    Status = SurveyStatus.New,
                    InviteCount = 0,
                    SubmitCount = 0,
                    DmsPath = ""
                };

                db.Survey.Add(survey);
                db.SaveChanges();
                //if (db.SaveChanges() > 0) {

                //files 1
                foreach (var fileid in model.CoverFilesId)
                {
                    var coverfile = new SurveyFile
                    {
                        FileCategory = SurveyFileCategory.CoverImage,
                        FileId = fileid,
                        ParentId = survey.ID
                    };

                    db.SurveyFile.Add(coverfile);
                }

                //files 2
                foreach (var fileid in model.AuthorFilesId)
                {
                    var authorfile = new SurveyFile
                    {
                        FileCategory = SurveyFileCategory.AuthorImage,
                        FileId = fileid,
                        ParentId = survey.ID
                    };

                    db.SurveyFile.Add(authorfile);
                }

                //files 3
                foreach (var fileid in model.ProofFilesId)
                {
                    var prooffile = new SurveyFile
                    {
                        FileCategory = SurveyFileCategory.ProofOfApproval,
                        FileId = fileid,
                        ParentId = survey.ID
                    };

                    db.SurveyFile.Add(prooffile);
                }

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
        public string Edit([FromBody] EditSurveyModelNoFile model)
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
                    survey.StartDate = DateTime.Parse(model.StartDate.ToString("yyyy/MM/dd") + " 00:00:00");
                    survey.EndDate = DateTime.Parse(model.EndDate.ToString("yyyy/MM/dd") + " 23:59:59");
                    survey.RequireLogin = model.RequireLogin;
                    //survey.Pictures = model.Pictures;
                    //survey.ProofOfApproval = model.ProofOfApproval;
                    //survey.CreatorId = model.CreatorId;

                    db.Entry(survey).State = EntityState.Modified;

                    //files 1

                    var attachments1 = db.SurveyFile.Where(s => s.FileCategory == SurveyFileCategory.CoverImage && s.ParentId == model.ID).ToList();

                    if (attachments1 != null)
                    {
                        if (model.CoverPictures == null)
                        {
                            foreach (var attachment in attachments1)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.SurveyFile.Remove(attachment);
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
                                    db.SurveyFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.CoverFilesId)
                    {
                        var coverfile = new SurveyFile
                        {
                            FileCategory = SurveyFileCategory.CoverImage,
                            FileId = fileid,
                            ParentId = survey.ID
                        };

                        db.SurveyFile.Add(coverfile);
                    }

                    //files 2

                    var attachments2 = db.SurveyFile.Where(s => s.FileCategory == SurveyFileCategory.AuthorImage && s.ParentId == model.ID).ToList();

                    if (attachments2 != null)
                    {
                        if (model.AuthorPictures == null)
                        {
                            foreach (var attachment in attachments2)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.SurveyFile.Remove(attachment);
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
                                    db.SurveyFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.AuthorFilesId)
                    {
                        var authorfile = new SurveyFile
                        {
                            FileCategory = SurveyFileCategory.AuthorImage,
                            FileId = fileid,
                            ParentId = survey.ID
                        };

                        db.SurveyFile.Add(authorfile);
                    }

                    //files 3

                    var attachments3 = db.SurveyFile.Where(s => s.FileCategory == SurveyFileCategory.ProofOfApproval && s.ParentId == model.ID).ToList();

                    if (attachments3 != null)
                    {
                        if (model.ProofOfApproval == null)
                        {
                            foreach (var attachment in attachments3)
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;
                                db.SurveyFile.Remove(attachment);
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
                                    db.SurveyFile.Remove(attachment);
                                }
                            }
                        }
                    }

                    foreach (var fileid in model.ProofFilesId)
                    {
                        var prooffile = new SurveyFile
                        {
                            FileCategory = SurveyFileCategory.ProofOfApproval,
                            FileId = fileid,
                            ParentId = survey.ID
                        };

                        db.SurveyFile.Add(prooffile);
                    }

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

        // Function to save survey template after creating/editing a survey build (w/o using model)
        // POST: api/RnP/Survey/SaveTemmplateNonmodel
        [Route("api/RnP/Survey/SaveTemplateNonmodel")]
        [HttpPost]
        [ValidationActionFilter]
        //public string SaveTemplateNonmodel(int TemplateID, string TemplateName, string TemplateDescription)
        public string SaveTemplateNonmodel(System.Web.Mvc.FormCollection form)
        {
            // TODO: check name existence and return "exists" if true

            if ((form["TemplateName"] != "") && (form["TemplateDescription"] != ""))
            {
                int tid = System.Int32.Parse(form["TemplateID"]);
                var survey = db.Survey.Where(v => v.ID == tid).FirstOrDefault();

                if (survey != null)
                {
                    survey.TemplateName = form["TemplateName"];
                    survey.TemplateDescription = form["TemplateDescription"];

                    db.Entry(survey).State = EntityState.Modified;
                    db.SaveChanges();

                    return survey.TemplateName;
                }
            }

            return "";
        }

        // Function to submit a survey for verification after reviewing.
        // POST: api/RnP/Survey/Submit
        [Route("api/RnP/Survey/Submit")]
        [HttpPost]
        [ValidationActionFilter]
        public string Submit([FromBody] UpdateSurveyReviewModel model)
        {

            if (ModelState.IsValid)
            {
                var survey = db.Survey.Where(p => p.ID == model.Survey.ID).FirstOrDefault();

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

                    //return survey.Title;
                    if (survey.Type == SurveyType.Public)
                    {
                        return survey.Title + "|" + "Public Mass" + "|" + survey.RefNo;
                    }
                    else
                    {
                        return survey.Title + "|" + "Targeted Groups" + "|" + survey.RefNo;
                    }
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

                //return survey.Title;
                if (survey.Type == SurveyType.Public)
                {
                    return survey.Title + "|" + "Public Mass" + "|" + survey.RefNo;
                }
                else
                {
                    return survey.Title + "|" + "Targeted Groups" + "|" + survey.RefNo;
                }
            }
            return "";
        }

        // Function to publish a survey from details page.
        // GET: api/RnP/Survey/PublishByID
        [Route("api/RnP/Survey/PublishByID")]
        public string PublishByID(int id, string BaseURL)
        {

            var survey = db.Survey.Where(p => p.ID == id).FirstOrDefault();

            if (survey != null)
            {
                survey.Status = SurveyStatus.Published;

                db.Entry(survey).State = EntityState.Modified;

                db.SaveChanges();

                //return publication.Title;
                if (survey.Type == SurveyType.Public)
                {
                    return survey.Title + "|" + "Public Mass" + "|" + survey.RefNo;
                }
                else
                {
                    var emailres = SendEmailNotificationSurveyBroadcast(survey, BaseURL);
                    return survey.Title + "|" + "Targeted Groups" + "|" + survey.RefNo;
                }
            }
            return "";
        }

        // Function to extend a survey start and/or end date
        // POST: api/RnP/Survey/Extend
        [Route("api/RnP/Survey/Extend")]
        [HttpPost]
        [ValidationActionFilter]
        public string Extend([FromBody] UpdateSurveyExtensionModel model)
        {

            if (ModelState.IsValid)
            {
                var survey = db.Survey.Where(p => p.ID == model.ID).FirstOrDefault();

                if (survey != null)
                {
                    survey.StartDate = model.NewStartDate;
                    survey.EndDate = model.NewEndDate;

                    db.Entry(survey).State = EntityState.Modified;

                    db.SaveChanges();

                    //return survey.Title;
                    if (survey.Type == SurveyType.Public)
                    {
                        return survey.Title + "|" + "Public Mass" + "|" + survey.RefNo;
                    }
                    else
                    {
                        return survey.Title + "|" + "Targeted Groups" + "|" + survey.RefNo;
                    }
                }
            }
            return "";
        }

        // Function to UNpublish a survey from details page.
        // POST: api/RnP/Survey/Unpublish
        [Route("api/RnP/Survey/Unpublish")]
        [HttpPost]
        public string Unpublish(int id)
        {

            var survey = db.Survey.Where(p => p.ID == id).FirstOrDefault();

            if (survey != null)
            {
                survey.Status = SurveyStatus.Unpublished;

                db.Entry(survey).State = EntityState.Modified;

                db.SaveChanges();

                //return publication.Title;
                if (survey.Type == SurveyType.Public)
                {
                    return survey.Title + "|" + "Public Mass" + "|" + survey.RefNo;
                }
                else
                {
                    return survey.Title + "|" + "Targeted Groups" + "|" + survey.RefNo;
                }
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
                    // HERE
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
                                        survey.Status = SurveyStatus.Verified;
                                        db.Entry(survey).State = EntityState.Modified;
                                        break;
                                    case SurveyApprovalLevels.Approver1:
                                        nextlevel = SurveyApprovalLevels.Approver2;
                                        break;
                                    case SurveyApprovalLevels.Approver2:
                                        nextlevel = SurveyApprovalLevels.Approver3;
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
                                // HERE
                                db.SaveChanges();
                            }

                        }

                        if (survey.Type == SurveyType.Public)
                        {
                            return survey.ID + "|" + survey.Title + "|" + "Public Mass" + "|" + survey.RefNo;
                        }
                        else
                        {
                            return survey.ID + "|" + survey.Title + "|" + "Targeted Groups" + "|" + survey.RefNo;
                        }
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

                    //return survey.Title;
                    if (survey.Type == SurveyType.Public)
                    {
                        return survey.Title + "|" + "Public Mass" + "|" + survey.RefNo;
                    }
                    else
                    {
                        return survey.Title + "|" + "Targeted Groups" + "|" + survey.RefNo;
                    }
                }
            }
            return "";
        }

        // Function to save survey response (testing)
        // POST: api/RnP/Survey/SubmitTest
        [Route("api/RnP/Survey/SubmitTest")]
        [HttpPost]
        [ValidationActionFilter]
        public string SubmitTest([FromBody] UpdateSurveyResponseModel model)
        {

            if (ModelState.IsValid)
            {
                var surveyresponse = new SurveyResponse
                {
                    SurveyID = model.SurveyID,
                    Type = model.Type,
                    UserId = model.UserId,
                    Email = model.Email,
                    Contents = model.Contents,
                    ResponseDate = DateTime.Now
                };

                db.SurveyResponse.Add(surveyresponse);
                db.SaveChanges();

                return "ok";
            }
            return "";
        }

        // Function to save survey response (actual)
        // POST: api/RnP/Survey/SubmitAnswers
        [Route("api/RnP/Survey/SubmitAnswers")]
        [HttpPost]
        [ValidationActionFilter]
        public string SubmitAnswers([FromBody] UpdateSurveyResponseModel model)
        {

            if (ModelState.IsValid)
            {
                var surveyresponse = new SurveyResponse
                {
                    SurveyID = model.SurveyID,
                    Type = model.Type,
                    UserId = model.UserId,
                    Email = model.Email,
                    Contents = model.Contents,
                    ResponseDate = DateTime.Now
                };

                db.SurveyResponse.Add(surveyresponse);
                db.SaveChanges();

                var survey = db.Survey.Where(p => p.ID == model.SurveyID).FirstOrDefault();

                if (survey != null)
                {
                    var emailres = SendEmailNotificationSurveySubmission(surveyresponse, survey);
                }

                return "ok";
            }
            return "";
        }

        // Function to get notification receivers based on notification category and type.
        // Called when sending notifications
        // TO CONSIDER: Currently cancellations notify all approvers.
        // GET: api/RnP/Survey/GetNotificationReceivers/
        [Route("api/RnP/Survey/GetNotificationReceivers")]
        public List<int> GetNotificationReceivers(NotificationCategory cat, NotificationType type, SurveyApprovalStatus status, bool forward)
        {
            List<int> receivers = new List<int> { };

            // prepare
            bool toadmin = false;
            bool toverifier = false;
            bool toapprover1 = false;
            bool toapprover2 = false;
            bool toapprover3 = false;

            if (type == NotificationType.Submit_Survey_Creation)
            {
                toverifier = true;
            }
            else if (type == NotificationType.Submit_Survey_Cancellation)
            {
                toverifier = true;
                toapprover1 = true;
                toapprover2 = true;
                toapprover3 = true;
            }
            else if (type == NotificationType.Submit_Survey_Publication)
            {
                toverifier = true;
                toapprover1 = true;
                toapprover2 = true;
                toapprover3 = true;
            }
            else if (type == NotificationType.Verify_Survey_Creation)
            {
                if (status == SurveyApprovalStatus.Rejected)
                {
                    toadmin = true;
                }
                else
                {
                    toapprover1 = true;
                }
            }
            else if (type == NotificationType.Approve_Survey_Creation_1)
            {
                if (status == SurveyApprovalStatus.Rejected)
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
            else if (type == NotificationType.Approve_Survey_Creation_2)
            {
                if (status == SurveyApprovalStatus.Rejected)
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
            else if (type == NotificationType.Approve_Survey_Creation_3)
            {
                if (status == SurveyApprovalStatus.Rejected)
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
                            if (myfunction == UserAccess.RnPSurveyEdit)
                            {
                                if (toadmin)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPSurveyVerify)
                            {
                                if (toverifier)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPSurveyApprove1)
                            {
                                if (toapprover1)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPSurveyApprove2)
                            {
                                if (toapprover2)
                                {
                                    receivers.Add(myuser.Id);
                                }
                            }
                            if (myfunction == UserAccess.RnPSurveyApprove3)
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
        // GET: api/RnP/Survey/SaveNotificationID/
        [Route("api/RnP/Survey/SaveNotificationID")]
        public bool SaveNotificationID(int id, int notificationid)
        {
            var survey = db.Survey.Where(p => p.ID == id).FirstOrDefault();

            if (survey != null)
            {
                survey.NotificationID = notificationid;

                db.Entry(survey).State = EntityState.Modified;
                //db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        // Private functions

        // Targeted Groups lookup

        // Check if logged in user's email is in target group
        // TODO: use group id
        [NonAction]
        private bool UserInGroup(string email, string groupname)
        {
            var group = db.TargetedGroups.Where(g => g.Active == true && g.Name == groupname).FirstOrDefault();

            if (group != null)
            {
                var user = db.TargetedGroupMembers.Where(gm => gm.TargetedGroupID == group.ID && gm.Email == email).FirstOrDefault();

                if (user != null)
                {
                    return true;
                }
            }

            return false;
        }

        // Get emails of all group members
        // TODO: use group id
        [NonAction]
        private List<string> GetGroupMemberEmails(string groupname)
        {
            List<string> memberemails = new List<string> { };

            var group = db.TargetedGroups.Where(g => g.Active == true && g.Name == groupname).FirstOrDefault();

            if (group != null)
            {
                var members = db.TargetedGroupMembers.Where(gm => gm.TargetedGroupID == group.ID).ToList();

                foreach (TargetedGroupMembers mymember in members)
                {
                    memberemails.Add(mymember.Email);
                }
            }

            return memberemails;
        }

        // BULK EMAIL

        [NonAction]
        public bool SendEmailNotificationSurveyBroadcast(Survey survey, string BaseURL)
        {
            ParameterListToSend paramToSend = new ParameterListToSend();
            paramToSend.SurveyTitle = survey.Title;
            paramToSend.SurveyType = "Research";
            paramToSend.SurveyCode = survey.RefNo;
            paramToSend.SurveyApproval = "";
            paramToSend.SurveyLink = $"<a href = '" + BaseURL + "/RnP/Home/TakeSurvey?refno=" + survey.RefNo + "&email={email}' > Take the Survey </a>";
            paramToSend.SurveyRespondentEmail = "";

            var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType.Submit_Survey_Distribution).FirstOrDefault();
            string Subject = generateBodyMessage("Survey Broadcast", NotificationType.Submit_Survey_Distribution, paramToSend);
            string Body = generateBodyMessage(template.TemplateMessage, NotificationType.Submit_Survey_Distribution, paramToSend);

            List<string> Email = new List<string> { };
            List<string> tmail = new List<string> { };

            var groups = survey.TargetGroup.Split(',');
            foreach (string group in groups)
            {
                tmail = GetGroupMemberEmails(group);
                Email = Email.Concat(tmail).ToList();
            }

            if (Email.Count > 0)
            {
                List<string> uniqueemails = Email.Distinct().ToList();
                Email = uniqueemails;
            }

            var sendresult = SendBulkEmail(NotificationType.Submit_Survey_Distribution, NotificationCategory.ResearchAndPublication, Email, paramToSend, Subject, Body, true);
            return true;
        }

        [NonAction]
        public bool SendEmailNotificationSurveySubmission(SurveyResponse model, Survey survey)
        {
            ParameterListToSend paramToSend = new ParameterListToSend();
            paramToSend.SurveyTitle = survey.Title;
            paramToSend.SurveyType = "";
            paramToSend.SurveyCode = survey.RefNo;
            paramToSend.SurveyApproval = "";
            paramToSend.SurveyLink = "";
            paramToSend.SurveyRespondentEmail = model.Email;

            var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType.Submit_Survey_Response).FirstOrDefault();
            string Subject = generateBodyMessage("Survey Response Submission", NotificationType.Submit_Survey_Response, paramToSend);
            string Body = generateBodyMessage(template.TemplateMessage, NotificationType.Submit_Survey_Response, paramToSend);

            List<string> Email = new List<string> { };

            Email = GetEmailsByAccess(UserAccess.RnPSurveyEdit);

            var sendresult = SendBulkEmail(NotificationType.Submit_Survey_Response, NotificationCategory.ResearchAndPublication, Email, paramToSend, Subject, Body);
            return true;
        }

        [NonAction]
        public List<string> GetEmailsByAccess(UserAccess UAccess)
        {
            List<string> emails = new List<string> { };

            //var allusers = db.User.Where(u => u.Display).ToList();
            var allusers = db.User.Where(u => u.Display && (u.UserType == UserType.Staff || u.UserType == UserType.SystemAdmin)).ToList();

            foreach (FEP.Model.User myuser in allusers)
            {
                if (myuser.UserAccount.IsEnable)
                {
                    var myroles = myuser.UserAccount.UserRoles;
                    foreach (UserRole myrole in myroles)
                    {
                        var myroleid = myrole.RoleId;
                        var myaccesses = db.RoleAccess.Where(ra => ra.RoleId == myroleid).ToList();
                        foreach (RoleAccess myaccess in myaccesses)
                        {
                            UserAccess myfunction = myaccess.UserAccess;
                            if (myfunction == UAccess)
                            {
                                emails.Add(myuser.Email);
                            }
                        }
                    }
                }
            }

            return emails;
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
        public async System.Threading.Tasks.Task<IHttpActionResult> SendBulkEmail(NotificationType NotificationType, NotificationCategory NotificationCategory, List<string> Emails, ParameterListToSend ParameterListToSend, string emailSubject, string emailBody, bool customlink = false)
        {
            bool success = true;
            foreach (string receiverEmailAddress in Emails)
            {
                int counter = 1;
                if (customlink)
                {
                    var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType).FirstOrDefault();
                    ParameterListToSend.SurveyLink = ParameterListToSend.SurveyLink.Replace("{email}", receiverEmailAddress);
                    emailBody = generateBodyMessage(template.TemplateMessage, NotificationType, ParameterListToSend);
                }
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