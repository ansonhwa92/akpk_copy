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
    public class RewardActivityPointsController : FEPController
    {
        // GET: Reward/RewardActivityPoints
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            /*
            var response = await WepApiMethod.SendApiAsync<List<DetailRewardActivityPointModel>>(HttpVerbs.Get, $"Reward/RewardActivityPoint");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            ListRewardActivityPointModel model = new ListRewardActivityPointModel(response.Data);
            */
            return View(new ListRewardActivityPointModel { });
        }

        // GET: Reward/RewardActivityPoints/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var response = await WepApiMethod.SendApiAsync<DetailRewardActivityPointModel>
                (HttpVerbs.Get, $"Reward/RewardActivityPoint?id={id}");
            if(!response.isSuccess)
            {
                return HttpNotFound();
            }
            return View(response.Data);
        }

        // GET: Reward/RewardActivityPoints/Create
        public async Task<ActionResult> Create()
        {
            CreateRewardActivityPointModel model = new CreateRewardActivityPointModel();
            var response = await WepApiMethod.SendApiAsync<List<ActivityDummyModel>>(HttpVerbs.Get, $"Reward/RewardActivityPoint/GetActivityList");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }
            model.CreatedBy = CurrentUser.UserId;
            model.ActivityDummyList = response.Data.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            return View(model);
        }

        // POST: Reward/RewardActivityPoints/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRewardActivityPointModel model)
        {
            if (ModelState.IsValid)
            {
                CreateRewardActivityPointModel obj = new CreateRewardActivityPointModel
                {
                    ActivityId = model.ActivityId,
                    Value = model.Value,
                    CreatedBy = (int)CurrentUser.UserId,
                    CreatedDate = DateTime.Now
                };

                var response = await WepApiMethod.SendApiAsync<CreateRewardActivityPointModel>
                    (HttpVerbs.Post, $"Reward/RewardActivityPoint", obj);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Create Reward Point for activity");
                    TempData["SuccessMessage"] = "Reward Point for Activity created successfully";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create Reward Point for Activity";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create Reward Point for Activity";
                return RedirectToAction("List");
            }

        }

        // GET: Reward/RewardActivityPoints/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if(id == null) { return HttpNotFound(); }
            var response = await WepApiMethod.SendApiAsync<DetailRewardActivityPointModel>
                 (HttpVerbs.Get, $"Reward/RewardActivityPoint?id={id}");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }

            EditRewardActivityPointModel model = new EditRewardActivityPointModel
            {
                Id = response.Data.Id,
                ActivityId = response.Data.ActivityId,
                ActivityName = response.Data.ActivityName,
                Value = response.Data.Value,
                CreatedBy = response.Data.CreatedBy,
                CreatedByName = response.Data.CreatedByName,
                CreatedDate = response.Data.CreatedDate
            };

            var response2 = await WepApiMethod.SendApiAsync<List<ActivityDummyModel>>(HttpVerbs.Get, $"Reward/RewardActivityPoint/GetActivityList");
            if (!response.isSuccess)
            {
                return HttpNotFound();
            }
            model.ActivityDummyList = response2.Data.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            return View(model);
        }

        // POST: Reward/RewardActivityPoints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditRewardActivityPointModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await WepApiMethod.SendApiAsync<EditRewardActivityPointModel>
                    (HttpVerbs.Put, $"Reward/RewardActivityPoint?id={model.Id}", model);

                if (response.isSuccess)
                {
                    await LogActivity(Modules.Setting, "Update Activity Points");
                    TempData["SuccessMessage"] = "Activity Points updated successfully";
                    return RedirectToAction("Details", "RewardActivityPoints", new { area = "Reward", @id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update Activity Points";
                    return RedirectToAction("Details", "RewardActivityPoints", new { area = "Reward", @id = model.Id });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Activity Points";
                return RedirectToAction("Details", "RewardActivityPoints", new { area = "Reward", @id = model.Id });
            }
        }

        // GET: Reward/RewardActivityPoints/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if(id == null) { return HttpNotFound(); }
            var response = await WepApiMethod.SendApiAsync<DetailRewardActivityPointModel>
                (HttpVerbs.Get, $"Reward/RewardActivityPoint?id={id}");

            if (!response.isSuccess) { return HttpNotFound(); }

            DeleteRewardActivityPointModel model = new DeleteRewardActivityPointModel
            {
                Id = response.Data.Id,
                ActivityId = response.Data.ActivityId,
                ActivityName = response.Data.ActivityName,
                Value = response.Data.Value,
                CreatedBy = response.Data.CreatedBy,
                CreatedByName = response.Data.CreatedByName,
                CreatedDate = response.Data.CreatedDate
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // POST: Reward/RewardActivityPoints/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await WepApiMethod.SendApiAsync<int>
                (HttpVerbs.Delete, $"Reward/RewardActivityPoint?id={id}");
            if (response.isSuccess)
            {
                await LogActivity(Modules.Setting, "Delete Activity Point");
                TempData["SuccessMessage"] = "Activity Points successfully deleted";
                return RedirectToAction("List");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete Activity Points";
                return RedirectToAction("List");
            }
        }
    }
}
