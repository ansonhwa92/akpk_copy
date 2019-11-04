using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/ContentCompletions")]
    public class ContentCompletionApiController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Mark complete this content, should put the mark in progress
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/ContentCompletions/")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Post([FromBody] ContentCompletionModel request)
        {
            if (ModelState.IsValid)
            {
                var currentContent = db.CourseContents.Find(request.ContentId);

                var nextContent = await db.CourseContents.Where(x => x.Order > currentContent.Order &&
                                x.CourseModuleId == currentContent.CourseModuleId)
                                .OrderBy(x => x.Order).FirstOrDefaultAsync();

                if (nextContent != null)
                {
                    request.nextContentId = nextContent.Id;
                    request.nextModuleId = currentContent.CourseModuleId;
                }
                else
                {
                    // get next module
                    var nextModule = await db.CourseModules.Where(x => x.CourseId == request.CourseId &&
                        x.Order > currentContent.CourseModuleId).OrderBy(x => x.Order).FirstOrDefaultAsync();

                    if (nextModule == null)
                    {
                        request.nextContentId = null;
                        request.nextModuleId = null;
                    }
                    else
                    {
                        // Wrong
                        request.nextModuleId = nextModule.Id;
                        request.nextContentId = null;
                    }
                }

                var course = await db.Courses.FindAsync(currentContent.CourseId);

                if (course != null & course.Status == CourseStatus.Published)
                {
                    // get enrollment info
                    var learner = await db.Learners.FirstOrDefaultAsync(x => x.UserId == request.UserId);

                    if (learner == null)
                    {
                        db.ErrorLog.Add(new ErrorLog
                        {
                            ErrorDescription = "Learner not found to record progress.",
                            ErrorDetails = $"Could not find learner with userid ={request.UserId} to record progress for course = { request.CourseId} ",
                            Module = Modules.Learning,
                        });

                        db.SaveChanges();

                        return BadRequest();
                    }

                    var enrollment = await db.Enrollments.FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.LearnerId == learner.Id);

                    if (enrollment == null)
                    {
                        db.ErrorLog.Add(new ErrorLog
                        {
                            ErrorDescription = "Enrollment not found to record progress.",
                            ErrorDetails = $"Could not find enrollment for userid ={request.UserId} to record progress for course = { request.CourseId} ",
                            Module = Modules.Learning,
                        });

                        db.SaveChanges();

                        return BadRequest();
                    }

                    var courseProgress = await db.CourseProgress.FirstOrDefaultAsync(x => x.ModuleId == currentContent.CourseModuleId &&
                     x.ContentId == request.ContentId && x.LearnerId == learner.Id);

                    if (courseProgress == null)
                    {
                        courseProgress = new CourseProgress
                        {
                            EnrollmentId = enrollment.Id,
                            CourseId = course.Id,
                            IsCompleted = true,
                            ContentId = request.ContentId,
                            ModuleId = currentContent.CourseModuleId,

                            LearnerId = learner.Id,
                        };

                        db.CourseProgress.Add(courseProgress);
                    }
                    else
                    {
                        courseProgress.ModuleId = currentContent.CourseModuleId;
                        courseProgress.CourseId = currentContent.CourseId;
                        courseProgress.IsCompleted = true;

                        db.SetModified(courseProgress);
                    }

                    await db.SaveChangesAsync();

                    // calculate progress.
                    var progressCount = db.CourseProgress.Where(x => x.EnrollmentId == enrollment.Id).Count();

                    course.UpdateCourseStat();

                    var totalContent = course.TotalContents;

                    var progressPercent = ((decimal)progressCount / (decimal)totalContent) * 100m;

                    enrollment.TotalContentsCompleted = progressCount;
                    enrollment.PercentageCompleted = progressPercent;

                    if((request.nextContentId == null && request.nextModuleId == null) || enrollment.TotalContentsCompleted == totalContent)
                    {
                        enrollment.Status = EnrollmentStatus.Completed;
                    }

                    db.SetModified(enrollment);

                    await db.SaveChangesAsync();

                    request.CourseId = course.Id;

                    return Ok(request);

                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("api/eLearning/ContentCompletions/")]
        public async Task<ContentCompletionModel> Get(int contentId)
        {
            var entity = await db.CourseContents
                            .Include(x => x.Question.FreeTextAnswers)
                            .Include(x => x.Question.MultipleChoiceAnswers)
                            .FirstOrDefaultAsync(x => x.Id == contentId);

            var model = new ContentCompletionModel
            {
                CourseId = entity.CourseId,
                CourseModuleId = entity.CourseModuleId,
                ContentId = entity.Id,
                Title = entity.Title,
            };

            if (entity.CompletionType == ContentCompletionType.Timer)
            {
                model.CompletionType = ContentCompletionType.Timer;
                model.Timer = entity.Timer;
                model.QuestionId = null;
                model.Question = null;
            }
            else if (entity.CompletionType == ContentCompletionType.ClickButton)
            {
                model.CompletionType = ContentCompletionType.ClickButton;
                model.QuestionId = null;
                model.Question = null;
                model.Timer = 0;
            }
            else if (entity.CompletionType == ContentCompletionType.AnswerQuestion)
            {
                model.CompletionType = ContentCompletionType.AnswerQuestion;
                model.Timer = 0;
                model.QuestionId = entity.Question.Id;
            }
            else
            {
                model.CompletionType = ContentCompletionType.ClickButton;
                model.QuestionId = null;
                model.Question = null;
                model.Timer = 0;
            }

            return model;
        }
    }
}