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

		}

		public static void AddAdministrator(DbEntities db, string Name, string ICNo, string Email, string MobileNo)
		{
			var user = db.User.Local.Where(r => r.ICNo == Email).FirstOrDefault() ?? db.User.Where(r => r.ICNo == Email).FirstOrDefault();

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

				db.User.Add(
					new User
					{
						Name = Name,
						ICNo = ICNo,
						Email = Email,
						MobileNo = MobileNo,
						UserType = UserType.Staff,
						Display = true,
						CreatedDate = DateTime.Now,
						UserAccount = useraccount,
						StaffProfile = staff
					});
			}
		}

	}
}
