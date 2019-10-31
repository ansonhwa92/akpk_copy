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
            query = query.Where(s => (request.CourseId == null || s.CourseId == request.CourseId)
               && (request.ModuleId == null || s.ModuleId == request.ModuleId)
               && (request.Date == null || s.StartDate <= request.Date && s.EndDate >= request.Date)
               && (request.Venue == null || s.Venue.Contains(request.Venue))
               );

            //quick search 
            if (!string.IsNullOrEmpty(request.search.value))
            {
                var value = request.search.value.Trim();

                query = query.Where(p => p.Course.Title.Contains(value)
                || p.Module.Title.Contains(value)
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
                    case "Course":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Course.Title);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Course.Title);
                        }

                        break;

                    case "Module":

                        if (sortAscending)
                        {
                            query = query.OrderBy(o => o.Module.Title);
                        }
                        else
                        {
                            query = query.OrderByDescending(o => o.Module.Title);
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
                    Course = s.Course.Title,
                    Module = s.Module.Title,
                    Date = s.StartDate,
                    Venue = s.Venue
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
                    Course = s.Course.Title,
                    Module = s.Module.Title,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Venue = s.Venue,
                    NoOfMale = s.NoOfMale,
                    NoOfFemale = s.NoOfFemale,
                    AgeRange = s.AgeRange,
                    SalaryRange = s.SalaryRange
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
                    CourseId = model.CourseId,
                    ModuleId = model.ModuleId,
                    StartDate = model.StartDate.Value,
                    EndDate = model.EndDate.Value,
                    Venue = model.Venue,
                    NoOfMale = model.NoOfMale,
                    NoOfFemale = model.NoOfFemale,
                    NoOfParticipant = model.NoOfMale + model.NoOfFemale,
                    AgeRange = model.AgeRange,
                    SalaryRange = model.SalaryRange,
                    CreatedBy = model.CreatedBy
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
                
                report.CourseId = model.CourseId;
                report.ModuleId = model.ModuleId;
                report.StartDate = model.StartDate;
                report.EndDate = model.EndDate;
                report.Venue = model.Venue;
                report.NoOfMale = model.NoOfMale;
                report.NoOfFemale = model.NoOfFemale;
                report.NoOfParticipant = model.NoOfMale + model.NoOfFemale;
                report.AgeRange = model.AgeRange;
                report.SalaryRange = model.SalaryRange;
                
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

    }
}
