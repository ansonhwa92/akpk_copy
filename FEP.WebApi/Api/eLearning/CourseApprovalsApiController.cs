using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/Courses")]
    public class CourseApprovalsApiController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Course creator submit for verification
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/eLearning/CourseApprovals/SubmitForVerification")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> SubmitForVerification(CourseApprovalLogModel request)
        {
            var entity = await db.Courses
                            .Include(x => x.CourseApprovalLog)
                            .FirstOrDefaultAsync(x => x.Id == request.CourseId);

            if (entity == null)
                return BadRequest();

            if (entity.Status != CourseStatus.Draft && entity.Status != CourseStatus.Amendment)
                return BadRequest();

            var user = await db.User.FindAsync(request.CreatedBy);

            if (user == null) return BadRequest();

            entity.Status = CourseStatus.Submitted;

            entity.UpdatedBy = request.CreatedBy;
            entity.UpdatedDate = DateTime.Now;

            // all course activity is log to table courseapprovallog
            entity.CourseApprovalLog.Add(
                    new CourseApprovalLog
                    {
                        CreatedByName = user.Name,
                        ActionDate = DateTime.Now,
                        Remark = "Course " + entity.Title + " submitted for verification.",
                        CourseId = entity.Id,
                        CreatedBy = request.CreatedBy,
                        ApprovalStatus = ApprovalStatus.Submitted
                    }
            );

            db.SetModified(entity);

            await db.SaveChangesAsync();

            request.Status = entity.Status;

            return Ok(request);
        }

        /// <summary>
        /// Check whether approved or not.
        /// Status          Remark
        /// --------        ------
        /// Submitted       ApprovalLevel.Verifier
        /// Verify        ApprovalLevel.Approval1
        /// FirstApproval   ApprovalLevel.Approval2
        /// SecondApproval  ApprovalLevel.Approval3
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/eLearning/CourseApprovals/SubmitForApproval")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> SubmitForApproval(CourseApprovalLogModel request)
        {
            var entity = await db.Courses
                           .Include(x => x.CourseApprovalLog)
                           .FirstOrDefaultAsync(x => x.Id == request.CourseId);

            if (entity == null)
                return BadRequest();

            if (entity.Status != CourseStatus.Submitted &&
                entity.Status != CourseStatus.Verified &&
                entity.Status != CourseStatus.FirstApproval &&
                entity.Status != CourseStatus.SecondApproval &&
                entity.Status != CourseStatus.ThirdApproval)
            {
                return BadRequest();
            }

            var user = await db.User.FindAsync(request.CreatedBy);

            if (user == null) return BadRequest();

            var approvalLog =
                    new CourseApprovalLog
                    {
                        CreatedByName = request.CreatedByName,
                        ActionDate = DateTime.Now,
                        CourseId = entity.Id,
                        CreatedBy = request.CreatedBy,
                        ApprovedByName = user.Name,
                        IsApproved = request.IsApproved,
                        Remark = request.Remark
                    };

            if (request.IsApproved)
            {
                approvalLog.ApprovalStatus = ApprovalStatus.Approved;

                // verifier approved
                if (entity.Status == CourseStatus.Submitted)
                {
                    entity.Status = CourseStatus.Verified;
                    approvalLog.ApprovalLevel = ApprovalLevel.Verifier;
                }
                else
                { // other than verifier
                    if (request.IsNextLevelRequired)
                    {
                        if (entity.Status == CourseStatus.Verified)
                        {
                            entity.Status = CourseStatus.FirstApproval;
                            approvalLog.ApprovalLevel = ApprovalLevel.Approver1;
                        }
                        else
                        if (entity.Status == CourseStatus.FirstApproval)
                        {
                            entity.Status = CourseStatus.SecondApproval;
                            approvalLog.ApprovalLevel = ApprovalLevel.Approver2;
                        }
                        else
                        if (entity.Status == CourseStatus.SecondApproval)
                        {
                            entity.Status = CourseStatus.ThirdApproval;
                            approvalLog.ApprovalLevel = ApprovalLevel.Approver3;
                        }
                    }
                    else
                    {
                        entity.Status = CourseStatus.Approved;
                        approvalLog.ApprovalStatus = ApprovalStatus.Approved;                        
                    }
                }
            }
            else // Rejected
            {
                entity.Status = CourseStatus.Amendment;
                approvalLog.ApprovalStatus = ApprovalStatus.Rejected;
            }

            entity.CourseApprovalLog.Add(approvalLog);

            db.SetModified(entity);

            await db.SaveChangesAsync();

            request.CourseTitle = entity.Title;
            request.Status = entity.Status;

            return Ok(request);
        }
    }
}