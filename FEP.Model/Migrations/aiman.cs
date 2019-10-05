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

			//string Name, string Email, string ICNo, string MobileNo, UserType UserType
			//AddUserEvent(db, "Event Admin", "aimannoramri@gmail.com", null, null, UserType.Staff);
			//AddUserEvent(db, "Event Approver 1", "aiman@primuscore.com", null, null, UserType.Staff);
			//AddUserEvent(db, "Event Approver 2", "aiman@primuscore.com", null, null, UserType.Staff);
			//AddUserEvent(db, "Event Approver 3", "aiman@primuscore.com", null, null, UserType.Staff);

		}

		//Administrator Event
		public static void AddUserEvent(DbEntities db, string Name, string Email, string ICNo, string MobileNo, UserType UserType)
		{
			var eventadmin = db.User.Local.Where(r => r.Name.Contains(Name)).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains(Name)).FirstOrDefault();
			if (eventadmin == null)
			{
				//var role = db.Role.Local.Where(r => r.Name.Contains("")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("")).FirstOrDefault();
				//List<UserRole> userroles = new List<UserRole>();
				//userroles.Add(new UserRole { Role = role });
				db.User.Add(
					new User
					{
						Name = Name,
						Email = Email,
						ICNo = ICNo,
						MobileNo = MobileNo,
						UserType = UserType,
						CreatedDate = DateTime.Now,
						Display = true,
						UserAccount = new UserAccount
						{
							LoginId = Email,
							HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
							Salt = "/ZCqmg==",
							IsEnable = true,
							LoginAttempt = 0,
							LastPasswordChange = DateTime.Now,
							LastLogin = DateTime.Now,
							//UserRoles = userroles
						},
					}
				);
			}

		}



	}
}
