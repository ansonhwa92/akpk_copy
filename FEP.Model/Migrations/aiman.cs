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
					new EventCategory { CategoryName = "Workshops",CreatedDate= DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Seminars", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Dialogues", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Conferences", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Symposium", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Convention", CreatedDate = DateTime.Now, Display = true }
					);
			}
		}


    }
}
