using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Reward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Reward
{
    [Route("api/Reward/RewardActivityPoint")]
    public class RewardActivityPointController : ApiController
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

        [Route("api/Reward/RewardActivityPoint/GetActivityList")]
        [HttpGet]
        public IHttpActionResult GetActivityList()
        {
            var model = db.ActivityDummy.ToList();
            return Ok(model);
        }

        [Route("api/Reward/RewardActivityPoint/GetActivityPointList")]
        [HttpPost]
        public IHttpActionResult Post(FilterRewardActivityPointModel request)
        {
            var query = db.RewardActivityPoint.Where(r => r.Display);
            var totalCount = query.Count();

            //advance search
            query = query.Where(s =>
            (request.ActivityName == null || s.Activity.Name.Contains(request.ActivityName))
            && (request.CreatedByName == null || s.User.Name.Contains(request.CreatedByName))
            && (request.Value == 0 || s.Value == request.Value)
            //&& (request.CreatedDate.ToString() == null || s.CreatedDate == request.CreatedDate)
            );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(a =>
                a.Activity.Name.Contains(value) ||
                a.User.Name.Contains(value) ||
                a.Value.ToString().Contains(value)// ||
                //a.CreatedDate.ToString().Contains(value)
                );
            }
            var filteredCount = query.Count();

            //order
            if(request.order != null)
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

                    case "CreatedByName":
                        if (sortAscending)
                            query = query.OrderBy(o => o.User.Name);
                        else
                            query = query.OrderByDescending(o => o.User.Name);
                        break;

                    case "Value":
                        if (sortAscending)
                            query = query.OrderBy(o => o.Value);
                        else
                            query = query.OrderByDescending(o => o.Value);
                        break;

                    case "CreatedDate":
                        if (sortAscending)
                            query = query.OrderBy(o => o.CreatedDate);
                        else
                            query = query.OrderByDescending(o => o.CreatedDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.Id);
            }

            var rewardActivityPointList = query.Skip(request.start).Take(request.length)
                .Select(s => new DetailRewardActivityPointModel
            {
                Id = s.Id,
                ActivityId = s.ActivityId,
                ActivityName = s.Activity.Name,
                Value = s.Value,
                CreatedDate = s.CreatedDate,
                CreatedBy = s.CreatedBy,
                CreatedByName = s.User.Name
            }).ToList();

            //ListRewardActivityPointModel model = new ListRewardActivityPointModel(rewardActivityPointList);

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = rewardActivityPointList.ToArray()
            });
        }

        [HttpGet]
        // GET: api/RewardActivityPoint/5
        public IHttpActionResult Get(int id)
        {
            var RewardActivityPoint = db.RewardActivityPoint.Where(a => a.Display && a.Id == id)
                .Select(s => new DetailRewardActivityPointModel
                {
                    Id = s.Id,
                    ActivityId = s.ActivityId,
                    ActivityName = s.Activity.Name,
                    Value = s.Value,
                    CreatedBy = s.CreatedBy,
                    CreatedByName = s.User.Name,
                    CreatedDate = s.CreatedDate
                }).FirstOrDefault();

            if(RewardActivityPoint == null)
            {
                return NotFound();
            }
            return Ok(RewardActivityPoint);
        }

        [HttpPost]
        // POST: api/RewardActivityPoint
        public IHttpActionResult Post(CreateRewardActivityPointModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var RewardActivityPoint = new RewardActivityPoint
            {
                ActivityId = model.ActivityId,
                Value = model.Value,
                CreatedDate = DateTime.Now,
                CreatedBy = model.CreatedBy.Value,
                Display = true
            };
            db.RewardActivityPoint.Add(RewardActivityPoint);
            db.SaveChanges();

            return Ok(RewardActivityPoint);
        }

        [HttpPut]
        // PUT: api/RewardActivityPoint/5
        public IHttpActionResult Put(int id, EditRewardActivityPointModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            if (id != model.Id) { return BadRequest(); }

            RewardActivityPoint obj = db.RewardActivityPoint.Where(a => a.Id == id).FirstOrDefault();
            obj.ActivityId = model.ActivityId;
            obj.Value = model.Value;
            obj.CreatedBy = model.CreatedBy.Value;
            obj.CreatedDate = DateTime.Now;

            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.Entry(obj).Property(x => x.ActivityId).IsModified = true;
            db.Entry(obj).Property(x => x.Value).IsModified = true;
            db.Entry(obj).Property(x => x.CreatedBy).IsModified = true;
            db.Entry(obj).Property(x => x.CreatedDate).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/RewardActivityPoint/5
        public IHttpActionResult Delete(int id)
        {
            RewardActivityPoint model = db.RewardActivityPoint.Find(id);
            if(model == null)
            {
                return BadRequest();
            }
            model.Display = false;
            db.RewardActivityPoint.Attach(model);
            db.Entry(model).Property(x => x.Display).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();
        }
    }
}
