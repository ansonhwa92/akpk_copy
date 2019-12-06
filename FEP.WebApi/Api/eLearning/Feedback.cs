using FEP.Helper;
using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/Feedback")]
    public class FeedbackController : ApiController
    {
        private DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]FeedbackCreateModel model)
        {
            try
            {
                using (DbEntities _db = new DbEntities())
                {
                    Feedback _new = new Feedback();

                    _new.Header = model.Header;
                    _new.Template = model.Template;
                    _new.CreatedBy = model.CreatedBy;
                    _new.CreatedDate = model.Created;
                    _new.UpdatedBy = model.CreatedBy;

                    _db.Feedback.Add(_new);
                    _db.SaveChanges();

                    return Ok(_new.Id);
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                return Ok();
                //return ex.InnerException.Message.ToString();
            }
        }

        [Route("api/eLearning/Feedback/PostNew")]
        [HttpPost]
        public IHttpActionResult PostNew(FeedbackModel model)
        {
            try
            {
                using (DbEntities _db = new DbEntities())
                {
                    FeedbackContent _new = new FeedbackContent();
                    _new.ViewId = model.NewPost.Visibility;
                    _new.CreatedBy = model.NewPost.UserId;
                    _new.CreatedDate = model.NewPost.Created;
                    _new.FeedbackId = model.NewPost.FeedbackId.Value;
                    _new.Content = model.NewPost.Post;

                    _db.FeedbackContent.Add(_new);
                    _db.SaveChanges();

                    FeedbackModel _toView = new FeedbackModel();
                    _toView.id = model.NewPost.FeedbackId.Value;
                    _toView.Children = GenerateFeedbackContent(model.NewPost.FeedbackId.Value);
                    _toView.Visibilities = new List<System.Web.Mvc.SelectListItem>();

                    return Ok(_toView);
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                return Ok();
                //return ex.InnerException.Message.ToString();
            }
        }


        [Route("api/eLearning/Feedback/GetVisibility")]
        [HttpGet]
        public IHttpActionResult GetVisibility()
        {
            using (DbEntities _db = new DbEntities())
            {
                List<FeedbackVisibility> categories = _db.FeedbackView.Select(c => new FeedbackVisibility() { Id = c.Id, Type = c.View.ToString() }).ToList();
                return Ok(categories);
            }
        }

        private List<FeedbackVisibility> GetVisibilities()
        {
            using (DbEntities _db = new DbEntities())
            {
                List<FeedbackVisibility> categories = _db.FeedbackView.Select(c => new FeedbackVisibility() { Id = c.Id, Type = c.View.ToString() }).ToList();
                return categories;
            }
        }

        public IHttpActionResult Get()
        {
            using (DbEntities _db = new DbEntities())
            {
                var categories = db.Discussions.Where(u => u.IsDeleted != true).Select(s => new CourseDiscussionModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CreatedBy = s.CreatedBy,
                    CreatedByName = db.User.Where(m => m.Id == s.CreatedBy).FirstOrDefault().Name,
                    CreatedOn = s.CreatedDate,
                    UpdatetedOn = s.UpdatedDate,
                    DiscussionVisibility = s.DiscussionVisibility,
                    FirstPost = s.Posts.Where(m => m.Id == s.FirstPost).FirstOrDefault().Message,
                    FirstPostId = s.FirstPost
                }).ToList();

                //foreach (var x in categories)
                //{
                //    var _getPost = db.DiscussionPosts.Where(m => m.DiscussionId == x.Id).ToList();
                //    if (_getPost.Count > 0)
                //    {
                //        x.FirstPost = _getPost[0];
                //        x.DiscussionStatus = _getPost.Count <= 1 ? "Created on " + x.CreatedOn.ToShortDateString() : "Latest reply " + (x.UpdatetedOn.HasValue ? x.UpdatetedOn.Value.ToShortDateString() : x.CreatedOn.ToShortDateString());
                //    }
                //}
                return Ok(categories);
            }

        }

        [Route("api/eLearning/Feedback/GetAll")]
        [HttpPost]
        public IHttpActionResult GetAll(FilterDiscussionModel request)
        {
            var query = db.Discussions.Where(x => (String.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name)) && x.IsDeleted != true && x.DiscussionVisibility == DiscussionVisibility.Everybody);

            var totalCount = query.Count();

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(p => p.Name.Contains(value));
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "DisplayDateTime":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.CreatedDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.CreatedDate);
                        }

                        break;

                    case "DiscussionCard":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Name);
                        }

                        break;

                    //case "Status":

                    //    if (sortAscending)
                    //    {
                    //        query = query.OrderBy(o => o.Status);
                    //    }
                    //    else
                    //    {
                    //        query = query.OrderByDescending(o => o.Status);
                    //    }

                    //    break;

                    default:
                        query = query.OrderBy(o => o.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(o => o.Id);
            }

            //var data = query.Skip(request.start).Take(request.length)
            //    .Select(s => new CourseDiscussionModel
            //    {
            //        Id = s.Id,
            //        Name = s.Name,
            //        CreatedBy = s.CreatedBy,
            //        CreatedByName = db.User.Where(m => m.Id == s.CreatedBy).FirstOrDefault().Name,
            //        CreatedOn = s.CreatedDate,
            //        UpdatetedOn = s.UpdatedDate,
            //        DiscussionVisibility = s.DiscussionVisibility,
            //        FirstPost = s.Posts.Where(m => m.Id == s.FirstPost).FirstOrDefault().Message,
            //        FirstPostId = s.FirstPost
            //    }).ToList();

            var data = query.Skip(request.start).Take(request.length)
               .Select(s => new CourseDiscussionListDataTableModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   CreatedBy = db.User.Where(m => m.Id == s.CreatedBy).FirstOrDefault().Name,
                   FirstPost = s.Posts.Where(m => m.Id == s.FirstPost).FirstOrDefault().Message,
                   DisplayDateTime = new DateTimeModel() { CreatedOn = s.CreatedDate, UpdatedOn = s.Posts.Where(m => m.DiscussionId == s.Id).OrderByDescending(m => m.CreatedDate).FirstOrDefault().CreatedDate },
                   DiscussionCard = new DiscussionCardModel() { Id = s.Id, Name = s.Name, CreatedBy = db.User.Where(m => m.Id == s.CreatedBy).FirstOrDefault().Name, FirstPost = s.Posts.Where(m => m.Id == s.FirstPost).FirstOrDefault().Message }
               }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });
        }

        [Route("api/eLearning/Feedback/GetFeedback")]
        [HttpGet]
        public IHttpActionResult GetFeedback(int id)
        {
            using (DbEntities _db = new DbEntities())
            {
                var _get = _db.Feedback.Find(id);
                if (_get != null)
                {
                    FeedbackModel _toView = new FeedbackModel();
                    _toView.id = id;
                    _toView.Visibilities = new List<System.Web.Mvc.SelectListItem>(); //(IEnumerable<System.Web.Mvc.SelectListItem>)GetVisibilities().ToList();
                    _toView.Children = GenerateFeedbackContent(id);

                    return Ok(_toView);
                }
            }
            return NotFound();
        }

        [Route("api/eLearning/Feedback/DeletePost")]
        [HttpGet]
        public IHttpActionResult DeletePost(int id, int UpdateId)
        {
            using (DbEntities _db = new DbEntities())
            {
                var _get = _db.FeedbackContent.Find(id);
                if (_get != null)
                {
                    _get.IsDeleted = true;
                    _get.UpdatedBy = UpdateId;
                    _get.UpdatedDate = DateTime.Now;

                    _db.FeedbackContent.Attach(_get);
                    _db.Entry(_get).Property(m => m.IsDeleted).IsModified = true;
                    _db.Entry(_get).Property(m => m.UpdatedBy).IsModified = true;
                    _db.Entry(_get).Property(m => m.UpdatedDate).IsModified = true;
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.SaveChanges();

                    FeedbackModel _toView = new FeedbackModel();
                    _toView.id = _get.FeedbackId;
                    _toView.Visibilities = new List<System.Web.Mvc.SelectListItem>(); //(IEnumerable<System.Web.Mvc.SelectListItem>)GetVisibilities().ToList();
                    _toView.Children = GenerateFeedbackContent(_get.FeedbackId);

                    return Ok(_toView);
                }
            }
            return NotFound();
        }

        private List<FeedbackContentModel> GenerateFeedbackContent(int _feedbackId)
        {
            List<FeedbackContentModel> result = new List<FeedbackContentModel>();
            using (DbEntities _db = new DbEntities())
            {
                var _getChild = _db.FeedbackContent.Where(m => m.FeedbackId == _feedbackId && m.IsDeleted == false).Select(c => new FeedbackContentModel()
                {
                    Avatar = c.User.UserAccount.Avatar,
                    Id = c.Id,
                    Created = c.CreatedDate,
                    UserId = c.User.Id,
                    Name = c.User.Name,
                    Updated = c.UpdatedDate,
                    Post = c.Content,
                    Visibility = c.FeedbackView.Id
                }).ToList();

                if (_getChild.Count > 0)
                {
                    result = _getChild;
                }
            }
            return result;
        }


        //[ValidationActionFilter]
        //public IHttpActionResult Post([FromBody]CreateCourseDiscussionModel model)
        //{
        //    // create new entry for discussion

        //    // create new entry for post

        //    // create new link from post to attachment if any
        //    try
        //    {
        //        using (DbEntities _db = new DbEntities())
        //        {
        //            DateTime _now = DateTime.Now;
        //            Discussion _discussion = new Discussion()
        //            {
        //                Name = model.Name,
        //                CreatedBy = model.UserId,
        //                UpdatedBy = model.UserId,
        //                UserId = model.UserId,
        //                CreatedDate = _now,
        //                DiscussionVisibility = model.DiscussionVisibility,
        //                Pinned = false,
        //                IsDeleted = false,
        //                FirstPost = 0,
        //            };

        //            if (model.DiscussionVisibility == DiscussionVisibility.Everybody)
        //            {

        //            }
        //            else if (model.DiscussionVisibility == DiscussionVisibility.GroupOnly)
        //            {
        //                _discussion.GroupId = model.GroupId;
        //            }

        //            _db.Discussions.Add(_discussion);
        //            _db.SaveChanges();

        //            DiscussionPost _post = new DiscussionPost()
        //            {
        //                DiscussionId = _discussion.Id,
        //                ParentId = null,
        //                Topic = model.Name,
        //                Message = model.Post.Message,
        //                IsDeleted = false,
        //                UserId = model.Post.UserId.Value,
        //                CreatedBy = model.Post.UserId.Value,
        //                CreatedDate = _now,
        //                UpdatedBy = model.Post.UserId.Value,
        //                UpdatedDate = _now
        //            };

        //            _db.DiscussionPosts.Add(_post);
        //            _db.SaveChanges();

        //            if (model.Post.GotAttachment.HasValue)
        //            {
        //                if (model.Post.GotAttachment.Value && (model.AttachmentId != null))
        //                {
        //                    DiscussionAttachment _attachment = new DiscussionAttachment()
        //                    {
        //                        AttachmentId = model.AttachmentId.Value,
        //                        PostId = _post.Id,
        //                    };

        //                    _db.DiscussionAttachment.Add(_attachment);
        //                    _db.SaveChanges();
        //                }
        //            }

        //            //var updateFirstPost

        //            var _toUpdateFirstPost = _db.Discussions.Find(_discussion.Id);
        //            if (_toUpdateFirstPost != null)
        //            {
        //                _toUpdateFirstPost.FirstPost = _post.Id;

        //                db.Discussions.Attach(_toUpdateFirstPost);
        //                db.Entry(_toUpdateFirstPost).Property(m => m.FirstPost).IsModified = true;
        //                db.Configuration.ValidateOnSaveEnabled = false;

        //                db.SaveChanges();
        //            }
        //            return Ok(_discussion.Id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return Ok();
        //        //return ex.InnerException.Message.ToString();
        //    }
        //}

        [Route("api/eLearning/Feedback/CreateAttachment")]
        [ValidationActionFilter]
        public IHttpActionResult CreateAttachment(CreateCourseDiscussionAttachmentModel attachment)
        {
            try
            {
                using (DbEntities _db = new DbEntities())
                {
                    FileDocument _attachment = new FileDocument()
                    {
                        FileName = attachment.FileName,
                        FileSize = attachment.FileSize,
                        FileType = attachment.FileType,
                        FilePath = attachment.FilePath,
                        FileTag = attachment.FileTag,
                        FileNameOnStorage = attachment.FileNameOnStorage,
                        CreatedBy = attachment.CreatedBy,
                        CreatedDate = attachment.CreatedDate,

                    };

                    _db.FileDocument.Add(_attachment);
                    _db.SaveChanges();

                    return Ok(_attachment.Id);
                }
            }
            catch
            {
                return Ok();
            }




        }

        [Route("api/eLearning/Feedback/GetParentPost")]
        [ValidationActionFilter]
        [HttpGet]
        public IHttpActionResult GetParentPost(int id)
        {
            using (DbEntities _db = new DbEntities())
            {
                var s = _db.DiscussionPosts.Find(id);

                if (s != null)
                {
                    var result = new DiscussionPost();

                    result.Id = s.Id;
                    result.DiscussionId = s.DiscussionId;
                    result.Topic = s.Topic;
                    return Ok(result);
                }
            }

            return NotFound();
        }

        [Route("api/eLearning/Feedback/GetDiscussion")]
        [ValidationActionFilter]
        [HttpGet]
        public IHttpActionResult GetDiscussion(int id)
        {
            using (DbEntities _db = new DbEntities())
            {
                var s = _db.Discussions.Where(u => u.Id == id && u.IsDeleted != true).FirstOrDefault();

                if (s != null)
                {
                    var result = new Discussion();
                    result.Id = s.Id;
                    result.Name = s.Name;
                    result.UserId = s.UserId;
                    result.FirstPost = s.FirstPost;
                    result.Pinned = s.Pinned;
                    result.DiscussionVisibility = s.DiscussionVisibility;
                    result.CreatedBy = s.CreatedBy;
                    result.CreatedDate = s.CreatedDate;
                    result.UpdatedBy = s.UpdatedBy;
                    result.UpdatedDate = s.UpdatedDate;

                    //DiscussionView _result = new DiscussionView();
                    //_result.NewDiscussionReply = new CreateCourseDiscussionPostModel();
                    //_result.Post = new List<DiscussionPost>();
                    //_result.Attachment = new List<DiscussionAttachment>();
                    //_result.Discussion = new Discussion();

                    //_result.Discussion.Id = s.Id;
                    //_result.Discussion.Name = s.Name;
                    //_result.Discussion.UserId = s.UserId;
                    //_result.Discussion.FirstPost = s.FirstPost;
                    //_result.Discussion.Pinned = s.Pinned;
                    //_result.Discussion.DiscussionVisibility = s.DiscussionVisibility;


                    //_result.Post = db.DiscussionPosts.Where(m => m.DiscussionId == id && m.IsDeleted != true).ToList();
                    //_result.Attachment = db.DiscussionAttachment.Where(m => m.Post.DiscussionId == id).ToList();

                    //_result.NewDiscussionReply.DiscussionId = id;
                    //_result.NewDiscussionReply.IsDeleted = false;
                    //_result.NewDiscussionReply.Message = "";

                    return Ok(result);
                }
            }

            return Ok();
        }

        [Route("api/eLearning/Feedback/GetPost")]
        [ValidationActionFilter]
        [HttpGet]
        public IHttpActionResult GetPost(int id)
        {
            using (DbEntities _db = new DbEntities())
            {
                var x = _db.DiscussionPosts.Where(u => u.DiscussionId == id && u.IsDeleted != true).ToList();

                if (x != null)
                {
                    List<DiscussionPostModel> _result = new List<DiscussionPostModel>();
                    foreach (var s in x)
                    {
                        var result = new DiscussionPostModel();
                        result.Id = s.Id;
                        result.DiscussionId = s.DiscussionId;
                        result.UserId = s.UserId;
                        result.IsDeleted = s.IsDeleted;
                        result.ParentId = s.ParentId;
                        result.Message = s.Message;
                        result.CreatedBy = s.CreatedBy;
                        result.CreatedDate = s.CreatedDate;
                        result.UpdatedBy = s.UpdatedBy;
                        result.UpdatedDate = s.UpdatedDate;
                        result.Topic = s.Topic;
                        result.CreatedByName = _db.User.Find(s.CreatedBy).Name;
                        result.CreatedByLevel = _db.User.Find(s.CreatedBy).UserType.ToString();
                        result.Avatar = s.User.UserAccount.Avatar;
                        _result.Add(result);
                        ;
                    }
                    return Ok(_result);
                }
            }

            return Ok();
        }

        [Route("api/eLearning/Feedback/GetAttachment")]
        [ValidationActionFilter]
        [HttpGet]
        public IHttpActionResult GetAttachment(int id)
        {
            using (DbEntities _db = new DbEntities())
            {
                var x = _db.DiscussionAttachment.Where(u => u.Post.DiscussionId == id).ToList();

                if (x != null)
                {
                    List<DiscussionAttachment> _result = new List<DiscussionAttachment>();
                    foreach (var s in x)
                    {
                        var result = new DiscussionAttachment();
                        result.Id = s.Id;
                        result.AttachmentId = s.AttachmentId;
                        result.PostId = s.PostId;
                        result.Attachment = _db.FileDocument.Find(s.AttachmentId);
                        _result.Add(result);
                    }
                    return Ok(_result);
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            var category = db.Discussions.Where(u => u.Id == id).FirstOrDefault();

            if (category != null)
            {
                category.IsDeleted = true;

                db.Discussions.Attach(category);
                db.Entry(category).Property(m => m.IsDeleted).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }

        [Route("api/eLearning/Feedback/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.Discussions.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.IsDeleted != true))
                    return Ok(true);
            }
            else
            {
                if (db.Discussions.Any(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.IsDeleted != true))
                    return Ok(true);
            }

            return NotFound();
        }

        [Route("api/eLearning/Feedback/GetDiscussionCategory")]
        [HttpGet]
        public IHttpActionResult ViewCategory()
        {
            List<CourseDiscussionVisibilityModel> categories = ((DiscussionVisibility[])Enum.GetValues(typeof(DiscussionVisibility))).Select(c => new CourseDiscussionVisibilityModel() { Id = (int)c, Name = c.ToString() }).ToList();

            return Ok(categories);
        }

        [Route("api/eLearning/Feedback/GetCourse")]
        [HttpGet]
        public IHttpActionResult ViewCourse(int UserId)
        {
            List<CourseCategoryModel> courses = db.Courses.Where(m => m.IsDeleted != true).Select(c => new CourseCategoryModel() { Id = (int)c.Id, Name = c.Code.ToString() + " - " + c.Title + " " }).ToList();

            return Ok(courses);
        }

        [Route("api/eLearning/Feedback/GetGroup")]
        [HttpGet]
        public IHttpActionResult ViewGroup(int id)
        {
            List<GroupCategoryModel> groups = db.Groups.Where(m => m.IsVisible != false).Select(c => new GroupCategoryModel() { Id = (int)c.Id, Name = c.Name.ToString() }).ToList();

            return Ok(groups);
        }

        [Route("api/eLearning/Feedback/AddDiscussionReply")]
        [ValidationActionFilter]
        public IHttpActionResult AddDiscussionReply([FromBody]DiscussionPost model)
        {

            using (DbEntities _db = new DbEntities())
            {
                DiscussionPost _post = new DiscussionPost();
                _post.DiscussionId = model.DiscussionId;
                _post.ParentId = model.ParentId;
                _post.Topic = model.Topic;
                _post.Message = model.Message;
                _post.IsDeleted = model.IsDeleted;
                _post.UserId = model.UserId;
                _post.CreatedBy = model.CreatedBy;
                _post.CreatedDate = model.CreatedDate;
                _post.UpdatedBy = model.UpdatedBy;

                db.DiscussionPosts.Add(_post);
                db.SaveChanges();

                return Ok(_post);
            }
        }

        [Route("api/eLearning/Feedback/AddAttachmentToDisccusion")]
        [ValidationActionFilter]
        public IHttpActionResult AddAttachmentToDisccusion(int pid, int aid)
        {
            if (aid > 0)
            {
                using (DbEntities _db = new DbEntities())
                {
                    DiscussionAttachment _attachment = new DiscussionAttachment()
                    {
                        AttachmentId = aid,
                        PostId = pid,
                    };

                    _db.DiscussionAttachment.Add(_attachment);
                    _db.SaveChanges();

                    return Ok(true);
                }
            }
            return NotFound();
        }

        [Route("api/eLearning/Feedback/EditMessage")]
        [HttpPost]
        public IHttpActionResult EditMessage(int id, string input)
        {
            if (id > 0)
            {
                using (DbEntities _db = new DbEntities())
                {
                    var _get = _db.DiscussionPosts.Find(id);
                    if (_get != null)
                    {
                        _get.Message = input;

                        _db.DiscussionPosts.Attach(_get);
                        _db.Entry(_get).Property(m => m.Message).IsModified = true;
                        _db.Configuration.ValidateOnSaveEnabled = false;

                        _db.SaveChanges();

                        return Ok(true);
                    }
                }
            }

            return NotFound();
        }

        [Route("api/eLearning/Feedback/DeleteMessage")]
        [HttpPost]
        public IHttpActionResult DeleteMessage(int id)
        {
            if (id > 0)
            {
                using (DbEntities _db = new DbEntities())
                {
                    var _get = _db.DiscussionPosts.Find(id);
                    if (_get != null)
                    {
                        _get.IsDeleted = true;

                        _db.DiscussionPosts.Attach(_get);
                        _db.Entry(_get).Property(m => m.IsDeleted).IsModified = true;
                        _db.Configuration.ValidateOnSaveEnabled = false;

                        _db.SaveChanges();

                        return Ok(true);
                    }
                }
            }

            return NotFound();
        }
    }
}