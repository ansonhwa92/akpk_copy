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
        [Route("api/eLearning/ContentCompletion/Complete")]
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

            var entity = await db.ContentCompletions
                            .Include(x => x.Question.FreeTextAnswers)
                            .Include(x => x.Question.MultipleChoiceAnswers)
                            .FirstOrDefaultAsync(x => x.Id == contentId);


            if (entity.CompletionType == ContentCompletionType.Timer)
            {
                return new ContentCompletionModel
                {

                    CompletionType = ContentCompletionType.Timer,
                    Timer = entity.Timer,
                    QuestionId = null,
                    Question = null
                };
            }
            else if (entity.CompletionType == ContentCompletionType.ClickButton)
            {
                return new ContentCompletionModel
                {
                    CompletionType = ContentCompletionType.ClickButton,
                    QuestionId = null,
                    Question = null,
                    Timer = 0
                };
            }
            else if (entity.CompletionType == ContentCompletionType.AnswerQuestion)
            {
                return new ContentCompletionModel
                {
                    CompletionType = ContentCompletionType.AnswerQuestion,
                    Timer = 0,
                    QuestionId = entity.Question.Id
                };
            }

            return null;
        }

    }
}