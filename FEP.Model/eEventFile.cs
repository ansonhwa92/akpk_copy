using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model
{
	[Table("EventFile")]
	public class EventFile
	{
		[Key]
		public int Id { get; set; }
		
        public EventFileCategory FileCategory { get; set; }
		public int FileId { get; set; }

		public int ParentId { get; set; }

        [ForeignKey("FileId")]
        public virtual FileDocument FileDocument { get; set; }
        
	}

	public enum EventFileCategory
	{
		PublicEvent,
        MediaInterview,
        ExhibitionRoadshow,
        EventSpeaker,
		EventAgenda,
		EventRequest
	}

}
