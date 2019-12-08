using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var bg = await db.CourseCertificates.ToListAsync();
            var temp = await db.CourseCertificateTemplates.ToListAsync();

            CertificatesModel model = new CertificatesModel
            {
                Background = bg.ToList(),
                Template = temp.ToList()
            };
                
            return Ok(model);
        }

        [HttpGet]
        [Route("api/eLearning/Certificates/GetCertificate")]
        public IHttpActionResult GetCertificate(int id)
        {
            var course = db.Courses.Where(u => u.Id == id).FirstOrDefault();

            var bg =  db.CourseCertificates.ToList();
            var temp =  db.CourseCertificateTemplates.ToList();

            CertificatesModel model = new CertificatesModel
            {
                Background = bg.ToList(),
                Template = temp.ToList(),
                CourseCertificateId = course.CourseCertificateId != null ? course.CourseCertificateId.Value:0,
                CourseCertificateTemplateId = course.CourseCertificateTemplateId != null ? course.CourseCertificateTemplateId.Value : 0,
            };

            return Ok(model);
        }


        //GET BACKGROUND
        [Route("api/eLearning/Certificate/GetBackground")]
        public IHttpActionResult Get(int id)
        {
            var bg = db.CourseCertificates.Where(u => u.Id == id).FirstOrDefault();

            CreateBackgroundModel model = new CreateBackgroundModel
            {
                Id = bg.Id,
                Name = bg.Name,
                Description = bg.Description,
                FileUpload = bg.FileUpload,
                TypePageOrientation = bg.TypePageOrientation
            };

            if (model != null)
            {
                return Ok(model);
            }

            return NotFound();
        }

        //GET BACKGROUND
        [Route("api/eLearning/Certificate/GetTemplate")]
        public IHttpActionResult GetTemplate(int id)
        {
            var temp = db.CourseCertificateTemplates.Where(u => u.Id == id).FirstOrDefault();

            CreateTemplateModel model = new CreateTemplateModel
            {
                Id = temp.Id,
                Name = temp.Name,
                Description = temp.Description,
                Template = temp.Template,
                TypePageOrientation = temp.TypePageOrientation
            };

            if (model != null)
            {
                return Ok(model);
            }

            return NotFound();
        }

        [Route("api/eLearning/Certificate/Create_Background")]
        //[HttpPost]
        //[ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateBackgroundModel model)
        {

            var cert = new CourseCertificate
            {
                Name = model.Name,
                Description = model.Description,
                BackgroundImageFilename =  model.FileName,
                FileUploadId = model.FileUploadId.Value,
                TypePageOrientation = model.TypePageOrientation

            };

            db.CourseCertificates.Add(cert);
            db.SaveChanges();

            return Ok(cert.Id);

        }

        [Route("api/eLearning/Certificate/Create_Template")]
        [HttpPost]
        [ValidationActionFilter]
        public IHttpActionResult Create_Template([FromBody]CreateTemplateModel model)
        {

            var temp = new CourseCertificateTemplate
            {
                Name = model.Name,
                Description = model.Description,
                Template = model.Template,
                TypePageOrientation = model.TypePageOrientation
            };

            db.CourseCertificateTemplates.Add(temp);
            db.SaveChanges();

            return Ok(temp.Id);

        }

        [Route("api/eLearning/Certificates/Update_Background")]
        //[HttpPost]
        //[ValidationActionFilter]
        public string Update_Background([FromBody]CreateBackgroundModel model)
        {
            if (model.Id != 0)
            {
                var cert = db.CourseCertificates.Where(x => x.Id == model.Id).FirstOrDefault();

                if (cert != null)
                {
                    cert.Name = model.Name;
                    cert.Description = model.Description;
                    cert.TypePageOrientation = model.TypePageOrientation;

                    db.Entry(cert).State = EntityState.Modified;
                    db.SaveChanges();

                    return model.Description;
                }
            }
            return "";
        }

        [Route("api/eLearning/Certificates/Update_Template")]
        //[HttpPost]
        //[ValidationActionFilter]
        public string Update_Template([FromBody]CreateTemplateModel model)
        {
            if (model.Id != 0)
            {
                var temp = db.CourseCertificateTemplates.Where(x => x.Id == model.Id).FirstOrDefault();

                if (temp != null)
                {
                    temp.Name = model.Name;
                    temp.Description = model.Description;
                    temp.Template = model.Template;
                    temp.TypePageOrientation = model.TypePageOrientation;

                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();

                    return model.Description;
                }
            }
            return "";
        }

        [Route("api/eLearning/Certificates/Delete")]
        public string Delete(int id)
        {
            var cert = db.CourseCertificates.Where(p => p.Id == id).FirstOrDefault();

            if (cert != null)
            {
                string ptitle = cert.Name;

                db.CourseCertificates.Remove(cert);

                db.SaveChanges();

                return ptitle;
            }

            return "";
        }

        [Route("api/eLearning/Certificates/Delete_Template")]
        public string Delete_Template(int id)
        {
            var temp = db.CourseCertificateTemplates.Where(p => p.Id == id).FirstOrDefault();

            if (temp != null)
            {
                string ptitle = temp.Name;

                db.CourseCertificateTemplates.Remove(temp);

                db.SaveChanges();

                return ptitle;
            }

            return "";
        }
    }
}