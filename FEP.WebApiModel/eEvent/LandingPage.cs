using FEP.WebApiModel.PublicEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.LandingPage
{
	public class BrowseEventModel 
	{
		public string Keyword { get; set; }

		public string Sorting { get; set; }

		public int LastIndex { get; set; }

		public int ItemCount { get; set; }

		public List<PublicEventModel> PublicEvents { get; set; } 
	}
}
