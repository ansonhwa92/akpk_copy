using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel
{
	public class EventCategoryApiModel
	{
		public int Id { get; set; }
		public string CategoryName { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public bool Display { get; set; }
	}
}
