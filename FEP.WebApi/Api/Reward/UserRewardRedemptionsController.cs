using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Reward;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Reward
{
    [Route("api/Reward/UserRewardRedemptions")]
    public class UserRewardRedemptionsController : ApiController
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

        [Route("api/Reward/UserRewardRedemptions/GetUserRewardRedemptionList")]
        [HttpPost]
        public IHttpActionResult Post(int id, FilterUserRewardRedemptionModel request)
        {
            //get user
            IQueryable<UserRewardRedemption> query;
            var user = db.User.Find(id);
            if (user == null)
            {
                query = db.UserRewardRedemption;
            }
            else
            {
                query = db.UserRewardRedemption.Where(r => r.UserId == id);
            }

            var totalCount = query.Count();

            //advance search
            query = query.Where(s =>
            (request.UserName == null || s.User.Name.Contains(request.UserName))
            && (request.RewardDescription == null || s.RewardRedemption.Description.Contains(request.RewardDescription))
            && (request.RewardStatus == null || s.RewardStatus == request.RewardStatus)
            && (request.RedeemDate == null || s.RedeemDate == request.RedeemDate)
            );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();
                query = query.Where(a =>
                a.User.Name.Contains(value) ||
                a.RewardRedemption.Description.Contains(value)
                );
            }

            var filteredCount = query.Count();

            //order
            if(request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";
                switch(sortBy)
                {
                    case "UserName":
                        if (sortAscending)
                            query = query.OrderBy(o => o.User.Name);
                        else
                            query = query.OrderByDescending(o => o.User.Name);
                        break;
                    case "RewardDescription":
                        if (sortAscending)
                            query = query.OrderBy(o => o.RewardRedemption.Description);
                        else
                            query = query.OrderByDescending(o => o.RewardRedemption.Description);
                        break;
                    case "RewardStatus":
                        if (sortAscending)
                            query = query.OrderBy(o => o.RewardStatus);
                        else
                            query = query.OrderByDescending(o => o.RewardStatus);
                        break;
                    case "RedeemDate":
                        if (sortAscending)
                            query = query.OrderBy(o => o.RedeemDate);
                        else
                            query = query.OrderByDescending(o => o.RedeemDate);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.Id);
            }

            var model = query.Skip(request.start).Take(request.length)
                .Select(s => new DetailUserRewardRedemptionModel
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserName = s.User.Name,
                    RewardRedemptionId = s.RewardRedemptionId,
                    RewardDescription = s.RewardRedemption.Description,
                    RedeemDate = s.RedeemDate,
                    RewardStatus = s.RewardStatus
                }).ToList();

            foreach(var item in model)
            {
                item.RewardStatusName = GetDisplayName(item.RewardStatus);
            }

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = model.ToArray()
            });
        }

        public string GetDisplayName(Enum val)
        {
            return ((DisplayAttribute)val.GetType()
                .GetMember(Enum.GetName(typeof(RewardStatus), val).ToString())
                                .First()
                                .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name;
        }

        [HttpGet]
        // GET: api/UserRewardRedemptions/5
        public IHttpActionResult Get(int id)
        {
            var model = db.UserRewardRedemption.Where(u => u.UserId == id)
                .Select(s => new DetailUserRewardRedemptionModel
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserName = s.User.Name,
                    RewardRedemptionId = s.RewardRedemptionId,
                    RewardDescription = s.RewardRedemption.Description,
                    RedeemDate = s.RedeemDate,
                    RewardStatus = s.RewardStatus
                }).FirstOrDefault();

            if(model == null) { return NotFound(); }

            return Ok(model);
        }

        [HttpPost]
        // POST: api/UserRewardRedemptions
        public IHttpActionResult Post(CreateUserRewardRedemptionModel model)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var obj = new UserRewardRedemption
            {
                UserId = model.UserId,
                RewardRedemptionId = model.RewardRedemptionId,
                RedeemDate = DateTime.Now,
                RewardStatus = RewardStatus.Open
            };
            db.UserRewardRedemption.Add(obj);
            db.SaveChanges();
            return Ok(obj);
        }

        [Route("api/Reward/UserRewardRedemptions/UsedReward")]
        [HttpPut]
        public IHttpActionResult UsedReward(int? id)
        {
            if(id == null) { return BadRequest(); }
            UserRewardRedemption model = db.UserRewardRedemption.Find(id);
            if(model == null) { return BadRequest(); }

            model.RewardStatus = RewardStatus.Closed;
            db.UserRewardRedemption.Attach(model);
            db.Entry(model).Property(x => x.RewardStatus).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok(model.Id);
        }

        // PUT: api/UserRewardRedemptions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserRewardRedemptions/5
        public void Delete(int id)
        {
        }
    }
}
