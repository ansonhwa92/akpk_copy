﻿using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/CourseGroup")]
    public class CourseGroupController : ApiController
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
            var groups = db.Groups.Where(u => u.IsVisible != false).Select(s => new ListCourseGroupModel
            {
                Id = s.Id,
                Name = s.Name,
                EnrollmentCode = s.EnrollmentCode,
                Description = s.Description,
                CreatedBy = s.CreatedBy,
                UpdatedBy = s.UpdatedBy,
                CreatedDate = s.CreatedDate,
                UpdatedDate = s.UpdatedDate,

            }).ToList();

            for (int x = 0; x < groups.Count; x++)
            {
                groups[x].CreatedByName = db.User.Find(groups[x].CreatedBy).Name;
                groups[x].UpdatedByName = db.User.Find(groups[x].UpdatedBy).Name;
            }



            return Ok(groups);
        }


        public IHttpActionResult Get(int id)
        {
            var groups = db.Groups.Where(u => u.IsVisible != false && u.Id == id).Select(s => new EditCourseGroupModel
            {
                Id = s.Id,
                Name = s.Name,
                EnrollmentCode = s.EnrollmentCode,
                Description = s.Description,
            }).FirstOrDefault();

            if (groups != null)
            {
                return Ok(groups);
            }

            return NotFound();
        }

        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateCourseGroupModel model)
        {

            using (DbEntities _db = new DbEntities())
            {
                DateTime _now = DateTime.Now;
                Model.eLearning.Group _group = new Model.eLearning.Group()
                {
                    Name = model.Name,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = _now,
                    UpdatedBy = model.UpdatedBy,
                    IsVisible = true,
                    EnrollmentCode = model.EnrollmentCode,
                    Description = model.Description,
                };

                _db.Groups.Add(_group);
                _db.SaveChanges();

                return Ok(_group.Id);
            }
        }

        [Route("api/eLearning/CourseGroup/JoinGroup")]
        [ValidationActionFilter]
        public IHttpActionResult JoinGroup([FromBody]JoinCourseGroupModel model)
        {

            using (DbEntities _db = new DbEntities())
            {
                DateTime _now = DateTime.Now;
                try
                {
                    var getGroupInfo = _db.Groups.Where(m => m.EnrollmentCode == model.EnrollmentCode).SingleOrDefault();

                    if (getGroupInfo != null)
                    {
                        var _getLearner = _db.Learners.Where(m => m.UserId == model.LearnerId).SingleOrDefault();

                        if (_getLearner != null)
                        {
                            Model.eLearning.GroupMember _newjoin = new Model.eLearning.GroupMember()
                            {
                                LearnerId = _getLearner.Id,
                                CreatedBy = getGroupInfo.CreatedBy,
                                CreatedDate = _now,
                                UpdatedBy = getGroupInfo.CreatedBy,
                                GroupId = getGroupInfo.Id,
                            };

                            _db.GroupMembers.Add(_newjoin);
                            _db.SaveChanges();

                            return Ok(getGroupInfo.Name);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
        }

        [Route("api/eLearning/CourseGroup/EditGroup")]
        [ValidationActionFilter]
        public IHttpActionResult EditGroup([FromBody]EditCourseGroupModel model)
        {
            using (DbEntities _db = new DbEntities())
            {
                DateTime _now = DateTime.Now;
                try
                {
                    var getGroupInfo = _db.Groups.Where(m => m.Id == model.Id).SingleOrDefault();

                    if (getGroupInfo != null)
                    {
                        var _checkCode = _db.Groups.Where(m => m.EnrollmentCode == model.EnrollmentCode).ToList();
                        if (_checkCode.Count > 0)
                        {
                            foreach (var existing in _checkCode)
                            {
                                if (existing.Id != model.Id)
                                {
                                    return NotFound();
                                }
                            }

                        }

                        getGroupInfo.Name = model.Name;
                        getGroupInfo.UpdatedBy = model.UpdatedBy;
                        getGroupInfo.UpdatedDate = _now;
                        getGroupInfo.IsVisible = true;
                        getGroupInfo.EnrollmentCode = model.EnrollmentCode;
                        getGroupInfo.Description = model.Description;

                        _db.Entry(getGroupInfo).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();


                        return Ok(model);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return NotFound();
                }
            }
        }

        [Route("api/eLearning/CourseGroup/GetAllLearner")]
        [HttpGet]
        public IHttpActionResult GetAllLearner(int id)
        {
            return Ok(GenerateMemberListing(id));
        }

        private List<ListGroupMemberModel> GenerateMemberListing(int id)
        {
            using (DbEntities _db = new DbEntities())
            {
                var getAllLearner = _db.Learners.Where(m => m.User.Display != false).Select(s => new ListGroupMemberModel
                {
                    GroupId = id,
                    LearnerId = s.Id,
                    UserId = s.UserId,
                    UserName = s.User.Name,
                    CourseEnrolled = s.CourseEnrolled,
                    CourseCompleted = s.CourseCompleted
                }).ToList();

                if (getAllLearner.Count > 0)
                {
                    var _getGroup = _db.Groups.Find(id);
                    if (_getGroup != null)
                    {
                        for (int x = 0; x < getAllLearner.Count; x++)
                        {
                            if (getAllLearner[x].UserId == _getGroup.CreatedBy)
                                getAllLearner[x].isOwner = true;

                            var _getMembers = _db.GroupMembers.Where(m => m.GroupId == id).ToList();
                            for (int xx = 0; xx < _getMembers.Count; xx++)
                            {
                                if (getAllLearner[x].LearnerId == _getMembers[xx].LearnerId)
                                    getAllLearner[x].isMember = true;
                            }
                        }

                        return getAllLearner;
                    }
                }
            }

            return null;
        }

        [Route("api/eLearning/CourseGroup/RemoveFromGroup")]
        [HttpGet]
        public IHttpActionResult RemoveFromGroup(int id, int GroupId)
        {
            using (DbEntities _db = new DbEntities())
            {
                var _check = _db.GroupMembers.Where(m => m.GroupId == GroupId && m.LearnerId == id).SingleOrDefault();
                if (_check!=null)
                {
                    _db.Entry(_check).State = System.Data.Entity.EntityState.Deleted;
                    _db.SaveChanges();
                    return Ok(GenerateMemberListing(id));
                }
            }

            return NotFound();
        }

        [Route("api/eLearning/CourseGroup/AddToGroup")]
        [HttpGet]
        public IHttpActionResult AddToGroup(int id, int GroupId, int uid)
        {
            using (DbEntities _db = new DbEntities())
            {
                var _check = _db.GroupMembers.Where(m => m.GroupId == GroupId).ToList();
                if (_check.Count == 0)
                {
                    addToGroup(id, GroupId, uid);
                    return Ok(GenerateMemberListing(id));
                }
                else
                {
                    if (!_check.Any(m => m.LearnerId == id))
                    {
                        addToGroup(id, GroupId, uid);
                        return Ok(GenerateMemberListing(id));
                    }
                }
            }

            return NotFound();
        }

        private void addToGroup(int id, int GroupId, int uid)
        {
            using (DbEntities _db = new DbEntities())
            {
                DateTime _now = DateTime.Now;
                GroupMember _new = new GroupMember()
                {
                    LearnerId = id,
                    GroupId = GroupId,
                    CreatedDate = _now,
                    CreatedBy = uid,
                    UpdatedBy = uid,
                };

                _db.GroupMembers.Add(_new);
                _db.SaveChanges();
            }
        }


        [Route("api/eLearning/CourseGroup/IsCodeExist")]
        [HttpGet]
        public IHttpActionResult IsCodeExist(string code)
        {
            if (db.Groups.Any(u => u.EnrollmentCode.Equals(code, StringComparison.CurrentCultureIgnoreCase)))
                return Ok(true);

            return Ok(false);
        }

        [Route("api/eLearning/CourseGroup/GetCode")]
        [HttpGet]
        public IHttpActionResult GetCode()
        {
            string str = GetRandomString();

            while (CheckCode(str))
            {
                str = GetRandomString();
            }

            return Ok(str);
        }

        private string GetRandomString()
        {
            int l = 16;
            Random r = new Random();
            string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";
            //StringBuilder rs = new StringBuilder();

            //while (l > 0)
            //{
            //    rs.Append(charsToUse[(int)(r.NextDouble() * charsToUse.Length)]);
            //    l--;
            //}

            MatchEvaluator RandomChar = delegate (Match m)
            {
                return charsToUse[r.Next(charsToUse.Length)].ToString();
            };

            string result = Regex.Replace("XXXX-XXXX-XXXX-XXXX", "X", RandomChar);

            return result;
        }

        private bool CheckCode(string name)
        {
            if (db.Groups.Any(u => u.EnrollmentCode.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
                return true;

            return false;
        }

    }
}