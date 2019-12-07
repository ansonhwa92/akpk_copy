using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.FileDocuments;
using FEP.WebApiModel.MediaInterview;
using FEP.WebApiModel.SLAReminder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eEvent
{
	[Route("api/eEvent/MediaInterviewRequest")]
	public class MediaInterviewRequestController : ApiController
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

		[Route("api/eEvent/MediaInterviewRequest/GetMediaList")]
		[HttpPost]
		public IHttpActionResult Post(FilterMediaInterviewRequestApiModel request)
		{
			var query = db.EventMediaInterviewRequest.Where(u => u.Display);

            if(request.MediaStatus != null)
            {
                query = db.EventMediaInterviewRequest.Where(u => u.MediaStatus == request.MediaStatus);
            }
            

            if (request.RequireAction == true)
            {
                var mediaStatusList = new List<MediaStatus?>();

                if (request.UserAccess == UserAccess.EventAdministratorCCD)
                {
                    mediaStatusList.Add(MediaStatus.New);
                    mediaStatusList.Add(MediaStatus.RequireAmendment);
                }
                else if (request.UserAccess == UserAccess.VerifierExhibitionCCD)
                {
                    mediaStatusList.Add(MediaStatus.PendingVerified);
                }
                else if (request.UserAccess == UserAccess.Approver1MediaInterview)
                {
                    mediaStatusList.Add(MediaStatus.Verified);
                }
                else if (request.UserAccess == UserAccess.Approver2MediaInterview)
                {
                    mediaStatusList.Add(MediaStatus.ApprovedByApprover1);

                }
                else if (request.UserAccess == UserAccess.Approver3MediaInterview)
                {
                    mediaStatusList.Add(MediaStatus.ApprovedByApprover2);
                }

                query = query.Where(u => mediaStatusList.Contains(u.MediaStatus));
            }

            var totalCount = query.Count();

			//advance search
			query = query.Where(s => (request.MediaName == null || s.MediaName.Contains(request.MediaName)));

			//quick search 
			if (!string.IsNullOrEmpty(request.search.value))
			{
				var value = request.search.value.Trim();

				query = query.Where(p => p.MediaName.Contains(value)
				//|| p.ICNo.Contains(value)
				//|| p.Email.Contains(value)
				//|| p.MobileNo.Contains(value)
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
					case "RefNo":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.RefNo);
						}
						else
						{
							query = query.OrderByDescending(o => o.RefNo);
						}

						break;

					case "BranchId":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.Branch.Name);
						}
						else
						{
							query = query.OrderByDescending(o => o.Branch.Name);
						}

						break;

					case "MediaName":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.MediaName);
						}
						else
						{
							query = query.OrderByDescending(o => o.MediaName);
						}

						break;

					case "MediaType":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.MediaType);
						}
						else
						{
							query = query.OrderByDescending(o => o.MediaType);
						}

						break;

					case "ContactPerson":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.ContactPerson);
						}
						else
						{
							query = query.OrderByDescending(o => o.ContactPerson);
						}

						break;

					case "DateStart":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.DateStart);
						}
						else
						{
							query = query.OrderByDescending(o => o.DateStart);
						}

						break;

					case "DateEnd":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.DateEnd);
						}
						else
						{
							query = query.OrderByDescending(o => o.DateEnd);
						}

						break;

					case "MediaStatus":

						if (sortAscending)
						{
							query = query.OrderBy(o => o.MediaStatus);
						}
						else
						{
							query = query.OrderByDescending(o => o.MediaStatus);
						}

						break;

					default:
						query = query.OrderByDescending(o => o.MediaName);
						break;
				}
			}
			else
			{
				query = query.OrderByDescending(o => o.MediaName);
			}

			var data = query.Skip(request.start).Take(request.length)
				.Select(s => new MediaInterviewRequestApiModel
				{
					Id = s.Id,
					RefNo = s.RefNo,
					MediaName = s.MediaName,
					MediaType = s.MediaType,
					ContactPerson = s.ContactPerson,
					DateStart = s.DateStart,
					DateEnd = s.DateEnd,
					MediaStatus = s.MediaStatus,
					BranchId = s.BranchId,
					BranchName = s.Branch.Name,
				}).ToList();

			data.ForEach(s =>
			{
				s.MediaTypeDesc = s.MediaType.GetDisplayName();
				s.MediaStatusDesc = s.MediaStatus.GetDisplayName();
			});

			return Ok(new DataTableResponse
			{
				draw = request.draw,
				recordsTotal = totalCount,
				recordsFiltered = filteredCount,
				data = data.ToArray()
			});
		}

		public List<MediaInterviewRequestApiModel> Get()
		{
			var model = db.EventMediaInterviewRequest.Where(i => i.Display).Select(i => new MediaInterviewRequestApiModel
			{
				Id = i.Id,
				MediaName = i.MediaName,
				MediaType = i.MediaType,
				ContactNo = i.ContactNo,
				AddressStreet1 = i.AddressStreet1,
				AddressStreet2 = i.AddressStreet2,
				AddressPoscode = i.AddressPoscode,
				AddressCity = i.AddressCity,
				State = i.State,
				Email = i.Email,
				DateStart = i.DateStart,
				DateEnd = i.DateEnd,
				Time = i.Time,
				Location = i.Location,
				Language = i.Language,
				Topic = i.Topic,
				RepUserName = i.User.Name,
				RepEmail = i.User.Email,
				RepMobileNumber = i.User.MobileNo,
				ContactPerson = i.ContactPerson,
				BranchId = i.BranchId,
				BranchName = i.Branch.Name
			}).ToList();

			return model;
		}

		public MediaInterviewApprovalModel Get(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(u => u.Id == id)
				.Select(i => new MediaInterviewRequestApiModel
				{
					Id = i.Id,
					MediaName = i.MediaName,
					MediaType = i.MediaType,
					ContactNo = i.ContactNo,
					AddressStreet1 = i.AddressStreet1,
					AddressStreet2 = i.AddressStreet2,
					AddressPoscode = i.AddressPoscode,
					AddressCity = i.AddressCity,
					State = i.State,
					Email = i.Email,
					DateStart = i.DateStart,
					DateEnd = i.DateEnd,
					Time = i.Time,
					Location = i.Location,
					Language = i.Language,
					Topic = i.Topic,
					RepUserName = i.User.Name,
					RepEmail = i.User.Email,
					RepMobileNumber = i.User.MobileNo,
					RepDesignation = i.User.StaffProfile.Designation.Name,
					ContactPerson = i.ContactPerson,
					CreatedBy = i.CreatedBy,
					CreatedDate = i.CreatedDate,
					RefNo = i.RefNo,
					MediaStatus = i.MediaStatus,
					CreatedByName = i.CreatedByUser.Name,
					BranchId = i.BranchId,
					BranchName = i.Branch.Name
				}).FirstOrDefault();

			if (media.MediaStatus != MediaStatus.ApprovedByApprover3 && media.MediaStatus != MediaStatus.RequireAmendment && media.MediaStatus != MediaStatus.RepAvailable && media.MediaStatus != MediaStatus.RepNotAvailable)
			{
				var approval = db.EventMediaInterviewApproval.Where(pa => pa.MediaId == id && pa.Status == EventApprovalStatus.None).Select(s => new ApprovalModel
				{
					Id = s.Id,
					MediaId = s.MediaId,
					Level = s.Level,
					ApproverId = 0,
					Status = EventApprovalStatus.None,
					Remarks = "",
					RequireNext = s.RequireNext
				}).FirstOrDefault();

				var evaluation = new MediaInterviewApprovalModel
				{
					mediainterview = media,
					approval = approval
				};

				evaluation.mediainterview.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.MediaInterview && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
				evaluation.mediainterview.RepUserId = db.MediaRepresentative.Where(r => r.MediaId == id).Select(s => s.UserId).ToArray();

				return evaluation;
			}
			else
			{
				var approval = db.EventMediaInterviewApproval.Where(pa => pa.MediaId == id).Select(s => new ApprovalModel
				{
					Id = s.Id,
					MediaId = s.MediaId,
					Level = s.Level,
					ApproverId = 0,
					Status = s.Status,
					Remarks = "",
					RequireNext = s.RequireNext
				}).FirstOrDefault();

				var evaluation = new MediaInterviewApprovalModel
				{
					mediainterview = media,
					approval = approval
				};

				evaluation.mediainterview.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.MediaInterview && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
				evaluation.mediainterview.RepUserId = db.MediaRepresentative.Where(r => r.MediaId == id).Select(s => s.UserId).ToArray();

				return evaluation;
			}
		}


		[HttpPost]
		public IHttpActionResult Post([FromBody] CreateMediaInterviewRequestApiModel model)
		{
			var media = new EventMediaInterviewRequest
			{
				MediaName = model.MediaName,
				MediaType = model.MediaType,
				ContactPerson = model.ContactPerson,
				ContactNo = model.ContactNo,
				AddressStreet1 = model.AddressStreet1,
				AddressStreet2 = model.AddressStreet2,
				AddressPoscode = model.AddressPoscode,
				AddressCity = model.AddressCity,
				State = model.State,
				Email = model.Email,
				DateStart = model.DateStart,
				DateEnd = model.DateEnd,
				Time = model.Time,
				Language = model.Language,
				Topic = model.Topic,
				CreatedBy = model.CreatedBy,
				CreatedDate = DateTime.Now,
				Display = true,
				MediaStatus = MediaStatus.New,
				BranchId = model.BranchId,
			};

			db.EventMediaInterviewRequest.Add(media);
			db.SaveChanges();

			foreach (var repid in model.RepUserId)
			{
				var mediarep = new MediaRepresentative
				{
					UserId = repid,
					MediaInterview = media,
				};

				db.MediaRepresentative.Add(mediarep);
			}

			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.MediaInterview,
					FileId = fileid,
					ParentId = media.Id
				};

				db.EventFile.Add(eventfile);
			}
			db.SaveChanges();

			if (media != null)
			{
				var approval = new EventMediaInterviewApproval
				{
					MediaId = media.Id,
					Level = EventApprovalLevel.Verifier,
					ApproverId = 0,
					Status = EventApprovalStatus.None,
					ApprovedDate = DateTime.Now,
					Remark = "",
					RequireNext = false
				};

				db.EventMediaInterviewApproval.Add(approval);
			}

			var refno = "EVT/" + DateTime.Now.ToString("yyMM");
			refno += "/" + media.Id.ToString("D4");
			media.RefNo = refno;

			db.Entry(media).State = EntityState.Modified;
			db.SaveChanges();

			return Ok(media.Id);
		}

		//Edit
		public IHttpActionResult Put(int id, [FromBody] EditMediaInterviewRequestApiModel model)
		{
			var media = db.EventMediaInterviewRequest.Where(u => u.Id == id).FirstOrDefault();

			if (media == null)
			{
				return NotFound();
			}

			media.MediaName = model.MediaName;
			media.MediaType = model.MediaType;
			media.ContactPerson = model.ContactPerson;
			media.ContactNo = model.ContactNo;
			media.AddressStreet1 = model.AddressStreet1;
			media.AddressStreet2 = model.AddressStreet2;
			media.AddressPoscode = model.AddressPoscode;
			media.AddressCity = model.AddressCity;
			media.State = model.State;
			media.Email = model.Email;
			media.DateStart = model.DateStart;
			media.DateEnd = model.DateEnd;
			media.Time = model.Time;
			media.Language = model.Language;
			media.Topic = model.Topic;
			media.MediaStatus = model.MediaStatus;
			media.RefNo = model.RefNo;
			media.BranchId = model.BranchId;

			db.Entry(media).State = EntityState.Modified;
			db.Entry(media).Property(x => x.CreatedDate).IsModified = false;
			db.Entry(media).Property(x => x.Display).IsModified = false;

			db.MediaRepresentative.RemoveRange(db.MediaRepresentative.Where(u => u.MediaId == id));//remove all
			foreach (var repid in model.RepUserId)
			{
				var mediarep = new MediaRepresentative
				{
					UserId = repid,
					MediaInterview = media,
				};

				db.MediaRepresentative.Add(mediarep);
			}

			//remove file 
			var attachments = db.EventFile.Where(s => s.FileCategory == EventFileCategory.MediaInterview && s.ParentId == model.Id).ToList();

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

						db.EventFile.Remove(attachment);
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

							db.EventFile.Remove(attachment);
						}
					}
				}
			}

			//add new file
			//files
			foreach (var fileid in model.FilesId)
			{
				var eventfile = new EventFile
				{
					FileCategory = EventFileCategory.MediaInterview,
					FileId = fileid,
					ParentId = media.Id
				};

				db.EventFile.Add(eventfile);
			}

			db.Configuration.ValidateOnSaveEnabled = true;
			db.SaveChanges();
			return Ok(true);
		}

		//Delete
		public IHttpActionResult Delete(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(u => u.Id == id).FirstOrDefault();

			if (media == null)
			{
				return NotFound();
			}
			media.Display = false;
			db.Entry(media).State = EntityState.Modified;
			db.SaveChanges();
			return Ok(true);
		}


		//Submit Public Event for Verification
		[Route("api/eEvent/MediaInterviewRequest/SubmitToVerify")]
		public IHttpActionResult SubmitToVerify(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media.MediaStatus == MediaStatus.RequireAmendment)
			{
				var approval = new EventMediaInterviewApproval
				{
					MediaId = media.Id,
					Level = EventApprovalLevel.Verifier,
					ApproverId = 0,
					Status = EventApprovalStatus.None,
					ApprovedDate = DateTime.Now,
					Remark = "",
					RequireNext = false
				};

				db.EventMediaInterviewApproval.Add(approval);
			}
			db.SaveChanges();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.PendingVerified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/MediaInterviewRequest/Verified")]
		public IHttpActionResult Verified(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();
			if (media != null)
			{
				media.MediaStatus = MediaStatus.Verified;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}


		[Route("api/eEvent/MediaInterviewRequest/RejectVerified")]
		public IHttpActionResult RejectVerified(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.RequireAmendment;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		//First Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/FirstApproved")]
		public IHttpActionResult FirstApproved(int id)
		{

			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover1;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		//Second Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/SecondApproved")]
		public IHttpActionResult SecondApproved(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover2;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		//Final Approved Public Event 
		[Route("api/eEvent/MediaInterviewRequest/FinalApproved")]
		public IHttpActionResult FinalApproved(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.ApprovedByApprover3;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		[Route("api/eEvent/MediaInterviewRequest/RepAvailable")]
		public IHttpActionResult RepAvailable(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.RepAvailable;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}

		//[Route("api/eEvent/MediaInterviewRequest/RepAvailable")]
		//public bool RepAvailable(int id)
		//{
		//	var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

		//	if (media != null)
		//	{
		//		ParameterListToSend paramToSend = new ParameterListToSend();
		//		paramToSend.EventCode = media.RefNo;
		//		paramToSend.EventName = media.MediaName;
		//		paramToSend.EventApproval = media.MediaStatus.GetDisplayName();
		//		paramToSend.EventLocation = "";
		//		paramToSend.EventMediaEmail = media.Email;

		//		var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType.RepAvailable_MediaInterview).FirstOrDefault();
		//		string Subject = generateBodyMessage("Media Interview - Representative Available", NotificationType.RepAvailable_MediaInterview, paramToSend);
		//		string Body = generateBodyMessage(template.TemplateMessage, NotificationType.RepAvailable_MediaInterview, paramToSend);

		//		var sendresult = SendBulkEmail(NotificationType.Submit_Survey_Response, NotificationCategory.ResearchAndPublication, Email, paramToSend, Subject, Body);

		//	}
		//	return true;
		//}

		//[NonAction]
		//public string generateBodyMessage(string TemplateText, NotificationType NotificationType, ParameterListToSend paramToSend)
		//{
		//	var ParamList = db.TemplateParameters.Where(p => p.NotificationType == NotificationType).ToList();
		//	string WholeText = TemplateText;
		//	foreach (var item in ParamList)
		//	{
		//		string theValue = GetPropertyValues(paramToSend, item.TemplateParameterType);
		//		string textToReplace = "[#" + item.TemplateParameterType + "]";
		//		WholeText = WholeText.Replace(textToReplace, theValue);
		//	}

		//	return WholeText;
		//}

		//[NonAction]
		//public string GetPropertyValues(Object obj, string propertyName)
		//{
		//	Type t = obj.GetType();
		//	System.Reflection.PropertyInfo[] props = t.GetProperties();
		//	string value = "";
		//	foreach (var prop in props)
		//		if (prop.Name == propertyName)
		//		{
		//			value = (prop.GetValue(obj))?.ToString();
		//			break;
		//		}
		//		else
		//			value = "";

		//	return value;
		//}

		//[NonAction]
		//public async System.Threading.Tasks.Task<IHttpActionResult> SendBulkEmail(NotificationType NotificationType, NotificationCategory NotificationCategory, string Emails, ParameterListToSend ParameterListToSend, string emailSubject, string emailBody, bool customlink = false)
		//{
		//	bool success = true;
		//	foreach (string receiverEmailAddress in Emails)
		//	{
		//		int counter = 1;
		//		if (customlink)
		//		{
		//			var template = db.NotificationTemplates.Where(t => t.NotificationType == NotificationType).FirstOrDefault();
		//			ParameterListToSend.SurveyLink = ParameterListToSend.SurveyLink.Replace("{email}", receiverEmailAddress);
		//			emailBody = generateBodyMessage(template.TemplateMessage, NotificationType, ParameterListToSend);
		//		}
		//		var response = await sendEmailUsingAPIAsync(DateTime.Now, (int)NotificationCategory, (int)NotificationType, receiverEmailAddress, emailSubject, emailBody, counter);
		//		if (response == null)
		//		{
		//			success = false;
		//		}
		//	}

		//	return Ok(success);
		//}

		[Route("api/eEvent/MediaInterviewRequest/RepNotAvailable")]
		public IHttpActionResult RepNotAvailable(int id)
		{
			var media = db.EventMediaInterviewRequest.Where(p => p.Id == id).FirstOrDefault();

			if (media != null)
			{
				media.MediaStatus = MediaStatus.RepNotAvailable;
				db.EventMediaInterviewRequest.Attach(media);
				db.Entry(media).Property(m => m.MediaStatus).IsModified = true;
				db.Configuration.ValidateOnSaveEnabled = false;
				db.SaveChanges();

				MediaInterviewRequestApiModel model = new MediaInterviewRequestApiModel
				{
					Id = media.Id,
					RefNo = media.RefNo,
					MediaStatus = media.MediaStatus,
					MediaName = media.MediaName,
					MediaStatusDesc = media.MediaStatus.GetDisplayName(),
				};
				return Ok(model);
			}
			return Ok();
		}



		[Route("api/eEvent/MediaInterviewRequest/GetSLAId")]
		public IHttpActionResult GetSLAId(int id)
		{
			var publicevent = db.EventMediaInterviewRequest.Where(p => p.Id == id).Select(i => i.SLAReminderStatusId).FirstOrDefault();

			return Ok(publicevent);
		}

		[Route("api/eEvent/MediaInterviewRequest/UpdateApproval")]
		[HttpPost]
		[ValidationActionFilter]
		public string UpdateApproval([FromBody] MediaInterviewApprovalModel model)
		{

			if (ModelState.IsValid)
			{
				var papproval = db.EventMediaInterviewApproval.Where(pa => pa.Id == model.approval.Id).FirstOrDefault();

				if (papproval != null)
				{
					papproval.ApproverId = model.approval.ApproverId;
					papproval.Status = model.approval.Status;
					papproval.ApprovedDate = DateTime.Now;
					papproval.Remark = model.approval.Remarks;
					papproval.RequireNext = model.approval.RequireNext;
					// requirenext is always set to true when coming from verifier approval, and always false from approver3

					db.Entry(papproval).State = EntityState.Modified;
					// HERE
					db.SaveChanges();

					var mediainterview = db.EventMediaInterviewRequest.Where(p => p.Id == papproval.MediaId).FirstOrDefault();
					if (mediainterview != null)
					{

						// proceed depending on requirenext
						if (model.approval.RequireNext == true)
						{

							EventApprovalLevel nextlevel;
							switch (papproval.Level)
							{
								case EventApprovalLevel.Verifier:
									nextlevel = EventApprovalLevel.Approver1;

									break;
								case EventApprovalLevel.Approver1:
									nextlevel = EventApprovalLevel.Approver2;
									break;
								case EventApprovalLevel.Approver2:
									nextlevel = EventApprovalLevel.Approver3;
									break;
								default:
									nextlevel = EventApprovalLevel.Approver1;
									break;
							}

							// create next approval record
							var pnewapproval = new EventMediaInterviewApproval
							{
								MediaId = papproval.MediaId,
								Level = nextlevel,
								ApproverId = 0,
								Status = EventApprovalStatus.None,
								ApprovedDate = DateTime.Now,
								Remark = "",
								RequireNext = false
							};

							db.EventMediaInterviewApproval.Add(pnewapproval);
							// HERE
							db.SaveChanges();
						}



						//return publication.Title;
						return mediainterview.Id + "|" + mediainterview.MediaName + "|" + mediainterview.RefNo + "|" + mediainterview.Location + "|" + mediainterview.MediaStatus;
					}
				}
			}

			return "";
		}


		[Route("api/eEvent/MediaInterviewRequest/GetHistory")]
		public List<MediaInterviewApprovalHistoryModel> GetHistory(int id)
		{
			var phistory = db.EventMediaInterviewApproval.Join(db.User, pa => pa.ApproverId, u => u.Id, (pa, u) => new { pa.MediaId, pa.Level, pa.ApproverId, pa.ApprovedDate, pa.Status, pa.Remark, UserName = u.Name })
				.Where(pa => pa.MediaId == id && pa.Status != EventApprovalStatus.None).OrderByDescending(pa => pa.ApprovedDate).Select(s => new MediaInterviewApprovalHistoryModel
			{
				Level = s.Level,
				ApproverId = s.ApproverId,
				ApprovalDate = s.ApprovedDate,
				UserName = s.UserName,
				Status = s.Status,
				Remarks = s.Remark
			}).ToList();

			return phistory;
		}


		[HttpGet]
		[Route("api/eEvent/MediaInterviewRequest/GetEditDelete")]
		public IHttpActionResult GetEditDelete(int id)
		{
			var model = db.EventMediaInterviewRequest.Where(i => i.Display && i.Id == id)
			   .Select(i => new DetailsMediaInterviewRequestApiModel
			   {
				   Id = i.Id,
				   MediaName = i.MediaName,
				   MediaType = i.MediaType,
				   ContactNo = i.ContactNo,
				   AddressStreet1 = i.AddressStreet1,
				   AddressStreet2 = i.AddressStreet2,
				   AddressPoscode = i.AddressPoscode,
				   AddressCity = i.AddressCity,
				   State = i.State,
				   Email = i.Email,
				   DateStart = i.DateStart,
				   DateEnd = i.DateEnd,
				   Time = i.Time,
				   Location = i.Location,
				   Language = i.Language,
				   Topic = i.Topic,
				   RepUserName = i.User.Name,
				   RepEmail = i.User.Email,
				   RepMobileNumber = i.User.MobileNo,
				   RepDesignation = i.User.StaffProfile.Designation.Name,
				   ContactPerson = i.ContactPerson,
				   CreatedBy = i.CreatedBy,
				   CreatedDate = i.CreatedDate,
				   RefNo = i.RefNo,
				   MediaStatus = i.MediaStatus,
				   CreatedByName = i.CreatedByUser.Name,
				   BranchId = i.BranchId,
				   BranchName = i.Branch.Name
			   }).FirstOrDefault();

			if (model == null)
			{
				return NotFound();
			}

			model.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.MediaInterview && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
			model.RepUserId = db.MediaRepresentative.Where(r => r.MediaId == id).Select(s => s.UserId).ToArray();

			return Ok(model);
		}

		[HttpGet]
		[Route("api/eEvent/MediaInterviewRequest/GetEmailOrganiser")]
		public string GetEmailOrganiser(int id)
		{
			var model = db.EventMediaInterviewRequest.Where(i => i.Display && i.Id == id)
			   .Select(i => new DetailsMediaInterviewRequestApiModel
			   {
				   Id = i.Id,
				   MediaName = i.MediaName,
				   MediaType = i.MediaType,
				   ContactNo = i.ContactNo,
				   AddressStreet1 = i.AddressStreet1,
				   AddressStreet2 = i.AddressStreet2,
				   AddressPoscode = i.AddressPoscode,
				   AddressCity = i.AddressCity,
				   State = i.State,
				   Email = i.Email,
				   DateStart = i.DateStart,
				   DateEnd = i.DateEnd,
				   Time = i.Time,
				   Location = i.Location,
				   Language = i.Language,
				   Topic = i.Topic,
				   RepUserName = i.User.Name,
				   RepEmail = i.User.Email,
				   RepMobileNumber = i.User.MobileNo,
				   RepDesignation = i.User.StaffProfile.Designation.Name,
				   ContactPerson = i.ContactPerson,
				   CreatedBy = i.CreatedBy,
				   CreatedDate = i.CreatedDate,
				   RefNo = i.RefNo,
				   MediaStatus = i.MediaStatus,
				   CreatedByName = i.CreatedByUser.Name,
				   BranchId = i.BranchId,
				   BranchName = i.Branch.Name
			   }).FirstOrDefault();
			
			model.Attachments = db.FileDocument.Where(f => f.Display).Join(db.EventFile.Where(e => e.FileCategory == EventFileCategory.MediaInterview && e.ParentId == id), s => s.Id, c => c.FileId, (s, b) => new Attachment { Id = s.Id, FileName = s.FileName }).ToList();
			model.RepUserId = db.MediaRepresentative.Where(r => r.MediaId == id).Select(s => s.UserId).ToArray();

			if (model == null)
			{
				return "";
			}

			return model.Email;
		}




	}
}

