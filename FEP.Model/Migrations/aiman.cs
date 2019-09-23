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
			AddUserEvent(db, "Event Admin", "EventAdmin@fep.com", null, null, UserType.Staff);
			AddUserEvent(db, "Event Approver 1", "Approver1@fep.com", null, null, UserType.Staff);
			AddUserEvent(db, "Event Approver 2", "Approver2@fep.com", null, null, UserType.Staff);
			AddUserEvent(db, "Event Approver 3", "Approver3@fep.com", null, null, UserType.Staff);
			AddUserEvent(db, "HAROLDEAN LIM @ LIM JIN LOK", "", "690315065097", "0106598960", UserType.Staff);
			AddUserEvent(db, "AZMAN BIN HASIM", "", "69062715005", "0192333703", UserType.Staff);
			AddUserEvent(db, "NOR FAZILLAH BINTI MOHD ZIN", "", "730201 14 - 5172", "0123844738", UserType.Staff);
			AddUserEvent(db, "MOHD JUSRIZAL BIN ARIS", "", "800906 - 08 - 5115", "0129986169", UserType.Staff);
			AddUserEvent(db, "NORMAZATULAKMAR BINTI RAZALI", "", "01234567890", "0123456789", UserType.Staff);

			AddUserEvent(db, "MOHD ASRI BIN MOHD RAMLI", " ASRIE.RAMLIE66@GMAIL.COM  ", "  750606025193 ", "   0135116440  ", UserType.Individual);
			AddUserEvent(db, "NUR ELENA KHO BINTI ABDULLAH  ", " ELENAME85@YAHOO.COM ", "  590109075034    ", "  604012460247   ", UserType.Individual);
			AddUserEvent(db, "BABA BIN HJ KASSAN   ", " HAYATISADAN @GMAIL.COM ", " 591220015931 ", " 0137986484   ", UserType.Individual);
			AddUserEvent(db, "ZAURA ", "  AZAURA39@GMAIL.COM  ", "  610109075830   ", "   01113135838  ", UserType.Individual);
			AddUserEvent(db, "ANJUM BIN YAAKOB ", "  ANJUMYAAKOB123@GMAIL.COM    ", "  610212715111   ", "   0124606030   ", UserType.Individual);
			AddUserEvent(db, "LOKE WAI SEE ", "  WAISEELOKE@HOTMAIL.COM  ", "  740108145214  ", "    0162212883   ", UserType.Individual);
			AddUserEvent(db, "SAPIAH BINTI IBRAHIM ", "  SAPIAH65@YMAIL.COM  ", "  651128105880 ", " 01137585615   ", UserType.Individual);
			AddUserEvent(db, "MAZLAN BIN MOHD YUSSOF ", " MAZHANN @YMAIL.COM ", " 701227125775 ", " 0198507079   ", UserType.Individual);
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
