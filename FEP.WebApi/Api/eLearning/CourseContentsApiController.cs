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
    [Route("api/eLearning/CourseContents")]
    public class CourseContentsApiController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        private readonly IMapper _mapper;

        public CourseContentsApiController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditContentModel, CourseContent>();

                cfg.CreateMap<CourseContent, CreateOrEditContentModel>();
            });

            _mapper = config.CreateMapper();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseContents/Upload")]
        [HttpPost]
        public IHttpActionResult Upload(FilterCourseModel request)
        {
            return Ok();
        }

        /// <summary>
        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseContents/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterCourseModel request)
        {
            return Ok();
        }

        /// <summary>
        /// To create a content for a course. Require courseId
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/CourseContents/Create")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Create([FromBody] CreateOrEditContentModel request)
        {
            if (ModelState.IsValid)
            {
                var content = _mapper.Map<CourseContent>(request);
                content.CompletionType = request.CompletionType;

                if (request.CompletionType == ContentCompletionType.Timer)
                {
                    content.Timer = request.Timer != null ? request.Timer.Value : 0;

                    content.CompletionType = ContentCompletionType.Timer;
                    content.QuestionId = null;
                    content.Question = null;
                }
                else if (request.CompletionType == ContentCompletionType.ClickButton)
                {
                    content.QuestionId = null;
                    content.Question = null;
                    content.Timer = 0;
                }
                else if (request.CompletionType == ContentCompletionType.AnswerQuestion)
                {
                    content.Timer = 0;
                    content.QuestionId = request.ContentQuestionId != null ? request.ContentQuestionId.Value : request.ContentQuestionId;
                }

                // check if the request come from front page, then create a new module then create the content.
                // if it comes from the module, then create the content there
                // differentiate by CreateContentFrom
                if (request.CreateContentFrom == CreateContentFrom.CourseFrontPage)
                {
                    var course = await db.Courses
                        .Include(x => x.Modules)
                        .FirstOrDefaultAsync(x => x.Id.Equals(request.CourseId));

                    if (course == null)
                        return BadRequest();

                    if (course.Modules == null)
                    {
                        course.Modules = new List<CourseModule>();
                    }

                    var module = new CourseModule
                    {
                        CourseId = request.CourseId,
                        Objectives = "Objective",
                        Description = "Description",
                        Title = request.Title,
                        Order = course.Modules.Count() > 0 ? (course.Modules.Max(x => x.Order) + 1) : 1
                    };

                    content.Order = 1;

                    module.ModuleContents = new List<CourseContent>();
                    module.ModuleContents.Add(content);

                    course.Modules.Add(module);

                    await db.SaveChangesAsync();

                    module.UpdateTotals();

                    db.SetModified(module);

                    await db.SaveChangesAsync();

                    return Ok(content.Id);
                }
                else
                {
                    var module = await db.CourseModules
                        .Include(x => x.ModuleContents)
                        .FirstOrDefaultAsync(x => x.Id.Equals(request.CourseModuleId));

                    if (module == null)
                        return BadRequest();

                    if (module.ModuleContents == null)
                    {
                        module.ModuleContents = new List<CourseContent>();
                    }

                    content.Order = module.ModuleContents.Count() > 0 ? module.ModuleContents.Max(x => x.Order) + 1 : 1;

                    module.ModuleContents.Add(content);

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }

                    return Ok(content.Id);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int? id)
        {
            var entity = await db.CourseContents
                .Include(x => x.ContentFile.FileDocument)
                .Include(x => x.Question)
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (entity == null)
                return NotFound();

            var module = await db.CourseModules.FirstOrDefaultAsync(x => x.Id == entity.CourseId);

            if (module == null)
                return NotFound();

            var course = await db.Courses.FindAsync(module.CourseId);

            if (course == null)
                return NotFound();


            var model = _mapper.Map<CreateOrEditContentModel>(entity);

            model.ModuleName = module.Title;
            model.Status = course.Status;

            // for uploaded content
            if (entity.ContentFile != null &&
                entity.ContentFile.FileDocument != null)
            {
                model.FileDocument = entity.ContentFile.FileDocument;
                model.FileDocumentId = entity.ContentFile.FileDocumentId;
                model.UploadFileName = entity.ContentFile.FileName;
            }

            if (entity.CompletionType == ContentCompletionType.AnswerQuestion &&
                entity.Question != null)
            {
                model.ContentQuestionId = entity.Question.Id;
                model.Question = entity.Question.Name;
            }
            if (entity.CompletionType == ContentCompletionType.Timer)
                model.Timer = entity.Timer;

            return Ok(model);
        }

        [Route("api/eLearning/CourseContents/Edit")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Edit([FromBody] CreateOrEditContentModel request)
        {
            if (ModelState.IsValid)
            {
                var content = await db.CourseContents
                    .Include(x => x.ContentFile.FileDocument)
                    .FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (content == null)
                    return BadRequest();

                content.CompletionType = request.CompletionType;

                content.ContentType = request.ContentType;
                content.Description = request.Description;
                content.ShowIFrameAs = request.ShowIFrameAs;
                content.Text = request.Text;

                content.Title = request.Title;
                content.Url = request.Url;
                content.VideoType = request.VideoType;
                content.AudioType = request.AudioType;
                content.DocumentType = request.DocumentType;

                if (request.ContentFileId != null)
                    content.ContentFileId = request.ContentFileId;

                content.CompletionType = request.CompletionType;

                if (request.CompletionType == ContentCompletionType.Timer)
                {
                    content.Timer = request.Timer != null ? request.Timer.Value : 0;

                    content.CompletionType = ContentCompletionType.Timer;
                    content.QuestionId = null;
                    content.Question = null;
                }
                else if (request.CompletionType == ContentCompletionType.ClickButton)
                {
                    content.QuestionId = null;
                    content.Question = null;
                    content.Timer = 0;
                }
                else if (request.CompletionType == ContentCompletionType.AnswerQuestion)
                {
                    content.Timer = 0;
                    content.QuestionId = request.ContentQuestionId != null ? request.ContentQuestionId.Value : request.ContentQuestionId;
                }

                try
                {
                    await db.SaveChangesAsync();

                    return Ok(true);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Route("api/eLearning/CourseContents/Delete")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var content = await db.CourseContents.FindAsync(id.Value);

            if (content != null)
            {
                DeleteContentModel model = new DeleteContentModel
                {
                    Title = content.Title,
                    CourseId = content.CourseId,
                    CourseModuleId = content.CourseModuleId
                };

                db.SetDeleted(content);

                await db.SaveChangesAsync();

                var module = await db.CourseModules.FindAsync(content.CourseModuleId);

                if (module != null)
                {
                    module.UpdateTotals();
                    await db.SaveChangesAsync();

                }

                return Ok(model);
            }

            return BadRequest(ModelState);
        }

        [Route("api/eLearning/CourseContents/GetAllAudio")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAudio(int? courseId)
        {
            if (courseId == null)
                return BadRequest();

            var entities = db.ContentFiles.Where(x => x.CourseId == courseId.Value && x.FileType == FileType.Audio)
                    .Include(x => x.FileDocument)
                    .Select(x => new AudioListModel
                    {
                        CourseId = x.CourseId,
                        Id = x.Id,
                        Name = x.FileDocument.FileName,
                        FileNameOnStorage = x.FileDocument.FileNameOnStorage,
                        FileDocumentId = x.FileDocument.Id,
                        ContentId = x.ContentId.Value
                    }
                    );

            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }


        [Route("api/eLearning/CourseContents/GetAllDocument")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllDocument(int? courseId)
        {
            if (courseId == null)
                return BadRequest();

            var entities = db.ContentFiles.Where(x => x.CourseId == courseId.Value && x.FileType == FileType.Document)
                    .Include(x => x.FileDocument)
                    .Select(x => new DocumentListModel
                    {
                        CourseId = x.CourseId,
                        Id = x.Id,
                        Name = x.FileDocument.FileName,
                        FileNameOnStorage = x.FileDocument.FileNameOnStorage,
                        FileDocumentId = x.FileDocument.Id,
                        ContentId = x.ContentId.Value
                    }
                    );

            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }
    }
}