using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FEP.Model.Migrations
{
    public static class mhafeez
    {

        public static string DisplayName(this Enum val)
        {
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }

        public static void Seed(DbEntities db)
        {
			//notification
			if (!db.NotificationSetting.Any())
			{
				foreach (NotificationType type in (NotificationType[])Enum.GetValues(typeof(NotificationType)))
				{
					db.NotificationSetting.AddOrUpdate(
						t => new { t.NotificationType },
						new NotificationSetting { NotificationType = type }
					);
				}
			}

			//default setting
			if (!db.SystemSetting.Any(m => m.Id == 0))
            {
                db.SystemSetting.Add(new SystemSetting { Id = 0, SystemTitle = "Financial Education Portal", ShortTitle = "FEP", SystemVersion = "1.0", SystemFooter = "<b>Copyright © 2017-2020</b> All rights reserved." });
            }

            if (!db.AccountSetting.Any(m => m.Id == 0))
            {
                db.AccountSetting.Add(new AccountSetting { Id = 0, IsPasswordExpiry = false, InactiveDuration = 30, IsLimitLoginAttempt = false, LoginAttemptLimit = null, PasswordExpiryDuration = null });
            }

            //default access
            DefaultAccess(db);
            
            //default role
            List<RoleAccess> roleaccess = new List<RoleAccess>();

            foreach (UserAccess useraccess in Enum.GetValues(typeof(UserAccess)))
            {
                roleaccess.Add(new RoleAccess { UserAccess = useraccess });
            }


            //role n roleaccess default for system admin
            var roleallaccess = db.Role.Local.Where(r => r.Name.Contains("All Access")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("All Access")).FirstOrDefault();

            if (roleallaccess == null)
            {
                db.Role.Add(
                    new Role
                    {
                        Name = "All Access",
                        Description = "All Access",
                        Display = true,
                        CreatedDate = DateTime.Now,
                        RoleAccess = roleaccess
                    });
            }
            
            //default user
            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            if (user == null)
            {
                                
                var role = db.Role.Local.Where(r => r.Name.Contains("All Access")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("All Access")).FirstOrDefault();

                List<UserRole> userroles = new List<UserRole>();

                userroles.Add(new UserRole { Role = role });

                db.User.Add(
                    new User
                    {
                        Name = "System Admin",
                        Email = "admin@fep.com",       
                        ICNo = "999999999999",
                        MobileNo = "88888888888",
                        UserType = UserType.SystemAdmin,                        
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "admin@fep.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,                           
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,   
                            UserRoles = userroles
                        },
                    }
                );


            }
            /*else
            {
                var role = db.Role.Local.Where(r => r.Name.Contains("All Access")).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains("All Access")).FirstOrDefault();

                List<UserRole> userroles = new List<UserRole>();

                userroles.Add(new UserRole { Role = role });
                db.User.Add(
                    new User
                    {
                        Name = "Tajul Admin",
                        Email = "tajulzaid@gmail.com",
                        UserType = UserType.SystemAdmin,
                        CreatedDate = DateTime.Now,
                        Display = true,
                        UserAccount = new UserAccount
                        {
                            LoginId = "tajulzaid@gmail.com",
                            HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==", //default abc123
                            Salt = "/ZCqmg==",
                            IsEnable = true,
                            LoginAttempt = 0,
                            LastPasswordChange = DateTime.Now,
                            LastLogin = DateTime.Now,
                            UserRoles = userroles
                        },
                    }
                );
            }*/

            AddRole(db, "Individual", "Default Individual");
            AddRole(db, "Individual with paper", "Individual with paper");
            AddRole(db, "Individual with paper to present", "Individual with paper to present");

            AddRole(db, "Agency", "Default Agency");
            AddRole(db, "Organizer", "Default Organizer");

            AddRole(db, "Trainer", "Default Trainer");
            AddRole(db, "Facilitator", "Default Facilitator");
            AddRole(db, "Speaker", "Default Speaker");

            AddRole(db, "Chief Editor", "Chief Editor");

            AddRole(db, "Staff", "Staff");

            AddRole(db, "Admin Event", "Admin Event");
            AddRole(db, "Admin R&P", "Admin R&P");
            AddRole(db, "Admin eLearning", "Admin eLearning");
            
            AddRole(db, "Event Reception", "Event Reception");
            AddRole(db, "Event Moderator", "Event Moderator");
            
            AddRole(db, "Verifier Event", "Verifier Event");
            AddRole(db, "Verifier R&P", "Verifier R&P");
            AddRole(db, "Verifier eLearning", "Verifier eLearning");
           
            AddRole(db, "Approver Event 1", "Approver Event 1");
            AddRole(db, "Approver R&P 1", "Approver R&P 1");
            AddRole(db, "Approver eLearning 1", "Approver eLearning 1");

            AddRole(db, "Approver Event 2", "Approver Event 2");
            AddRole(db, "Approver R&P 2", "Approver R&P 2");
            AddRole(db, "Approver eLearning 2", "Approver eLearning 2");

            AddRole(db, "Approver Event 3", "Approver Event 3");
            AddRole(db, "Approver R&P 3", "Approver R&P 3");
            AddRole(db, "Approver eLearning 3", "Approver eLearning 3");

			//staff
			AddStaff(db, "HAROLDEAN LIM @ LIM JIN LOK", "690315065097", "", "0106598960");
			AddStaff(db, "AZMAN BIN HASIM", "690629715005", "", "0192333703");
			AddStaff(db, "NOR FAZILLAH BINTI MOHD ZIN", "730201145172", "", "012-3844738");
			AddStaff(db, "MOHD JUSRIZAL BIN ARIS", "800906085115", "", "0129986169");
			AddStaff(db, "NORMAZATULAKMAR BINTI RAZALI", "840417145056", "", "0132666146");
			AddStaff(db, "AHMAD FIRDAUS BIN MOHAMAD ARIFF", "820521085833", "", "0192685275");
			AddStaff(db, "NUR HAYATI BINTI MAT SALLEH", "811204086360", "", "0192938100");
			AddStaff(db, "ALIAS MAT ALI", "690120115613", "", "0199657462");
			AddStaff(db, "CHEONG PUCK KIN", "710730075247", "", "0125557843");
			AddStaff(db, "SAIDI YAACOB", "720718115479", "", "0126313058");
			AddStaff(db, "GUNASEGARAN MUNUSAMY", "690704105777", "", "0163913722");
			AddStaff(db, "MOHD RIZAUDDIN NOOR", "770315036149", "", "0122888804");
			AddStaff(db, "YEO WEE KIAK", "780831135169", "", "0168697380");
			AddStaff(db, "IDRIS BIN KASIM", "620115125629", "", "0198928440");
			AddStaff(db, "EZREEN EZAIRY BIN HUSSIN", "770406055417", "", "0134283991");
			AddStaff(db, "TAN GUAT YOU", "641108085342", "", "");
			AddStaff(db, "NORASHIKIN ALI", "670109055500", "", "0122672512");
			AddStaff(db, "NOR FAZLEEN BINTI  ZAKARIA", "681105035100", "", "019 3578803");
			AddStaff(db, "RUS LINA BINTI RUS DIN", "790727055328", "", "0129045947");
			AddStaff(db, "SUZAIDI MD SHAUKAT", "651123065293", "", "60193175584");
			AddStaff(db, "MANSOR KESOT @ ALI", "670317016039", "", "0193226959");
			AddStaff(db, "S NABINARAJ A/L SHANMUGANATHAN", "830430105623", "", "0122428166");
			AddStaff(db, "JUNAINAH BINTI AHMAD ESA", "740112016562", "", "60162333341");
			AddStaff(db, "MUZAINI BIN MURSHID", "791114145923", "", "0126141042");
			AddStaff(db, "FARAIK HARIM BIN ABDUL RAZAK", "780920076009", "", "0125726342");
			AddStaff(db, "FARIZA BINTI FADZIL", "770303026242", "", "0193033581");
			AddStaff(db, "VIJAYA A/P SUNDRAMOORTHY", "751125145340", "", "0122233957");
			AddStaff(db, "RENUKA A/P THIAGARAJAH", "750909086516", "", "0123553094");
			AddStaff(db, "MOHD ZURAIDI ZAKARIA", "770711075801", "", "60133316659");
			AddStaff(db, "AZWANI ZAM ABDUL RANI", "760201085517", "", "012-326 1691");
			AddStaff(db, "NOORHALIZA BINTI ABU BAKAR", "740218086518", "", "0192797217");
			AddStaff(db, "NOR HALIZA BT SUDIN", "841217145718", "", "60123447896");
			AddStaff(db, "TAJUDIN BIN D.A GAFFOR", "670911106459", "", "60122763928");
			AddStaff(db, "LOGESWARI A/P LECHIMUNAN", "841108086050", "", "6017-3143730");
			AddStaff(db, "HADI ISMA BIN CHE WIL", "770101038245", "", "017-2454588");
			AddStaff(db, "AMBIGAI RATCHAGI A/P MATHAVARAYAN", "740122145114", "", "60162341004");
			AddStaff(db, "FADZELINA BINTI MOHAMED ROZELAN", "810926015180", "", "0197174348");
			AddStaff(db, "FADILAH WATI BINTI KAMIS", "760727055048", "", "0177502238");
			AddStaff(db, "ROMZI BIN ABU BAKAR", "681110085815", "", "0123607094");
			AddStaff(db, "FIRDAUSA ROSEFATIMAH BINTI MOHD YUSOF", "810731145030", "", "60126714978");
			AddStaff(db, "ZURINA BT HARMAINI", "820320065094", "", "60179782762");
			AddStaff(db, "THIRU SELVI A/P MURUGAN", "720208055306", "", "");
			AddStaff(db, "NOR AKMAR YAAKUB", "600203045508", "", "0193138277");
			AddStaff(db, "MOHAMAD KHALIL BIN JAMALDIN", "560929085185", "", "60122192344");
			AddStaff(db, "SHEIRLY @ SANDRA ALFRED", "780927125248", "", "0167985200");
			AddStaff(db, "VERONICA KOW LI LIAN", "640429075984", "", "60122916632");
			AddStaff(db, "HABIBAH BINTI ABDULLAH", "540305055318", "", "60172801952");
			AddStaff(db, "TUNG KA YIN", "811021145620", "", "0173675619");
			AddStaff(db, "NIRMALA A/P M.SUPRAMANIAM", "760507145252", "", "0162480765");
			AddStaff(db, "CHONG SOON LAN", "710802055174", "", "");
			AddStaff(db, "TAN CHEE HONG", "671018075033", "", "60124686382");
			AddStaff(db, "MOHAMAD YUSRI BIN MOHD YUSOFF", "870824025403", "", "60194146508");
			AddStaff(db, "MOHD NORDIN BIN ABD RAHMAN", "721030086053", "", "60126176154");
			AddStaff(db, "SUZLINA BINTI OMAR", "641003105802", "", "60193785247");
			AddStaff(db, "IZHAR BIN ISWAN", "870601145133", "", "60176555100");
			AddStaff(db, "NOORSHAKILA BINTI MOHAMAD DAROS", "850330016656", "", "60137193222");
			AddStaff(db, "SITI AISHAH BINTI MD DIN", "850310145514", "", "01137626684");
			AddStaff(db, "CHONG KOK FEI", "730319085857", "", "60126214444");
			AddStaff(db, "SITI NAZIRAH BINTI MOHD NOOR", "881015075084", "", "60195119684");
			AddStaff(db, "YONG CITY", "750718055688", "", "60127672388");
			AddStaff(db, "ALIF FAIZUDIN BIN JAMALUDIN", "891125146751", "", "60193450623");
			AddStaff(db, "MOHD ZAIMI BIN AB RAZAK", "790705145947", "", "0166668245");
			AddStaff(db, "SITI SUHAILA BINTI AB HAMID", "740705035558", "", "60183513537");
			AddStaff(db, "SYAIFUL ANUAR BIN BOKHARI", "751108105611", "", "60129209784");
			AddStaff(db, "MOHAMMAD AL-RASHID BIN ABU BAKAR", "900804145963", "", "60107008300");
			AddStaff(db, "VICKNESWARAN A/L MUNIANDY", "881226565331", "", "60102485168");
			AddStaff(db, "NORFAUZIAHANIM BT JAAFAR", "840707025618", "", "60122833746");
			AddStaff(db, "NOOR AZIERA EZURIEN BINTI A.AZIZ", "890518015336", "", "60133633527");
			AddStaff(db, "NOOR HAMIZA BINTI MOHD MUSWAN", "811020085048", "", "60123342645");
			AddStaff(db, "SITI AZREENA AIDA BT HAMAT", "880824565046", "", "01110402408");
			AddStaff(db, "AZHARUL ADHA BIN DZULKARNAIN", "850906086025", "", "0176214467");
			AddStaff(db, "NORLIYANA BINTI MOHD REDZUAN", "890203085356", "", "60172937796");
			AddStaff(db, "ROSNATASHA BINTI ABU BAKAR", "781127146264", "", "0183211260");
			AddStaff(db, "MOHD KHAIRIL ASHRAF BIN MOHAMAD KARID", "830819035133", "", "0122689759");
			AddStaff(db, "SHEETALJIT KAUR BAINS", "810608095082", "", "0162260484");
			AddStaff(db, "AMILA NORJIHAH BINTI MUSA", "850710115306", "", "0192224895");
			AddStaff(db, "VARATHARAJAN A/L RAMAN", "770518145695", "", "0126144786");
			AddStaff(db, "MONARITA BINTI JUNAIDI", "791105715066", "", "0122655455");
			AddStaff(db, "VICTOR SABIN", "820326125369", "", "016-8049472");
			AddStaff(db, "TAN SZE CHUN", "790706105031", "", "0178788166");
			AddStaff(db, "LAI KAM BENG", "530725015647", "", "0192103188");
			AddStaff(db, "NUR HASLIZAWATY BINTI AHMAD", "820801035056", "", "60111193156");
			AddStaff(db, "FATIMAH BINTI MOHD ALI", "680208025760", "", "60174222452");
			AddStaff(db, "NAVAKUMAR A/L BALAKRISHNAN", "820217025801", "", "60164849479");
			AddStaff(db, "MOHD HANIS BIN HUSSIN", "870918065297", "", "6017-9636463");
			AddStaff(db, "ROSNANI BINTI ALI", "700526015408", "", "90136115125");
			AddStaff(db, "HAFIZAH BINTI ABDUL KARIM", "840426065090", "", "0192407065");
			AddStaff(db, "PREMA A/P VALAISHAN", "720229015688", "", "60177893986");
			AddStaff(db, "NOR MAZLI BIN AWANG", "740507115557", "", "60199397574");
			AddStaff(db, "LEE CHAN LI", "741008105770", "", "60126745445");
			AddStaff(db, "WONG KEET SEONG", "841109065235", "", "60179731127");
			AddStaff(db, "MARLENE MARGARET NICHOL", "570903135016", "", "60198189531");
			AddStaff(db, "NURAINI BINTI MAT JUDIN", "870409125132", "", "60138535812");
			AddStaff(db, "MUHAMMAD MU'AZ BIN AZAM", "900503025597", "", "0192274707");
			AddStaff(db, "AZADDIN BIN NGAH TASIR", "610425085539", "", "0123920200");
			AddStaff(db, "OSMAN BIN YUSOP", "810228015159", "", "60197778457");
			AddStaff(db, "GRACY NEALDA A.DUKIM", "860626496272", "", "60168111149");
			AddStaff(db, "SITI HASLEEZA BINTI MOHD HASNAN", "840224146028", "", "60123930280");
			AddStaff(db, "RAJA MOHAMMAD HISHAM BIN RAJA MUZAFAR SHAH", "791010105599", "", "60166397882");
			AddStaff(db, "NOOR SYAFINI BINTI MOHAMED NOOR", "880427035770", "", "60132549380");
			AddStaff(db, "NURHAZAR BINTI MD. ARIS", "751022035640", "", "60107840854");
			AddStaff(db, "MOHD AZMI BIN MOHD SUPIAN", "681005106055", "", "60123851752");
			AddStaff(db, "MUHAMMAD MU'ADZ BIN ZULKIFLI", "900817086667", "", "601126262218");
			AddStaff(db, "HARTINI BINTI HUSSIN", "781018025500", "", "60134939330");
			AddStaff(db, "SOLEHAH BINTI AHMAD BAIJURI", "850301085644", "", "60125035588");
			AddStaff(db, "MOHD AZIZUDDIN BIN ABDULLAH", "841231115179", "", "60123010969");
			AddStaff(db, "HAFAZIL BIN MOHD ADAM", "710527085095", "", "60129832416");
			AddStaff(db, "ZAIDI BIN MOHAMMAD", "740928055505", "", "60196420327");
			AddStaff(db, "NURSAKINA BINTI ZAKARIA", "910815146442", "", "60173171964");
			AddStaff(db, "NOORNADIA BINTI ABD JALIL", "860921405038", "", "60122795471");
			AddStaff(db, "NURSHUHAIDA BINTI AB. RAZAK", "900818145104", "", "01152211428");
			AddStaff(db, "SITI ZAHARAH BINTI ABD. HALIM", "860830025822", "", "601347332291");
			AddStaff(db, "ZAMRI BIN ZAINAL", "670104055085", "", "60123228554");
			AddStaff(db, "MOHD FAIZAL BIN MOHD SALLEH", "691124015251", "", "60195555241");
			AddStaff(db, "CHAN MIEW LING", "571001105840", "", "0129136330");
			AddStaff(db, "NORANI BINTI MD MAKHTAR", "940524036152", "", "0148226154");
			AddStaff(db, "MOHD SYAHRUL AZMI BIN ESHA", "810316085819", "", "60194567234");
			AddStaff(db, "MOHAMMAD FARID BIN FADZIL", "741014017881", "", "0175335744");
			AddStaff(db, "ABDUL RAHMAN BIN ABDUL", "670627095027", "", "60133480804");
			AddStaff(db, "SABARIAH BINTI KARIM", "901112055048", "", "60176783665");
			AddStaff(db, "IDA FARIDAH BINTI MOHD ROSDI", "810318065096", "", "60102187894");
			AddStaff(db, "TANG YOW HUA", "671003086303", "", "60124776350");
			AddStaff(db, "MOHD HAFFIZ BIN TALIB", "860920566251", "", "0197176283");
			AddStaff(db, "SAZLIN BT ZAINAL ABIDIN", "790205015548", "", "60123430502");
			AddStaff(db, "MUHAMMAD HAZIQ BIN MOHMAD ZAINI", "950802145535", "", "60169354723");
			AddStaff(db, "SITI AZLINA BINTI K ABDUL RAHMAN", "950509075208", "", "60162488582");
			AddStaff(db, "MOHD SHAIRAZI BIN NOOR ZAKRI", "871002565245", "", "60149594281");
			AddStaff(db, "FARHANNA BINTI UMAR @ OMAR", "861003525908", "", "60138323242");
			AddStaff(db, "MUHAMAD HIZAM BIN JAMALUDIN", "790624105097", "", "60166368406");
			AddStaff(db, "WAN MOHD KAMARUL BIN WAN MOHAMAD", "771207145123", "", "60176043368");
			AddStaff(db, "AZURIN BINTI LANI", "860125145298", "", "60197786159");
			AddStaff(db, "MUHAMMAD SUFRI REDZUAN BIN RAZALI", "880201115225", "", "60179336649");
			AddStaff(db, "HALIM BIN SALEH", "720301065445", "", "60192343709");
			AddStaff(db, "ROHANIZAM BINTI TALIB", "720614016468", "", "60192335353");
			AddStaff(db, "AZHAR BIN HAMZAH", "760206025935", "", "60193770266");
			AddStaff(db, "MOHD ADNAN ANAN BIN ABDULLAH", "600811105973", "", "60162773529");
			AddStaff(db, "NIK MOHD FATTAH BIN NIK HAMDAN", "820226145207", "", "60123637174");
			AddStaff(db, "MOHAMMAD MAFRUKHIN BIN MOKHTAR", "810919015895", "", "60193495125");
			AddStaff(db, "FARAH AZIEYATUL NABILAH BINTI MOHAMED NAZAM", "930618146476", "", "60193747485");
			AddStaff(db, "RAMANADASS A/L SATHIASEELAN", "931209146437", "", "60192717373");
			AddStaff(db, "MOHD AZRIL BIN KHAIRUL ANUAR", "931009105091", "", "");
			AddStaff(db, "NORFAIZAH BINTI MOHAMED NOR", "891216146220", "", "60126431469");
			AddStaff(db, "MUHAMMAD FARIHIIN BIN ABDUL LATIF", "890515146103", "", "60176473873");
			AddStaff(db, "NUR HANIZA BINTI ZAHARAN", "820614115608", "", "0182814066");
			AddStaff(db, "NUR ZAIHAN BINTI MOHD HATA", "850107145674", "", "0123540175");
			AddStaff(db, "MOHD ZAMANI BIN MOHD YUSOFF", "740216025607", "", "0133603471");
			AddStaff(db, "NOR AMALINA BINTI ABDUL RAHMAN", "921109035482", "", "0179702482");
			AddStaff(db, "MUHAMAD HAMIZAN BIN JAAFAR", "930318146909", "", "0172092501");
			AddStaff(db, "FATIN SHAKIRAH BINTI AB AZIZ", "900826035554", "", "01139970300");
			AddStaff(db, "NHANTHINE A/P SELLAMUTHAIYA", "890403015598", "", "0175321711");
			AddStaff(db, "NURUL ILLYANA BINTI AHMAD SAFIAN", "910114145842", "", "0123468370");
			AddStaff(db, "MOHARAM ALI", "930611065665", "", "01117952526");
			AddStaff(db, "IRWAN ISKANDAR BIN AZHARUDDIN", "750602145593", "", "0186600702");
			AddStaff(db, "AKMALHAKIMI BIN ABDULLAH", "851113035185", "", "0176796935");
			AddStaff(db, "MUHAMMAD ISHAM SAFUAN BIN ISMAIL", "961211146003", "", "0102864418");
			AddStaff(db, "MUHAMMAD AMIR ALFAN BIN ZAINUDIN", "921211145581", "", "0187714092");
			AddStaff(db, "FATIN AMIRAH BINTI MOHAMAD", "931023115042", "", "01137247973");
			AddStaff(db, "SITI NAJIHAH BINTI MOHAMED AZLAN", "940223106196", "", "0138211082");
			AddStaff(db, "IZZATUL NABILAH BINTI MOHD ARIFF", "940219106046", "", "0193302064");
			AddStaff(db, "FARAHANA BINTI ZULKEFLI", "940414055248", "", "0132171867");
			AddStaff(db, "JELITA SONIAWATI BINTI FIRDAUS", "920825017504", "", "01137303882");
			AddStaff(db, "NUR SYAMIMIE BINTI ZAMRI", "921121025244", "", "0174663193");
			AddStaff(db, "KHOIRUNNISA BINTI MOHD NAZARI", "940104065512", "", "0139130294");
			AddStaff(db, "VICTORIA ANAK SANEL", "930227135104", "", "0198758345");
			AddStaff(db, "NURRUL WAN NADIAH BINTI AHMAD LATFFI", "900212085856", "", "0104482212");
			AddStaff(db, "SITI WAN SITA BINTI HAKIM", "911220126498", "", "0165885429");
			AddStaff(db, "KHAIRUL FAIZI BIN AB AZIZ", "690920105739", "", "0122685080");
			AddStaff(db, "AMEZA AFIFAH BINTI FAIZAL", "931019136430", "", "0146832143");
			AddStaff(db, "MOHD YUSUF BIN RAMLY", "880808355081", "", "0134475684");
			AddStaff(db, "AHMAD MUSLIHUDDIN BIN ROZLAN", "940215035295", "", "0145085400");
			AddStaff(db, "MUHAMMAD NADZRI BIN AHMAD ZAKI", "940806086289", "", "0175325530");
			AddStaff(db, "MUHAMMAD HAFIZUL AZWAN BIN MOHD SHAMZAN", "930826145005", "", "0172266046");
			AddStaff(db, "IKHWAN HAZIQ BIN AMINUDIN", "921112146079", "", "0182269759");
			AddStaff(db, "AIDA SALWA BINTI JUSOH @ SHAFIE", "940414035060", "", "0172990604");
			AddStaff(db, "RAUDHA NABILA BINTI MOHD YUSOFF", "940814105320", "", "0193841408");
			AddStaff(db, "ISMALISA BINTI ISMAIL", "820508086188", "", "0193502367");
			AddStaff(db, "IZZAT FIRDAUS BIN AHMAD", "900118105371", "", "0193713077");
			AddStaff(db, "SITI NOR ZALIKA BINTI JAWAHIR", "881103015874", "", "0128839341");
			AddStaff(db, "NIRMALL A/L GUNASEKARAN", "930922105695", "", "0173221973");

			


			if (!db.State.Any())
            {
                db.State.AddOrUpdate(s => s.Name,
                    new State { Name = "Johor", Display = true },
                    new State { Name = "Kedah", Display = true },
                    new State { Name = "Kelantan", Display = true },
                    new State { Name = "Melaka", Display = true },
                    new State { Name = "Negeri Sembilan", Display = true },
                    new State { Name = "Pahang", Display = true },
                    new State { Name = "Pulau Pinang", Display = true },
                    new State { Name = "Perak", Display = true },
                    new State { Name = "Perlis", Display = true },
                    new State { Name = "Selangor", Display = true },
                    new State { Name = "Terengganu", Display = true },
                    new State { Name = "Sabah", Display = true },
                    new State { Name = "Sarawak", Display = true },
                    new State { Name = "Wilayah Persekutuan Kuala Lumpur", Display = true },
                    new State { Name = "Wilayah Persekutuan Labuan", Display = true },
                    new State { Name = "Wilayah Persekutuan Putrajaya", Display = true }
                );
            }

            if (!db.Ministry.Any())
            {
                db.Ministry.AddOrUpdate(s => s.Name,
                    new Ministry { Name = "Jabatan Perdana Menteri Malaysia(JPM)", Display = true },
                    new Ministry { Name = "Kementerian Kewangan Malaysia(MOF)", Display = true },
                    new Ministry { Name = "Kementerian Dalam Negeri Malaysia(KDN)", Display = true },
                    new Ministry { Name = "Kementerian Hal Ehwal Ekonomi Malaysia(MEA)", Display = true },
                    new Ministry { Name = "Kementerian Perdagangan Dalam Negeri dan Hal Ehwal Pengguna Malaysia(KPDNHEP)", Display = true },
                    new Ministry { Name = "Kementerian Luar Negeri Malaysia(KLN)", Display = true },
                    new Ministry { Name = "Kementerian Pertahanan Malaysia(MINDEF)", Display = true },
                    new Ministry { Name = "Kementerian Pendidikan Malaysia(KPM)", Display = true },
                    new Ministry { Name = "Kementerian Pembangunan Luar Bandar Malaysia(KPLB)", Display = true },
                    new Ministry { Name = "Kementerian Kerja Raya Malaysia(KKR)", Display = true },
                    new Ministry { Name = "Kementerian Kesihatan Malaysia(KKM)", Display = true },
                    new Ministry { Name = "Kementerian Komunikasi dan Multimedia Malaysia(KKMM)", Display = true },
                    new Ministry { Name = "Kementerian Pembangunan Usahawan Malaysia(MED)", Display = true },
                    new Ministry { Name = "Kementerian Perumahan dan Kerajaan Tempatan Malaysia(KPKT)", Display = true },
                    new Ministry { Name = "Kementerian Pelancongan dan Kebudayaan Malaysia(MOTAC)", Display = true },
                    new Ministry { Name = "Kementerian Pengangkutan Malaysia(MOT)", Display = true },
                    new Ministry { Name = "Kementerian Pembangunan Wanita, Keluarga dan Masyarakat Malaysia(KPWKM)", Display = true },
                    new Ministry { Name = "Kementerian Pertanian dan Industri Asas Tani Malaysia(MOA)", Display = true },
                    new Ministry { Name = "Kementerian Industri Utama Malaysia(MPI)", Display = true },
                    new Ministry { Name = "Kementerian Perdagangan Antarabangsa dan Industri Malaysia(MITI)", Display = true },
                    new Ministry { Name = "Kementerian Sains, Teknologi, Inovasi, Sumber Asli dan Perubahan Iklim Malaysia(MESTECC)", Display = true },
                    new Ministry { Name = "Kementerian Sumber Manusia Malaysia(KSM)", Display = true },
                    new Ministry { Name = "Kementerian Air, Sumber Asli dan Tanah Malaysia(KATS)", Display = true },
                    new Ministry { Name = "Kementerian Wilayah Persekutuan Malaysia(KWP)", Display = true },
                    new Ministry { Name = "Mahkamah", Display = true },
                    new Ministry { Name = "Suruhanjaya", Display = true }
                    );

            }

            if (!db.Sector.Any())
            {
                db.Sector.AddOrUpdate(s => s.Name,
                    new Sector { Name = "Aerospace", Display = true },
                    new Sector { Name = "Transport", Display = true },
                    new Sector { Name = "Computer", Display = true },
                    new Sector { Name = "Telecommunication", Display = true },
                    new Sector { Name = "Agriculture", Display = true },
                    new Sector { Name = "Construction", Display = true },
                    new Sector { Name = "Education", Display = true },
                    new Sector { Name = "Pharmaceutical", Display = true },
                    new Sector { Name = "Food", Display = true },
                    new Sector { Name = "HealthCare", Display = true },
                    new Sector { Name = "Hospitality", Display = true },
                    new Sector { Name = "Entertainment", Display = true },
                    new Sector { Name = "News Media", Display = true },
                    new Sector { Name = "Energy", Display = true },
                    new Sector { Name = "Manufacturing", Display = true },
                    new Sector { Name = "Music", Display = true },
                    new Sector { Name = "Mining", Display = true },
                    new Sector { Name = "Worldwide Web", Display = true },
                    new Sector { Name = "Electronic", Display = true }
                );
            }

            if (!db.Country.Any())
            {
                db.Country.AddOrUpdate(s => s.Name,
                    new Country { Name = "Afghanistan", Display = true },
                    new Country { Name = "Albania", Display = true },
                    new Country { Name = "Algeria", Display = true },
                    new Country { Name = "Andorra", Display = true },
                    new Country { Name = "Angola", Display = true },
                    new Country { Name = "Anguilla", Display = true },
                    new Country { Name = "Antigua & Barbuda", Display = true },
                    new Country { Name = "Argentina", Display = true },
                    new Country { Name = "Armenia", Display = true },
                    new Country { Name = "Australia", Display = true },
                    new Country { Name = "Austria", Display = true },
                    new Country { Name = "Azerbaijan", Display = true },
                    new Country { Name = "Bahamas", Display = true },
                    new Country { Name = "Bahrain", Display = true },
                    new Country { Name = "Bangladesh", Display = true },
                    new Country { Name = "Barbados", Display = true },
                    new Country { Name = "Belarus", Display = true },
                    new Country { Name = "Belgium", Display = true },
                    new Country { Name = "Belize", Display = true },
                    new Country { Name = "Benin", Display = true },
                    new Country { Name = "Bermuda", Display = true },
                    new Country { Name = "Bhutan", Display = true },
                    new Country { Name = "Bolivia", Display = true },
                    new Country { Name = "Bosnia & Herzegovina", Display = true },
                    new Country { Name = "Botswana", Display = true },
                    new Country { Name = "Brazil", Display = true },
                    new Country { Name = "Brunei Darussalam", Display = true },
                    new Country { Name = "Bulgaria", Display = true },
                    new Country { Name = "Burkina Faso", Display = true },
                    new Country { Name = "Burundi", Display = true },
                    new Country { Name = "Cambodia", Display = true },
                    new Country { Name = "Cameroon", Display = true },
                    new Country { Name = "Canada", Display = true },
                    new Country { Name = "Cape Verde", Display = true },
                    new Country { Name = "Cayman Islands", Display = true },
                    new Country { Name = "Central African Republic", Display = true },
                    new Country { Name = "Chad", Display = true },
                    new Country { Name = "Chile", Display = true },
                    new Country { Name = "China", Display = true },
                    new Country { Name = "China - Hong Kong / Macau", Display = true },
                    new Country { Name = "Colombia", Display = true },
                    new Country { Name = "Comoros", Display = true },
                    new Country { Name = "Congo", Display = true },
                    new Country { Name = "Congo, Democratic Republic of (DRC)", Display = true },
                    new Country { Name = "Costa Rica", Display = true },
                    new Country { Name = "Croatia", Display = true },
                    new Country { Name = "Cuba", Display = true },
                    new Country { Name = "Cyprus", Display = true },
                    new Country { Name = "Czech Republic", Display = true },
                    new Country { Name = "Denmark", Display = true },
                    new Country { Name = "Djibouti", Display = true },
                    new Country { Name = "Dominica", Display = true },
                    new Country { Name = "Dominican Republic", Display = true },
                    new Country { Name = "Ecuador", Display = true },
                    new Country { Name = "Egypt", Display = true },
                    new Country { Name = "El Salvador", Display = true },
                    new Country { Name = "Equatorial Guinea", Display = true },
                    new Country { Name = "Eritrea", Display = true },
                    new Country { Name = "Estonia", Display = true },
                    new Country { Name = "Eswatini", Display = true },
                    new Country { Name = "Ethiopia", Display = true },
                    new Country { Name = "Fiji", Display = true },
                    new Country { Name = "Finland", Display = true },
                    new Country { Name = "France", Display = true },
                    new Country { Name = "French Guiana", Display = true },
                    new Country { Name = "Gabon", Display = true },
                    new Country { Name = "Gambia, Republic of The", Display = true },
                    new Country { Name = "Georgia", Display = true },
                    new Country { Name = "Germany", Display = true },
                    new Country { Name = "Ghana", Display = true },
                    new Country { Name = "Great Britain", Display = true },
                    new Country { Name = "Greece", Display = true },
                    new Country { Name = "Grenada", Display = true },
                    new Country { Name = "Guadeloupe", Display = true },
                    new Country { Name = "Guatemala", Display = true },
                    new Country { Name = "Guinea", Display = true },
                    new Country { Name = "Guinea-Bissau", Display = true },
                    new Country { Name = "Guyana", Display = true },
                    new Country { Name = "Haiti", Display = true },
                    new Country { Name = "Honduras", Display = true },
                    new Country { Name = "Hungary", Display = true },
                    new Country { Name = "Iceland", Display = true },
                    new Country { Name = "India", Display = true },
                    new Country { Name = "Indonesia", Display = true },
                    new Country { Name = "Iran", Display = true },
                    new Country { Name = "Iraq", Display = true },
                    new Country { Name = "Israel and the Occupied Territories", Display = true },
                    new Country { Name = "Italy", Display = true },
                    new Country { Name = "Ivory Coast (Cote d'Ivoire)", Display = true },
                    new Country { Name = "Jamaica", Display = true },
                    new Country { Name = "Japan", Display = true },
                    new Country { Name = "Jordan", Display = true },
                    new Country { Name = "Kazakhstan", Display = true },
                    new Country { Name = "Kenya", Display = true },
                    new Country { Name = "Korea, Democratic Republic of (North Korea)", Display = true },
                    new Country { Name = "Korea, Republic of (South Korea)", Display = true },
                    new Country { Name = "Kosovo", Display = true },
                    new Country { Name = "Kuwait", Display = true },
                    new Country { Name = "Kyrgyz Republic (Kyrgyzstan)", Display = true },
                    new Country { Name = "Laos", Display = true },
                    new Country { Name = "Latvia", Display = true },
                    new Country { Name = "Lebanon", Display = true },
                    new Country { Name = "Lesotho", Display = true },
                    new Country { Name = "Liberia", Display = true },
                    new Country { Name = "Libya", Display = true },
                    new Country { Name = "Liechtenstein", Display = true },
                    new Country { Name = "Lithuania", Display = true },
                    new Country { Name = "Luxembourg", Display = true },
                    new Country { Name = "Madagascar", Display = true },
                    new Country { Name = "Malawi", Display = true },
                    new Country { Name = "Malaysia", Display = true },
                    new Country { Name = "Maldives", Display = true },
                    new Country { Name = "Mali", Display = true },
                    new Country { Name = "Malta", Display = true },
                    new Country { Name = "Martinique", Display = true },
                    new Country { Name = "Mauritania", Display = true },
                    new Country { Name = "Mauritius", Display = true },
                    new Country { Name = "Mayotte", Display = true },
                    new Country { Name = "Mexico", Display = true },
                    new Country { Name = "Moldova, Republic of", Display = true },
                    new Country { Name = "Monaco", Display = true },
                    new Country { Name = "Mongolia", Display = true },
                    new Country { Name = "Montenegro", Display = true },
                    new Country { Name = "Montserrat", Display = true },
                    new Country { Name = "Morocco", Display = true },
                    new Country { Name = "Mozambique", Display = true },
                    new Country { Name = "Myanmar/Burma", Display = true },
                    new Country { Name = "Namibia", Display = true },
                    new Country { Name = "Nepal", Display = true },
                    new Country { Name = "New Zealand", Display = true },
                    new Country { Name = "Nicaragua", Display = true },
                    new Country { Name = "Niger", Display = true },
                    new Country { Name = "Nigeria", Display = true },
                    new Country { Name = "North Macedonia, Republic of", Display = true },
                    new Country { Name = "Norway", Display = true },
                    new Country { Name = "Oman", Display = true },
                    new Country { Name = "Pacific Islands", Display = true },
                    new Country { Name = "Pakistan", Display = true },
                    new Country { Name = "Panama", Display = true },
                    new Country { Name = "Papua New Guinea", Display = true },
                    new Country { Name = "Paraguay", Display = true },
                    new Country { Name = "Peru", Display = true },
                    new Country { Name = "Philippines", Display = true },
                    new Country { Name = "Poland", Display = true },
                    new Country { Name = "Portugal", Display = true },
                    new Country { Name = "Puerto Rico", Display = true },
                    new Country { Name = "Qatar", Display = true },
                    new Country { Name = "Reunion", Display = true },
                    new Country { Name = "Romania", Display = true },
                    new Country { Name = "Russian Federation", Display = true },
                    new Country { Name = "Rwanda", Display = true },
                    new Country { Name = "Saint Kitts and Nevis", Display = true },
                    new Country { Name = "Saint Lucia", Display = true },
                    new Country { Name = "Saint Vincent and the Grenadines", Display = true },
                    new Country { Name = "Samoa", Display = true },
                    new Country { Name = "Sao Tome and Principe", Display = true },
                    new Country { Name = "Saudi Arabia", Display = true },
                    new Country { Name = "Senegal", Display = true },
                    new Country { Name = "Serbia", Display = true },
                    new Country { Name = "Seychelles", Display = true },
                    new Country { Name = "Sierra Leone", Display = true },
                    new Country { Name = "Singapore", Display = true },
                    new Country { Name = "Slovak Republic (Slovakia)", Display = true },
                    new Country { Name = "Slovenia", Display = true },
                    new Country { Name = "Solomon Islands", Display = true },
                    new Country { Name = "Somalia", Display = true },
                    new Country { Name = "South Africa", Display = true },
                    new Country { Name = "South Sudan", Display = true },
                    new Country { Name = "Spain", Display = true },
                    new Country { Name = "Sri Lanka", Display = true },
                    new Country { Name = "Sudan", Display = true },
                    new Country { Name = "Suriname", Display = true },
                    new Country { Name = "Sweden", Display = true },
                    new Country { Name = "Switzerland", Display = true },
                    new Country { Name = "Syria", Display = true },
                    new Country { Name = "Tajikistan", Display = true },
                    new Country { Name = "Tanzania", Display = true },
                    new Country { Name = "Thailand", Display = true },
                    new Country { Name = "Netherlands", Display = true },
                    new Country { Name = "Timor Leste", Display = true },
                    new Country { Name = "Togo", Display = true },
                    new Country { Name = "Trinidad & Tobago", Display = true },
                    new Country { Name = "Tunisia", Display = true },
                    new Country { Name = "Turkey", Display = true },
                    new Country { Name = "Turkmenistan", Display = true },
                    new Country { Name = "Turks & Caicos Islands", Display = true },
                    new Country { Name = "Uganda", Display = true },
                    new Country { Name = "Ukraine", Display = true },
                    new Country { Name = "United Arab Emirates", Display = true },
                    new Country { Name = "United States of America (USA)", Display = true },
                    new Country { Name = "Uruguay", Display = true },
                    new Country { Name = "Uzbekistan", Display = true },
                    new Country { Name = "Venezuela", Display = true },
                    new Country { Name = "Vietnam", Display = true },
                    new Country { Name = "Virgin Islands (UK)", Display = true },
                    new Country { Name = "Virgin Islands (US)", Display = true },
                    new Country { Name = "Yemen", Display = true },
                    new Country { Name = "Zambia", Display = true },
                    new Country { Name = "Zimbabwe", Display = true }
                );
            }

        }


        public static void DefaultAccess(DbEntities db)
        {
            //access
            foreach (UserAccess useraccess in Enum.GetValues(typeof(UserAccess)))
            {

                Modules module;

                int access = (int)useraccess;

                if (access >= 0 && access <= 1000)
                {
                    module = Modules.Home;
                }
                else if (access >= 1001 && access <= 2000)
                {
                    module = Modules.Event;
                }
                else if (access >= 2001 && access <= 3000)
                {
                    module = Modules.RnP;
                }
                else if (access >= 3001 && access <= 4000)
                {
                    module = Modules.Learning;
                }
                else if (access >= 4001 && access <= 5000)
                {
                    module = Modules.Admin;
                }
                else if (access >= 5001 && access <= 6000)
                {
                    module = Modules.Setting;
                }
                else //if (access >= 6001 && access <= 7000)
                {
                    module = Modules.Report;
                }
                        
                db.Access.AddOrUpdate(a => a.UserAccess, new Access { UserAccess = useraccess, Module = module, Description = useraccess.DisplayName() });

            }
        }

        public static void AddRole(DbEntities db, string RoleName, string Description)
        {

            var role = db.Role.Local.Where(r => r.Name.Contains(RoleName)).FirstOrDefault() ?? db.Role.Where(r => r.Name.Contains(RoleName)).FirstOrDefault();

            if (role == null)
            {
                db.Role.Add(
                    new Role
                    {
                        Name = RoleName,
                        Description = Description,
                        Display = true,
                        CreatedDate = DateTime.Now                       
                    });
            }

        }

        public static void AddStaff(DbEntities db, string Name, string ICNo, string Email, string MobileNo)
        {
            var user = db.User.Local.Where(r => r.ICNo == ICNo).FirstOrDefault() ?? db.User.Where(r => r.ICNo == ICNo).FirstOrDefault();

            if (user == null)
            {

                var useraccount = new UserAccount
                {
                    LoginId = ICNo,
                    IsEnable = true,
                    LoginAttempt = 0,
                    HashPassword = "02N3k+8BBkCL+kZx+ZG/bfmKG4YGafIrkWW0D1Va7osvWkNxbWc9PQ==",
                    Salt = "/ZCqmg=="
                };

                var staff = new StaffProfile
                {
                    StaffId = "",
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
