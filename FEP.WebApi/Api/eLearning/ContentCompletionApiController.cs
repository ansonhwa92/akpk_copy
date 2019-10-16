using AutoMapper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
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

                return Ok(nextContent);

                //TODO: MARK THE USER PROGRESS. IF TRIAL, IGNORE

                //return Ok(nextContent.Id.ToString());
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