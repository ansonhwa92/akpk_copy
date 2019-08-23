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
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public long FileSize { get; set; }
		public string FileDescription { get; set; }
		public FileCategory? Category { get; set; }

		public DateTime? UploadedDate { get; set; }
		public int? CreatedBy { get; set; }
		public bool Display { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual PublicEvent Event { get; set; }
	}

	public enum FileCategory
	{
		NewFile,
		CancellationFile,
		ModificationFIle
	}

	[Table("MediaFile")]
	public class MediaFile
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public long FileSize { get; set; }
		public string FileDescription { get; set; }
		public DateTime? UploadedDate { get; set; }
		public int? CreatedBy { get; set; }
		public bool Display { get; set; }

		public int? EventId { get; set; }
		[ForeignKey("EventId")]
		public virtual EventMediaInterviewRequest EventMediaInterview { get; set; }
	}

	 
}
