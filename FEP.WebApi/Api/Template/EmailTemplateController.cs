using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Template
{
    [Route("api/Template/Email")]
    public class EmailTemplateController : ApiController
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

        // GET: api/EmailTemplate
        [Route("api/Template/Email/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterEmailTemplateModel request)
        {
            var query = db.EmailTemplates.Where(t => t.Display);
            var totalCount = query.Count();

            //advanceSearch
            query = query.Where(s => 
            (request.TemplateName == null || s.TemplateName.Contains(request.TemplateName))
            );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => 
                p.TemplateName.Contains(value)
                );
            }
            var filteredCount = query.Count();

            //order
            if (request.order != null)
            {
                string sortBy = request.columns[request.order[0].column].data;
                bool sortAscending = request.order[0].dir.ToLower() == "asc";

                switch (sortBy)
                {
                    case "TemplateName":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.TemplateName);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.TemplateName);
                        }

                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.TemplateName);
            }

            var emailTemplates = query.Skip(request.start).Take(request.length)
                .Select(s => new EmailTemplateModel
                {
                    Id = s.Id,
                    TemplateName = s.TemplateName,
                    CreatedDate = s.CreatedDate,
                    LastModified = s.LastModified,
                    CreatedBy = s.CreatedBy
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = emailTemplates.ToArray()
            });
        }

        [HttpGet]
        // GET: api/EmailTemplate/5
        public IHttpActionResult Get(int id)
        {
            var emailTemplate = db.EmailTemplates.Where(t => t.Display && t.Id == id).Select(s => new EmailTemplateModel
            {
                Id = s.Id,
                TemplateName = s.TemplateName,
                TemplateMessage = s.TemplateMessage,
                CreatedDate = s.CreatedDate,
                LastModified = s.LastModified,
                CreatedBy = s.CreatedBy
            }).FirstOrDefault();

            if(emailTemplate == null)
            {
                return NotFound();
            }

            return Ok(emailTemplate);
        }

        [HttpPost]
        // POST: api/EmailTemplate
        public IHttpActionResult Post(CreateEmailTemplateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var EmailTemplate = new EmailTemplate
            {
                TemplateName = model.TemplateName,
                TemplateMessage = model.TemplateMessage,
                CreatedDate = DateTime.Now,
                CreatedBy = model.CreatedBy,
                Display = true
            };

            db.EmailTemplates.Add(EmailTemplate);
            db.SaveChanges();

            return Ok(EmailTemplate);
        }

        // PUT: api/EmailTemplate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EmailTemplate/5
        public void Delete(int id)
        {
        }
    }
}
