using AutoMapper;
using FEP.Helper;
using FEP.Intranet.Areas.eLearning.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public static class ContentApiUrl
    {
        public const string GetContent = "eLearning/CourseContents/";
        public const string GetAllQuestions = "eLearning/Question/GetAll";
        public const string Create = "eLearning/CourseContents/Create";
        public const string Edit = "eLearning/CourseContents/Edit";
        public const string Delete = "eLearning/CourseContents/Delete";

        public const string GetAllAudio = "eLearning/CourseContents/GetAllAudio";
        public const string GetAllDocument = "eLearning/CourseContents/GetAllDocument";
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

        [HasAccess(UserAccess.CourseEdit)]
        // GET: eLearning/CourseContents/Create
        public async Task<ActionResult> Create(int? courseId, int? moduleId, CreateContentFrom createContentFrom,
            CourseContentType courseContentType, string courseTitle)
        {
            if (courseId == null)
            {
                TempData["ErrorMessage"] = "Could not find a course to create the content to.";

                return Redirect(Request.UrlReferrer.ToString());
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

            if (courseContentType == CourseContentType.Audio)
                await GetAllAudio(courseId.Value);

            if (courseContentType == CourseContentType.Document)
                await GetAllDocument(courseId.Value);

            return View(model);
        }

        /// <summary>
        /// create a content. If the content have a file to upload, then we will make 2 calls to API.
        /// 1. Call to create content, to get the contentid for the file to be uploaded. However, FileName
        ///     is set to null first, otherwise json error
        /// 2. Call to upload the file.
        ///
        /// Can this be done in 1 call? of course can. I am just too lazy to do it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: eLearning/CourseModules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrEditContentModel model, string SubmitType="Save")
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;

                var currentFileName = model.File;
                string contentId = null;

                model.File = null;

                // check slideshare url and change to embed code
                if (model.ContentType == CourseContentType.Document)
                {
                    await GetAllDocument(model.CourseId, model.ContentFileId != null ? model.ContentFileId.Value : -1);

                    if (model.DocumentType == DocumentType.UseSlideshare)
                    {
                        if (!model.Url.Contains("embed_code"))
                        {
                            model.Url = await SlideshareHelper.GetEmbedCode(model.Url);
                        }
                    }
                }

                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, ContentApiUrl.Create, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Content successfully added.";

                    await LogActivity(Modules.Learning, "Create content success : " + model.Title);

                    contentId = response.Data;
                }
                else
                {
                    TempData["ErrorMessage"] = "Error adding content.";

                    await LogActivity(Modules.Learning, "Create content failed : " + model.Title);

                    return RedirectToAction(nameof(Create), new
                    {
                        area = "eLearning",
                        @courseId = model.CourseId,
                        @moduleId = model.CourseModuleId,
                        @createContentFrom = model.CreateContentFrom,
                        courseContentType = model.ContentType,
                        @courseTitle = model.PageTitle
                    });


                }

                // Check if this creation include fileupload, which will require us to save the file
                model.File = currentFileName;
                if (((model.ContentType == CourseContentType.Video && model.VideoType == VideoType.UploadVideo) ||
                    (model.ContentType == CourseContentType.Audio && model.AudioType == AudioType.UploadAudio) ||
                    (model.ContentType == CourseContentType.Document && model.DocumentType == DocumentType.UploadDocument) ||
                    (model.ContentType == CourseContentType.Flash) ||
                    (model.ContentType == CourseContentType.Pdf) ||
                    (model.ContentType == CourseContentType.Powerpoint)) &&
                    model.File != null)
                {
                    // upload the file

                    var result = await new FileController().UploadToApi<List<FileDocumentModel>>(model.File);

                    if (result.isSuccess)
                    {
                        var data = result.Data;

                        var fileDocument = data[0];

                        fileDocument.FileType = model.ContentType.ToString();
                        fileDocument.CreatedBy = CurrentUser.UserId.Value;

                        fileDocument.CourseId = model.CourseId;

                        if (model.ContentType == CourseContentType.Audio)
                            fileDocument.ContentFileType = FileType.Audio;

                        if (model.ContentType == CourseContentType.Video)
                            fileDocument.ContentFileType = FileType.Video;

                        if (model.ContentType == CourseContentType.Document)
                            fileDocument.ContentFileType = FileType.Document;

                        if (!string.IsNullOrEmpty(contentId))
                            fileDocument.ContentId = int.Parse(contentId);

                        var resultUpload = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, FileApiUrl.UploadInfo, fileDocument);

                        if (resultUpload.isSuccess)
                        {
                            model.ContentFileId = int.Parse(resultUpload.Data);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Cannot upload file";
                        }
                    }
                }

                if (SubmitType.Equals("SaveAndView"))
                    return RedirectToAction("View", "CourseContents", new { area = "eLearning", @id = contentId });
                else
                    return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = model.CourseModuleId });
            }

            TempData["ErrorMessage"] = "Cannot add content.";

            // await GetAllQuestions(model.CourseId);

            //return View(model);
            return RedirectToAction(nameof(Create), new
            {
                area = "eLearning",
                @courseId = model.CourseId,
                @moduleId = model.CourseModuleId,
                @createContentFrom = model.CreateContentFrom,
                courseContentType = model.ContentType,
                @courseTitle = model.PageTitle
            });
        }

        [HasAccess(UserAccess.CourseEdit)]
        // GET: eLearning/CourseContents/Create
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Could not find the content.";

                return Redirect(Request.UrlReferrer.ToString());
            }

            var model = await TryGetContent(id.Value);

            await GetAllQuestions(model.CourseId, model.ContentQuestionId != null ? model.ContentQuestionId.Value : -1);

            if (model.ContentType == CourseContentType.Audio)
                await GetAllAudio(model.CourseId, model.ContentFileId != null ? model.ContentFileId.Value : -1);

            if (model.ContentType == CourseContentType.Document)
            {
                await GetAllDocument(model.CourseId, model.ContentFileId != null ? model.ContentFileId.Value : -1);

                if (model.DocumentType == DocumentType.UseSlideshare)
                {
                    if (!model.Url.Contains("embed_code"))
                    {
                        model.Url = await SlideshareHelper.GetEmbedCode(model.Url);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateOrEditContentModel model, string SubmitType="Save")
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId.Value;

                var currentFileName = model.File;
                model.File = null;
                var modelFileDocument = model.FileDocument;

                // check slideshare url and change to embed code
                if (model.ContentType == CourseContentType.Document)
                {
                    await GetAllDocument(model.CourseId, model.ContentFileId != null ? model.ContentFileId.Value : -1);

                    if (model.DocumentType == DocumentType.UseSlideshare)
                    {
                        if (!model.Url.Contains("embed_code"))
                        {
                            model.Url = await SlideshareHelper.GetEmbedCode(model.Url);
                        }
                    }
                }

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, ContentApiUrl.Edit, model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Content successfully edited.";

                    await LogActivity(Modules.Learning, "Edit content : " + model.Title);

                    // Check if this creation include fileupload, which will require us to save the file
                    model.File = currentFileName;
                    if (((model.ContentType == CourseContentType.Video && model.VideoType == VideoType.UploadVideo) ||
                        (model.ContentType == CourseContentType.Audio && model.AudioType == AudioType.UploadAudio) ||
                        (model.ContentType == CourseContentType.Document && model.DocumentType == DocumentType.UploadDocument) ||
                        (model.ContentType == CourseContentType.Flash) ||
                        (model.ContentType == CourseContentType.Pdf) ||
                        (model.ContentType == CourseContentType.Powerpoint)) &&
                        model.File != null)
                    {
                        // upload the file
                        var result = await new FileController().UploadToApi<List<FileDocumentModel>>(model.File);

                        if (result.isSuccess)
                        {
                            var data = result.Data;

                            var fileDocument = data[0];

                            fileDocument.FileType = model.ContentType.ToString();
                            fileDocument.CreatedBy = CurrentUser.UserId.Value;
                            fileDocument.ContentFileType = model.FileType;
                            fileDocument.CourseId = model.CourseId;
                            fileDocument.ContentId = model.Id;

                            if (model.ContentType == CourseContentType.Audio)
                                fileDocument.ContentFileType = FileType.Audio;

                            if (model.ContentType == CourseContentType.Video)
                                fileDocument.ContentFileType = FileType.Video;

                            if (model.ContentType == CourseContentType.Document)
                                fileDocument.ContentFileType = FileType.Document;

                            var resultUpload = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, FileApiUrl.UploadInfo, fileDocument);

                            if (resultUpload.isSuccess)
                            {
                                model.ContentFileId = int.Parse(resultUpload.Data);
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Cannot upload file";
                            }
                        }
                    }

                    if (SubmitType.Equals("SaveAndView"))
                        return RedirectToAction("View", "CourseContents", new { area = "eLearning", @id = model.Id });
                    else
                        return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = model.CourseModuleId });
               }
            }

            TempData["ErrorMessage"] = "Cannot edit content.";

            await GetAllQuestions(model.CourseId);

            return View(model);
        }

        [HasAccess(UserAccess.CourseCreate)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Could not find the content.";

                return Redirect(Request.UrlReferrer.ToString());
            }

            var model = await TryGetContent(id.Value);

            if (model == null)
            {
                TempData["ErrorMessage"] = "Could not find the content.";

                return Redirect(Request.UrlReferrer.ToString());
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Could not find the content.";

                return Redirect(Request.UrlReferrer.ToString());
            }

            var response = await WepApiMethod.SendApiAsync<DeleteContentModel>(HttpVerbs.Post, ContentApiUrl.Delete + $"?id={id.Value}");

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = "Content Deleted";

                var user = CurrentUser.Name;
                var data = response.Data;

                await LogActivity(Modules.Learning, "User : " + user + " delete the content with title : " + data.Title);

                return RedirectToAction("Content", "CourseModules", new { area = "eLearning", @id = data.CourseModuleId });
            }

            TempData["ErrorMessage"] = "Cannot delete content.";

            return RedirectToAction("Delete", "CourseContents", new { area = "eLearning", @id = id.Value });
        }

        private async Task GetAllQuestions(int id, int selectedId = -1)
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<IEnumerable<QuestionOnlyModel>>(HttpVerbs.Get, ContentApiUrl.GetAllQuestions + $"?id={id}");

            if (response.isSuccess)
                ViewBag.Questions = new SelectList(response.Data, "Id", "Name", selectedId);
            else
            {
                ViewBag.Questions = new SelectList(new List<QuestionOnlyModel>
                {
                    new QuestionOnlyModel{ Id = 999, Name = "Error"}
                }, "Id", "Name");

                TempData["Error"] = "Cannot find any questions to display.";
            }
        }

        private async Task GetAllAudio(int courseId, int selectedId = -1)
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<IEnumerable<AudioListModel>>(HttpVerbs.Get,
                        ContentApiUrl.GetAllAudio + $"?courseId={courseId}");

            if (response.isSuccess)
                ViewBag.AudioList = new SelectList(response.Data, "Id", "Name", selectedId);
            else
            {
                ViewBag.AudioList = new SelectList(new List<AudioListModel>
                {
                    new AudioListModel{ Id = 999, Name = "Error"}
                }, "Id", "Name");

                TempData["Error"] = "Cannot find any audio to display.";
            }
        }

        private async Task GetAllDocument(int courseId, int selectedId = -1)
        {
            // this should be queried from webapi
            var response = await WepApiMethod.SendApiAsync<IEnumerable<DocumentListModel>>(HttpVerbs.Get,
                        ContentApiUrl.GetAllDocument + $"?courseId={courseId}");

            if (response.isSuccess)
                ViewBag.DocumentList = new SelectList(response.Data, "Id", "Name", selectedId);
            else
            {
                ViewBag.DocumentList = new SelectList(new List<DocumentListModel>
                {
                    new DocumentListModel{ Id = 999, Name = "Error"}
                }, "Id", "Name");

                TempData["Error"] = "Cannot find any audio to display.";
            }
        }

        public async Task<ActionResult> View(int id)
        {
            var content = await TryGetContent(id);

            if (content != null)
            {
                switch (content.ContentType)
                {
                    case CourseContentType.Video:
                        // If its youtube video ensure the word 'embed' is there, if not, put it in
                        // eg. https://www.youtube.com/watch?v=WEDIj9JBTC8
                        if (content.VideoType == VideoType.ExternalVideo)
                        {
                            content.Url = YouTubeUrlHelper.ConvertToEmbeddedUrl(content.Url);
                        }
                        break;

                    default:
                        break;
                }

                return View(content);
            }
            else
            {
                TempData["ErrorMessage"] = "Could not find the content.";

                return Redirect(Request.UrlReferrer.ToString());
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

        public async Task<string> GetSlideshare(string newUrl)
        {
            if (!newUrl.Contains("embed_code"))
            {
                var result = await SlideshareHelper.GetEmbedCode(newUrl);

                return result;
            }

            return newUrl;
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