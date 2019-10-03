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

        [Route("api/Reward/UserRewardPoints/GetUserRewardPointsList")]
        [HttpPost]
        // GET: api/UserRewardPoints
        public IHttpActionResult Post(FilterUserRewardPointsModel request)
        {
            var query = db.UserRewardPoints.Where(r => r.Display);
            var totalCount = query.Count();

            //advance search
            query = query.Where(s =>
            (request.ActivityName == null || s.Activity.Name.Contains(request.ActivityName))
            && (request.RewardType == null || s.RewardType == request.RewardType)
            && (request.UserName == null || s.User.Name.Contains(request.UserName))
            && (request.PointsReceived == null || s.PointsReceived == request.PointsReceived)
            );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(a =>
                a.Activity.Name.Contains(value) ||
                a.RewardType.GetType()
                            .GetMember(a.RewardType.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>().GetName().Contains(value) ||
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
                            query = query.OrderBy(o => o.Activity.Name);
                        else
                            query = query.OrderByDescending(o => o.Activity.Name);
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
                    ActivityId = s.ActivityId.Value,
                    ActivityName = (s.ActivityId.HasValue) ? s.Activity.Name : null,
                    PointsReceived = s.PointsReceived,
                    UserId = s.UserId,
                    RewardType = s.RewardType,
                    RewardedBy = s.RewardedBy.Value,
                    RewardedByName = (s.RewardedBy.HasValue) ? s.RewardSender.Name : null,
                    DateReceived = s.DateReceived
                }).ToList();

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
                    ActivityId = s.ActivityId.Value,
                    ActivityName = (s.ActivityId.HasValue)?s.Activity.Name:null,
                    PointsReceived = s.PointsReceived,
                    UserId = s.UserId,
                    RewardType = s.RewardType,
                    RewardedBy = s.RewardedBy.Value,
                    RewardedByName = (s.RewardedBy.HasValue)?s.RewardSender.Name:null,
                    DateReceived = s.DateReceived
                }).FirstOrDefault();

            if(model == null) { return NotFound(); }

            return Ok(model);
        }

        [HttpPost]
        // POST: api/UserRewardPoints
        public IHttpActionResult Post(CreateUserRewardPointsModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userRewardPoints = new UserRewardPoints
            {
                ActivityId = model.ActivityId.Value,
                PointsReceived = model.PointsReceived,
                UserId = model.UserId,
                RewardType = model.RewardType,
                RewardedBy = model.RewardedBy.Value,
                DateReceived = model.DateReceived
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
