﻿using FEP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using FEP.WebApiModel.eLearning;

namespace FEP.Intranet.Areas.eLearning.Controllers
{
    public class CourseGroupController : FEPController
    {
        private string StorageRoot
        {
            get { return Path.Combine(Server.MapPath("~/Files")); }
        }

        public async Task<ActionResult> List()
        {
            var model = new List<ListCourseGroupModel>();
            var response = await WepApiMethod.SendApiAsync<List<ListCourseGroupModel>>(HttpVerbs.Get, $"eLearning/CourseGroup");

            if (response.isSuccess)
                return View(response.Data);

            return View(new List<ListCourseGroupModel>());
        }

        [HttpGet]
        public async Task<ActionResult> _Create()
        {
            var model = new CreateCourseGroupModel();

            var _eCode = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Get, $"eLearning/CourseGroup/GetCode");

            if (_eCode.isSuccess)
            {
                model.EnrollmentCode = _eCode.Data;
                model.CreatedBy = CurrentUser.UserId.Value;
                model.UpdatedBy = CurrentUser.UserId.Value;

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailGroupCode;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create(CreateCourseGroupModel model)
        {
            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eLearning/CourseGroup/IsCodeExist?code={model.EnrollmentCode}");

            if (!nameResponse.isSuccess)
            {
                TempData["ErrorMessage"] = "Group Code Not Valid";
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                DateTime _now = DateTime.Now;
                var currentUserId = CurrentUser.UserId.Value;
                var response = await WepApiMethod.SendApiAsync<int>(HttpVerbs.Post, $"eLearning/CourseGroup", model);

                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = Language.eLearning.CourseGroup.AlertSuccessCreate;
                    //LogActivity(Modules.Learning, "Create Discussion Topic", model);
                }
                else
                {
                    TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailCreate;
                }
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult _Join()
        {
            var model = new JoinCourseGroupModel();
            model.EnrollmentCode = "";
            model.UserId = CurrentUser.UserId.Value;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Join(JoinCourseGroupModel model)
        {
            var nameResponse = await WepApiMethod.SendApiAsync<bool>(HttpVerbs.Get, $"eLearning/CourseGroup/IsCodeExist?code={model.EnrollmentCode}");

            if (nameResponse.isSuccess)
            {
                if (ModelState.IsValid)
            {
                    DateTime _now = DateTime.Now;
                    var currentUserId = CurrentUser.UserId.Value;
                    var response = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Post, $"eLearning/CourseGroup/JoinGroup", model);
                    if (response.isSuccess)
                    {
                        TempData["SuccessMessage"] = Language.eLearning.CourseGroup.AlertSuccessJoin;
                        //LogActivity(Modules.Learning, "Create Discussion Topic", model);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailJoin;
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Group Code Not Valid";
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {
            var model = new EditCourseGroupModel();

            var getgroup = await WepApiMethod.SendApiAsync<EditCourseGroupModel>(HttpVerbs.Get, $"eLearning/CourseGroup?id={id}");

            if (getgroup.isSuccess)
            {
                model = getgroup.Data;

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailLoadGroup;
                return RedirectToAction("List");
            }
            //var model = new CreateCourseGroupModel();

            //var _eCode = await WepApiMethod.SendApiAsync<string>(HttpVerbs.Get, $"eLearning/CourseGroup/GetCode");

            //if (_eCode.isSuccess)
            //{
            //    model.EnrollmentCode = _eCode.Data;
            //    model.CreatedBy = CurrentUser.UserId.Value;
            //    model.UpdatedBy = CurrentUser.UserId.Value;

            //    return View(model);
            //}
            //else
            //{
            //    TempData["ErrorMessage"] = "Fail generate eCode";
            //    return RedirectToAction("List");
            //}

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> _Edit(int id)
        {
            var model = new EditCourseGroupModel();

            var getgroup = await WepApiMethod.SendApiAsync<EditCourseGroupModel>(HttpVerbs.Get, $"eLearning/CourseGroup?id={id}");

            if (getgroup.isSuccess)
            {
                model = getgroup.Data;

                model.UpdatedBy = CurrentUser.UserId.Value;

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailUpdate;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<ActionResult> _Edit(EditCourseGroupModel model)
        {
            //var model = new EditCourseGroupModel();
            if (ModelState.IsValid)
            {
                var getgroup = await WepApiMethod.SendApiAsync<EditCourseGroupModel>(HttpVerbs.Post, $"eLearning/CourseGroup/EditGroup", model);

                if (getgroup.isSuccess)
                {
                    model = getgroup.Data;
                    TempData["SuccessMessage"] = Language.eLearning.CourseGroup.AlertSuccessUpdate;

                    return RedirectToAction("Detail", new { id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailUpdate;
                    return RedirectToAction("List");
                }
            }
            TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailUpdate;
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> _UserList(int id)
        {
            var model = new List<ListGroupMemberModel>();

            var getgroup = await WepApiMethod.SendApiAsync<List<ListGroupMemberModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/GetAllLearner?id={id}");

            if (getgroup.isSuccess)
            {
                model = getgroup.Data;

                //model.UpdatedBy = CurrentUser.UserId.Value;

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "fail get group";
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public async Task<ActionResult> _CoursesList(int id)
        {
            var model = new List<ListCourseModel>();

            var getgroup = await WepApiMethod.SendApiAsync<List<ListCourseModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/GetAllCourse?id={id}");

            if (getgroup.isSuccess)
            {
                model = getgroup.Data;

                //model.UpdatedBy = CurrentUser.UserId.Value;

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailCreateCourseList;
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public async Task<ActionResult> _Subscribe(int id, int GroupId)
        {
            var model = new List<ListGroupMemberModel>();
            int CreatedById = CurrentUser.UserId.Value;
            var addtogroup = await WepApiMethod.SendApiAsync<List<ListGroupMemberModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/AddToGroup?id={id}&GroupId={GroupId}&uid={CreatedById}");

            if (addtogroup.isSuccess)
            {
                model = addtogroup.Data;

                //model.UpdatedBy = CurrentUser.UserId.Value;

                return View("_UserList", model);
            }
            else
            {
                var getgroup = await WepApiMethod.SendApiAsync<List<ListGroupMemberModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/GetAllLearner?id={GroupId}");
                if (getgroup.isSuccess)
                {
                    model = getgroup.Data;
                }

                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailAddUser;
                return View("_UserList", model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> _UnSubscribe(int id, int GroupId)
        {
            var model = new List<ListGroupMemberModel>();
            var removefromgroup = await WepApiMethod.SendApiAsync<List<ListGroupMemberModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/RemoveFromGroup?id={id}&GroupId={GroupId}");

            if (removefromgroup.isSuccess)
            {
                model = removefromgroup.Data;

                //model.UpdatedBy = CurrentUser.UserId.Value;

                return View("_UserList", model);
            }
            else
            {
                var getgroup = await WepApiMethod.SendApiAsync<List<ListGroupMemberModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/GetAllLearner?id={GroupId}");
                if (getgroup.isSuccess)
                {
                    model = getgroup.Data;
                }


                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailRemoveUser;
                return View("_UserList", model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> _AddCourseEvent(int id, int GroupId)
        {
            var model = new List<ListCourseModel>();
            int CreatedById = CurrentUser.UserId.Value;
            var addtogroup = await WepApiMethod.SendApiAsync<List<ListCourseModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/AddCourseEventToGroup?id={id}&GroupId={GroupId}&uid={CreatedById}");

            if (addtogroup.isSuccess)
            {
                model = addtogroup.Data;

                //model.UpdatedBy = CurrentUser.UserId.Value;

                return View("_CoursesList", model);
            }
            else
            {

                var getgroup = await WepApiMethod.SendApiAsync<List<ListCourseModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/GetAllCourse?id={GroupId}");

                if (getgroup.isSuccess)
                {
                    model = getgroup.Data;
                }
                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailAddCourse;
                return View("_CoursesList", model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> _RemoveCourseEvent(int id, int GroupId)
        {
            var model = new List<ListCourseModel>();
            int CreatedById = CurrentUser.UserId.Value;
            var removefromgroup = await WepApiMethod.SendApiAsync<List<ListCourseModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/RemoveCourseEventFromGroup?id={id}&GroupId={GroupId}&uid={CreatedById}");

            if (removefromgroup.isSuccess)
            {
                model = removefromgroup.Data;

                //model.UpdatedBy = CurrentUser.UserId.Value;

                return View("_CoursesList", model);
            }
            else
            {
                var getgroup = await WepApiMethod.SendApiAsync<List<ListCourseModel>>(HttpVerbs.Get, $"eLearning/CourseGroup/GetAllCourse?id={GroupId}");

                if (getgroup.isSuccess)
                {
                    model = getgroup.Data;
                }
                TempData["ErrorMessage"] = Language.eLearning.CourseGroup.AlertFailRemoveCourse;
                return View("_CoursesList", model);
            }
        }
    }
}