using FEP.Helper;
using FEP.Model;
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
    public class UserRewardRedemptionsController : FEPController
    {
        // GET: Reward/UserRewardRedemptions
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            ListUserRewardRedemptionModel model = new ListUserRewardRedemptionModel() { };
            model.RewardStatusList = (Enum.GetValues(typeof(RewardStatus)).Cast<int>()
                .Select(e => new SelectListItem()
                {
                    Text = ((DisplayAttribute)
                    typeof(RewardStatus)
                    .GetMember(Enum.GetName(typeof(RewardStatus), e).ToString())
                    .First()
                    .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Name,
                    //Enum.GetName(typeof(NotificationType), e),
                    Value = e.ToString()
                })).ToList();

            return View(model);
        }

        // GET: Reward/UserRewardRedemptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if(id == null) { return HttpNotFound(); }

            var response = await WepApiMethod.SendApiAsync<DetailUserRewardRedemptionModel>
                (HttpVerbs.Get, $"Reward/UserRewardRedemptions?id={id}");

            if (!response.isSuccess) { return HttpNotFound(); }

            return View(response.Data);
        }

        // GET: Reward/UserRewardRedemptions/Create
        public ActionResult Create(int? id)
        {
            if(id == null) { return HttpNotFound(); }

            CreateUserRewardRedemptionModel model = new CreateUserRewardRedemptionModel();
            model.RewardRedemptionId = (int)id;
            model.RewardStatus = RewardStatus.Open;
            model.RedeemDate = DateTime.Now;
            model.UserId = CurrentUser.UserId.Value;
            return View(model);
        }

        // POST: Reward/UserRewardRedemptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserRewardRedemptionModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = CurrentUser.UserId.Value;
                model.RewardStatus = RewardStatus.Open;
                model.RedeemDate = DateTime.Now;

                var response = await WepApiMethod.SendApiAsync<CreateUserRewardRedemptionModel>
                    (HttpVerbs.Post, $"Reward/UserRewardRedemptions", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "User Redeemed Reward");
                    TempData["SuccessMessage"] = "Reward Redeemed successfully";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to Redeem Reward";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Redeem Reward";
                return RedirectToAction("List");
            }
        }

        public async Task<ActionResult> UpdateUsedReward(int? id)
        {
            if(id == null) { return HttpNotFound(); }
            var response = await WepApiMethod.SendApiAsync<int>
                (HttpVerbs.Put, $"Reward/UserRewardRedemptions/UsedReward?id={id}");

            if (!response.isSuccess) { return HttpNotFound(); }

            return RedirectToAction("Details", new { id = response.Data });
        }

        // GET: Reward/UserRewardRedemptions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reward/UserRewardRedemptions/Edit/5
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

        // GET: Reward/UserRewardRedemptions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reward/UserRewardRedemptions/Delete/5
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
