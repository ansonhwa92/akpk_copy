using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.eEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FEP.Intranet.Areas.eEvent.Controllers
{
    public class DutyRosterController : FEPController
    {
        // GET: eEvent/DutyRoster
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> List()
		{
			var response = await WepApiMethod.SendApiAsync<List<DutyRosterModel>>(HttpVerbs.Get, $"eEvent/DutyRoster");

			if (response.isSuccess)
				return View(response.Data);

			return View(new List<DutyRosterModel>());
		}

		// GET: eEvent/DutyRoster/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: eEvent/DutyRoster/_Create
        public async Task<ActionResult> _Create()
        {
			var model = new CreateDutyRosterModel();

			model.UserList = new SelectList(await GetUsers(), "Id", "Name");

			return View(model);
		}

		// POST: eEvent/DutyRoster/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> _Create(CreateDutyRosterModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eEvent/DutyRoster", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = Language.DutyRoster.AlertSuccessCreate;

					await LogActivity(Modules.Event, "Create Duty Roster", model);

					return RedirectToAction("List");
				}
			}

			TempData["ErrorMessage"] = Language.DutyRoster.AlertFailCreate;

			return RedirectToAction("List");

		}

		// GET: eEvent/DutyRoster/Edit/5
		public ActionResult _Edit(int id, string No, DateTime? Date, DateTime? StartTime, DateTime? EndTime )
        {
			var model = new EditDutyRosterModel
			{
				Id = id,
				No = No,
				Date = Date,
				StartTime = StartTime,
				EndTime = EndTime,
			};

			return View(model);
        }

        // POST: eEvent/DutyRoster/Edit/5
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> _Edit(EditDutyRosterModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Put, $"eEvent/DutyRoster?id={model.Id}", model);

				if (response.isSuccess)
				{
					TempData["SuccessMessage"] = Language.DutyRoster.AlertSuccessUpdate;

					await LogActivity(Modules.Event, "Update Duty Roster", model);

					return RedirectToAction("List");
				}
			}

			TempData["ErrorMessage"] = Language.DutyRoster.AlertFailUpdate;
			return RedirectToAction("List");
		}

		public ActionResult _Delete(int id, string No, DateTime? Date, DateTime? StartTime, DateTime? EndTime)
		{

			var model = new DeleteDutyRosterModel
			{
				Id = id,
				No = No,
				Date = Date,
				StartTime = StartTime,
				EndTime = EndTime,
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> _Delete(int id)
		{

			var response = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Delete, $"eEvent/DutyRoster?id={id}");

			if (response.isSuccess)
			{
				TempData["SuccessMessage"] = Language.DutyRoster.AlertSuccessDelete;

				await LogActivity(Modules.Setting, "Delete Duty Roster", new { id = id });

				return RedirectToAction("List");
			}

			TempData["ErrorMessage"] = Language.DutyRoster.AlertFailDelete;

			return RedirectToAction("List");

		}


		[NonAction]
		private async Task<IEnumerable<UserModel>> GetUsers()
		{

			var roles = Enumerable.Empty<UserModel>();

			var response = await WepApiMethod.SendApiAsync<List<UserModel>>(HttpVerbs.Get, $"Administration/User");

			if (response.isSuccess)
			{
				roles = response.Data.OrderBy(o => o.Name);
			}

			return roles;

		}


	}
}
