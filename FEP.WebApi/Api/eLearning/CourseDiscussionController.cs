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
    [Route("api/eLearning/CourseDiscussion")]
    public class CourseDiscussionController : ApiController
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
                    FirstPost = s.Posts.Where(m=>m.Id == s.FirstPost).FirstOrDefault().Message,
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var category = db.Discussions.Where(u => u.Id == id && u.IsDeleted != true).Select(s => new CourseDiscussionModel
            {
                Id = s.Id,
                Name = s.Name
            }).FirstOrDefault();

            if (category != null)
            {
                return Ok(category);
            }

            return NotFound();
        }


        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateCourseDiscussionModel model)
        {
            // create new entry for discussion

            // create new entry for post

            // create new link from post to attachment if any
            try
            {
                using (DbEntities _db = new DbEntities())
                {
                    DateTime _now = DateTime.Now;
                    Discussion _discussion = new Discussion()
                    {
                        Name = model.Name,
                        CreatedBy = model.UserId,
                        UpdatedBy = model.UserId,
                        UserId = model.UserId,
                        CreatedDate = _now,
                        DiscussionVisibility = model.DiscussionVisibility,
                        Pinned = false,
                        IsDeleted = false,
                        FirstPost = 0,
                    };

                    if (model.DiscussionVisibility == DiscussionVisibility.Everybody)
                    {

                    }
                    else if (model.DiscussionVisibility == DiscussionVisibility.GroupOnly)
                    {
                        _discussion.GroupId = model.GroupId;
                    }

                    _db.Discussions.Add(_discussion);
                    _db.SaveChanges();

                    DiscussionPost _post = new DiscussionPost()
                    {
                        DiscussionId = _discussion.Id,
                        ParentId = null,
                        Topic = model.Name,
                        Message = model.Post.Message,
                        IsDeleted = false,
                        UserId = model.Post.UserId.Value,
                        CreatedBy = model.Post.UserId.Value,
                        CreatedDate = _now,
                        UpdatedBy = model.Post.UserId.Value,
                        UpdatedDate = _now
                    };

                    _db.DiscussionPosts.Add(_post);
                    _db.SaveChanges();

                    if (model.Post.GotAttachment.HasValue)
                    {
                        if (model.Post.GotAttachment.Value && (model.AttachmentId != null))
                        {
                            DiscussionAttachment _attachment = new DiscussionAttachment()
                            {
                                AttachmentId = model.AttachmentId.Value,
                                PostId = _post.Id,
                            };

                            _db.DiscussionAttachment.Add(_attachment);
                            _db.SaveChanges();
                        }
                    }

                    //var updateFirstPost

                    var _toUpdateFirstPost = _db.Discussions.Find(_discussion.Id);
                    if (_toUpdateFirstPost != null)
                    {
                        _toUpdateFirstPost.FirstPost = _post.Id;

                        db.Discussions.Attach(_toUpdateFirstPost);
                        db.Entry(_toUpdateFirstPost).Property(m => m.FirstPost).IsModified = true;
                        db.Configuration.ValidateOnSaveEnabled = false;

                        db.SaveChanges();
                    }
                    return Ok(_discussion.Id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Ok();
                //return ex.InnerException.Message.ToString();
            }
        }

        [Route("api/eLearning/CourseDiscussion/CreateAttachment")]
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
            catch {
                return Ok();
            }

            

           
        }

        [Route("api/eLearning/CourseDiscussion/GetParentPost")]
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

        [Route("api/eLearning/CourseDiscussion/GetDiscussion")]
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

        [Route("api/eLearning/CourseDiscussion/GetPost")]
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
                        _result.Add(result);;
                    }
                    return Ok(_result);
                }
            }

            return Ok();
        }

        [Route("api/eLearning/CourseDiscussion/GetAttachment")]
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

        [Route("api/eLearning/CourseDiscussion/IsNameExist")]
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

        [Route("api/eLearning/CourseDiscussion/GetDiscussionCategory")]
        [HttpGet]
        public IHttpActionResult ViewCategory()
        {
            List<CourseDiscussionVisibilityModel> categories = ((DiscussionVisibility[])Enum.GetValues(typeof(DiscussionVisibility))).Select(c => new CourseDiscussionVisibilityModel() { Id = (int)c, Name = c.ToString() }).ToList();

            return Ok(categories);
        }

        [Route("api/eLearning/CourseDiscussion/GetCourse")]
        [HttpGet]
        public IHttpActionResult ViewCourse(int UserId)
        {
            List<CourseCategoryModel> courses = db.Courses.Where(m => m.IsDeleted != true).Select(c => new CourseCategoryModel() { Id = (int)c.Id, Name = c.Code.ToString() + " - " + c.Title + " " }).ToList();

            return Ok(courses);
        }

        [Route("api/eLearning/CourseDiscussion/GetGroup")]
        [HttpGet]
        public IHttpActionResult ViewGroup(int id)
        {
            List<GroupCategoryModel> groups = db.Groups.Where(m => m.IsVisible != false).Select(c => new GroupCategoryModel() { Id = (int)c.Id, Name = c.Name.ToString()}).ToList();

            return Ok(groups);
        }

        [Route("api/eLearning/CourseDiscussion/AddDiscussionReply")]
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

        [Route("api/eLearning/CourseDiscussion/AddAttachmentToDisccusion")]
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
    }
}