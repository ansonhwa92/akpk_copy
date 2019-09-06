using FEP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eEvent
{
	public class ReturnExhibitionRoadshowRequestModel
	{
		public int Id { get; set; }
		[Display(Name = "ExRoadEventName", ResourceType = typeof(Language.Event))]
		public string EventName { get; set; }
		[Display(Name = "ExRoadOrganiser", ResourceType = typeof(Language.Event))]
		public string Organiser { get; set; }
		[Display(Name = "ExRoadLocation", ResourceType = typeof(Language.Event))]
		public string Location { get; set; }
		[Display(Name = "ExRoadStartDate", ResourceType = typeof(Language.Event))]
		public DateTime? StartDate { get; set; }
		[Display(Name = "ExRoadEndDate", ResourceType = typeof(Language.Event))]
		public DateTime? EndDate { get; set; }
		[Display(Name = "ExRoadStartTime", ResourceType = typeof(Language.Event))]
		public DateTime? StartTime { get; set; }
		[Display(Name = "ExRoadEndTime", ResourceType = typeof(Language.Event))]
		public DateTime? EndTime { get; set; }
		[Display(Name = "ExRoadParticipantRequirement", ResourceType = typeof(Language.Event))]
		public string ParticipationRequirement { get; set; }
		[Display(Name = "ExRoadExhibitionStatus", ResourceType = typeof(Language.Event))]
		public ExhibitionStatus? ExhibitionStatus { get; set; }
		[Display(Name = "ExRoadReceivedById", ResourceType = typeof(Language.Event))]
		public int? ReceivedById { get; set; }
		[Display(Name = "ExRoadReceivedByName", ResourceType = typeof(Language.Event))]
		public string ReceivedByName { get; set; }
		[Display(Name = "ExRoadReceivedDate", ResourceType = typeof(Language.Event))]
		public DateTime? ReceivedDate { get; set; }
		[Display(Name = "ExRoadReceive_Via", ResourceType = typeof(Language.Event))]
		public string Receive_Via { get; set; }

	}
}
