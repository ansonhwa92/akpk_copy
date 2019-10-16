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
					new EventCategory { CategoryName = "Workshops", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Seminars", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Dialogues", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Conferences", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Symposium", CreatedDate = DateTime.Now, Display = true },
					new EventCategory { CategoryName = "Convention", CreatedDate = DateTime.Now, Display = true }
					);
			}

			AddAdministrator(db, "Admin FEP", "012345678913", "fep.akpk@gmail.com", "0123456789");
			AddAdministrator(db, "Verifier R&P", "012345678913", "verifierrnp@yahoo.com", "0123456789");
			AddAdministrator(db, "Verifier Event", "012345678914", "verifierevent@yahoo.com", "0123456789");
			AddAdministrator(db, "Approver 1", "012345678915", "approverfirst@yahoo.com", "0123456789");
			AddAdministrator(db, "Approver 2", "012345678916", "approversecond@yahoo.com", "0123456789");
			AddAdministrator(db, "Approver 3", "012345678917", "approverthird@yahoo.com", "0123456789");

			AddExternalExhibitor(db, "External Exhibitor 1", "0123456789", "ExternalExhibitor1@yahoo.com", "PrimusCore Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 2", "0123456789", "ExternalExhibitor2@yahoo.com", "GrabFood Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 3", "0123456789", "ExternalExhibitor3@yahoo.com", "DH Robotic Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 4", "0123456789", "ExternalExhibitor4@yahoo.com", "DahMakan Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 5", "0123456789", "ExternalExhibitor5@yahoo.com", "SilverFern Sdn Bhd");
			AddExternalExhibitor(db, "External Exhibitor 6", "0123456789", "ExternalExhibitor6@yahoo.com", "FoodPanda Sdn Bhd");

		}

		public static void AddAdministrator(DbEntities db, string Name, string ICNo, string Email, string MobileNo)
		{
			var user = db.User.Local.Where(r => r.Email == Email).FirstOrDefault() ?? db.User.Where(r => r.Email == Email).FirstOrDefault();

			if (user == null)
			{

				var useraccount = new UserAccount
				{
					LoginId = Email,
					IsEnable = true,
					LoginAttempt = 0,
					HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
					Salt = "/ZCqmg=="
				};

				var staff = new StaffProfile
				{
					BranchId = null,
					DepartmentId = null,
					DesignationId = null
				};

				
			}
		}

		public static void AddExternalExhibitor(DbEntities db, string Name, string PhoneNo, string Email, string CompanyName)
		{
			var externalexhibitor = db.EventExternalExhibitor.Local.Where(r => r.Email == Email).FirstOrDefault() ?? db.EventExternalExhibitor.Where(r => r.Email == Email).FirstOrDefault();

			if (externalexhibitor == null)
			{
				db.EventExternalExhibitor.Add(new EventExternalExhibitor
					{
						Name = Name,
						Email = Email,
						PhoneNo = PhoneNo,
						CompanyName = CompanyName,
						Display = true,
						CreatedDate = DateTime.Now,
					});
			}
		}


	}
}
