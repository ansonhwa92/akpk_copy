using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Template;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            && (request.CreatedByName == null || s.User.Name.Contains(request.CreatedByName))
            );

            //quick search
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => 
                p.TemplateName.Contains(value)
                || p.User.Name.Contains(value)
                || p.CreatedDate.ToString().Contains(value)
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
                    case "CreatedByName":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.User.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.User.Name);
                        }

                        break;
                    case "CreatedDate":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.CreatedDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.CreatedDate);
                        }

                        break;
                    case "LastModified":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.LastModified);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.LastModified);
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
                    CreatedBy = s.CreatedBy,
                    CreatedByName = s.User.Name
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
                CreatedBy = s.CreatedBy,
                CreatedByName = s.User.Name
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

            var timeNow = DateTime.Now;
            var EmailTemplate = new EmailTemplate
            {
                TemplateName = model.TemplateName,
                TemplateMessage = model.TemplateMessage,
                CreatedDate = timeNow,
                LastModified = timeNow,
                CreatedBy = model.CreatedBy,
                Display = true
            };

            db.EmailTemplates.Add(EmailTemplate);
            db.SaveChanges();

            return Ok(EmailTemplate);
        }

        [HttpPut]
        // PUT: api/EmailTemplate/5
        public IHttpActionResult Put(int id, EditEmailTemplateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            EmailTemplate template = db.EmailTemplates.Where(t => t.Id == id).FirstOrDefault();
            template.TemplateName = model.TemplateName;
            template.TemplateMessage = model.TemplateMessage;
            template.LastModified = DateTime.Now;

            db.Entry(template).State = EntityState.Modified;
            db.Entry(template).Property(x => x.TemplateName).IsModified = true;
            db.Entry(template).Property(x => x.TemplateMessage).IsModified = true;
            db.Entry(template).Property(x => x.LastModified).IsModified = true;
            db.Entry(template).Property(x => x.CreatedBy).IsModified = false;

            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();

        }

        // DELETE: api/EmailTemplate/5
        public IHttpActionResult Delete(int id)
        {
            EmailTemplate tmp = db.EmailTemplates.Find(id);
            if(tmp == null)
            {
                return BadRequest();
            }

            tmp.Display = false;
            db.EmailTemplates.Attach(tmp);
            db.Entry(tmp).Property(x => x.Display).IsModified = true;
            db.Configuration.ValidateOnSaveEnabled = true;
            db.SaveChanges();

            return Ok();

        }
    }
}
