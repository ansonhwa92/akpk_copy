using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.RnP;
using FEP.WebApiModel.Setting;
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
using Newtonsoft.Json;
using System.IO;
using System.Data;
using CsvHelper;


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

            if(request.Status != null || request.RequireAction == true)
            {
                query = GetSurveyByFilter(request, query);
            }

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

        // DataTable function for listing and filtering published surveys (and results)
        // POST: api/RnP/Survey/GetPublished (DataTable)
        [Route("api/RnP/Survey/GetPublished")]
        [HttpPost]
        public IHttpActionResult GetPublished(FilterSurveyModel request)
        {

            var query = db.Survey.Where(p => p.Status == SurveyStatus.Published);

            var totalCount = query.Count();

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

            foreach (var survey in data)
            {
                var surveyimages = db.SurveyImages.Where(i => i.SurveyID == survey.ID).Select(s => new SurveyImagesModel
                {
                    ID = s.ID,
                    SurveyID = s.SurveyID,
                    CoverPicture = s.CoverPicture,
                    AuthorPicture = s.AuthorPicture
                }).FirstOrDefault();

                if (surveyimages != null)
                {
                    if ((surveyimages.CoverPicture != null) && (surveyimages.CoverPicture != ""))
                    {
                        survey.CoverPicture = surveyimages.CoverPicture.Substring(surveyimages.CoverPicture.LastIndexOf('\\') + 1);
                    }
                    if ((surveyimages.AuthorPicture != null) && (surveyimages.AuthorPicture != ""))
                    {
                        survey.AuthorPicture = surveyimages.AuthorPicture.Substring(surveyimages.AuthorPicture.LastIndexOf('\\') + 1);
                    }
                }
            }

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

            foreach (var survey in data)
            {
                var surveyimages = db.SurveyImages.Where(i => i.SurveyID == survey.ID).Select(s => new SurveyImagesModel
                {
                    ID = s.ID,
                    SurveyID = s.SurveyID,
                    CoverPicture = s.CoverPicture,
                    AuthorPicture = s.AuthorPicture
                }).FirstOrDefault();

                if (surveyimages != null)
                {
                    if ((surveyimages.CoverPicture != null) && (surveyimages.CoverPicture != ""))
                    {
                        survey.CoverPicture = surveyimages.CoverPicture.Substring(surveyimages.CoverPicture.LastIndexOf('\\') + 1);
                    }
                    if ((surveyimages.AuthorPicture != null) && (surveyimages.AuthorPicture != ""))
                    {
                        survey.AuthorPicture = surveyimages.AuthorPicture.Substring(surveyimages.AuthorPicture.LastIndexOf('\\') + 1);
                    }
                }
            }

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

            var surveyimages = db.SurveyImages.Where(i => i.SurveyID == id).Select(s => new SurveyImagesModel
            {
                ID = s.ID,
                SurveyID = s.SurveyID,
                CoverPicture = s.CoverPicture,
                AuthorPicture = s.AuthorPicture
            }).FirstOrDefault();

            if (surveyimages != null)
            {
                if ((surveyimages.CoverPicture != null) && (surveyimages.CoverPicture != ""))
                {
                    survey.CoverPicture = surveyimages.CoverPicture.Substring(surveyimages.CoverPicture.LastIndexOf('\\') + 1);
                }
                if ((surveyimages.AuthorPicture != null) && (surveyimages.AuthorPicture != ""))
                {
                    survey.AuthorPicture = surveyimages.AuthorPicture.Substring(surveyimages.AuthorPicture.LastIndexOf('\\') + 1);
                }
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

        // Function to check if survey title exists
        [Route("api/RnP/Survey/TitleExists")]
        [HttpGet]
        public IHttpActionResult TitleExists(int? id, string title)
        {

            if (id == null)
            {
                if (db.Survey.Any(p => p.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && p.Status != SurveyStatus.Trashed))
                    return Ok(true);
            }
            else
            {
                if (db.Survey.Any(p => p.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && p.ID != id && p.Status != SurveyStatus.Trashed))
                    return Ok(true);
            }

            return NotFound();
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
                    Answers = model.Answers,
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
                    Answers = model.Answers,
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

        /*
         * The following API calls are for Image uploads
         * 1. 
         * 2. 
         */

        // GET: api/RnP/Survey/UploadImages
        [Route("api/RnP/Survey/UploadImages")]
        [HttpGet]
        public int UploadImages(int surveyid, string coverpic, string authorpic)
        {
            var surveyimages = new SurveyImages
            {
                SurveyID = surveyid,
                CoverPicture = coverpic,
                AuthorPicture = authorpic
            };

            db.SurveyImages.Add(surveyimages);
            db.SaveChanges();

            return surveyimages.ID;
        }

        // GET: api/RnP/Survey/UpdateImagesCover
        [Route("api/RnP/Survey/UpdateImagesCover")]
        [HttpGet]
        public int UpdateImagesCover(int surveyid, string coverpic)
        {
            var surveyimages = db.SurveyImages.Where(pi => pi.SurveyID == surveyid).FirstOrDefault();

            if (surveyimages != null)
            {
                surveyimages.CoverPicture = coverpic;
                db.Entry(surveyimages).State = EntityState.Modified;
                db.SaveChanges();

                return surveyimages.ID;
            }

            return 0;
        }

        // GET: api/RnP/Survey/UpdateImagesAuthor
        [Route("api/RnP/Survey/UpdateImagesAuthor")]
        [HttpGet]
        public int UpdateImagesAuthor(int surveyid, string authorpic)
        {
            var surveyimages = db.SurveyImages.Where(pi => pi.SurveyID == surveyid).FirstOrDefault();

            if (surveyimages != null)
            {
                surveyimages.AuthorPicture = authorpic;
                db.Entry(surveyimages).State = EntityState.Modified;
                db.SaveChanges();

                return surveyimages.ID;
            }

            return 0;
        }

        // GET: api/RnP/Survey/UpdateImageSurveyID
        [Route("api/RnP/Survey/UpdateImageSurveyID")]
        [HttpGet]
        public int UpdateImageSurveyID(int id, int surveyid)
        {
            var surveyimages = db.SurveyImages.Where(pi => pi.ID == id).FirstOrDefault();

            if (surveyimages != null)
            {
                surveyimages.SurveyID = surveyid;
                db.Entry(surveyimages).State = EntityState.Modified;
                db.SaveChanges();

                return surveyimages.ID;
            }

            return 0;
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
                toadmin = true;
                toverifier = true;
            }
            else if (type == NotificationType.Submit_Survey_Cancellation)
            {
                toadmin = true;
                toverifier = true;
                toapprover1 = true;
                toapprover2 = true;
                toapprover3 = true;
            }
            else if (type == NotificationType.Submit_Survey_Publication)
            {
                toadmin = true;
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
                    toverifier = true;
                }
                else
                {
                    toverifier = true;
                    toapprover1 = true;
                }
            }
            else if (type == NotificationType.Approve_Survey_Creation_1)
            {
                if (status == SurveyApprovalStatus.Rejected)
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
            else if (type == NotificationType.Approve_Survey_Creation_2)
            {
                if (status == SurveyApprovalStatus.Rejected)
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
            else if (type == NotificationType.Approve_Survey_Creation_3)
            {
                if (status == SurveyApprovalStatus.Rejected)
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

        // Survey analysis functions

        // Function to compile survey answers for display only.
        // GET: api/RnP/Survey/CompileAnswers/5
        [Route("api/RnP/Survey/CompileAnswers")]
        [HttpGet]
        public SurveyResultsModel CompileAnswers(int id)
        {
            SurveyResultsModel results = CompileResults(id);
            if (results != null)
            {
                results.CSVOutput = CompileToCsv(results);
                //results.XLSOutput = CompileToXls(results);
                //results.PDFOutput = CompileToPdf(results);
            }

            return results;
        }

        // csv
        // Function to export to csv
        [Route("api/RnP/Survey/ExportToCsv")]
        [HttpGet]
        //public HttpResponseMessage ExportToCsv(int id)
        public string ExportToCsv(int id)
        {
            SurveyResultsModel results = CompileResults(id);
            if (results != null)
            {
                string csvresults = CompileToCsv(results);
                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                //result.Content = new StringContent(csvresults);
                //result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                //result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "export.csv" };
                //return result;
                return csvresults;
            }
            else
            {
                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NotFound);
                //return result;
                return "";
            }
        }

        // xls
        // Function to export to xls
        [Route("api/RnP/Survey/ExportToXls")]
        [HttpGet]
        public HttpResponseMessage ExportToXls(int id)
        {
            SurveyResultsModel results = CompileResults(id);
            if (results != null)
            {
                string csvresults = CompileToCsv(results);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(csvresults);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "export.csv" };
                return result;
            }
            else
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NotFound);
                return result;
            }
        }

        // pdf
        // Function to export to pdf
        [Route("api/RnP/Survey/ExportToPdf")]
        [HttpGet]
        public HttpResponseMessage ExportToPdf(int id)
        {
            SurveyResultsModel results = CompileResults(id);
            if (results != null)
            {
                string csvresults = CompileToCsv(results);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(csvresults);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "export.csv" };
                return result;
            }
            else
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NotFound);
                return result;
            }
        }

        // Private functions

        // export conversion functions

        // compile to csv
        [NonAction]
        private string CompileToCsv(SurveyResultsModel results)
        {
            if (results != null)
            {
                StringWriter csvString = new StringWriter();
                using (var csv = new CsvWriter(csvString))
                {
                    //csv.Configuration.SkipEmptyRecords = true;
                    //csv.Configuration.WillThrowOnMissingField = false;
                    csv.Configuration.Delimiter = ",";

                    foreach (var q in results.Questions)
                    {
                        csv.WriteField(q);
                    }
                    csv.NextRecord();

                    foreach (var a in results.Answers)
                    {
                        foreach (var afield in a)
                        {
                            csv.WriteField(afield);
                        }
                        csv.NextRecord();
                    }

                    return csvString.ToString();
                }
            }
            return "";
        }

        // main massaging function
        [NonAction]
        private SurveyResultsModel CompileResults(int id)
        {
            var results = db.Survey.Where(p => p.ID == id).Select(s => new SurveyResultsModel
            {
                ID = s.ID,
                Type = s.Type,
                Category = s.Category,
                RefNo = s.RefNo,
                Title = s.Title,
                Description = s.Description,
                TargetGroup = s.TargetGroup,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                RequireLogin = s.RequireLogin,
                Contents = s.Contents,
                Active = s.Active,
                CreatorId = s.CreatorId,
                CreatorName = "",
                DateAdded = s.DateAdded,
                Status = s.Status,
                InviteCount = s.InviteCount,
                SubmitCount = s.SubmitCount,
                RawQuestions = "",
                ParticipantCount = 0,
                RawResults = "",
                RawAnswers = "",
                CSVOutput = "",
                XLSOutput = "",
                PDFOutput = ""
            }).FirstOrDefault();

            if (results != null)
            {
                // break up questions
                var strippedq = results.Contents.Substring(13, results.Contents.Length - 16);
                var pages = JsonConvert.DeserializeObject<List<SurveyPagesModel>>(strippedq);

                foreach (var mypage in pages)
                {
                    foreach (var myelement in mypage.elements)
                    {
                        results.RawQuestions = results.RawQuestions + myelement.title + "\r\n";
                        results.Questions.Add(myelement.title);
                        if ((myelement.type == "dropdown") || (myelement.type == "radiogroup"))
                        {
                            var newsc = new SurveySingleChoiceAnswersModel();
                            List<SurveyElementsChoicesModel> choices = new List<SurveyElementsChoicesModel>();
                            choices = JsonConvert.DeserializeObject<List<SurveyElementsChoicesModel>>(Convert.ToString(myelement.choices));
                            foreach (SurveyElementsChoicesModel choice in choices)
                            {
                                newsc.question = myelement.title;
                                newsc.questionname = myelement.name;
                                newsc.questiontype = myelement.type;
                                newsc.values.Add(choice.value);
                                newsc.answers.Add(choice.text);
                                newsc.counts.Add(0);
                            }
                            results.SingleChoices.Add(newsc);
                        }
                        else if (myelement.type == "rating")
                        {
                            var newsc = new SurveySingleChoiceAnswersModel();
                            for (var rate = 1; rate <= 20; rate++)
                            {
                                newsc.question = myelement.title;
                                newsc.questionname = myelement.name;
                                newsc.questiontype = myelement.type;
                                newsc.values.Add(rate.ToString());
                                newsc.answers.Add(rate.ToString());
                                newsc.counts.Add(0);
                            }
                            results.SingleChoices.Add(newsc);
                        }
                        else if (myelement.type == "boolean")
                        {
                            var newsc = new SurveySingleChoiceAnswersModel();
                            newsc.question = myelement.title;
                            newsc.questionname = myelement.name;
                            newsc.questiontype = myelement.type;
                            newsc.values.Add("True");
                            newsc.values.Add("False");
                            newsc.answers.Add("True");
                            newsc.answers.Add("False");
                            newsc.counts.Add(0);
                            newsc.counts.Add(0);
                            results.SingleChoices.Add(newsc);
                        }
                        else if (myelement.type == "imagepicker")
                        {
                            var newsc = new SurveySingleChoiceAnswersModel();
                            List<SurveyElementsImageChoicesModel> choices = new List<SurveyElementsImageChoicesModel>();
                            choices = JsonConvert.DeserializeObject<List<SurveyElementsImageChoicesModel>>(Convert.ToString(myelement.choices));
                            foreach (SurveyElementsImageChoicesModel choice in choices)
                            {
                                newsc.question = myelement.title;
                                newsc.questionname = myelement.name;
                                newsc.questiontype = myelement.type;
                                newsc.values.Add(choice.value);
                                newsc.answers.Add(choice.value);
                                newsc.counts.Add(0);
                            }
                            results.SingleChoices.Add(newsc);
                        }
                        else if (myelement.type == "checkbox")
                        {
                            var newsc = new SurveyMultipleChoiceAnswersModel();
                            List<SurveyElementsChoicesModel> choices = new List<SurveyElementsChoicesModel>();
                            choices = JsonConvert.DeserializeObject<List<SurveyElementsChoicesModel>>(Convert.ToString(myelement.choices));
                            foreach (SurveyElementsChoicesModel choice in choices)
                            {
                                newsc.question = myelement.title;
                                newsc.questionname = myelement.name;
                                newsc.questiontype = myelement.type;
                                newsc.values.Add(choice.value);
                                newsc.answers.Add(choice.text);
                                newsc.counts.Add(0);
                            }
                            results.MultipleChoices.Add(newsc);
                        }
                    }
                }

                var responses = db.SurveyResponse.Where(p => p.SurveyID == id).Select(s => new SurveyResponseModel
                {
                    ID = s.ID,
                    SurveyID = s.SurveyID,
                    Type = s.Type,
                    UserId = s.UserId,
                    Email = s.Email,
                    Contents = s.Contents,
                    Answers = s.Answers
                }).ToList();

                results.ParticipantCount = responses.Count;

                foreach (SurveyResponseModel response in responses)
                {
                    results.Answers.Add(new List<string>());
                    var lastindex = results.Answers.Count() - 1;
                    results.RawResults = results.RawResults + response.Contents + "\r\n";
                    var inputs = JsonConvert.DeserializeObject<List<SurveyAnswersModel>>(response.Answers);

                    string lastq = "";
                    int lastqindex = 0;
                    string lastrow = "";

                    foreach (var myinput in inputs)
                    {
                        string realq = "";
                        int dotpos = myinput.question.IndexOf('.');
                        if (dotpos == -1)
                        {
                            realq = myinput.question;
                        }
                        else
                        {
                            realq = myinput.question.Substring(0, dotpos);
                        }

                        if (realq != lastq)
                        {
                            results.RawOutput = results.RawOutput + "#" + myinput.answer;

                            // raw
                            if (myinput.questionType == "checkbox")
                            {
                                List<string> items = new List<string>();
                                items = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(myinput.answer));
                                string itemstring = "";
                                foreach (string item in items)
                                {
                                    itemstring = itemstring + "|" + item;
                                }
                                itemstring = itemstring.Substring(1);
                                results.Answers[lastindex].Add(itemstring);
                            }
                            else if (myinput.questionType == "dropdown")
                            {
                                var rowpos = 0;
                                var colpos = 0;
                                rowpos = myinput.question.IndexOf("row_");
                                colpos = myinput.question.IndexOf("col_");
                                if ((rowpos != -1) && (colpos != -1))
                                {
                                    lastrow = "1";
                                    results.Answers[lastindex].Add("Row 1: " + Convert.ToString(myinput.answer));
                                }
                                else
                                {
                                    results.Answers[lastindex].Add(Convert.ToString(myinput.answer));
                                }
                            }
                            else
                            {
                                results.Answers[lastindex].Add(Convert.ToString(myinput.answer));
                            }
                            lastqindex = results.Answers[lastindex].Count() - 1;

                            // stats
                            foreach (SurveySingleChoiceAnswersModel sq in results.SingleChoices)
                            {
                                if (sq.questionname == realq)
                                {
                                    int ai = -1;
                                    foreach (var ans in sq.values)
                                    {
                                        ai++;
                                        if (ans == myinput.answer.ToString())
                                        {
                                            sq.counts[ai] = sq.counts[ai] + 1;
                                        }
                                        if (sq.questiontype == "rating")
                                        {
                                            if (myinput.answer > sq.maxrating)
                                            {
                                                sq.maxrating = int.Parse(myinput.answer.ToString());
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (SurveyMultipleChoiceAnswersModel sq in results.MultipleChoices)
                            {
                                if (sq.questionname == realq)
                                {
                                    int ai = -1;
                                    foreach (var ans in sq.values)
                                    {
                                        ai++;
                                        if (myinput.answer.ToString().Contains(ans))
                                        {
                                            sq.counts[ai] = sq.counts[ai] + 1;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // raw
                            if (myinput.questionType == "dropdown")
                            {
                                var rowpos = 0;
                                var colpos = 0;
                                rowpos = myinput.question.IndexOf("row_");
                                colpos = myinput.question.IndexOf("col_");
                                if ((rowpos != -1) && (colpos != -1))
                                {
                                    var rownumpos = rowpos + 4;
                                    var nextdotpos = myinput.question.IndexOf('.', rowpos);
                                    var rownumlen = nextdotpos - rownumpos;
                                    var rownum = myinput.question.Substring(rownumpos, rownumlen);
                                    if (rownum == lastrow)
                                    {
                                        results.RawOutput = results.RawOutput + "|" + myinput.answer;
                                        results.Answers[lastindex][lastqindex] = results.Answers[lastindex][lastqindex] + "|" + Convert.ToString(myinput.answer);
                                    }
                                    else
                                    {
                                        results.RawOutput = results.RawOutput + "|" + myinput.answer;
                                        results.Answers[lastindex][lastqindex] = results.Answers[lastindex][lastqindex] + ";" + "Row " + rownum + ": " + Convert.ToString(myinput.answer);
                                        lastrow = rownum;
                                    }
                                }
                                else
                                {
                                    results.RawOutput = results.RawOutput + "|" + myinput.answer;
                                    results.Answers[lastindex][lastqindex] = results.Answers[lastindex][lastqindex] + "|" + Convert.ToString(myinput.answer);
                                }
                            }
                            else
                            {
                                results.RawOutput = results.RawOutput + "|" + myinput.answer;
                                results.Answers[lastindex][lastqindex] = results.Answers[lastindex][lastqindex] + "|" + Convert.ToString(myinput.answer);
                            }

                            // stats
                            // not yet, applicable to single matrix only
                        }
                        lastq = realq;
                    }
                    results.RawOutput = results.RawOutput + "\r\n";
                }

                // mean/median/mode
                foreach (SurveySingleChoiceAnswersModel sq in results.SingleChoices)
                {
                    int asum = 0;
                    int acount = 0;
                    int medcount = 0;
                    int medpos1 = 0;
                    float medpos2 = 0;
                    int medlow = 0;
                    int medhigh = 0;
                    List<int> medseries = new List<int>();
                    int maxcount = 0;
                    int maxindex = 0;

                    if (results.Answers.Count > 0)
                    {
                        int ai = 0;
                        int aindex = -1;
                        foreach (var ans in sq.counts)
                        {
                            ai++;
                            aindex++;
                            acount = acount + ans;
                            asum = asum + (ai * ans);
                            for (var i = 1; i <= ans; i++)
                            {
                                medseries.Add(ai);
                            }
                            if (ans > maxcount)
                            {
                                maxcount = ans;
                                maxindex = aindex;
                            }
                        }

                        sq.mean = (int)Math.Round(decimal.Parse((asum / acount).ToString()));

                        medseries.Sort();
                        medcount = medseries.Count;
                        if (medcount % 2 == 0)
                        {
                            medpos2 = (medcount + 1) / 2;
                            medlow = (int)Math.Floor(decimal.Parse(medpos2.ToString()));
                            medhigh = (int)Math.Ceiling(decimal.Parse(medpos2.ToString()));
                            sq.median = (medseries[medlow - 1] + medseries[medhigh - 1]) / 2;
                        }
                        else
                        {
                            medpos1 = (medcount + 1) / 2;
                            sq.median = medseries[medpos1 - 1];
                        }

                        sq.mode = maxindex;
                        System.Diagnostics.Debug.WriteLine(sq.question);
                        System.Diagnostics.Debug.WriteLine(maxindex);
                        System.Diagnostics.Debug.WriteLine(sq.answers[maxindex]);
                    }
                    else
                    {
                        sq.mean = 0;
                        sq.median = 0;
                        sq.mode = 0;
                    }

                    //if (sq.mean <= 0) sq.mean = 1;
                    //if (sq.median <= 0) sq.median = 1;
                    //if (sq.mode <= 0) sq.mode = 1;
                }

                foreach (SurveyMultipleChoiceAnswersModel sq in results.MultipleChoices)
                {
                    int asum = 0;
                    int acount = 0;
                    int medcount = 0;
                    int medpos1 = 0;
                    float medpos2 = 0;
                    int medlow = 0;
                    int medhigh = 0;
                    List<int> medseries = new List<int>();
                    int maxcount = 0;
                    int maxindex = 0;

                    if (results.Answers.Count > 0)
                    {
                        int ai = 0;
                        int aindex = -1;
                        foreach (var ans in sq.counts)
                        {
                            ai++;
                            aindex++;
                            acount = acount + ans;
                            asum = asum + (ai * ans);
                            for (var i = 1; i <= ans; i++)
                            {
                                medseries.Add(ai);
                            }
                            if (ans > maxcount)
                            {
                                maxcount = ans;
                                maxindex = aindex;
                            }
                        }

                        sq.mean = (int)Math.Round(decimal.Parse((asum / acount).ToString()));

                        medseries.Sort();
                        medcount = medseries.Count;
                        if (medcount % 2 == 0)
                        {
                            medpos2 = (medcount + 1) / 2;
                            medlow = (int)Math.Floor(decimal.Parse(medpos2.ToString()));
                            medhigh = (int)Math.Ceiling(decimal.Parse(medpos2.ToString()));
                            sq.median = (medseries[medlow - 1] + medseries[medhigh - 1]) / 2;
                        }
                        else
                        {
                            medpos1 = (medcount + 1) / 2;
                            sq.median = medseries[medpos1 - 1];
                        }

                        sq.mode = maxindex;
                        System.Diagnostics.Debug.WriteLine(sq.question);
                        System.Diagnostics.Debug.WriteLine(maxindex);
                        System.Diagnostics.Debug.WriteLine(sq.answers[maxindex]);
                    }
                    else
                    {
                        sq.mean = 0;
                        sq.median = 0;
                        sq.mode = 0;
                    }

                    //if (sq.mean <= 0) sq.mean = 1;
                    //if (sq.median <= 0) sq.median = 1;
                    //if (sq.mode <= 0) sq.mode = 1;
                }

                return results;
            }

            return null;
        }

        // eLearning

        // Function to check if a certain quiz has been done by user
        [Route("api/RnP/Survey/ContentQuizCompleted")]
        [HttpGet]
        public bool ContentQuizCompleted(int quizid, int userid)
        {
            if (db.CourseContentAnswers.Any(cc => cc.QuizId == quizid && cc.UserId == userid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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


        private IQueryable<Survey> GetSurveyByFilter(FilterSurveyModel request, IQueryable<Survey> surveys)
        {
            if (request.Status != null)
            {
                if (request.Status == SurveyStatus.Verified)
                {
                    surveys = surveys.Where(q => db.SurveyApproval.Where(pa => pa.SurveyID == q.ID && pa.Status == SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level == request.ApprovalLevel);
                }
                else if (request.Status == SurveyStatus.VerifierRejected)
                {
                    surveys = surveys.Where(q => q.Status == SurveyStatus.VerifierRejected
                                                || q.Status == SurveyStatus.ApproverRejected);
                }
                else
                {
                    surveys = surveys.Where(q => q.Status == request.Status);
                }
            }

            if (request.RequireAction == true)
            {
                if (request.UserAccess == UserAccess.RnPSurveyEdit)
                {
                    surveys = surveys.Where(q => q.Status == SurveyStatus.New
                                                        || q.Status == SurveyStatus.VerifierRejected
                                                        || q.Status == SurveyStatus.ApproverRejected);
                }
                else if (request.UserAccess == UserAccess.RnPSurveyVerify)
                {
                    surveys = surveys.Where(q => db.SurveyApproval.Where(pa => pa.SurveyID == q.ID && pa.Status == SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level == SurveyApprovalLevels.Verifier);
                }
                else if (request.UserAccess == UserAccess.RnPSurveyApprove1)
                {
                    surveys = surveys.Where(q => db.SurveyApproval.Where(pa => pa.SurveyID == q.ID && pa.Status == SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level == SurveyApprovalLevels.Approver1);
                }
                else if (request.UserAccess == UserAccess.RnPSurveyApprove2)
                {
                    surveys = surveys.Where(q => db.SurveyApproval.Where(pa => pa.SurveyID == q.ID && pa.Status == SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level == SurveyApprovalLevels.Approver2);
                }
                else if (request.UserAccess == UserAccess.RnPSurveyApprove3)
                {
                    surveys = surveys.Where(q => db.SurveyApproval.Where(pa => pa.SurveyID == q.ID && pa.Status == SurveyApprovalStatus.None).OrderByDescending(pa => pa.ApprovalDate).FirstOrDefault().Level == SurveyApprovalLevels.Approver3);
                }
            }

            return surveys;
        }
    }
}