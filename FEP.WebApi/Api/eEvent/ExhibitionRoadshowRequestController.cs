using FEP.Model;
using FEP.WebApiModel.eEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.eEvent
{
	[Route("api/eEvent/ExhibitionRoadshowRequest")]
	public class ExhibitionRoadshowRequestController : ApiController
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


		public List<ReturnExhibitionRoadshowRequestModel> Get()
		{
			var exhibitions = db.EventExhibitionRequest.Where(e => e.Display).Select(i => new ReturnExhibitionRoadshowRequestModel
			{
				Id = i.Id,
				EventName = i.EventName,
				Organiser = i.Organiser,
				Location = i.Location,
				StartDate = i.StartDate,
				EndDate = i.EndDate,
				StartTime = i.StartTime,
				EndTime = i.EndTime,
				ExhibitionStatus = i.ExhibitionStatus,
				ParticipationRequirement = i.ParticipationRequirement,
				ReceivedById = i.ReceivedById,
				ReceivedByName = i.ReceivedBy.Name,
				ReceivedDate = i.ReceivedDate,
				Receive_Via = i.Receive_Via
			}).ToList();

			return exhibitions;
		}




	}
}
