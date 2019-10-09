using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/Certificate")]
	public class CertificatesController : ApiController
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

        public IHttpActionResult Get()
        {
            var cert = db.CourseCertificates.Select(s => new CertificatesModel
            {
                Id = s.Id,
                Description = s.Description,
                FileUpload = s.FileUpload
            }).ToList();
                
            return Ok(cert);
        }


        public IHttpActionResult Get(int id)
        {
            var cert = db.CourseCertificates.Where(u => u.Id == id).Select(s => new CertificatesModel
            {
                Id = s.Id,
                Description = s.Description,
                FileUpload = s.FileUpload
            }).FirstOrDefault();

            if (cert != null)
            {
                return Ok(cert);
            }

            return NotFound();
        }

        [Route("api/eLearning/Certificate/Create_Background")]
        [HttpPost]
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateBackgroundModel model)
        {

            var cert = new CourseCertificate
            {
                Description = model.Description,
                BackgroundImageFilename =  model.FileName,
                FileUploadId = model.FileUploadId.Value
            };

            db.CourseCertificates.Add(cert);
            db.SaveChanges();

            return Ok(cert.Id);

        }


        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]CertificatesModel model)
        {

            var cert = db.CourseCertificates.Where(s => s.Id == id).FirstOrDefault();

            if (cert != null)
            {
                cert.Description = model.Description;

                db.Entry(cert).State = EntityState.Modified;
                //db.Entry(cert).Property(x => x.Display).IsModified = false;

                db.SaveChanges();

                return Ok(true);
            }
            else
            {
                return NotFound();
            }

        }


        [Route("api/eLearning/Certificates/Delete")]
        public string Delete(int id)
        {
            var cert = db.CourseCertificates.Where(p => p.Id == id).FirstOrDefault();

            if (cert != null)
            {
                string ptitle = cert.Description;

                db.CourseCertificates.Remove(cert);

                db.SaveChanges();

                return ptitle;
            }

            return "";
        }

        [Route("api/eEvent/EventCategory/IsNameExist")]
        [HttpGet]
        public IHttpActionResult IsNameExist(int? id, string name)
        {
            if (id == null)
            {
                if (db.EventCategory.Any(u => u.CategoryName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Display))
                    return Ok(true);
            }
            else
            {
                if (db.EventCategory.Any(u => u.CategoryName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && u.Id != id && u.Display))
                    return Ok(true);
            }

            return NotFound();
        }
    }
}