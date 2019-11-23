using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.eLearning;
using FEP.WebApiModel.FileDocuments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/TOTReport")]
    public class TOTReportController : ApiController
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

        [Route("api/eLearning/TOTReport/GetAll")]
        [HttpPost]
        public IHttpActionResult Post(FilterTOTReportModel request)
        {

            var query = db.TOTReport.AsEnumerable();

            var totalCount = query.Count();

            //advance search
            query = query.Where(s => (request.Module == null || s.Module.Contains(request.Module))
               && (request.Date == null || s.StartDate <= request.Date && s.EndDate >= request.Date)
               && (request.Venue == null || s.Venue.Contains(request.Venue))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.Module.Contains(value)
                || p.Venue.Contains(value)
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

                    case "Module":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Module);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Module);
                        }

                        break;

                    case "Date":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.StartDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.StartDate);
                        }

                        break;

                    case "Venue":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Venue);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Venue);
                        }

                        break;

                    case "CreatedBy":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.User.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.User.Name);
                        }

                        break;

                    default:
                        query = query.OrderByDescending(o => o.CreatedDate);
                        break;
                }

            }
            else
            {
                query = query.OrderByDescending(o => o.CreatedDate);
            }

            var data = query.Skip(request.start).Take(request.length)
                .Select(s => new TOTReportModel
                {
                    Id = s.Id,                  
                    Module = s.Module,
                    Date = s.StartDate.ToString("dd/MM/yyyy"),
                    Venue = s.Venue,
                    CreatedBy = s.User.Name + " (" + s.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") + ")"
                }).ToList();

            return Ok(new DataTableResponse
            {
                draw = request.draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = data.ToArray()
            });

        }


        public IHttpActionResult Get(int id)
        {
            var report = db.TOTReport.Where(u => u.Id == id)
                .Select(s => new DetailsTOTReportModel
                {
                    Id = s.Id, 
                    Module = s.Module,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    StartTime = s.StartDate,
                    EndTime = s.EndDate,
                    Venue = s.Venue,
                    AgeR1NoOfMale = s.AgeR1NoOfMale,
                    AgeR1NoOfFemale = s.AgeR1NoOfFemale,
                    AgeR2NoOfMale = s.AgeR2NoOfMale,
                    AgeR2NoOfFemale = s.AgeR2NoOfFemale,
                    AgeR3NoOfMale = s.AgeR3NoOfMale,
                    AgeR3NoOfFemale = s.AgeR3NoOfFemale,
                    AgeR4NoOfMale = s.AgeR4NoOfMale,
                    AgeR4NoOfFemale = s.AgeR4NoOfFemale,
                    AgeR5NoOfMale = s.AgeR5NoOfMale,
                    AgeR5NoOfFemale = s.AgeR5NoOfFemale,
                    SalaryR1NoOfMale = s.SalaryR1NoOfMale,
                    SalaryR1NoOfFemale = s.SalaryR1NoOfFemale,
                    SalaryR2NoOfMale = s.SalaryR2NoOfMale,
                    SalaryR2NoOfFemale = s.SalaryR2NoOfFemale,
                    SalaryR3NoOfMale = s.SalaryR3NoOfMale,
                    SalaryR3NoOfFemale = s.SalaryR3NoOfFemale,
                    SalaryR4NoOfMale = s.SalaryR4NoOfMale,
                    SalaryR4NoOfFemale = s.SalaryR4NoOfFemale,
                    SalaryR5NoOfMale = s.SalaryR5NoOfMale,
                    SalaryR5NoOfFemale = s.SalaryR5NoOfFemale,
                    SalaryR6NoOfMale = s.SalaryR6NoOfMale,
                    SalaryR6NoOfFemale = s.SalaryR6NoOfFemale,
                    CreatedBy = s.User.Name,
                    CreatedDate = s.CreatedDate
                })
                .FirstOrDefault();

            if (report == null)
            {
                return NotFound();
            }

            report.Attachments = db.FileDocument.Where(f => f.Display).Join(db.TOTReportFile.Where(e => e.TOTReportId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();

            return Ok(report);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CreateTOTReportModel model)
        {

            if (ModelState.IsValid)
            {

                var report = new TOTReport
                {                   
                    Module = model.Module,
                    StartDate = model.StartDate.Value,
                    EndDate = model.EndDate.Value,
                    Venue = model.Venue,                   
                    NoOfParticipant = model.NoOfMale + model.NoOfFemale,
                    AgeR1NoOfMale = model.AgeR1NoOfMale,
                    AgeR1NoOfFemale = model.AgeR1NoOfFemale,
                    AgeR2NoOfMale = model.AgeR2NoOfMale,
                    AgeR2NoOfFemale = model.AgeR2NoOfFemale,
                    AgeR3NoOfMale = model.AgeR3NoOfMale,
                    AgeR3NoOfFemale = model.AgeR3NoOfFemale,
                    AgeR4NoOfMale = model.AgeR4NoOfMale,
                    AgeR4NoOfFemale = model.AgeR4NoOfFemale,
                    AgeR5NoOfMale = model.AgeR5NoOfMale,
                    AgeR5NoOfFemale = model.AgeR5NoOfFemale,
                    SalaryR1NoOfMale = model.SalaryR1NoOfMale,
                    SalaryR1NoOfFemale = model.SalaryR1NoOfFemale,
                    SalaryR2NoOfMale = model.SalaryR2NoOfMale,
                    SalaryR2NoOfFemale = model.SalaryR2NoOfFemale,
                    SalaryR3NoOfMale = model.SalaryR3NoOfMale,
                    SalaryR3NoOfFemale = model.SalaryR3NoOfFemale,
                    SalaryR4NoOfMale = model.SalaryR4NoOfMale,
                    SalaryR4NoOfFemale = model.SalaryR4NoOfFemale,
                    SalaryR5NoOfMale = model.SalaryR5NoOfMale,
                    SalaryR5NoOfFemale = model.SalaryR5NoOfFemale,
                    SalaryR6NoOfMale = model.SalaryR6NoOfMale,
                    SalaryR6NoOfFemale = model.SalaryR6NoOfFemale,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                db.TOTReport.Add(report);

                foreach (var fileid in model.FilesId)
                {
                    var reportfile = new TOTReportFile
                    {
                        FileId = fileid,
                        Report = report
                    };
                    db.TOTReportFile.Add(reportfile);
                }

                db.SaveChanges();

                return Ok(report);
            }

            return BadRequest(ModelState);

        }

        public IHttpActionResult Put(int id, [FromBody] EditTOTReportModel model)
        {

            if (ModelState.IsValid)
            {

                var report = db.TOTReport.Where(u => u.Id == id).FirstOrDefault();

                if (report == null)
                {
                    return NotFound();
                }
               
                report.Module = model.Module;
                report.StartDate = model.StartDate;
                report.EndDate = model.EndDate;
                report.Venue = model.Venue;
                report.AgeR1NoOfMale = model.AgeR1NoOfMale;
                report.AgeR1NoOfFemale = model.AgeR1NoOfFemale;
                report.AgeR2NoOfMale = model.AgeR2NoOfMale;
                report.AgeR2NoOfFemale = model.AgeR2NoOfFemale;
                report.AgeR3NoOfMale = model.AgeR3NoOfMale;
                report.AgeR3NoOfFemale = model.AgeR3NoOfFemale;
                report.AgeR4NoOfMale = model.AgeR4NoOfMale;
                report.AgeR4NoOfFemale = model.AgeR4NoOfFemale;
                report.AgeR5NoOfMale = model.AgeR5NoOfMale;
                report.AgeR5NoOfFemale = model.AgeR5NoOfFemale;
                report.SalaryR1NoOfMale = model.SalaryR1NoOfMale;
                report.SalaryR1NoOfFemale = model.SalaryR1NoOfFemale;
                report.SalaryR2NoOfMale = model.SalaryR2NoOfMale;
                report.SalaryR2NoOfFemale = model.SalaryR2NoOfFemale;
                report.SalaryR3NoOfMale = model.SalaryR3NoOfMale;
                report.SalaryR3NoOfFemale = model.SalaryR3NoOfFemale;
                report.SalaryR4NoOfMale = model.SalaryR4NoOfMale;
                report.SalaryR4NoOfFemale = model.SalaryR4NoOfFemale;
                report.SalaryR5NoOfMale = model.SalaryR5NoOfMale;
                report.SalaryR5NoOfFemale = model.SalaryR5NoOfFemale;
                report.SalaryR6NoOfMale = model.SalaryR6NoOfMale;
                report.SalaryR6NoOfFemale = model.SalaryR6NoOfFemale;
                report.NoOfParticipant = model.NoOfMale + model.NoOfFemale;               
                
                db.Entry(report).State = EntityState.Modified;
                
                //remove file 
                var attachments = db.TOTReportFile.Where(s => s.TOTReportId == model.Id).ToList();

                if (attachments != null)
                {
                    //delete all
                    if (model.Attachments == null)
                    {
                        foreach (var attachment in attachments)
                        {
                            attachment.FileDocument.Display = false;
                            db.FileDocument.Attach(attachment.FileDocument);
                            db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;

                            db.TOTReportFile.Remove(attachment);
                        }
                    }
                    else
                    {
                        foreach (var attachment in attachments)
                        {
                            if (!model.Attachments.Any(u => u.Id == attachment.FileDocument.Id))//delete if not exist anymore
                            {
                                attachment.FileDocument.Display = false;
                                db.FileDocument.Attach(attachment.FileDocument);
                                db.Entry(attachment.FileDocument).Property(m => m.Display).IsModified = true;

                                db.TOTReportFile.Remove(attachment);
                            }
                        }
                    }
                }

                //add files
                foreach (var fileid in model.FilesId)
                {
                    var reportfile = new TOTReportFile
                    {                       
                        FileId = fileid,
                        TOTReportId =id
                    };

                    db.TOTReportFile.Add(reportfile);
                }

                db.Configuration.ValidateOnSaveEnabled = true;
                db.SaveChanges();

                return Ok(true);

            }

            return BadRequest(ModelState);

        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {

            var report = db.TOTReport.Where(u => u.Id == id).FirstOrDefault();

            if (report == null)
            {
                return NotFound();
            }

            var files = db.TOTReportFile.Where(t => t.TOTReportId == id).ToList();

            foreach(var file in files)
            {
                db.FileDocument.Remove(file.FileDocument);
                db.TOTReportFile.Remove(file);
            }

            db.TOTReport.Remove(report);
            db.SaveChanges();

            return Ok(true);
        }

      
    }
}
