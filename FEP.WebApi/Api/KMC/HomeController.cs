using FEP.Model;
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
    [Route("api/KMC/Home")]
    public class HomeController : ApiController
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

        //[Route("api/KMC/Home/GetCategory")]
        //public IHttpActionResult GetCategory(int? userId)
        //{
        //    if (userId == null) //public user
        //    {
        //        var categories = db.KMCCategory.Join(db.KMCs, s => (int)s.Id, s => s.KMCCategoryId, (s, e) => new { Category = s, KMCs = e }).Where(u => u.KMCs.IsPublic).Select(s => new CategoryModel
        //        {
        //            Id = s.Category.Id,
        //            Title = s.Category.Title
        //        }).ToList();

        //        return Ok(categories);
        //    }
        //    else //registered user
        //    {               
        //        var kmcs = db.KMCs.Join(db.KMCRole, s => s.Id, s => s.KMCId, (s, e) => new { KMC = s, KMCRole = e }).Join(db.UserRole.Where(u => u.UserId == userId), s => s.KMCRole.RoleId, s => s.RoleId, (s , e) => new CategoryModel 
        //        {
        //            Id = s.KMC.Category.Id,
        //            Title = s.KMC.Category.Title
        //        }).Distinct().ToList();

        //        return Ok(kmcs);
        //    }

        //}

        [Route("api/KMC/Home/GetAll")]
        [HttpPost]
        public IHttpActionResult GetKMC(int? userId, int categoryId, FilterKMCModel filter)
        {
            var query = db.KMCs.Where(k => k.KMCCategoryId == categoryId && k.IsShow
                && (filter.Title == null || k.Title.Contains(filter.Title))
                && (filter.CreatedBy == null || k.User.Name.Contains(filter.CreatedBy))
                && (filter.DateFrom == null || filter.DateTo == null || DbFunctions.TruncateTime(k.CreatedDate) <= DbFunctions.TruncateTime(filter.DateTo) && DbFunctions.TruncateTime(k.CreatedDate) >= DbFunctions.TruncateTime(filter.DateFrom))
            );

            var query1 = query.Where(k => k.IsPublic).ToList();

            var query2 = query.Where(k => !k.IsPublic).Join(db.KMCRole, s => s.Id, s => s.KMCId, (s, e) => new { KMC = s, KMCRole = e }).Join(db.UserRole.Where(u => u.UserId == userId), s => s.KMCRole.RoleId, s => s.RoleId, (s, e) => s.KMC).ToList();

            var query3 = query1.Union(query2);

            var data = query3.Select(s => new KMCModel
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
                data = data.Where(k => k.Title.Contains(filter.QuickSearch)
                        || k.CreatedBy.Contains(filter.QuickSearch));
            }

            var kmc = data.ToList();

            return Ok(kmc);

        }


    }
}
