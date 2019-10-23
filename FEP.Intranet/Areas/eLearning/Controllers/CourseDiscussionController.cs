using FEP.Helper;
using FEP.Intranet.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class CourseDiscussionController : FEPController
    {
        DbEntities db = new DbEntities();

        private string StorageRoot
        {
            //get { return Path.Combine(Server.MapPath("~/Attachments")); }
            // get { return AppSettings.FileDocPath + "DiscussionAttachment"; }
            get { return HostingEnvironment.MapPath(@"/App_Data/" + "DiscussionAttachment"); }
        }
        // GET: eLearning/CourseDiscussion

        private long RequestLimit
        {
            get {
                    const long DefaultAllowedContentLengthBytes = 1073741824;
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(System.Web.HttpRuntime.AppDomainAppPath + "/web.config"))
                    {
                        System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                        xmlDocument.LoadXml(reader.ReadToEnd());

                        if (xmlDocument.GetElementsByTagName("requestLimits").Count > 0)
                        {
                            var maxAllowedContentLength = xmlDocument.GetElementsByTagName("requestLimits")[0].Attributes.Cast<System.Xml.XmlAttribute>().FirstOrDefault(atributo => atributo.Name.Equals("maxAllowedContentLength"));
                            return Convert.ToInt64(maxAllowedContentLength.Value);
                        }
                        else
                            return DefaultAllowedContentLengthBytes;
                    }
            }
        }

        public async Task<ActionResult> List()
        {
            var response = await WepApiMethod.SendApiAsync<List<CourseDiscussionModel>>(HttpVerbs.Get, $"eLearning/CourseDiscussion");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<CourseDiscussionModel>());
        }

        [HasAccess(UserAccess.CourseDiscussionCreate)]
        [HttpGet]
        public async Task<ActionResult> _Create()
        {
            var model = new CreateCourseDiscussionModel();
            model.Post = new CreateCourseDiscussionPostModel();

            model.DiscussionVisibilities = new SelectList(await GetViewCategories(), "Id", "Name", 0);
            model.Groups = new SelectList(await GetGroups(), "Id", "Name", 0);

            return View(model);
        }

        [HasAccess(UserAccess.CourseDiscussionCreate)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateCourseDiscussionModel model)
        {
            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eLearning/CourseDiscussion/IsNameExist?id={null}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = Language.eLearning.CourseCategory.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                DateTime _now = DateTime.Now;
                model.Post.UserId = model.UserId = CurrentUser.UserId.Value;
                if (!Directory.Exists(StorageRoot))
                {
                    Directory.CreateDirectory(StorageRoot);
                }
                int _attachmentId = 0;
                var r = new List<CreateCourseDiscussionAttachmentModel>();

                foreach (string file in Request.Files)// set 1 only
                {
                    var headers = Request.Headers;

                    if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    {
                        //UploadWholeFile(Request, statuses);

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var filex = Request.Files[i];
                            if (!String.IsNullOrEmpty(filex.FileName))
                            {
                                if (filex.ContentLength < RequestLimit)
                                {
                                    var fileName = Path.GetFileName(filex.FileName);
                                    var fileId = _now.Ticks.ToString() + "_" + fileName;
                                    var fullPath = Path.Combine(StorageRoot, fileId);
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(StorageRoot);
                                        filex.SaveAs(fullPath);
                                    }
                                    catch
                                    {
                                        TempData["ErrorMessage"] = "Error save attachment";
                                        return RedirectToAction("List");
                                    }

                                    var attachment = new CreateCourseDiscussionAttachmentModel()
                                    {
                                        Id = 0,
                                        FileName = fileName,
                                        FileSize = filex.ContentLength,
                                        FileType = filex.ContentType,
                                        FilePath = @"~\App_Data\DiscussionAttachment\" + fileId,
                                        FileTag = model.Name,
                                        FileNameOnStorage = fileId,
                                        CreatedBy = model.UserId,
                                        CreatedDate = _now,
                                    };

                                    var attachmentresponse = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/CourseDiscussion/CreateAttachment", attachment);

                                    if (attachmentresponse.isSuccess)
                                    {
                                        _attachmentId = attachmentresponse.Data;
                                        model.Post.GotAttachment = true;
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = "Attachment uploaded is too big";
                                    return RedirectToAction("List");
                                }
                            }
                            else
                            {
                                model.Post.GotAttachment = false;
                            }

                        }
                    }
                }


                if (model.Post.GotAttachment.HasValue)
                {
                    if (_attachmentId > 0 && model.Post.GotAttachment.Value)
                    {
                        model.AttachmentId = _attachmentId;
                    }
                }



                var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/CourseDiscussion", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = "Create New Discussion Completed";
                    LogActivity(Modules.Learning, "Create Discussion Topic", model);
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed Create New Discussion";
                }

            }

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<ActionResult> View(int id)
        {
            var model = new DiscussionView();
            model.NewDiscussionReply = new CreateCourseDiscussionPostModel();
            model.Post = new List<DiscussionPostModel>();
            model.Attachment = new List<DiscussionAttachment>();

            model.Discussion = new Discussion();
            var DiscussionResponse = await WepApiMethod.SendApiAsync<Discussion>(HttpVerbs.Get, $"eLearning/CourseDiscussion/GetDiscussion?id={id}");
            if (DiscussionResponse.isSuccess)
            {
                model.Discussion = DiscussionResponse.Data;

                var PostResponse = await WepApiMethod.SendApiAsync<List<DiscussionPostModel>>(HttpVerbs.Get, $"eLearning/CourseDiscussion/GetPost?id={id}");
                if (PostResponse.isSuccess)
                {
                    model.Post = PostResponse.Data;
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed Load Discussion Thread";
                    return RedirectToAction("List");
                }

                var AttachmentResponse = await WepApiMethod.SendApiAsync<List<DiscussionAttachment>>(HttpVerbs.Get, $"eLearning/CourseDiscussion/GetAttachment?id={id}");
                if (AttachmentResponse.isSuccess)
                {
                    model.Attachment = AttachmentResponse.Data;
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed Load Discussion Thread";
                    return RedirectToAction("List");
                }

                model.NewDiscussionReply.DiscussionId = id;
                model.NewDiscussionReply.IsDeleted = false;
                model.NewDiscussionReply.Message = "";
                model.NewDiscussionReply.UserId = CurrentUser.UserId;

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "Failed Load Discussion Thread";
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNewReply(DiscussionView NewPost)
        {
            if (NewPost != null)
            {

                if (NewPost.NewDiscussionReply != null)
                {
                    DateTime _now = DateTime.Now;
                    int _attachmentId = 0;
                    //int attachmentId = SaveAttachment(_now, Request.Files).Result;
                    foreach (string file in Request.Files)// set 1 only
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var filex = Request.Files[i];
                            if (!String.IsNullOrEmpty(filex.FileName))
                            {
                                var fileName = Path.GetFileName(filex.FileName);
                                var fileId = _now.Ticks.ToString() + "_" + fileName;
                                var fullPath = Path.Combine(StorageRoot, fileId);
                                try
                                {
                                    System.IO.Directory.CreateDirectory(StorageRoot);
                                    filex.SaveAs(fullPath);

                                    var attachment = new CreateCourseDiscussionAttachmentModel()
                                    {
                                        Id = 0,
                                        FileName = fileName,
                                        FileSize = filex.ContentLength,
                                        FileType = filex.ContentType,
                                        FilePath = @"~\App_Data\DiscussionAttachment\" + fileId,
                                        FileTag = "",
                                        FileNameOnStorage = fileId,
                                        CreatedBy = CurrentUser.UserId.Value,
                                        CreatedDate = _now,

                                    };

                                    var attachmentresponse = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/CourseDiscussion/CreateAttachment", attachment);

                                    if (attachmentresponse.isSuccess)
                                    {
                                        _attachmentId = attachmentresponse.Data;
                                    }


                                }
                                catch
                                {
                                    TempData["ErrorMessage"] = "Error save attachment";
                                    //return RedirectToAction("List");
                                }
                            }
                        }
                    }

                    // add attachment


                    // add reply (attach attachment id if any)

                    DiscussionPost _post = new DiscussionPost();
                    _post.DiscussionId = NewPost.NewDiscussionReply.DiscussionId;
                    _post.ParentId = null;
                    _post.Topic = "";
                    _post.Message = NewPost.NewDiscussionReply.Message;
                    _post.IsDeleted = false;
                    _post.UserId = CurrentUser.UserId.Value;
                    _post.CreatedBy = CurrentUser.UserId.Value;
                    _post.CreatedDate = _now;
                    _post.UpdatedBy = CurrentUser.UserId.Value;

                    //db.DiscussionPosts.Add(_post);
                    //db.SaveChanges();
                    var addNewReply = await WepApiMethod.SendApiAsync<DiscussionPost>(HttpVerbs.Post, $"eLearning/CourseDiscussion/AddDiscussionReply", _post);
                    if (addNewReply.isSuccess)
                    {
                        var addedReply = addNewReply.Data;

                        if (_attachmentId > 0)
                        {
                            var linkAttachment = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Post, $"eLearning/CourseDiscussion/AddAttachmentToDisccusion?pid={addedReply.Id}&aid={_attachmentId}");

                            if (linkAttachment.isSuccess)
                            {

                            }
                            else
                            {

                            }
                            //DiscussionAttachment _attachment = new DiscussionAttachment()
                            //{
                            //    AttachmentId = _attachmentId,
                            //    PostId = _post.Id,
                            //};

                            //db.DiscussionAttachment.Add(_attachment);
                            //db.SaveChanges();
                        }
                    }
                    else
                    {
                        //error
                    }
                }




            }
            return RedirectToAction("View", new { id = NewPost.NewDiscussionReply.DiscussionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNewComment(DiscussionView NewPost)
        {
            if (NewPost != null)
            {
                if (NewPost.NewDiscussionReply != null)
                {
                    var DiscussionResponse = await WepApiMethod.SendApiAsync<DiscussionPost>(HttpVerbs.Get, $"eLearning/CourseDiscussion/GetParentPost?id={NewPost.NewDiscussionReply.ParentId}");

                    if (DiscussionResponse.isSuccess)
                    {
                        var parentPost = DiscussionResponse.Data;

                        if (NewPost.NewDiscussionReply != null)
                        {
                            DateTime _now = DateTime.Now;

                            DiscussionPost _post = new DiscussionPost();
                            _post.DiscussionId = parentPost.DiscussionId;
                            _post.ParentId = parentPost.Id;
                            _post.Topic = parentPost.Topic;
                            _post.Message = NewPost.NewDiscussionReply.Message;
                            _post.IsDeleted = false;
                            _post.UserId = CurrentUser.UserId.Value;
                            _post.CreatedBy = CurrentUser.UserId.Value;
                            _post.CreatedDate = _now;
                            _post.UpdatedBy = CurrentUser.UserId.Value;

                            db.DiscussionPosts.Add(_post);
                            db.SaveChanges();

                            return RedirectToAction("View", new { id = parentPost.DiscussionId });
                        }
                    }
                    else
                    {

                    }
                }
            }
            return RedirectToAction("List");
        }

        public ActionResult _CommentReply(int id)
        {

            ViewBag.PostId = id;
            return PartialView("_CommentReply");
        }

        [HttpGet]
        public ActionResult _DiscussionReply()
        {
            var model = new DiscussionView();
            ModelState.Clear();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _DiscussionReply(CreateCourseDiscussionPostModel NewPost)
        {
            var model = new CreateCourseDiscussionPostModel();
            ModelState.Clear();
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit(EditCourseDiscussionModel model)
        {

            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eLearning/CourseDiscussion/IsNameExist?id={model.Id}&name={model.Name}");

            if (nameResponse.isSuccess)
            {
                // TempData["ErrorMessage"] = Language.CourseDiscussion.ValidExistName;
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eLearning/CourseDiscussion?id={model.Id}", model);

                if (response.isSuccess)
                {
                    // TempData["SuccessMessage"] = Language.CourseDiscussion.AlertSuccessUpdate;

                    LogActivity(Modules.Learning, "Update Parameter Discussion Topic", model);

                    return RedirectToAction("List");
                }
            }

            //TempData["ErrorMessage"] = Language.CourseDiscussion.AlertFailUpdate;

            return RedirectToAction("List");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Delete(int id)
        {

            var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eLearning/CourseDiscussion?id={id}");

            if (response.isSuccess)
            {
                // TempData["SuccessMessage"] = Language.CourseDiscussion.AlertSuccessDelete;

                LogActivity(Modules.Learning, "Delete Parameter Discussion Topic", new { id = id });

                return RedirectToAction("List");
            }

            // TempData["ErrorMessage"] = Language.CourseDiscussion.AlertFailDelete;

            return RedirectToAction("List");

        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles()
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    //UploadWholeFile(Request, statuses);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var filex = Request.Files[i];

                        var fullPath = Path.Combine(StorageRoot, Path.GetFileName(filex.FileName));

                        filex.SaveAs(fullPath);

                        // add to db to get id then attach to statuses
                        var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/CourseDiscussion", filex.FileName);

                        statuses.Add(new ViewDataUploadFilesResult()
                        {
                            name = filex.FileName,
                            size = filex.ContentLength,
                            type = filex.ContentType,
                            url = "/Home/Download/" + filex.FileName,
                            delete_url = "/Home/Delete/" + filex.FileName,
                            thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                            delete_type = "GET",
                        });


                    }
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }

                // add to db to get attachment id

                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";

                return result;
            }

            return Json(r);
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1)
                throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fullName = Path.Combine(StorageRoot, Path.GetFileName(fileName));

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/Home/Download/" + fileName,
                delete_url = "/Home/Delete/" + fileName,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
            });
        }

        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var fullPath = Path.Combine(StorageRoot, Path.GetFileName(file.FileName));

                file.SaveAs(fullPath);

                // add to db to get id then attach to statuses
                //var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/CourseDiscussion", model);

                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/Home/Download/" + file.FileName,
                    delete_url = "/Home/Delete/" + file.FileName,
                    thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                    delete_type = "GET",
                });


            }
        }

        [NonAction]
        private async Task<IEnumerable<CourseDiscussionVisibilityModel>> GetViewCategories()
        {
            var sectors = Enumerable.Empty<CourseDiscussionVisibilityModel>();
            var response = await WepApiMethod.SendApiAsync<List<CourseDiscussionVisibilityModel>>(HttpVerbs.Get, $"eLearning/CourseDiscussion/GetDiscussionCategory");

            if (response.isSuccess)
            {
                sectors = response.Data;
            }
            return sectors;
        }

        [NonAction]
        private async Task<IEnumerable<GroupCategoryModel>> GetGroups()
        {
            var courses = Enumerable.Empty<GroupCategoryModel>();
            var response = await WepApiMethod.SendApiAsync<List<GroupCategoryModel>>(HttpVerbs.Get, $"eLearning/CourseDiscussion/GetGroup?id={CurrentUser.UserId.Value}");

            if (response.isSuccess)
            {
                courses = response.Data;
            }
            return courses;
        }

        public ActionResult DownloadAttachment(string input, string filetype)
        {
            string path = Server.MapPath(input);
            if (System.IO.File.Exists(path))
            {
                return File(path, filetype);
            }
            return HttpNotFound();
        }
    }

    public class ViewDataUploadFilesResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }
}