using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Reward;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Reward
{
    [Route("api/Reward/RewardRedemption")]
    public class RewardRedemptionController : ApiController
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

        [Route("api/Reward/RewardRedemption/GetRewardRedemptionList")]
        [HttpPost]
        // GET: api/RewardRedemption
        public IHttpActionResult Post(FilterRewardRedemptionModel request)
        {
            var query = db.RewardRedemption.Where(r => r.Display);
            var totalCount = query.Count();

            //advance search
            query = query.Where(s =>
            (request.RewardCode == null || s.RewardCode.Contains(request.RewardCode))
            && (request.PointsToRedeem == null || s.PointsToRedeem == request.PointsToRedeem)
            && (request.ValidDuration == null || s.ValidDuration == request.ValidDuration)
            && (request.CreatedByName == null || s.User.Name.Contains(request.CreatedByName))
            );
            
            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(a =>
                a.RewardCode.Contains(value) ||
                a.PointsToRedeem.ToString().Contains(value) ||
                a.ValidDuration.ToString().Contains(value) ||
                a.User.Name.Contains(value)
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
                    case "RewardCode":
                        if (sortAscending)
                            query = query.OrderBy(o => o.RewardCode);
                        else
                            query = query.OrderByDescending(o => o.RewardCode);
                        break;

                    case "PointsToRedeem":
                        if (sortAscending)
                            query = query.OrderBy(o => o.PointsToRedeem);
                        else
                            query = query.OrderByDescending(o => o.PointsToRedeem);
                        break;

                    case "ValidDuration":
                        if (sortAscending)
                            query = query.OrderBy(o => o.ValidDuration);
                        else
                            query = query.OrderByDescending(o => o.ValidDuration);
                        break;

                    case "CreatedByName":
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

            var rewardRedemption = query.Skip(request.start).Take(request.length)
                .Select(s => new DetailRewardRedemptionModel
                {
                    Id = s.Id,
                    RewardCode = s.RewardCode,
                    DiscountValue = s.DiscountValue,
                    PointsToRedeem = s.PointsToRedeem,
                    ValidDuration = s.ValidDuration,
                    CreatedBy = s.CreatedBy,
                    CreatedByName = s.User.Name,
                    CreatedDate = s.CreatedDate,
                    Description = s.Description
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = rewardRedemption.ToArray()
            });
        }

        [HttpGet]
        // GET: api/RewardRedemption/5
        public IHttpActionResult Get(int id)
        {
            var model = db.RewardRedemption.Where(r => r.Display && r.Id == id)
                .Select(s => new DetailRewardRedemptionModel
                {
                    Id = s.Id,
                    RewardCode = s.RewardCode,
                    DiscountValue = s.DiscountValue,
                    Description = s.Description,
                    PointsToRedeem = s.PointsToRedeem,
                    ValidDuration = s.ValidDuration,
                    CreatedBy = s.CreatedBy,
                    CreatedByName = s.User.Name,
                    CreatedDate = s.CreatedDate
                }).FirstOrDefault();

            if(model == null) { return NotFound(); }

            return Ok(model);
        }

        [HttpPost]
        // POST: api/RewardRedemption
        public IHttpActionResult Post(CreateRewardRedemptionModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            //check if code already exist
            var rwd = db.RewardRedemption.Where(r => r.RewardCode == model.RewardCode).FirstOrDefault();
            if(rwd != null) { return BadRequest("Reward Code already exist. Please use another code"); }

            var rewardRedemption = new RewardRedemption
            {
                RewardCode = model.RewardCode,
                Description = model.Description,
                DiscountValue = model.DiscountValue,
                PointsToRedeem = model.PointsToRedeem,
                ValidDuration = model.ValidDuration,
                CreatedBy = model.CreatedBy.Value,
                CreatedDate = DateTime.Now,
                Display = true
            };
            db.RewardRedemption.Add(rewardRedemption);
            db.SaveChanges();

            return Ok(rewardRedemption);
        }

        [HttpPut]
        // PUT: api/RewardRedemption/5
        public IHttpActionResult Put(int id, EditRewardRedemptionModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            if (id != model.Id) { return BadRequest(); }

            RewardRedemption obj = db.RewardRedemption.Where(r => r.Id == id).FirstOrDefault();
            obj.Description = model.Description;
            obj.DiscountValue = model.DiscountValue;
            obj.RewardCode = model.RewardCode;
            obj.PointsToRedeem = model.PointsToRedeem;
            obj.ValidDuration = model.ValidDuration;
            obj.CreatedBy = model.CreatedBy.Value;
            obj.CreatedDate = DateTime.Now;

            db.Entry(obj).State = EntityState.Modified;
            db.Entry(obj).Property(x => x.Description).IsModified = true;
            db.Entry(obj).Property(x => x.RewardCode).IsModified = true;
            db.Entry(obj).Property(x => x.DiscountValue).IsModified = true;
            db.Entry(obj).Property(x => x.PointsToRedeem).IsModified = true;
            db.Entry(obj).Property(x => x.ValidDuration).IsModified = true;
            db.Entry(obj).Property(x => x.CreatedBy).IsModified = true;
            db.Entry(obj).Property(x => x.CreatedDate).IsModified = true;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/RewardRedemption/5
        public IHttpActionResult Delete(int id)
        {
            RewardRedemption model = db.RewardRedemption.Find(id);
            if(model == null) { return BadRequest(); }

            model.Display = false;
            db.RewardRedemption.Attach(model);
            db.Entry(model).Property(x => x.Display).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();
        }
    }
}
