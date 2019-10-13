using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.Reward;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Reward.Controllers
{
    public class UserRewardPointsController : FEPController
    {
        // GET: Reward/UserRewardPoints
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> List()
        {
            if(CurrentUser.UserType != UserType.SystemAdmin)
            {
                //get user's total points
                var response = await WepApiMethod.SendApiAsync<int>
                    (HttpVerbs.Get, $"Reward/UserRewardPoints/GetUserTotalPoints?id={CurrentUser.UserId}");
                if (response.isSuccess)
                {
                    ViewBag.TotalPoints = response.Data;
                }
            }

            ListUserRewardPointsModel model = new ListUserRewardPointsModel() { };
            model.RewardTypeList = (Enum.GetValues(typeof(RewardType)).Cast<int>()
                .Select(e => new SelectListItem()
                {
                    Text = ((DisplayAttribute)
                    typeof(RewardType)
                    .GetMember(Enum.GetName(typeof(RewardType), e).ToString())
                    .First()
                    .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name,
                    //Enum.GetName(typeof(NotificationType), e),
                    Value = e.ToString()
                })).ToList();

            return View(model);
        }

        // GET: Reward/UserRewardPoints/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if(id == null) { return HttpNotFound(); }

            var response = await WepApiMethod.SendApiAsync<DetailUserRewardPointsModel>
                (HttpVerbs.Get, $"Reward/UserRewardPoints?id={id}");

            if (!response.isSuccess) { return HttpNotFound(); }

            return View(response.Data);
        }

        // GET: Reward/UserRewardPoints/Create
        public async Task<ActionResult> AwardUser()
        {
            CreateUserRewardPointsModel model = new CreateUserRewardPointsModel();
            model.RewardType = RewardType.AdminReward;
            model.RewardedBy = CurrentUser.UserId;

            var response = await WepApiMethod.SendApiAsync<List<UserModel>>
                (HttpVerbs.Get, $"Administration/User");
            if(response.isSuccess)
            {
                //List<SelectListItem> items = new List<SelectListItem>();
                model.userList = response.Data.Select(e => new SelectListItem()
                    {
                        Text = e.Name,
                        Value = e.Id.ToString()
                    }
                ).ToList();
            }

            return View(model);
        }

        // POST: Reward/UserRewardPoints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AwardUser(CreateUserRewardPointsModel model)
        {
            if (ModelState.IsValid)
            {
                CreateUserRewardPointsModel obj = new CreateUserRewardPointsModel
                {
                    UserId = model.UserId,
                    PointsReceived = model.PointsReceived,
                    RewardType = RewardType.AdminReward,
                    AwardReason = model.AwardReason,
                    RewardedBy = CurrentUser.UserId,
                    DateReceived = DateTime.Now
                };

                var response = await WepApiMethod.SendApiAsync<CreateUserRewardPointsModel>
                                (HttpVerbs.Post, $"Reward/UserRewardPoints/AwardPoints", obj);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Award Points to User");
                    TempData["SuccessMessage"] = "Points Awarded successfully";

                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create Award Points";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create Award Points";
                return RedirectToAction("List");
            }
        }

        // GET: Reward/UserRewardPoints/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reward/UserRewardPoints/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reward/UserRewardPoints/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reward/UserRewardPoints/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
