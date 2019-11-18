using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FEP.WebApiModel.eEvent
{
	public class DutyRosterModel
	{
		public int Id { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "DRDate", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? Date { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DRStartTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? StartTime { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DREndTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? EndTime { get; set; }
	}

	public class CreateDutyRosterModel
	{
		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "DRDate", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? Date { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DRStartTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? StartTime { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DREndTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? EndTime { get; set; }

		[Display(Name = "DROfficer", ResourceType = typeof(Language.DutyRoster))]
		public int[] UserId { get; set; }
		public IEnumerable<SelectListItem> UserList { get; set; }
	}

	public class EditDutyRosterModel
	{
		public int Id { get; set; }
		public string No { get; set; }

		[DataType(DataType.Date)]
		[UIHint("Date")]
		[Display(Name = "DRDate", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? Date { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DRStartTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? StartTime { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DREndTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? EndTime { get; set; }

		[Display(Name = "DROfficer", ResourceType = typeof(Language.DutyRoster))]
		public int[] UserId { get; set; }
		public IEnumerable<SelectListItem> UserList { get; set; }
	}

	public class DeleteDutyRosterModel
	{
		public int Id { get; set; }
		public string No { get; set; }

		[UIHint("Date")]
		[Display(Name = "DRDate", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? Date { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DRStartTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? StartTime { get; set; }

		[DataType(DataType.Time)]
		[UIHint("Time")]
		[Display(Name = "DREndTime", ResourceType = typeof(Language.DutyRoster))]
		public DateTime? EndTime { get; set; }

		public IEnumerable<SelectListItem> UserList { get; set; }
	}
}
