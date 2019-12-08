using FEP.Model;
using FEP.WebApiModel.Administration;
using FEP.WebApiModel.FileDocuments;
using FEP.WebApiModel.KMC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.KMC
{
    [Route("api/KMC/Manage")]
    public class ManageController : ApiController
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

        [HttpGet]
        [Route("api/KMC/Manage/GetAll")]
        public IHttpActionResult GetAll([FromUri]int categoryId)
        {
            var kmc = db.KMCs.Where(k => k.KMCCategoryId == categoryId).Select(s => new KMCModel
            {
                Id = s.Id,
                Title = s.Title,
                ThumbnailUrl = s.Thumbnail,
                CreatedBy = s.User.Name,
                CreatedDate = s.CreatedDate,
                AvatarUrl = s.User.UserAccount.Avatar
            }).ToList();

            return Ok(kmc);

        }

        [HttpPost]
        [Route("api/KMC/Manage/GetAll")]
        public IHttpActionResult GetAll([FromUri]int categoryId, FilterKMCModel filter)
        {
            var query = db.KMCs.Where(k => k.KMCCategoryId == categoryId
                && (filter.Title == null || k.Title.Contains(filter.Title))
                && (filter.CreatedBy == null || k.User.Name.Contains(filter.CreatedBy))
                && (filter.DateFrom == null || filter.DateTo == null || DbFunctions.TruncateTime(k.CreatedDate) <= DbFunctions.TruncateTime(filter.DateTo) && DbFunctions.TruncateTime(k.CreatedDate) >= DbFunctions.TruncateTime(filter.DateFrom))
            ).Select(s => new KMCModel
            {
                Id = s.Id,
                Title = s.Title,
                ThumbnailUrl = s.Thumbnail,
                CreatedBy = s.User.Name,
                CreatedDate = s.CreatedDate,
                AvatarUrl = s.User.UserAccount.Avatar
            });

            if (filter.QuickSearch != null)
            {
                query = query.Where(k => k.Title.Contains(filter.QuickSearch)
                        || k.CreatedBy.Contains(filter.QuickSearch));

            }

            var kmc = query.ToList();

            return Ok(kmc);

        }

        public IHttpActionResult Get([FromUri]int id)
        {
            var kmc = db.KMCs.Where(k => k.Id == id).Select(s => new DetailsKMCModel
            {
                Id = s.Id,
                Category = new CategoryModel { Id = s.KMCCategoryId, Title = s.Category.Title },
                Title = s.Title,
                Description = s.Description,
                ThumbnailUrl = s.Thumbnail,
                Type = s.KMCType,
                IsEditor = s.IsEditor,
                IsShow = s.IsShow,
                IsPublic = s.IsPublic,
                FileId = s.FileId,
                FileName = s.FileDoc.FileName,
                FileType = s.FileType,
                EditorCode = s.EditorCode,
                CreatedBy = s.User.Name,
                CreatedDate = s.CreatedDate,
            }).FirstOrDefault();

            if (kmc == null)
            {
                return NotFound();
            }

            if (kmc.FileId != null)
            {
                kmc.File = db.FileDocument.Where(f => f.Id == kmc.FileId).Select(s => new Attachment { Id = s.Id, FileName = s.FileName }).FirstOrDefault();
            }

            kmc.Roles = db.KMCRole.Where(k => k.KMCId == id).Select(s => new RoleModel { Id = s.RoleId, Name = s.Role.Name, Description = s.Role.Description }).ToList();

            return Ok(kmc);

        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CreateKMCModel model)
        {
            if (model.IsPublic)
            {
                ModelState.Remove("model.RoleIds");
            }

            if (model.IsEditor)
            {
                ModelState.Remove("model.File");
                ModelState.Remove("model.Type");
            }
            else
            {
                ModelState.Remove("model.EditorCode");
            }

            if (ModelState.IsValid)
            {

                var kmc = new KMCs
                {
                    KMCCategoryId = model.KMCCategoryId,
                    Title = model.Title,
                    Description = model.Description,
                    Thumbnail = model.ThumbnailUrl,
                    IsEditor = model.IsEditor,
                    KMCType = model.Type,
                    FileId = model.FileId,
                    FileType = model.FileType,
                    EditorCode = model.EditorCode,
                    IsPublic = model.IsPublic,
                    IsShow = model.IsShow,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                if (!model.IsPublic && model.RoleIds != null)
                {
                    foreach (var roleid in model.RoleIds)
                    {
                        var kmcrole = new KMCRole
                        {
                            RoleId = roleid,
                            KMC = kmc,
                        };

                        db.KMCRole.Add(kmcrole);
                    }
                }

                db.KMCs.Add(kmc);

                db.SaveChanges();

                return Ok(kmc.Id);
            }

            return BadRequest(ModelState);

        }

        public IHttpActionResult Put(int id, [FromBody]EditKMCModel model)
        {
            if (model.IsPublic)
            {
                ModelState.Remove("model.RoleIds");
            }

            if (model.IsEditor)
            {
                ModelState.Remove("model.File");
                ModelState.Remove("model.Type");
            }
            else
            {
                ModelState.Remove("model.EditorCode");
            }

            var kmc = db.KMCs.Where(s => s.Id == id).FirstOrDefault();

            if (kmc != null)
            {
                if (ModelState.IsValid)
                {
                    kmc.Title = model.Title;
                    kmc.Description = model.Description;
                    kmc.Thumbnail = model.ThumbnailUrl;
                    kmc.IsEditor = model.IsEditor;
                    kmc.KMCType = model.Type;
                    kmc.FileId = model.FileId;
                    kmc.FileType = model.FileType;
                    kmc.EditorCode = model.EditorCode;
                    kmc.IsPublic = model.IsPublic;
                    kmc.IsShow = model.IsShow;

                    db.Entry(kmc).State = EntityState.Modified;
                    db.Entry(kmc).Property(x => x.KMCCategoryId).IsModified = false;

                    db.KMCRole.RemoveRange(db.KMCRole.Where(k => k.KMCId == id));//remove all

                    if (!model.IsPublic && model.RoleIds != null)
                    {
                        foreach (var roleid in model.RoleIds)
                        {
                            var kmcrole = new KMCRole
                            {
                                RoleId = roleid,
                                KMC = kmc,
                            };

                            db.KMCRole.Add(kmcrole);
                        }
                    }

                    db.SaveChanges();

                    return Ok(true);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return NotFound();
            }

        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            var kmc = db.KMCs.Where(u => u.Id == id).FirstOrDefault();

            if (kmc == null)
            {
                return NotFound();
            }

            var file = db.FileDocument.Where(f => f.Id == kmc.FileId).FirstOrDefault();

            db.KMCs.Remove(kmc);

            if (file != null)
                db.FileDocument.Remove(file);

            db.SaveChanges();

            return Ok(true);
        }


    }
}
