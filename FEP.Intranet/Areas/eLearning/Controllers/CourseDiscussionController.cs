using FEP.Helper;
using FEP.Intranet.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class CourseDiscussionController : FEPController
    {

        private string StorageRoot
        {
            //get { return Path.Combine(Server.MapPath("~/Attachments")); }
            get { return AppSettings.FileDocPath + "DiscussionAttachment"; }
        }
        // GET: eLearning/CourseDiscussion
        public async Task<ActionResult> List()
        {
            //var gg = new List<CourseDiscussionModel>();
            //var response = await WepApiMethod.SendApiAsync<List<ListCourseGroupModel>>(HttpVerbs.Get, $"eLearning/CourseGroup");
            var response = await WepApiMethod.SendApiAsync<List<CourseDiscussionModel>>(HttpVerbs.Get, $"eLearning/CourseDiscussion");
            //using (DbEntities db = new DbEntities())
            //{

            //    var categories = db.Discussions.Where(u => u.IsDeleted != true).Select(s => new CourseDiscussionModel
            //    {
            //        Id = s.Id,
            //        Name = s.Name,
            //        CreatedBy = s.CreatedBy,
            //        CreatedByUser = db.User.Where(m => m.Id == s.CreatedBy).FirstOrDefault(),
            //        CreatedOn = s.CreatedDate,
            //        UpdatetedOn = s.UpdatedDate
            //    }).ToList();

            //    foreach (var x in categories)
            //    {
            //        var _getPost = db.DiscussionPosts.Where(m => m.DiscussionId == x.Id).ToList();
            //        if (_getPost.Count > 0)
            //        {
            //            x.FirstPost = _getPost[0];
            //            x.DiscussionStatus = _getPost.Count <= 1 ? "Created on " + x.CreatedOn.ToShortDateString() : "Latest reply " + (x.UpdatetedOn.HasValue ? x.UpdatetedOn.Value.ToShortDateString() : x.CreatedOn.ToShortDateString());
            //        }

            //        gg.Add(x);
            //    }

            //    return View(gg);
            //}
            if (response.isSuccess)
                return View(response.Data);

            return View(new List<CourseDiscussionModel>());
        }

        [HttpGet]
        public async Task<ActionResult> _Create()
        {
            var model = new CreateCourseDiscussionModel();
            model.Post = new CreateCourseDiscussionPostModel();

            model.DiscussionVisibilities = new SelectList(await GetViewCategories(), "Id", "Name", 0);
            model.Groups = new SelectList(await GetGroups(), "Id", "Name", 0);

            return View(model);
        }

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
                                var fileName = Path.GetFileName(filex.FileName);
                                var fileId = _now.Ticks.ToString() + "_" + fileName;
                                var fullPath = Path.Combine(StorageRoot, fileId);
                                try
                                {

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
                                    FileName = fileId,
                                    FileSize = filex.ContentLength,
                                    FileType = filex.ContentType,
                                    FilePath = fullPath,
                                    FileTag = model.Name,
                                    FileNameOnStorage = @"data:image/png;base64," + EncodeFile(fullPath),
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
        DbEntities db = new DbEntities();
        [HttpGet]
        public async Task<ActionResult> View(int id)
        {
            var model = new DiscussionView();
            model.NewDiscussionReply = new CreateCourseDiscussionPostModel();
            model.Post = new List<DiscussionPostModel>();
            model.Attachment = new List<DiscussionAttachment>();

            //DiscussionView result = new DiscussionView();
            model.Discussion = new Discussion();
            //var s = db.Discussions.Where(u => u.Id == id && u.IsDeleted != true).FirstOrDefault();


            //model.Discussion.Id = s.Id;
            //model.Discussion.Name = s.Name;
            //model.Discussion.UserId = s.UserId;
            //model.Discussion.FirstPost = s.FirstPost;
            //model.Discussion.Pinned = s.Pinned;
            //model.Discussion.DiscussionVisibility = s.DiscussionVisibility;

            //if (model.Discussion != null)
            //{
            //    model.Post = db.DiscussionPosts.Where(m => m.DiscussionId == model.Discussion.Id && m.IsDeleted != true).ToList();
            //    model.Attachment = db.DiscussionAttachment.Where(m => m.Post.DiscussionId == model.Discussion.Id).ToList();
            //    model.NewDiscussionReply.DiscussionId = model.Discussion.Id;
            //    model.NewDiscussionReply.IsDeleted = false;
            //    model.NewDiscussionReply.Message = "";
            //    model.NewDiscussionReply.UserId = CurrentUser.UserId.Value;

            //    // return Ok(result);
            //}

            //using (DbEntities _db = new DbEntities())
            //{
            //    var s = _db.Discussions.Where(u => u.Id == id && u.IsDeleted != true).FirstOrDefault();

            //    if (s != null)
            //    {
            //        DiscussionView _result = new DiscussionView();
            //        _result.NewDiscussionReply = new CreateCourseDiscussionPostModel();
            //        _result.Post = new List<DiscussionPost>();
            //        _result.Attachment = new List<DiscussionAttachment>();
            //        _result.Discussion = new Discussion();

            //        _result.Discussion.Id = s.Id;
            //        _result.Discussion.Name = s.Name;
            //        _result.Discussion.UserId = s.UserId;
            //        _result.Discussion.FirstPost = s.FirstPost;
            //        _result.Discussion.Pinned = s.Pinned;
            //        _result.Discussion.DiscussionVisibility = s.DiscussionVisibility;

            //        _result.Post = db.DiscussionPosts.Where(m => m.DiscussionId == id && m.IsDeleted != true).ToList();
            //        _result.Attachment = db.DiscussionAttachment.Where(m => m.Post.DiscussionId == id).ToList();
            //        _result.NewDiscussionReply.DiscussionId = id;
            //        _result.NewDiscussionReply.IsDeleted = false;
            //        _result.NewDiscussionReply.Message = "";

            //        // return Ok(_result);
            //    }
            //}

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

                                    filex.SaveAs(fullPath);

                                    var attachment = new CreateCourseDiscussionAttachmentModel()
                                    {
                                        Id = 0,
                                        FileName = fileId,
                                        FileSize = filex.ContentLength,
                                        FileType = filex.ContentType,
                                        FilePath = fullPath,
                                        FileTag = "",
                                        FileNameOnStorage = @"data:image/png;base64," + EncodeFile(fullPath),
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

                    db.DiscussionPosts.Add(_post);
                    db.SaveChanges();

                    if (_attachmentId > 0)
                    {
                        DiscussionAttachment _attachment = new DiscussionAttachment()
                        {
                            AttachmentId = _attachmentId,
                            PostId = _post.Id,
                        };

                        db.DiscussionAttachment.Add(_attachment);
                        db.SaveChanges();
                    }
                }




            }
            return RedirectToAction("View", new { id = NewPost.NewDiscussionReply.DiscussionId });
        }

        public ActionResult DisplaySearchResults(int id)
        {


            return PartialView("_CommentReply");
        }

        private async Task<int> SaveAttachment(DateTime _now, HttpFileCollectionBase currentFiles)
        {



            return 0;

        }

        [HttpGet]
        public ActionResult _DiscussionReply()
        {
            //if (DiscussionReply != null)
            {
                var model = new DiscussionView();
                ModelState.Clear();
                return PartialView(model);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult _DiscussionReply(CreateCourseDiscussionPostModel NewPost)
        {
            //if (DiscussionReply != null)
            {
                var model = new CreateCourseDiscussionPostModel();
                ModelState.Clear();
                return PartialView(model);
            }
            return PartialView();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> _Create(CreateCourseDiscussionModel model)
        //{

        //    var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eLearning/CourseDiscussion/IsNameExist?id={null}&name={model.Topic}");

        //    if (nameResponse.isSuccess)
        //    {
        //        TempData["ErrorMessage"] = Language.CourseDiscussion.ValidExistName;
        //        return RedirectToAction("List");
        //    }

        //    if (!model.Type.HasValue)
        //        model.Type = LearningViewType.User;

        //    if (ModelState.IsValid)
        //    {

        //        var r = new List<ViewDataUploadFilesResult>();

        //        foreach (string file in Request.Files)
        //        {
        //            var statuses = new List<ViewDataUploadFilesResult>();
        //            var headers = Request.Headers;

        //            if (string.IsNullOrEmpty(headers["X-File-Name"]))
        //            {
        //                //UploadWholeFile(Request, statuses);

        //                for (int i = 0; i < Request.Files.Count; i++)
        //                {
        //                    var filex = Request.Files[i];

        //                    var fullPath = Path.Combine(StorageRoot, Path.GetFileName(filex.FileName));

        //                    filex.SaveAs(fullPath);

        //                    // add to db to get id then attach to statuses
        //                    var attachmentresponse = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/CourseDiscussion", filex.FileName);

        //                    statuses.Add(new ViewDataUploadFilesResult()
        //                    {
        //                        name = filex.FileName,
        //                        size = filex.ContentLength,
        //                        type = filex.ContentType,
        //                        url = "/Home/Download/" + filex.FileName,
        //                        delete_url = "/Home/Delete/" + filex.FileName,
        //                        thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
        //                        delete_type = "GET",
        //                    });


        //                }
        //            }
        //            else
        //            {
        //                UploadPartialFile(headers["X-File-Name"], Request, statuses);
        //            }

        //            // add to db to get attachment id

        //            //JsonResult result = Json(statuses);
        //            //result.ContentType = "text/plain";

        //            //return result;
        //        }

        //        var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/CourseDiscussion", model);

        //        if (response.isSuccess)
        //        {
        //            TempData["SuccessMessage"] = Language.CourseDiscussion.AlertSuccessCreate;

        //            LogActivity(Modules.Learning, "Create Parameter Discussion Topic", model);

        //            return RedirectToAction("List");
        //        }
        //    }

        //    model.ViewCategories = new SelectList(await GetViewCategories(), "Id", "Topic", 0);

        //    TempData["ErrorMessage"] = Language.CourseDiscussion.AlertFailCreate;

        //    return RedirectToAction("List");

        //}

        //public ActionResult _Edit(int id, string No, string Name, LearningViewCategory ViewCategory, LearningViewType Type, int? GroupId, int? CourseId, int? UserId)
        //{

        //    var model = new EditCourseDiscussionModel
        //    {
        //        //         public int Id { get; set; }
        //        //public LearningViewCategory ViewCategory { get; set; }

        //        //public LearningViewType Type { get; set; }
        //        //public int? GroupId { get; set; }
        //        //public int? CourseId { get; set; }
        //        //public int? UserId { get; set; }
        //        Id = id,
        //        Topic = Name,
        //        ViewCategory = ViewCategory,
        //        Type = Type,
        //        GroupId = GroupId,
        //        CourseId = CourseId,
        //        UserId = UserId
        //    };

        //    return View(model);
        //}

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

        //public ActionResult _Delete(int id, string No, string Name)
        //{

        //    //var model = new DeleteCourseDiscussionModel
        //    //{
        //    //    Id = id,
        //    //    No = No,
        //    //    Topic = Name
        //    //};

        //    return View(model);
        //}

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