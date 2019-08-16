using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.Migrations
{
    public static class aiman
    {

        public static void Seed(DbEntities db)
        {
			if (!db.EventCategory.Any())
			{
				db.EventCategory.AddOrUpdate(c => c.CategoryName,
					new EventCategory { CategoryName = "Workshops", Display = true },
					new EventCategory { CategoryName = "Seminars", Display = true },
					new EventCategory { CategoryName = "Dialogues", Display = true },
					new EventCategory { CategoryName = "Conferences", Display = true },
					new EventCategory { CategoryName = "Symposium", Display = true },
					new EventCategory { CategoryName = "Convention", Display = true }
					);
			}
		}


    }
}
