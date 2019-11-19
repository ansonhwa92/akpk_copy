using FEP.Model;
using FEP.WebApiModel.KMC;
using System;
using System.Collections.Generic;
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

        public IHttpActionResult Get([FromUri]int id)
        {
            var kmc = db.KMCs.Where(k => k.Id == id).Select(s => new DetailsKMCModel
            {
                Id = s.Id,
                Title = s.Title,
                ThumbnailUrl = s.Thumbnail,
                CreatedBy = s.User.Name,
                CreatedDate = s.CreatedDate,                
            }).FirstOrDefault();

            return Ok(kmc);

        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CreateKMCModel model)
        {
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
                    EditorCode = model.EditorCode,
                    IsPublic = model.IsPublic,
                    IsShow = model.IsShow,                    
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                db.KMCs.Add(kmc);

                db.SaveChanges();

                return Ok(true);
            }

            return BadRequest(ModelState);

        }


    }
}
