using AutoMapper;
using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class ContentApiUrl
    {
        public const string GetContent = "eLearning/CourseContents/";
        public const string GetAllQuestions = "eLearning/Question/GetAll";
        public const string Create = "eLearning/CourseContents/Create";
        public const string Complete = "eLearning/CourseContents/Complete";
    }

    public class CourseContentsController : FEPController
    {
        private DbEntities db = new DbEntities();

        private readonly IMapper _mapper;

        public CourseContentsController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrEditContentModel, CourseContent>();
            });

            _mapper = config.CreateMapper();
        }

        // GET: eLearning/CourseContents/Create
        public async Task<ActionResult> Create(int? courseId, int? moduleId, CreateContentFrom createContentFrom,
            CourseContentType courseContentType, string courseTitle)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CreateOrEditContentModel model = new CreateOrEditContentModel
            {
                ContentType = courseContentType,
                PageTitle = courseTitle,
                FileDocument = new FileDocument(),
                CreateContentFrom = createContentFrom,
                CourseId = courseId.Value,
            };

            if (createContentFrom == CreateContentFrom.Module)
            {
                model.CourseModuleId = moduleId.Value;
            }

            await GetAllQuestions(courseId.Value);

            return View(model);
        }

        // POST: eLearning/CourseModules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrEditContentModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, ContentApiUrl.Create, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Content successfully added.";

                    await LogActivity(Modules.Learning, "Create content : " + model.Title);

                    var id = response.Data;

                    if (model.CreateContentFrom == CreateContentFrom.CourseFrontPage)
                        return RedirectToAction("Content", "Courses", new { id = model.CourseId });
                    else
                        return RedirectToAction("Content", "CourseModules", new { id = model.CourseModuleId });
                }

                //// --- FOR TESTING ONLY ----
                ///

                // check if the request come from front page, then create a new module then create the content.
                // if it comes from the module, then create the content there
                // differentiate by CreateContentFrom

                //var content = _mapper.Map<CourseContent>(model);
                //if (model.CreateContentFrom == CreateContentFrom.CourseFrontPage)
                //{
                //    var course = await db.Courses
                //        .Include(x => x.Modules)
                //        .FirstOrDefaultAsync(x => x.Id.Equals(model.CourseId));

                //    if (course == null)
                //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //    var module = new CourseModule
                //    {
                //        CourseId = model.CourseId,
                //        Objectives = "Objective",
                //        Description = "Description",
                //        Title = model.Title,
                //        Order = course.Modules != null ? (course.Modules.Max(x => x.Order) + 1) : 1
                //    };

                //    module.ModuleContents = new List<CourseContent>();
                //    module.ModuleContents.Add(content);

                //    course.Modules.Add(module);

                //    await db.SaveChangesAsync();

                //    return RedirectToAction("Content", "Courses", new { id = model.CourseId });
                //}
                //else
                //{
                //    var module = await db.CourseModules
                //        .Include(x => x.ModuleContents)
                //        .FirstOrDefaultAsync(x => x.Id.Equals(model.CourseModuleId));

                //    if (module == null)
                //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //    module.ModuleContents = new List<CourseContent>();
                //    module.ModuleContents.Add(content);

                //    return RedirectToAction("Content", "ModuleCourses", new { id = model.CourseModuleId });
                //}
            }

            TempData["ErrorMessage"] = "Cannot add content. Please ensure all required fields are filled in.";

            await GetAllQuestions(model.CourseId);

            return View(model);

            //return View(new { courseId = model.CourseId, moduleId = model.CourseModuleId,
            //    createContentFrom = model.CreateContentFrom,
            //    courseContentType = model.ContentType, courseTitle = model.PageTitle });
        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(CreateOrEditContentModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CurrentUser.Name;

                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, ContentApiUrl.Complete, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Content Completed";

                    await LogActivity(Modules.Learning, "User : " + user + " complete this content : " + model.Title);

                    var nextContent = response.Data;

                    if (nextContent < 0) // go to index, no more content this module
                        return RedirectToAction("Content", "CourseModules", new { id = model.CourseId });
                    else
                        return RedirectToAction("Content", "CourseContents", new { id = nextContent });
                }

            }
            TempData["ErrorMessage"] = "Cannot add content. Please ensure all required fields are filled in.";

            return View(model);
        }

        private async Task GetAllQuestions(int id)
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<IEnumerable<QuestionOnlyModel>>(HttpVerbs.Get, ContentApiUrl.GetAllQuestions + $"?id={id}");

            if (response.isSuccess)
                ViewBag.Questions = new SelectList(response.Data, "Id", "Name");
            else
            {
                ViewBag.Questions = new SelectList(new List<QuestionOnlyModel>
                {
                    new QuestionOnlyModel{ Id = 999, Name = "Error"}
                }, "Id", "Name");

                TempData["Error"] = "Cannot find any questions to display.";
            }
        }

        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await TryGetContent(id.Value);

            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(model);
        }

        
        public async Task<ActionResult> ViewVideo(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {
                // If its youtube video ensure the word 'embed' is there, if not, put it in
                // ex https://www.youtube.com/watch?v=WEDIj9JBTC8
                if (content.VideoType == VideoType.ExternalVideo)
                {
                    content.Url = YouTubeUrlHelper.ConvertToEmbeddedUrl(content.Url);
                }
                else
                {
                    // TODO : get the uploaded file info

                }
                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ActionResult> ViewRichText(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {

                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ActionResult> ViewDocument(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {

                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ActionResult> ViewIFrame(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {

                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ActionResult> ViewAudio(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {

                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ActionResult> ViewWebLink(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {                               
                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public async Task<ActionResult> ViewTest(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {

                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public async Task<ActionResult> ViewAssignment(int id)
        {
            //var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            var content = await TryGetContent(id);

            if (content != null)
            {

                return View(content);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [ChildActionOnly]
        public async Task<ActionResult> Video(int id)
        {
            var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;
            //var content = await TryGetContent(id);

            if (content != null)
            {
                // If its youtube video ensure the word 'embed' is there, if not, put it in
                // ex https://www.youtube.com/watch?v=WEDIj9JBTC8
                if (content.VideoType == VideoType.ExternalVideo)
                {
                    content.Url = YouTubeUrlHelper.ConvertToEmbeddedUrl(content.Url);
                }
                else
                {
                    // TODO : get the uploaded file info

                }
                return PartialView("_video", content);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [ChildActionOnly]
        public ActionResult RichText(int id)
        {
            var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;

            if (content != null)
            {

                return PartialView("_richText", content);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [ChildActionOnly]
        public ActionResult Document(int id)
        {
            var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;

            if (content != null)
            {

                return PartialView("_document", content);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [ChildActionOnly]
        public ActionResult IFrame(int id)
        {
            var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;

            if (content != null)
            {

                return PartialView("_iframe", content);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [ChildActionOnly]
        public ActionResult Audio(int id)
        {
            var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;

            if (content != null)
            {

                return PartialView("_audio", content);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        [ChildActionOnly]
        public ActionResult Weblink(int id)
        {
            var content = Task.Run(() => TryGetContent(id).GetAwaiter().GetResult()).Result;

            if (content != null)
            {

                return PartialView("_weblink", content);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<CreateOrEditContentModel> TryGetContent(int id)
        {
            var response = await WepApiMethod.SendApiAsync<CreateOrEditContentModel>(HttpVerbs.Get, ContentApiUrl.GetContent + $"?id={id}");

            if (response.isSuccess)
            {
                return response.Data;
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}