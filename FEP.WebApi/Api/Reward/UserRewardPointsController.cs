using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Reward;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace FEP.WebApi.Api.Reward
{
    [Route("api/Reward/UserRewardPoints")]
    public class UserRewardPointsController : ApiController
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

        public string GetDisplayName(Enum val)
        {
            return ((DisplayAttribute)val.GetType()
                .GetMember(Enum.GetName(typeof(RewardType), val).ToString())
                                .First()
                                .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
        }

        [Route("api/Reward/UserRewardPoints/GetUserPointsCollection")]
        [HttpGet]
        public IHttpActionResult GetUserPointsCollection(int id)
        {
            var points = db.UserRewardPoints.Where(u => u.UserId == id && u.Display).ToList();
            int totalPoints = points.Sum(p => p.PointsReceived);

            return Ok(totalPoints);
        }
        [Route("api/Reward/UserRewardPoints/GetUserPointsUsed")]
        [HttpGet]
        public IHttpActionResult GetUserPointsUsed(int id)
        {
            var redeemed = db.UserRewardRedemption.Where(r => r.UserId == id).ToList();
            int usedPoints = redeemed.Sum(u => u.RewardRedemption.PointsToRedeem);

            return Ok(usedPoints);
        }

        [Route("api/Reward/UserRewardPoints/GetUserPointsLeft")]
        [HttpGet]
        public IHttpActionResult GetUserPointsLeft(int id)
        {
            var points = db.UserRewardPoints.Where(u => u.UserId == id && u.Display).ToList();
            int totalPoints = points.Sum(p => p.PointsReceived);

            var redeemed = db.UserRewardRedemption.Where(r => r.UserId == id).ToList();
            int usedPoints = redeemed.Sum(u => u.RewardRedemption.PointsToRedeem);

            int pointsLeft = totalPoints - usedPoints;

            return Ok(pointsLeft);
        }




        [Route("api/Reward/UserRewardPoints/GetUserRewardPointsList")]
        [HttpPost]
        // GET: api/UserRewardPoints
        public IHttpActionResult Post(int id,FilterUserRewardPointsModel request)
        {
            //get user
            IQueryable<UserRewardPoints> query;
            var user = db.User.Find(id);
            if (user == null) {
                query = db.UserRewardPoints.Where(r => r.Display);
            }
            else
            {
                query = db.UserRewardPoints.Where(r => r.Display && r.UserId == id);
            }
            
            var totalCount = query.Count();

            //advance search
            query = query.Where(s =>
            (request.CourseName == null || s.Course.Title.Contains(request.CourseName))
            && (request.RewardType == null || s.RewardType == request.RewardType)
            && (request.UserName == null || s.User.Name.Contains(request.UserName))
            && (request.PointsReceived == null || s.PointsReceived == request.PointsReceived)
            );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(a =>
                a.Course.Title.Contains(value) ||
                /*a.RewardType.GetType()
                            .GetMember(a.RewardType.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>().GetName().Contains(value) ||*/
                a.User.Name.Contains(value) ||
                a.PointsReceived.ToString().Contains(value)
                );
                
            }

            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";
                switch (sortBy)
                {
                    case "ActivityName":
                        if (sortAscending)
                            query = query.OrderBy(o => o.Course.Title);
                        else
                            query = query.OrderByDescending(o => o.Course.Title);
                        break;
                    case "PointsReceived":
                        if (sortAscending)
                            query = query.OrderBy(o => o.PointsReceived);
                        else
                            query = query.OrderByDescending(o => o.PointsReceived);
                        break;
                    case "RewardType":
                        if (sortAscending)
                            query = query.OrderBy(o => o.RewardType);
                        else
                            query = query.OrderByDescending(o => o.RewardType);
                        break;
                    case "DateReceived":
                        if (sortAscending)
                            query = query.OrderBy(o => o.DateReceived);
                        else
                            query = query.OrderByDescending(o => o.DateReceived);
                        break;
                    case "UserName":
                        if (sortAscending)
                            query = query.OrderBy(o => o.User.Name);
                        else
                            query = query.OrderByDescending(o => o.User.Name);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.Id);
            }

            var model = query.Skip(request.start).Take(request.length)
                .Select(s => new DetailUserRewardPointsModel
                {
                    Id = s.Id,
                    CourseId = s.CourseId.Value,
                    CourseName = (s.CourseId.HasValue) ? s.Course.Title : null,
                    PointsReceived = s.PointsReceived,
                    UserId = s.UserId,
                    UserName = s.User.Name,
                    RewardType = s.RewardType,
                    RewardedBy = s.RewardedBy.Value,
                    RewardedByName = (s.RewardedBy.HasValue) ? s.RewardSender.Name : null,
                    DateReceived = s.DateReceived
                }).ToList();

            foreach(var item in model)
            {
                item.RewardTypeName = GetDisplayName(item.RewardType);
            }

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = model.ToArray()
            });
        }

        [HttpGet]
        // GET: api/UserRewardPoints/5
        public IHttpActionResult Get(int id)
        {
            var model = db.UserRewardPoints.Where(u => u.Display && u.Id == id)
                .Select(s => new DetailUserRewardPointsModel
                {
                    Id = s.Id,
                    CourseId = s.CourseId ?? null,
                    CourseName = (s.CourseId.HasValue)?s.Course.Title:null,
                    PointsReceived = s.PointsReceived,
                    UserId = s.UserId,
                    UserName = s.User.Name,
                    RewardType = s.RewardType,
                    RewardedBy = s.RewardedBy.Value,
                    AwardReason = s.AwardReason,
                    RewardedByName = (s.RewardedBy.HasValue)?s.RewardSender.Name:null,
                    DateReceived = s.DateReceived
                }).FirstOrDefault();

            if(model == null) { return NotFound(); }

            model.RewardTypeName = GetDisplayName(model.RewardType);

            return Ok(model);
        }

        [HttpPost]
        // POST: api/UserRewardPoints
        public IHttpActionResult Post(CreateUserRewardPointsModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userRewardPoints = new UserRewardPoints
            {
                CourseId = model.CourseId.Value,
                PointsReceived = model.PointsReceived,
                UserId = model.UserId,
                RewardType = RewardType.ActivityReward,
                DateReceived = DateTime.Now, //model.DateReceived,
                Display = true
            };
            db.UserRewardPoints.Add(userRewardPoints);
            db.SaveChanges();
            return Ok(userRewardPoints);
        }

        [Route("api/Reward/UserRewardPoints/UserGetPointByCourseCompleted")]
        [HttpPost]
        // POST: api/UserRewardPoints
        public IHttpActionResult UserGetPointByCourseCompleted(UserGetPointByCourseCompleted model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var getUser = db.User.Find(model.UserId);
            if(getUser == null) { return BadRequest("User not exist"); }

            var getActivityPoint = db.RewardActivityPoint.Where(a => a.CourseId == model.CourseId && a.Display == true).FirstOrDefault();
            if(getActivityPoint == null) { return BadRequest("Course not exist"); }

            //check if already received
            var getUserReward = db.UserRewardPoints.Where(r => r.UserId == model.UserId && r.CourseId == model.CourseId).FirstOrDefault();
            if(getUserReward != null)
            {
                return BadRequest("This user already received points for this course");
            }

            var userRewardPoints = new UserRewardPoints
            {
                CourseId = getActivityPoint.CourseId,
                PointsReceived = getActivityPoint.Value,
                UserId = model.UserId,
                RewardType = RewardType.ActivityReward,
                DateReceived = DateTime.Now, //model.DateReceived,
                Display = true
            };
            db.UserRewardPoints.Add(userRewardPoints);
            db.SaveChanges();

            var tempObj = new
            {
                Course_Name = getActivityPoint.Course.Title,
                Course_Id = model.CourseId,
                Points_Received = userRewardPoints.PointsReceived,
                Date_Received = userRewardPoints.DateReceived,
                User_Name = userRewardPoints.User.Name,
                User_Id = model.UserId
            };

            return Ok(tempObj);
        }

        [Route("api/Reward/UserRewardPoints/AwardPoints")]
        [HttpPost]
        public IHttpActionResult AwardPoints(CreateUserRewardPointsModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userRewardPoints = new UserRewardPoints
            {
                PointsReceived = model.PointsReceived,
                UserId = model.UserId,
                AwardReason = model.AwardReason,
                RewardType = RewardType.AdminReward,
                RewardedBy = model.RewardedBy,
                DateReceived = DateTime.Now, //model.DateReceived,
                Display = true
            };
            
            db.UserRewardPoints.Add(userRewardPoints);
            db.SaveChanges();
            return Ok(userRewardPoints);
        }

        // PUT: api/UserRewardPoints/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserRewardPoints/5
        public IHttpActionResult Delete(int id)
        {
            UserRewardPoints model = db.UserRewardPoints.Find(id);
            if(model == null) { return BadRequest(); }

            model.Display = false;
            db.UserRewardPoints.Attach(model);
            db.Entry(model).Property(x => x.Display).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();
        }
    }
}
