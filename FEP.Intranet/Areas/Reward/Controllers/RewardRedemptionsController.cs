using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Reward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.Reward.Controllers
{
    public class RewardRedemptionsController : FEPController
    {
        // GET: Reward/RewardRedemptions
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View(new ListRewardRedemptionModel { });
        }

        // GET: Reward/RewardRedemptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if(id == null) { return HttpNotFound(); }
            var response = await WepApiMethod.SendApiAsync<DetailRewardRedemptionModel>
                (HttpVerbs.Get, $"Reward/RewardRedemption?id={id}");

            if (!response.isSuccess) { return HttpNotFound(); }

            return View(response.Data);
        }

        // GET: Reward/RewardRedemptions/Create
        public ActionResult Create()
        {
            CreateRewardRedemptionModel model = new CreateRewardRedemptionModel();
            model.CreatedBy = CurrentUser.UserId;

            return View(model);
        }

        // POST: Reward/RewardRedemptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRewardRedemptionModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = CurrentUser.UserId;
                model.CreatedDate = DateTime.Now;

                var response = await WepApiMethod.SendApiAsync<CreateRewardRedemptionModel>
                    (HttpVerbs.Post, $"Reward/RewardRedemption", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Create Reward Redemption Settings");
                    TempData["SuccessMessage"] = "Reward Redemption Settings created successfully";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create  Reward Redemption Settings";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create  Reward Redemption Settings";
                return RedirectToAction("List");
            }
        }

        // GET: Reward/RewardRedemptions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return HttpNotFound(); }
            var response = await WepApiMethod.SendApiAsync<DetailRewardRedemptionModel>
                (HttpVerbs.Get, $"Reward/RewardRedemption?id={id}");
            if (!response.isSuccess) { return HttpNotFound(); }

            EditRewardRedemptionModel model = new EditRewardRedemptionModel
            {
                Id = response.Data.Id,
                Description = response.Data.Description,
                RewardCode = response.Data.RewardCode,
                PointsToRedeem = response.Data.PointsToRedeem,
                ValidDuration = response.Data.ValidDuration,
                CreatedBy = response.Data.CreatedBy
            };

            return View(model);
        }

        // POST: Reward/RewardRedemptions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditRewardRedemptionModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<EditRewardActivityPointModel>
                    (HttpVerbs.Put, $"Reward/RewardRedemption?id={model.Id}", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Update Reward Redemption Setting");
                    TempData["SuccessMessage"] = "Reward Redemption Settings updated successfully";
                    return RedirectToAction("Details", "RewardRedemptions", new { area = "Reward", @id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update Reward Redemption Setting";
                    return RedirectToAction("Details", "RewardRedemptions", new { area = "Reward", @id = model.Id });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Reward Redemption Setting";
                return RedirectToAction("Details", "RewardRedemptions", new { area = "Reward", @id = model.Id });
            }
        }

        // GET: Reward/RewardRedemptions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) { return HttpNotFound(); }
            var response = await WepApiMethod.SendApiAsync<DetailRewardRedemptionModel>
                (HttpVerbs.Get, $"Reward/RewardRedemption?id={id}");

            if (!response.isSuccess) { return HttpNotFound(); }

            DeleteRewardRedemptionModel model = new DeleteRewardRedemptionModel
            {
                Id = response.Data.Id,
                Description = response.Data.Description,
                RewardCode = response.Data.RewardCode,
                PointsToRedeem = response.Data.PointsToRedeem,
                ValidDuration = response.Data.ValidDuration,
                CreatedBy = response.Data.CreatedBy,
                CreatedByName = response.Data.CreatedByName,
                CreatedDate = response.Data.CreatedDate
            };

            return View(model);
        }

        // POST: Reward/RewardRedemptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await WepApiMethod.SendApiAsync<int>
                (HttpVerbs.Delete, $"Reward/RewardRedemption?id={id}");
            if (response.isSuccess)
            {
                await LogActivity(Modules.Setting, "Delete Activity Point");
                TempData["SuccessMessage"] = "Reward Redemption Settings successfully deleted";
                return RedirectToAction("List");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete Reward Redemption Settings";
                return RedirectToAction("List");
            }
        }
    }
}
