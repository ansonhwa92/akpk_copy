using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.Migrations
{
    public static class firus
    {
        public static void Seed(DbEntities db)
        {
            //publication category
            if (!db.PublicationCategory.Any())
            {
                db.PublicationCategory.Add(new PublicationCategory { Name = "Articles" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Books" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Facts Sheet" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Journals" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Literature Reviews" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Reports" });
                db.PublicationCategory.Add(new PublicationCategory { Name = "Research Papers" });
            }
        }

    }
}
