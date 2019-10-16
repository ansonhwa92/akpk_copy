using FEP.Model;
using FEP.WebApiModel.eLearning;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/Question")]
    public class QuestionController : ApiController
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

        [Route("api/eLearning/Question/GetAll")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll(int? id)
        {
            var entity = db.Questions.Where(x => x.CourseId == id.Value);

            if (entity == null)
                return NotFound();

            var model = new List<QuestionOnlyModel>();

            foreach(var item in entity)
            {
                var question = new QuestionOnlyModel
                {
                    Id = item.Id,
                    Name = item.Name
                };

                model.Add(question);
            }

            return Ok(model);
        }
    }
}
