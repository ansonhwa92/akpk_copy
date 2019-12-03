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
            //cte

            db.Months.AddOrUpdate(s => s.Month,
                new Months { Month = 1, Name = "Jan" },
                new Months { Month = 2, Name = "Feb" },
                new Months { Month = 3, Name = "Mar" },
                new Months { Month = 4, Name = "Apr" },
                new Months { Month = 5, Name = "May" },
                new Months { Month = 6, Name = "Jun" },
                new Months { Month = 7, Name = "Jul" },
                new Months { Month = 8, Name = "Aug" },
                new Months { Month = 9, Name = "Sep" },
                new Months { Month = 10, Name = "Oct" },
                new Months { Month = 11, Name = "Nov" },
                new Months { Month = 12, Name = "Dec" }
            );


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
                        CountryCode = "60",
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


            var individualaccess = new List<RoleAccess> 
            { 
                new RoleAccess { UserAccess = UserAccess.LearningMenu },
                new RoleAccess { UserAccess = UserAccess.EventMenu },
                new RoleAccess { UserAccess = UserAccess.RnPPublicationMenu },
                new RoleAccess { UserAccess = UserAccess.RnPSurveyMenu },
                new RoleAccess { UserAccess = UserAccess.KMCMenu }                
            };

            var companyaccess = new List<RoleAccess>
            {
                new RoleAccess { UserAccess = UserAccess.LearningMenu },
                new RoleAccess { UserAccess = UserAccess.EventMenu },
                new RoleAccess { UserAccess = UserAccess.RnPPublicationMenu },
                new RoleAccess { UserAccess = UserAccess.RnPSurveyMenu },
                new RoleAccess { UserAccess = UserAccess.KMCMenu }
            };
            
            AddRole(db, "Individual", "Default Individual", individualaccess, true, false, false);
            AddRole(db, "Individual with paper", "Individual with paper");
            AddRole(db, "Individual with paper to present", "Individual with paper to present");

            AddRole(db, "Agency", "Default Agency", companyaccess, false, true, false);
            AddRole(db, "Organizer", "Default Organizer");

            //AddRole(db, "Trainer", "Default Trainer");
            AddRole(db, "Facilitator", "Default Facilitator");
            AddRole(db, "Speaker", "Default Speaker");

            AddRole(db, "Chief Editor", "Chief Editor");

            var staffaccess = new List<RoleAccess>
            {
                new RoleAccess { UserAccess = UserAccess.LearningMenu },
                new RoleAccess { UserAccess = UserAccess.EventMenu },
                new RoleAccess { UserAccess = UserAccess.RnPPublicationMenu },
                new RoleAccess { UserAccess = UserAccess.RnPSurveyMenu },
                new RoleAccess { UserAccess = UserAccess.KMCMenu }
            };

            AddRole(db, "Staff", "Default Staff", staffaccess, false, false, true);

			//AddRole(db, "Admin Event", "Admin Event");
			AddRole(db, "Admin R&P", "Admin R&P");
            //AddRole(db, "Admin eLearning", "Admin eLearning");

            //AddRole(db, "Event Reception", "Event Reception");
            //AddRole(db, "Event Moderator", "Event Moderator");

            //AddRole(db, "Verifier Event", "Verifier Event");
            AddRole(db, "Verifier R&P", "Verifier R&P");
            //AddRole(db, "Verifier eLearning", "Verifier eLearning");

            //AddRole(db, "Approver Event 1", "Approver Event 1");
            AddRole(db, "Approver R&P 1", "Approver R&P 1");
            //AddRole(db, "Approver eLearning 1", "Approver eLearning 1");

            //AddRole(db, "Approver Event 2", "Approver Event 2");
            AddRole(db, "Approver R&P 2", "Approver R&P 2");
            //AddRole(db, "Approver eLearning 2", "Approver eLearning 2");

            //AddRole(db, "Approver Event 3", "Approver Event 3");
            AddRole(db, "Approver R&P 3", "Approver R&P 3");
            //AddRole(db, "Approver eLearning 3", "Approver eLearning 3");

            AddRole(db, "Admin Finance", "Admin Finance");

            //state
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

            if (!db.Branch.Any())
            {
                var state = db.State.Local.Where(r => r.Name == "Wilayah Persekutuan Kuala Lumpur").FirstOrDefault() ?? db.State.Where(r => r.Name == "Wilayah Persekutuan Kuala Lumpur").FirstOrDefault();

                db.Branch.AddOrUpdate(s => s.Name,
                    new Branch { State = state, Name = "HQ", Display = true }
                    );

            }


            if (!db.Department.Any())
            {
                db.Department.AddOrUpdate(s => s.Name,
                    new Department { Name = "CEO Office", Display = true },
                    new Department { Name = "Channel Management", Display = true },
                    new Department { Name = "Compliance & Risk Management", Display = true },
                    new Department { Name = "Corporate Communications", Display = true },
                    new Department { Name = "Corporate Services", Display = true },
                    new Department { Name = "DMP Management", Display = true },
                    new Department { Name = "Finance", Display = true },
                    new Department { Name = "Financial Education", Display = true },
                    new Department { Name = "Human Capital", Display = true },
                    new Department { Name = "IT", Display = true },
                    new Department { Name = "Operations", Display = true }
                    );

            }

            if (!db.Designation.Any())
            {
                db.Designation.AddOrUpdate(s => s.Name,
                    new Designation { Name = "CALL CENTRE AGENT -H & I", Display = true },
                    new Designation { Name = "CEO", Display = true },
                    new Designation { Name = "CLERICAL ", Display = true },
                    new Designation { Name = "CLERICAL OFFICER-I1", Display = true },
                    new Designation { Name = "CLERICAL OFFICER-I2", Display = true },
                    new Designation { Name = "DEPUTY MANAGER-F2", Display = true },
                    new Designation { Name = "EXECUTIVE", Display = true },
                    new Designation { Name = "EXECUTIVE OFFICER-H1", Display = true },
                    new Designation { Name = "EXECUTIVE OFFICER-H2", Display = true },
                    new Designation { Name = "GENERAL MANAGER-C1", Display = true },
                    new Designation { Name = "MANAGER", Display = true },
                    new Designation { Name = "MANAGER-E1", Display = true },
                    new Designation { Name = "MANAGER-E2", Display = true },
                    new Designation { Name = "SENIOR EXECUTIVE", Display = true },
                    new Designation { Name = "SENIOR EXECUTIVE-G1", Display = true },
                    new Designation { Name = "SENIOR EXECUTIVE-G2", Display = true },
                    new Designation { Name = "SENIOR MANAGER", Display = true },
                    new Designation { Name = "SENIOR MANAGER-D1", Display = true },
                    new Designation { Name = "SENIOR MANAGER-D2", Display = true }
                    );

            }

          
            #region add staff
            AddStaff(db, "haroldean.l", "HAROLDEAN LIM @ LIM JIN LOK", "690315065097", "haroldean@akpk.org.my", "60106598960", "Human Capital", "MANAGER-E1", "0002");
            AddStaff(db, "azman.h", "AZMAN BIN HASIM", "690629715005", "azman@akpk.org.my", "60192333703", "Corporate Services", "GENERAL MANAGER-C1", "0003");
            AddStaff(db, "norfazillah.mz", "NOR FAZILLAH BINTI MOHD ZIN", "730201145172", "dilla@akpk.org.my", "60123844738", "Human Capital", "EXECUTIVE OFFICER-H1", "0006");
            AddStaff(db, "mohdjusrizal.a", "MOHD JUSRIZAL BIN ARIS", "800906085115", "jusrizal@akpk.org.my", "60129986169", "Channel Management", "SENIOR EXECUTIVE-G2", "0007");
            AddStaff(db, "nmazatulakmar.r", "NORMAZATULAKMAR BINTI RAZALI", "840417145056", "akmar@akpk.org.my", "60132666146", "Channel Management", "SENIOR EXECUTIVE-G2", "0008");
            AddStaff(db, "firdaus", "AHMAD FIRDAUS BIN MOHAMAD ARIFF", "820521085833", "firdaus@akpk.org.my", "60192685275", "Human Capital", "SENIOR EXECUTIVE-G2", "0009");
            AddStaff(db, "hayati", "NUR HAYATI BINTI MAT SALLEH", "811204086360", "hayati@akpk.org.my", "60192938100", "Finance", "SENIOR EXECUTIVE-G2", "0010");
            AddStaff(db, "alias.ma", "ALIAS BIN MAT ALI", "690120115613", "alias@akpk.org.my", "60199657462", "Channel Management", "MANAGER-E1", "0013");
            AddStaff(db, "puckkin.c", "CHEONG PUCK KIN", "710730075247", "pkcheong@akpk.org.my", "60125557843", "Channel Management", "MANAGER-E2", "0015");
            AddStaff(db, "saidi.y", "SAIDI BIN YAACOB", "720718115479", "saidi@akpk.org.my", "60126313058", "Channel Management", "DEPUTY MANAGER-F2", "0017");
            AddStaff(db, "gunasegaran.m", "GUNASEGARAN A/L MUNUSAMY", "690704105777", "segaran@akpk.org.my", "60163913722", "Channel Management", "MANAGER-E1", "0018");
            AddStaff(db, "mohdrizauddin.n", "MOHD RIZAUDDIN BIN NOOR", "770315036149", "rizauddin@akpk.org.my", "60122888804", "Channel Management", "MANAGER-E2", "0019");
            AddStaff(db, "weekiak.y", "YEO WEE KIAK", "780831135169", "wkyeo@akpk.org.my", "60168697380", "Channel Management", "DEPUTY MANAGER-F2", "0020");
            AddStaff(db, "idris.k", "IDRIS BIN KASIM", "620115125629", "idris@akpk.org.my", "60198928440", "Channel Management", "MANAGER-E1", "0025");
            AddStaff(db, "ezreenezairy.h", "EZREEN EZAIRY BIN HUSSIN", "770406055417", "ezreen@akpk.org.my", "60134283991", "Finance", "MANAGER-E2", "0026");
            AddStaff(db, "guatyou.t", "TAN GUAT YOU", "641108085342", "tgy@akpk.org.my", "60167726985", "DMP Management", "MANAGER", "0030351");
            AddStaff(db, "norashikin.a", "NORASHIKIN ALI", "670109055500", "shikinali@akpk.org.my", "60122672512", "Channel Management", "EXECUTIVE OFFICER-H1", "0031123");
            AddStaff(db, "norfazleen.z", "NOR FAZLEEN BINTI  ZAKARIA", "681105035100", "fazleen@akpk.org.my", "6019 3578803", "Operations", "GENERAL MANAGER-C1", "0034");
            AddStaff(db, "ruslina.rd", "RUS LINA BINTI RUS DIN", "790727055328", "ruslina@akpk.org.my", "60129045947", "DMP Management", "EXECUTIVE OFFICER-H1", "0036");
            AddStaff(db, "suzaidi.ms", "SUZAIDI BIN MD. SHAUKAT", "651123065293", "suzaidi@akpk.org.my", "60193175584", "Finance", "MANAGER", "0036159");
            AddStaff(db, "mansor.k", "MANSOR BIN KESOT @ ALI", "670317016039", "mansor@akpk.org.my", "60193226959", "Channel Management", "SENIOR MANAGER", "0036531");
            AddStaff(db, "nabinaraj.s", "S.NABINARAJ A/L K.SHANMUGANATHAN", "830430105623", "raj@akpk.org.my", "60122428166", "Channel Management", "SENIOR EXECUTIVE-G1", "0038");
            AddStaff(db, "junainah.a", "JUNAINAH BINTI AHMAD ESA", "740112016562", "junainah@akpk.org.my", "60162333341", "Channel Management", "MANAGER", "0046156");
            AddStaff(db, "muzaini.m", "MUZAINI BIN MURSHID", "791114145923", "muzaini@akpk.org.my", "60126141042", "DMP Management", "DEPUTY MANAGER-F2", "0047");
            AddStaff(db, "faraikharim.ar", "FARAIK HARIM BIN ABDUL RAZAK", "780920076009", "faraik@akpk.org.my", "60125726342", "Channel Management", "SENIOR EXECUTIVE-G1", "0048");
            AddStaff(db, "fariza.f", "FARIZA BINTI FADZIL", "770303026242", "fariza@akpk.org.my", "60193033581", "Channel Management", "EXECUTIVE OFFICER-H1", "0049");
            AddStaff(db, "vijaya", "VIJAYA A/P SUNDRAMOORTHY", "751125145340", "vijaya@akpk.org.my", "60122233957", "Human Capital", "MANAGER-E2", "0050");
            AddStaff(db, "renuka.t", "RENUKA A/P THIAGARAJAH", "750909086516", "renuka@akpk.org.my", "60123553094", "Channel Management", "MANAGER", "005163A");
            AddStaff(db, "mohdzuraidi.z", "MOHD ZURAIDI BIN ZAKARIA", "770711075801", "zuraidi@akpk.org.my", "60133316659", "Human Capital", "MANAGER", "0053374");
            AddStaff(db, "Azwan", "AZWANI ZAM ABDUL RANI", "760201085517", "azwan@akpk.org.my", "60123261691", "Financial Education", "MANAGER", "0055783");
            AddStaff(db, "noorhaliza.ab", "NOORHALIZA BINTI ABU BAKAR", "740218086518", "liz@akpk.org.my", "60192797217", "Financial Education", "DEPUTY MANAGER-F2", "0056");
            AddStaff(db, "norhaliza.s", "NOR HALIZA BINTI SUDIN", "841217145718", "haliza@akpk.org.my", "60123447896", "DMP Management", "EXECUTIVE OFFICER-H1", "0060");
            AddStaff(db, "tajudin", "TAJUDIN BIN D A GAFFOR", "670911106459", "tajudin@akpk.org.my", "60122763928", "Human Capital", "CLERICAL OFFICER-I1", "0065");
            AddStaff(db, "logeswari.l", "LOGESWARI A/P LECHIMUNAN", "841108086050", "loges@akpk.org.my", "60173143730", "Finance", "SENIOR EXECUTIVE-G2", "0067");
            AddStaff(db, "hadiisma.cw", "HADI ISMA BIN CHE WIL", "770101038245", "hadi@akpk.org.my", "60172454588", "Finance", "CLERICAL OFFICER-I1", "0068");
            AddStaff(db, "aratchagi.m", "AMBIGAI RATCHAGI A/P MATHAVARAYAN", "740122145114", "amy@akpk.org.my", "60162341004", "Channel Management", "CLERICAL OFFICER-I1", "0069");
            AddStaff(db, "fadzelina.mr", "FADZELINA BINTI MOHAMED ROZELAN", "810926015180", "lina@akpk.org.my", "60197174348", "Channel Management", "EXECUTIVE OFFICER-H2", "0071");
            AddStaff(db, "fadilahwati.k", "FADILAH WATI BINTI KAMIS", "760727055048", "wati@akpk.org.my", "60177502238", "Channel Management", "EXECUTIVE OFFICER-H1", "0072");
            AddStaff(db, "romzi.ab", "ROMZI BIN ABU BAKAR", "681110085815", "romzi@akpk.org.my", "60123607094", "Channel Management", "MANAGER-E2", "0075");
            AddStaff(db, "frosefatimah.my", "FIRDAUSA ROSEFATIMAH BINTI MOHD YUSOF", "810731145030", "rosefatimah@akpk.org.my", "60126714978", "Channel Management", "EXECUTIVE OFFICER-H1", "0076");
            AddStaff(db, "zurina.h", "ZURINA BINTI HARMAINI", "820320065094", "rina@akpk.org.my", "60179782762", "Channel Management", "EXECUTIVE OFFICER-H1", "0077");
            AddStaff(db, "thiruselvi.m", "THIRU SELVI A/P MURUGAN", "720208055306", "selvi@akpk.org.my", "60122267710", "DMP Management", "SENIOR EXECUTIVE-G1", "0081");
            AddStaff(db, "norakmar.y", "NOR AKMAR BINTI YAAKUB", "600203045508", "norakmar@akpk.org.my", "60193138277", "Financial Education", "SENIOR MANAGER-D2", "0083");
            AddStaff(db, "mohamadkhalil.j", "MOHAMAD KHALIL BIN JAMALDIN", "560929085185", "khalil@akpk.org.my", "60122192344", "Corporate Communications", "MANAGER", "0084");
            AddStaff(db, "sheirly.a", "SHEIRLY @ SANDRA ALFRED", "780927125248", "sheirly@akpk.org.my", "60167985200", "Channel Management", "SENIOR EXECUTIVE-G1", "0086");
            AddStaff(db, "veronica.kll", "VERONICA KOW LI LIAN", "640429075984", "veronica@akpk.org.my", "60122916632", "Channel Management", "DEPUTY MANAGER-F2", "0088");
            AddStaff(db, "habibah.a", "HABIBAH BINTI ABDULLAH", "540305055318", "habibah@akpk.org.my", "60172801952", "Channel Management", "MANAGER", "0091");
            AddStaff(db, "kayin.t", "TUNG KA YIN", "811021145620", "tkayin@akpk.org.my", "60173675619", "Channel Management", "SENIOR EXECUTIVE-G1", "0092");
            AddStaff(db, "nirmala", "NIRMALA A/P M SUPRAMANIAM", "760507145252", "nirmala@akpk.org.my", "60162480765", "Financial Education", "MANAGER-E1", "0094");
            AddStaff(db, "soonlan.c", "CHONG SOON LAN", "710802055174", "slchong@akpk.org.my", "60129867288", "Channel Management", "SENIOR EXECUTIVE-G1", "0096");
            AddStaff(db, "cheehong.t", "TAN CHEE HONG", "671018075033", "chtan@akpk.org.my", "60124686382", "Channel Management", "MANAGER-E2", "0098");
            AddStaff(db, "mohamadyusri.my", "MOHAMAD YUSRI BIN MOHD YUSOFF", "870824025403", "yusri@akpk.org.my", "60194146508", "Channel Management", "CLERICAL OFFICER-I1", "0099");
            AddStaff(db, "mohdnordin.ar", "MOHD NORDIN BIN ABD RAHMAN", "721030086053", "nordin@akpk.org.my", "60126176154", "Channel Management", "MANAGER-E2", "0103");
            AddStaff(db, "suzlina.o", "SUZLINA BINTI OMAR", "641003105802", "suzlina@akpk.org.my", "60193785247", "IT", "SENIOR EXECUTIVE-G1", "0104");
            AddStaff(db, "izhar.i", "IZHAR BIN ISWAN", "870601145133", "izhar@akpk.org.my", "60176555100", "CEO Office", "SENIOR EXECUTIVE-G2", "0105");
            AddStaff(db, "noorshakila.md", "NOORSHAKILA BINTI MOHAMAD DAROS", "850330016656", "noorshakila@akpk.org.my", "60137193222", "Channel Management", "CLERICAL OFFICER-I1", "0109");
            AddStaff(db, "sitiaishah.md", "SITI AISHAH BINTI MD DIN", "850310145514", "aishah@akpk.org.my", "60132930283", "Human Capital", "CLERICAL OFFICER-I1", "0110");
            AddStaff(db, "KFChong", "CHONG KOK FEI", "730319085857", "desmond@akpk.org.my", "60126214444", "Financial Education", "MANAGER-E2", "0115");
            AddStaff(db, "sitinazirah.mn", "SITI NAZIRAH BINTI MOHD NOOR", "881015075084", "nazirah@akpk.org.my", "60195119684", "Channel Management", "CLERICAL OFFICER-I1", "0116");
            AddStaff(db, "city.y", "YONG CITY", "750718055688", "elaine@akpk.org.my", "60127672388", "Channel Management", "SENIOR EXECUTIVE-G1", "0122");
            AddStaff(db, "aliffaizudin.j", "ALIF FAIZUDIN BIN JAMALUDIN", "891125146751", "alif.f@akpk.org.my", "60193450623", "Corporate Communications", "CLERICAL OFFICER-I1", "0123");
            AddStaff(db, "mohdzaimi.ar", "MOHD ZAIMI BIN AB RAZAK", "790705145947", "mohdzaimi.ar@akpk.org.my", "60166668245", "IT", "SENIOR EXECUTIVE", "0296");
            AddStaff(db, "sitisuhaila.ah", "SITI SUHAILA BINTI AB HAMID", "740705035558", "suhaila@akpk.org.my", "60183513537", "Channel Management", "SENIOR EXECUTIVE-G1", "0125");
            AddStaff(db, "syaifulanuar.b", "SYAIFUL ANUAR BIN BOKHARI", "751108105611", "syaiful@akpk.org.my", "60129209784", "Channel Management", "DEPUTY MANAGER-F2", "0126");
            AddStaff(db, "rashid", "MOHAMMAD AL-RASHID BIN ABU BAKAR", "900804145963", "rashid@akpk.org.my", "60107008300", "Human Capital", "CLERICAL OFFICER-I1", "0127");
            AddStaff(db, "vickneswaran.m", "VICKNESWARAN A/L MUNIANDY", "881226565331", "vicknes@akpk.org.my", "60102485168", "Channel Management", "SENIOR EXECUTIVE-G2", "0128");
            AddStaff(db, "norfauziahanim.j", "NORFAUZIAHANIM BINTI JAAFAR", "840707025618", "hanim@akpk.org.my", "60122833746", "Channel Management", "SENIOR EXECUTIVE-G2", "0131");
            AddStaff(db, "azie", "NOOR AZIERA EZURIEN BINTI A. AZIZ", "890518015336", "azie@akpk.org.my", "60133633527", "Financial Education", "SENIOR EXECUTIVE-G1", "0132");
            AddStaff(db, "noorhamiza.mm", "NOOR HAMIZA BINTI MOHD MUSWAN", "811020085048", "eja@akpk.org.my", "60123342645", "Operations", "EXECUTIVE OFFICER-H1", "0133");
            AddStaff(db, "sitiazreena.h", "SITI AZREENA AIDA BINTI HAMAT", "880824565046", "aida@akpk.org.my", "601110402408", "Channel Management", "SENIOR EXECUTIVE-G2", "0134");
            AddStaff(db, "azharuladha.d", "AZHARUL ADHA BIN DZULKARNAIN", "850906086025", "azharul@akpk.org.my", "60176214467", "DMP Management", "EXECUTIVE OFFICER-H1", "0135");
            AddStaff(db, "Norliyana", "NORLIYANA BINTI MOHD REDZUAN", "890203085356", "norliyana@akpk.org.my", "60172937796", "Finance", "EXECUTIVE OFFICER-H2", "0138");
            AddStaff(db, "rosnatasha.ab", "ROSNATASHA BINTI ABU BAKAR", "781127146264", "natasha@akpk.org.my", "60183211260", "DMP Management", "EXECUTIVE OFFICER-H1", "0139");
            AddStaff(db, "khairilashraf.mk", "MOHD KHAIRIL ASHRAF BIN MOHAMAD KARID", "830819035133", "ashraf@akpk.org.my", "60122689759", "Financial Education", "SENIOR EXECUTIVE-G2", "0140");
            AddStaff(db, "sheetal", "SHEETALJIT KAUR BAINS", "810608095082", "sheetal@akpk.org.my", "60162260484", "CEO Office", "DEPUTY MANAGER-F2", "0149");
            AddStaff(db, "amila", "AMILA NORJIHAH BINTI MUSA", "850710115306", "amila@akpk.org.my", "60192224895", "Financial Education", "SENIOR EXECUTIVE-G1", "0151");
            AddStaff(db, "varatharajan.r", "VARATHARAJAN A/L RAMAN", "770518145695", "rajan@akpk.org.my", "60126144786", "Channel Management", "SENIOR EXECUTIVE-G1", "0154");
            AddStaff(db, "monarita.j", "MONARITA BINTI JUNAIDI", "791105715066", "monarita@akpk.org.my", "60122655455", "Financial Education", "DEPUTY MANAGER-F2", "0156");
            AddStaff(db, "victor.s", "VICTOR SABIN", "820326125369", "victor@akpk.org.my", "60168049472", "Channel Management", "CLERICAL OFFICER-I1", "0157");
            AddStaff(db, "szechun.t", "TAN SZE CHUN", "790706105031", "chun@akpk.org.my", "60178788166", "IT", "MANAGER-E1", "0158");
            AddStaff(db, "lai.kb", "LAI KAM BENG", "530725015647", "laikb@akpk.org.my", "60192103188", "Channel Management", "SENIOR EXECUTIVE", "0162");
            AddStaff(db, "nurhaslizawaty.a", "NUR HASLIZAWATY BINTI AHMAD", "820801035056", "haslizawaty@akpk.org.my", "60111193156", "Finance", "SENIOR EXECUTIVE-G2", "0165");
            AddStaff(db, "fatimah.ma", "FATIMAH BINTI MOHD ALI", "680208025760", "fatimah@akpk.org.my", "60174222452", "Channel Management", "SENIOR EXECUTIVE-G1", "0166");
            AddStaff(db, "navakumar.b", "NAVAKUMAR A/L BALAKRISHNAN", "820217025801", "navakumar@akpk.org.my", "60164849479", "Channel Management", "SENIOR EXECUTIVE-G1", "0168");
            AddStaff(db, "mohdhanis.h", "MOHD HANIS BIN HUSSIN", "870918065297", "hanis@akpk.org.my", "60179636463", "Channel Management", "SENIOR EXECUTIVE-G1", "0169");
            AddStaff(db, "rosnani.a", "ROSNANI BINTI ALI", "700526015408", "rosnani@akpk.org.my", "60183211260", "Channel Management", "SENIOR EXECUTIVE-G1", "0170");
            AddStaff(db, "hafizah.ak", "HAFIZAH BINTI ABDUL KARIM", "840426065090", "hafizah@akpk.org.my", "60192407065", "DMP Management", "SENIOR EXECUTIVE-G2", "0172");
            AddStaff(db, "prema.v", "PREMA A/P VALAISHAN", "720229015688", "prema@akpk.org.my", "60177893986", "Channel Management", "SENIOR EXECUTIVE-G1", "0173");
            AddStaff(db, "normazli.a", "NOR MAZLI BIN AWANG", "740507115557", "mazli@akpk.org.my", "60199397574", "Channel Management", "SENIOR EXECUTIVE-G1", "0174");
            AddStaff(db, "chanli.l", "LEE CHAN LI", "741008105770", "chanli@akpk.org.my", "60126745445", "Channel Management", "MANAGER-E2", "0175");
            AddStaff(db, "keetseong.w", "WONG KEET SEONG", "841109065235", "kseong@akpk.org.my", "60179731127", "Financial Education", "SENIOR EXECUTIVE-G2", "0179");
            AddStaff(db, "mmargaret.n", "MARLENE MARGARET ANAK JOHN NICHOL", "570903135016", "margaret@akpk.org.my", "60198189531", "Channel Management", "MANAGER", "0180");
            AddStaff(db, "nuraini.mj", "NURAINI BINTI MAT JUDIN", "870409125132", "nuraini@akpk.org.my", "60138535812", "Channel Management", "EXECUTIVE OFFICER-H1", "0182");
            AddStaff(db, "muhammadmuaz.a", "MUHAMMAD MU'AZ BIN AZAM", "900503025597", "muaz@akpk.org.my", "60192274707", "IT", "EXECUTIVE OFFICER-H2", "0186");
            AddStaff(db, "azaddin.nt", "AZADDIN BIN NGAH TASIR", "610425085539", "azaddin@akpk.org.my", "60123920200", "CEO Office", "CEO", "0188");
            AddStaff(db, "osman.y", "OSMAN BIN YUSOP", "810228015159", "osman@akpk.org.my", "60197778457", "Channel Management", "SENIOR EXECUTIVE-G2", "0189");
            AddStaff(db, "gracynealda.d", "GRACY NEALDA A. DUKIM", "860626496272", "gracy@akpk.org.my", "60168111149", "Channel Management", "SENIOR EXECUTIVE-G1", "0190");
            AddStaff(db, "sitihasleeza.mh", "SITI HASLEEZA BINTI MOHD HASNAN", "840224146028", "hasleeza@akpk.org.my", "60123930280", "Channel Management", "CLERICAL OFFICER-I2", "0191");
            AddStaff(db, "rajahisham.rms", "RAJA MOHAMMAD HISHAM BIN RAJA MUZAFAR SHAH", "791010105599", "hisham@akpk.org.my", "60166397882", "IT", "SENIOR EXECUTIVE-G1", "0193");
            AddStaff(db, "noorsyafini.mn", "NOOR SYAFINI BINTI MOHAMED NOOR", "880427035770", "syafini@akpk.org.my", "60132549380", "Corporate Services", "CLERICAL OFFICER-I2", "0194");
            AddStaff(db, "nurhazar.ma", "NURHAZAR BINTI MD.ARIS", "751022035640", "nurhazar@akpk.org.my", "60107840854", "Human Capital", "SENIOR EXECUTIVE-G1", "0195");
            AddStaff(db, "mohdazmi.ms", "MOHD AZMI BIN MOHD SUPIAN", "681005106055", "azmi@akpk.org.my", "60123851752", "IT", "SENIOR MANAGER-D1", "0198");
            AddStaff(db, "muhammadmuadz.z", "MUHAMMAD MU'ADZ BIN ZULKIFLI", "900817086667", "muadz@akpk.org.my", "601126262218", "Financial Education", "EXECUTIVE OFFICER-H2", "0201");
            AddStaff(db, "hartini.h", "HARTINI BINTI HUSSIN", "781018025500", "hartini@akpk.org.my", "60134939330", "Channel Management", "SENIOR EXECUTIVE-G1", "0202");
            AddStaff(db, "solehah.ab", "SOLEHAH BINTI AHMAD BAIJURI", "850301085644", "solehah@akpk.org.my", "60125035588", "Channel Management", "SENIOR EXECUTIVE-G1", "0203");
            AddStaff(db, "mohdazizuddin.a", "MOHD AZIZUDDIN BIN ABDULLAH", "841231115179", "azizuddin@akpk.org.my", "60123010969", "Channel Management", "SENIOR EXECUTIVE-G2", "0208");
            AddStaff(db, "hafazil.ma", "HAFAZIL BIN MOHD ADAM", "710527085095", "hafazil@akpk.org.my", "60129832416", "DMP Management", "SENIOR MANAGER-D2", "0210");
            AddStaff(db, "zaidi.m", "ZAIDI BIN MOHAMMAD", "740928055505", "zaidi@akpk.org.my", "60196420327", "Channel Management", "SENIOR EXECUTIVE-G1", "0212");
            AddStaff(db, "noornadia.aj", "NOORNADIA BINTI ABD JALIL", "860921405038", "nadia@akpk.org.my", "60122795471", "DMP Management", "CLERICAL OFFICER-I2", "0219");
            AddStaff(db, "shuhaida", "NURSHUHAIDA BINTI AB.RAZAK", "900818145104", "shuhaida@akpk.org.my", "601152211428", "Finance", "EXECUTIVE OFFICER-H1", "0220");
            AddStaff(db, "sitizaharah.ah", "SITI ZAHARAH BINTI ABD. HALIM", "860830025822", "zaharah@akpk.org.my", "601347332291", "Channel Management", "CLERICAL ", "0221");
            AddStaff(db, "zamri.z", "ZAMRI BIN ZAINAL", "670104055085", "zamri@akpk.org.my", "60123228554", "Channel Management", "MANAGER-E2", "0222");
            AddStaff(db, "mohdfaizal.ms", "MOHD FAIZAL BIN MOHD SALLEH", "691124015251", "mohd.faizal@akpk.org.my", "60195555241", "Channel Management", "SENIOR EXECUTIVE-G1", "0225");
            AddStaff(db, "miewling.c", "CHAN MIEW LING", "571001105840", "miewling.c@akpk.org.my", "60129136330", "Channel Management", "SENIOR EXECUTIVE", "0274");
            AddStaff(db, "norani.mm", "NORANI BINTI MD MAKHTAR", "940524036152", "norani.mm@akpk.org.my", "60148226154", "Channel Management", "CALL CENTRE AGENT -H & I", "0271");
            AddStaff(db, "msyahrulazmi.e", "MOHD SYAHRUL AZMI BIN ESHA", "810316085819", "syahrul.azmi@akpk.org.my", "60194567234", "Channel Management", "SENIOR EXECUTIVE-G2", "0226");
            AddStaff(db, "mohammadfarid.f", "MOHAMMAD FARID BIN FADZIL", "741014017881", "mohammad.farid@akpk.org.my", "60175335744", "Channel Management", "SENIOR EXECUTIVE-G1", "0227");
            AddStaff(db, "rahman", "ABDUL RAHMAN BIN ABDUL", "670627095027", "abdul.rahman@akpk.org.my", "60133480804", "Compliance & Risk Management", "MANAGER-E2", "0228");
            AddStaff(db, "idafaridah.mr", "IDA FARIDAH BINTI MOHD ROSDI", "810318065096", "ida.faridah@akpk.org.my", "60102187894", "Channel Management", "SENIOR EXECUTIVE-G1", "0230");
            AddStaff(db, "yowhua.t", "TANG YOW HUA", "671003086303", "yowhua.tang@akpk.org.my", "60124776350", "DMP Management", "EXECUTIVE OFFICER-H1", "0231");
            AddStaff(db, "mohdhaffiz.t", "MOHD HAFFIZ BIN TALIB", "860920566251", "mohdhaffiz.t@akpk.org.my", "60197176283", "Human Capital", "EXECUTIVE OFFICER-H1", "0234");
            AddStaff(db, "sazlin.za", "SAZLIN BT ZAINAL ABIDIN", "790205015548", "sazlin.za@akpk.org.my", "60123430502", "Corporate Communications", "MANAGER-E2", "0235");
            AddStaff(db, "muhammadhaziq.m", "MUHAMMAD HAZIQ BIN MOHMAD ZAINI", "950802145535", "muhammadhaziq.m@akpk.org.my", "60169354723", "Channel Management", "CLERICAL OFFICER-I2", "0236");
            AddStaff(db, "sitiazlina.abd", "SITI AZLINA BINTI K ABDUL RAHMAN", "950509075208", "sitiazlina.abd@akpk.org.my", "60162488582", "Channel Management", "CALL CENTRE AGENT -H & I", "0237");
            AddStaff(db, "mohdshairazi.nz", "MOHD SHAIRAZI BIN NOOR ZAKRI", "871002565245", "mohdshairazi.nz@akpk.org.my", "60149594281", "Compliance & Risk Management", "SENIOR EXECUTIVE-G1", "0239");
            AddStaff(db, "farhanna.u", "FARHANNA BINTI UMAR @ OMAR", "861003525908", "farhanna.u@akpk.org.my", "60138323242", "Channel Management", "SENIOR EXECUTIVE-G2", "0240");
            AddStaff(db, "muhamadhizam.j", "MUHAMAD HIZAM BIN JAMALUDIN", "790624105097", "muhamadhizam.j@akpk.org.my", "60166368406", "Channel Management", "SENIOR EXECUTIVE-G1", "0241");
            AddStaff(db, "wankamarul.wm", "WAN MOHD KAMARUL BIN WAN MOHAMAD", "771207145123", "wankamarul.wm@akpk.org.my", "60176043368", "IT", "EXECUTIVE OFFICER-H1", "0242");
            AddStaff(db, "azurin.l", "AZURIN BINTI LANI", "860125145298", "azurin.l@akpk.org.my", "60197786159", "Channel Management", "CLERICAL ", "0243");
            AddStaff(db, "muhammadsufri.r", "MUHAMMAD SUFRI REDZUAN BIN RAZALI", "880201115225", "muhammadsufri.r@akpk.org.my", "60179336649", "Channel Management", "CLERICAL ", "0244");
            AddStaff(db, "halim.s", "HALIM BIN SALEH", "720301065445", "halim.s@akpk.org.my", "60192343709", "IT", "MANAGER-E2", "0246");
            AddStaff(db, "rohanizam.t", "ROHANIZAM BINTI TALIB", "720614016468", "rohanizam.t@akpk.org.my", "60192335353", "Channel Management", "SENIOR EXECUTIVE-G1", "0247");
            AddStaff(db, "azhar.hamzah", "AZHAR BIN HAMZAH", "760206025935", "azhar.hamzah@akpk.org.my", "60193770266", "Channel Management", "SENIOR EXECUTIVE-G1", "0248");
            AddStaff(db, "mohdadnan.a", "MOHD ADNAN ANAN BIN ABDULLAH", "600811105973", "mohdadnan@akpk.org.my", "60162773529", "Compliance & Risk Management", "MANAGER", "0249");
            AddStaff(db, "nikfattah.nh", "NIK MOHD FATTAH BIN NIK HAMDAN", "820226145207", "nikfattah.nh@akpk.org.my", "60123637174", "DMP Management", "EXECUTIVE OFFICER-H1", "0250");
            AddStaff(db, "mafrukhin.m", "MOHAMMAD MAFRUKHIN BIN MOKHTAR", "810919015895", "mafrukhin.m@akpk.org.my", "60193495125", "Financial Education", "MANAGER-E2", "0251");
            AddStaff(db, "fazieyatulnabila.ms", "FARAH AZIEYATUL NABILAH BINTI MOHAMED NAZAM", "930618146476", "fazieyatulnabila.ms@akpk.org.my", "60193747485", "Channel Management", "CLERICAL ", "0252");
            AddStaff(db, "ramanadass.s", "RAMANADASS A/L SATHIASEELAN", "931209146437", "ramanadass.s@akpk.org.my", "60192717373", "Channel Management", "CLERICAL ", "0253");
            AddStaff(db, "norfaizah.mn", "NORFAIZAH BINTI MOHAMED NOR", "891216146220", "norfaizah.mn@akpk.org.my", "60126431469", "Finance", "CLERICAL OFFICER-I2", "0258");
            AddStaff(db, "mfarihiin.al", "MUHAMMAD FARIHIIN BIN ABDUL LATIF", "890515146103", "mfarihiin.al@akpk.org.my", "60176473873", "Corporate Communications", "SENIOR EXECUTIVE-G2", "0259");
            AddStaff(db, "nurhaniza.z", "NUR HANIZA BINTI ZAHARAN", "820614115608", "nurhaniza.z@akpk.org.my", "60182814066", "Channel Management", "SENIOR EXECUTIVE", "0261");
            AddStaff(db, "mohdzamani.my", "MOHD ZAMANI BIN MOHD YUSOFF", "740216025607", "mohdzamani.my@akpk.org.my", "60133603471", "Channel Management", "SENIOR EXECUTIVE-G1", "0265");
            AddStaff(db, "noramalina.ar", "NOR AMALINA BINTI ABDUL RAHMAN", "921109035482", "noramalina.ar@akpk.org.my", "60179702482", "Channel Management", "CLERICAL ", "0268");
            AddStaff(db, "muhammadhamizan.j", "MUHAMAD HAMIZAN BIN JAAFAR", "930318146909", "muhammadhamizan.j@akpk.org.my", "60172092501", "Corporate Communications", "SENIOR EXECUTIVE-G2", "0267");
            AddStaff(db, "fatinshakirah.aa", "FATIN SHAKIRAH BINTI AB AZIZ", "900826035554", "fatinshakirah.a@akpk.org.my", "601139970300", "Channel Management", "CLERICAL ", "0269");
            AddStaff(db, "nhanthine.s", "NHANTHINE A/P SELLAMUTHAIYA", "890403015598", "nhanthine.s@akpk.org.my", "60175321711", "Human Capital", "SENIOR EXECUTIVE", "0270");
            AddStaff(db, "nurulillyana.as", "NURUL ILLYANA BINTI AHMAD SAFIAN", "910114145842", "nurulillyana.as@akpk.org.my", "60123468370", "Channel Management", "CLERICAL ", "0272");
            AddStaff(db, "moharam.a", "MOHARAM ALI", "930611065665", "moharam.a@akpk.org.my", "601117952526", "Channel Management", "CLERICAL ", "0273");
            AddStaff(db, "irwaniskandar.a", "IRWAN ISKANDAR BIN AZHARUDDIN", "750602145593", "irwaniskandar.a@akpk.org.my", "60186600702", "Corporate Communications", "SENIOR EXECUTIVE-G2", "0275");
            AddStaff(db, "akmalhakimi.a", "AKMALHAKIMI BIN ABDULLAH", "851113035185", "akmalhakimi.a@akpk.org.my", "60176796935", "Compliance & Risk Management", "SENIOR EXECUTIVE-G1", "0276");
            AddStaff(db, "muhammadisham.i", "MUHAMMAD ISHAM SAFUAN BIN ISMAIL", "961211146003", "muhammadisham.i@akpk.org.my", "60102864418", "IT", "CLERICAL OFFICER-I2", "0277");
            AddStaff(db, "wannadiah.al", "NURRUL WAN NADIAH BINTI AHMAD LATFFI", "900212085856", "wannadiah.al@akpk.org.my", "60104482212", "Financial Education", "EXECUTIVE OFFICER-H2", "0279");
            AddStaff(db, "sitiwan.sh", "SITI WAN SITA BINTI HAKIM", "911220126498", "sitiwan.sh@akpk.org.my", "60165885429", "Channel Management", "CLERICAL ", "0281");
            AddStaff(db, "khairulfaizi.aa", "KHAIRUL FAIZI BIN AB AZIZ", "690920105739", "khairulfaizi.aa@akpk.org.my", "60122685080", "IT", "MANAGER", "0284");
            AddStaff(db, "amezaafifah.f", "AMEZA AFIFAH BINTI FAIZAL", "931019136430", "amezaafifah.f@akpk.org.my", "60146832143", "Channel Management", "CLERICAL OFFICER-I2", "0285");
            AddStaff(db, "mohdyusuf.r", "MOHD YUSUF BIN RAMLY", "880808355081", "mohdyusuf.r@akpk.org.my", "60134475684", "IT", "SENIOR EXECUTIVE", "0286");
            AddStaff(db, "ahmadmuslihuddin.r", "AHMAD MUSLIHUDDIN BIN ROZLAN", "940215035295", "ahmadmuslihuddin.r@akpk.org.my", "60145085400", "Channel Management", "SENIOR EXECUTIVE", "0287");
            AddStaff(db, "muhammadnadzri", "MUHAMMAD NADZRI BIN AHMAD ZAKI", "940806086289", "muhammadnadzri@akpk.org.my", "60175325530", "Channel Management", "SENIOR EXECUTIVE", "0288");
            AddStaff(db, "mhafizulazwan.ms", "MUHAMMAD HAFIZUL AZWAN BIN MOHD SHAMZAN", "930826145005", "mhafizulazwan.ms@akpk.org.my", "60172266046", "Channel Management", "SENIOR EXECUTIVE", "0289");
            AddStaff(db, "ikhwanhaziq.a", "IKHWAN HAZIQ BIN AMINUDIN", "921112146079", "ikhwanhaziq.a@akpk.org.my", "60182269759", "Financial Education", "SENIOR EXECUTIVE", "0290");
            AddStaff(db, "aidasalwa.j", "AIDA SALWA BINTI JUSOH @ SHAFIE", "940414035060", "aidasalwa.j@akpk.org.my", "60172990604", "Channel Management", "SENIOR EXECUTIVE", "0291");
            AddStaff(db, "raudhanabila.my", "RAUDHA NABILA BINTI MOHD YUSOFF", "940814105320", "raudhanabila.my@akpk.org.my", "60193841408", "Human Capital", "SENIOR EXECUTIVE", "0292");
            AddStaff(db, "ismalisa.i", "ISMALISA BINTI ISMAIL", "820508086188", "ismalisa.i@akpk.org.my", "60193502367", "Channel Management", "SENIOR EXECUTIVE", "0293");
            AddStaff(db, "izzatfirdaus.a", "IZZAT FIRDAUS BIN AHMAD", "900118105371", "izzatfirdaus.a@akpk.org.my", "60193713077", "Human Capital", "SENIOR EXECUTIVE", "0294");
            AddStaff(db, "sitinorzalika.j", "SITI NOR ZALIKA BINTI JAWAHIR", "881103015874", "sitinorzalika.j@akpk.org.my", "60128839341", "CEO Office", "CLERICAL ", "0297");
            AddStaff(db, "nirmall.g", "NIRMALL A/L GUNASEKARAN", "930922105695", "nirmall.g@akpk.org.my", "60173221973", "Financial Education", "SENIOR EXECUTIVE", "G021");
            AddStaff(db, "nurulshamimi.r", "NURUL SYAMIMI BINTI RAMLI", "911113036112", "nurulshamimi.r@akpk.org.my", "60136731685", "Corporate Communications", "SENIOR EXECUTIVE", "0298");
            AddStaff(db, "farahana.z", "FARAHANA BINTI ZULKEFLI", "940414055248", "farahana.z@akpk.org.my", "60132171867", "Financial Education", "SENIOR EXECUTIVE", "0299");
            AddStaff(db, "fatinamirah.m", "FATIN AMIRAH BINTI MOHAMAD", "931023115042", "fatinamirah.m@akpk.org.my", "601137247973", "IT", "SENIOR EXECUTIVE", "0300");
            AddStaff(db, "jelitasoniawati.f", "JELITA SONIAWATI BINTI FIRDAUS", "920825017504", "jelitasoniawati.f@akpk.org.my", "60113730388", "IT", "SENIOR EXECUTIVE", "0301");
            AddStaff(db, "sitinajihah.ma", "SITI NAJIHAH BINTI MOHAMED AZLAN", "940223106196", "sitinajihah.ma@akpk.org.my", "60138211082", "Financial Education", "SENIOR EXECUTIVE", "0302");
            AddStaff(db, "nuramalina.y", "NUR AMALINA NADIA BINTI YAHYA", "910313145044", "nuramalina.y@akpk.org.my", "60148008272", "Channel Management", "SENIOR EXECUTIVE", "0303");
            AddStaff(db, "ainhusna.an", "AIN HUSNA BINTI AMIM NORDIN", "910403146438", "ainhusna.an@akpk.org.my", "60137934152", "Channel Management", "EXECUTIVE", "0304");
            AddStaff(db, "muhammadsyahmie.ah", "MUHAMMAD SYAHMIE BIN ABDUL HALIM", "920801145499", "muhammadsyahmie.ah@akpk.org.my", "60123300454", "Compliance & Risk Management", "SENIOR EXECUTIVE", "0306");
            AddStaff(db, "syedmohdfauzi.sh", "SYED MOHD FAUZI BIN SAID HUSSIN", "810509085313", "syedmohdfauzi.sh@akpk.org.my", "60192765313", "Financial Education", "SENIOR EXECUTIVE", "0307");
            AddStaff(db, "nursafwanah.i", "NUR SAFWANAH BINTI ISMAIL", "901021145072", "nursafwanah.i@akpk.org.my", "60136261594", "Finance", "SENIOR EXECUTIVE", "0311");
            AddStaff(db, "nursyahirah.r", "NUR SYAHIRAH BINTI RAHMAT", "961010146146", "nursyahirah.r@akpk.org.my", "60132038300", "Channel Management", "CLERICAL ", "0308");
            AddStaff(db, "intannoorsyakira.z", "INTAN NOOR SYAKIRA BINTI ZAINUDDIN", "900730065548", "intannoorsyakira.z@akpk.org.my", "60145280966", "Channel Management", "CLERICAL ", "0309");
            AddStaff(db, "wannuradibah.cm", "WAN NURADIBAH BINTI CHE MOHD LUDIN", "920212105798", "wannuradibah.cm@akpk.org.my", "60133138516", "Channel Management", "CLERICAL ", "0310");
            AddStaff(db, "fep_user1", "FEP USER 1", "123456789012", "fepuser1@fep.com", "012345678", "IT", "SENIOR EXECUTIVE", "xxxx");
            AddStaff(db, "fep_user2", "FEP USER 2", "123456789013", "fepuser2@fep.com", "012345678", "IT", "SENIOR EXECUTIVE", "xxxy");
            AddStaff(db, "fep_user3", "FEP USER 3", "123456789014", "fepuser3@fep.com", "012345678", "IT", "SENIOR EXECUTIVE", "xxxz");
            AddStaff(db, "fep_user4", "FEP USER 4", "123456789015", "fepuser4@fep.com", "012345678", "IT", "SENIOR EXECUTIVE", "xxxf");
            #endregion

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
                    new Country { Name = "Afghanistan", CountryCode1 = "93", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Albania", CountryCode1 = "355", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Algeria", CountryCode1 = "213", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "American Samoa", CountryCode1 = "1-684", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Andorra", CountryCode1 = "376", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Angola", CountryCode1 = "244", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Anguilla", CountryCode1 = "1-264", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Antarctica", CountryCode1 = "672", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Antigua and Barbuda", CountryCode1 = "1-268", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Argentina", CountryCode1 = "54", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Armenia", CountryCode1 = "374", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Aruba", CountryCode1 = "297", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Australia", CountryCode1 = "61", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Austria", CountryCode1 = "43", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Azerbaijan", CountryCode1 = "994", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bahamas", CountryCode1 = "1-242", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bahrain", CountryCode1 = "973", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bangladesh", CountryCode1 = "880", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Barbados", CountryCode1 = "1-246", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Belarus", CountryCode1 = "375", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Belgium", CountryCode1 = "32", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Belize", CountryCode1 = "501", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Benin", CountryCode1 = "229", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bermuda", CountryCode1 = "1-441", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bhutan", CountryCode1 = "975", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bolivia", CountryCode1 = "591", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bosnia and Herzegovina", CountryCode1 = "387", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Botswana", CountryCode1 = "267", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Brazil", CountryCode1 = "55", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "British Indian Ocean Territory", CountryCode1 = "246", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "British Virgin Islands", CountryCode1 = "1-284", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Brunei", CountryCode1 = "673", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Bulgaria", CountryCode1 = "359", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Burkina Faso", CountryCode1 = "226", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Burundi", CountryCode1 = "257", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cambodia", CountryCode1 = "855", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cameroon", CountryCode1 = "237", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Canada", CountryCode1 = "1", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cape Verde", CountryCode1 = "238", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cayman Islands", CountryCode1 = "1-345", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Central African Republic", CountryCode1 = "236", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Chad", CountryCode1 = "235", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Chile", CountryCode1 = "56", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "China", CountryCode1 = "86", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Christmas Island", CountryCode1 = "61", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cocos Islands", CountryCode1 = "61", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Colombia", CountryCode1 = "57", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Comoros", CountryCode1 = "269", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cook Islands", CountryCode1 = "682", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Costa Rica", CountryCode1 = "506", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Croatia", CountryCode1 = "385", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cuba", CountryCode1 = "53", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Curacao", CountryCode1 = "599", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Cyprus", CountryCode1 = "357", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Czech Republic", CountryCode1 = "420", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Democratic Republic of the Congo", CountryCode1 = "243", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Denmark", CountryCode1 = "45", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Djibouti", CountryCode1 = "253", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Dominica", CountryCode1 = "1-767", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Dominican Republic", CountryCode1 = "1-809", CountryCode2 = "1-829", CountryCode3 = "1-849", Display = true },
                    new Country { Name = "East Timor", CountryCode1 = "670", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Ecuador", CountryCode1 = "593", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Egypt", CountryCode1 = "20", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "El Salvador", CountryCode1 = "503", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Equatorial Guinea", CountryCode1 = "240", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Eritrea", CountryCode1 = "291", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Estonia", CountryCode1 = "372", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Ethiopia", CountryCode1 = "251", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Falkland Islands", CountryCode1 = "500", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Faroe Islands", CountryCode1 = "298", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Fiji", CountryCode1 = "679", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Finland", CountryCode1 = "358", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "France", CountryCode1 = "33", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "French Polynesia", CountryCode1 = "689", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Gabon", CountryCode1 = "241", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Gambia", CountryCode1 = "220", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Georgia", CountryCode1 = "995", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Germany", CountryCode1 = "49", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Ghana", CountryCode1 = "233", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Gibraltar", CountryCode1 = "350", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Greece", CountryCode1 = "30", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Greenland", CountryCode1 = "299", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Grenada", CountryCode1 = "1-473", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Guam", CountryCode1 = "1-671", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Guatemala", CountryCode1 = "502", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Guernsey", CountryCode1 = "44-1481", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Guinea", CountryCode1 = "224", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Guinea-Bissau", CountryCode1 = "245", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Guyana", CountryCode1 = "592", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Haiti", CountryCode1 = "509", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Honduras", CountryCode1 = "504", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Hong Kong", CountryCode1 = "852", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Hungary", CountryCode1 = "36", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Iceland", CountryCode1 = "354", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "India", CountryCode1 = "91", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Indonesia", CountryCode1 = "62", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Iran", CountryCode1 = "98", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Iraq", CountryCode1 = "964", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Ireland", CountryCode1 = "353", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Isle of Man", CountryCode1 = "44-1624", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Israel", CountryCode1 = "972", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Italy", CountryCode1 = "39", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Ivory Coast", CountryCode1 = "225", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Jamaica", CountryCode1 = "1-876", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Japan", CountryCode1 = "81", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Jersey", CountryCode1 = "44-1534", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Jordan", CountryCode1 = "962", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Kazakhstan", CountryCode1 = "7", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Kenya", CountryCode1 = "254", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Kiribati", CountryCode1 = "686", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Kosovo", CountryCode1 = "383", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Kuwait", CountryCode1 = "965", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Kyrgyzstan", CountryCode1 = "996", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Laos", CountryCode1 = "856", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Latvia", CountryCode1 = "371", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Lebanon", CountryCode1 = "961", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Lesotho", CountryCode1 = "266", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Liberia", CountryCode1 = "231", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Libya", CountryCode1 = "218", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Liechtenstein", CountryCode1 = "423", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Lithuania", CountryCode1 = "370", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Luxembourg", CountryCode1 = "352", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Macau", CountryCode1 = "853", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Macedonia", CountryCode1 = "389", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Madagascar", CountryCode1 = "261", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Malawi", CountryCode1 = "265", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Malaysia", CountryCode1 = "60", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Maldives", CountryCode1 = "960", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Mali", CountryCode1 = "223", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Malta", CountryCode1 = "356", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Marshall Islands", CountryCode1 = "692", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Mauritania", CountryCode1 = "222", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Mauritius", CountryCode1 = "230", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Mayotte", CountryCode1 = "262", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Mexico", CountryCode1 = "52", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Micronesia", CountryCode1 = "691", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Moldova", CountryCode1 = "373", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Monaco", CountryCode1 = "377", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Mongolia", CountryCode1 = "976", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Montenegro", CountryCode1 = "382", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Montserrat", CountryCode1 = "1-664", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Morocco", CountryCode1 = "212", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Mozambique", CountryCode1 = "258", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Myanmar", CountryCode1 = "95", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Namibia", CountryCode1 = "264", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Nauru", CountryCode1 = "674", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Nepal", CountryCode1 = "977", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Netherlands", CountryCode1 = "31", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Netherlands Antilles", CountryCode1 = "599", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "New Caledonia", CountryCode1 = "687", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "New Zealand", CountryCode1 = "64", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Nicaragua", CountryCode1 = "505", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Niger", CountryCode1 = "227", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Nigeria", CountryCode1 = "234", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Niue", CountryCode1 = "683", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "North Korea", CountryCode1 = "850", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Northern Mariana Islands", CountryCode1 = "1-670", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Norway", CountryCode1 = "47", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Oman", CountryCode1 = "968", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Pakistan", CountryCode1 = "92", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Palau", CountryCode1 = "680", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Palestine", CountryCode1 = "970", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Panama", CountryCode1 = "507", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Papua New Guinea", CountryCode1 = "675", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Paraguay", CountryCode1 = "595", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Peru", CountryCode1 = "51", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Philippines", CountryCode1 = "63", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Pitcairn", CountryCode1 = "64", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Poland", CountryCode1 = "48", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Portugal", CountryCode1 = "351", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Puerto Rico", CountryCode1 = "1-787", CountryCode2 = "1-939", CountryCode3 = "", Display = true },
                    new Country { Name = "Qatar", CountryCode1 = "974", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Republic of the Congo", CountryCode1 = "242", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Reunion", CountryCode1 = "262", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Romania", CountryCode1 = "40", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Russia", CountryCode1 = "7", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Rwanda", CountryCode1 = "250", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saint Barthelemy", CountryCode1 = "590", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saint Helena", CountryCode1 = "290", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saint Kitts and Nevis", CountryCode1 = "1-869", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saint Lucia", CountryCode1 = "1-758", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saint Martin", CountryCode1 = "590", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saint Pierre and Miquelon", CountryCode1 = "508", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saint Vincent and the Grenadines", CountryCode1 = "1-784", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Samoa", CountryCode1 = "685", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "San Marino", CountryCode1 = "378", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Sao Tome and Principe", CountryCode1 = "239", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Saudi Arabia", CountryCode1 = "966", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Senegal", CountryCode1 = "221", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Serbia", CountryCode1 = "381", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Seychelles", CountryCode1 = "248", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Sierra Leone", CountryCode1 = "232", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Singapore", CountryCode1 = "65", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Sint Maarten", CountryCode1 = "1-721", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Slovakia", CountryCode1 = "421", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Slovenia", CountryCode1 = "386", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Solomon Islands", CountryCode1 = "677", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Somalia", CountryCode1 = "252", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "South Africa", CountryCode1 = "27", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "South Korea", CountryCode1 = "82", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "South Sudan", CountryCode1 = "211", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Spain", CountryCode1 = "34", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Sri Lanka", CountryCode1 = "94", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Sudan", CountryCode1 = "249", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Suriname", CountryCode1 = "597", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Svalbard and Jan Mayen", CountryCode1 = "47", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Swaziland", CountryCode1 = "268", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Sweden", CountryCode1 = "46", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Switzerland", CountryCode1 = "41", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Syria", CountryCode1 = "963", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Taiwan", CountryCode1 = "886", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Tajikistan", CountryCode1 = "992", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Tanzania", CountryCode1 = "255", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Thailand", CountryCode1 = "66", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Togo", CountryCode1 = "228", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Tokelau", CountryCode1 = "690", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Tonga", CountryCode1 = "676", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Trinidad and Tobago", CountryCode1 = "1-868", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Tunisia", CountryCode1 = "216", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Turkey", CountryCode1 = "90", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Turkmenistan", CountryCode1 = "993", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Turks and Caicos Islands", CountryCode1 = "1-649", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Tuvalu", CountryCode1 = "688", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "U.S. Virgin Islands", CountryCode1 = "1-340", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Uganda", CountryCode1 = "256", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Ukraine", CountryCode1 = "380", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "United Arab Emirates", CountryCode1 = "971", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "United Kingdom", CountryCode1 = "44", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "United States", CountryCode1 = "1", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Uruguay", CountryCode1 = "598", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Uzbekistan", CountryCode1 = "998", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Vanuatu", CountryCode1 = "678", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Vatican", CountryCode1 = "379", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Venezuela", CountryCode1 = "58", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Vietnam", CountryCode1 = "84", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Wallis and Futuna", CountryCode1 = "681", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Western Sahara", CountryCode1 = "212", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Yemen", CountryCode1 = "967", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Zambia", CountryCode1 = "260", CountryCode2 = "", CountryCode3 = "", Display = true },
                    new Country { Name = "Zimbabwe", CountryCode1 = "263", CountryCode2 = "", CountryCode3 = "", Display = true }
                );
            }

            if (!db.KMCCategory.Any())
            {
                db.KMCCategory.AddOrUpdate(s => s.Title,
                    new KMCCategory { Title = "Infographic", Display = true },
                    new KMCCategory { Title = "Video", Display = true },
                    new KMCCategory { Title = "Audio", Display = true },
                    new KMCCategory { Title = "Article", Display = true }
                );
            }

            DefaultSLAReminder(db);
            DefaultParameterGroup(db);
            DefaultTemplate(db);



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
                    module = Modules.Setting;
                }               
                else //if (access >= 6001 && access <= 7000)
                {
                    module = Modules.Report;
                }

                db.Access.AddOrUpdate(a => a.UserAccess, new Access { UserAccess = useraccess, Module = module, Description = useraccess.DisplayName() });

            }
        }

        public static void AddRole(DbEntities db, string RoleName, string Description, List<RoleAccess> roleaccess = null, bool IsPublicIndividualDefault = false, bool IsPublicCompanyDefault = false, bool IsStaffDefault = false)
        {

            var role = db.Role.Local.Where(r => r.Name == RoleName).FirstOrDefault() ?? db.Role.Where(r => r.Name == RoleName).FirstOrDefault();

            if (role == null)
            {
                role = db.Role.Add(
                    new Role
                    {
                        Name = RoleName,
                        Description = Description,
                        Display = true,
                        CreatedDate = DateTime.Now,                       
                        RoleAccess = roleaccess
                    });

                if (IsPublicIndividualDefault)
                {
                    db.RoleDefault.Add(new RoleDefault { Role = role, DefaultRoleType = DefaultRoleType.DefaultIndividual });
                }

                if (IsPublicCompanyDefault)
                {
                    db.RoleDefault.Add(new RoleDefault { Role = role, DefaultRoleType = DefaultRoleType.DefaultCompany });
                }

                if (IsStaffDefault)
                {
                    db.RoleDefault.Add(new RoleDefault { Role = role, DefaultRoleType = DefaultRoleType.DefaultStaff });
                }

            }

        }

        public static Department AddDepartment(DbEntities db, string Name)
        {
            var department = db.Department.Local.Where(r => r.Name == Name).FirstOrDefault() ?? db.Department.Where(r => r.Name == Name).FirstOrDefault();

            if (department == null)
            {
                department = new Department
                {
                    Name = Name,
                    Display = true
                };

                db.Department.Add(department);
            }

            return department;

        }

        public static Designation AddDesignation(DbEntities db, string Name)
        {
            var designation = db.Designation.Local.Where(r => r.Name == Name).FirstOrDefault() ?? db.Designation.Where(r => r.Name == Name).FirstOrDefault();

            if (designation == null)
            {
                designation = new Designation
                {
                    Name = Name,
                    Display = true
                };

                db.Designation.Add(designation);
            }

            return designation;

        }

        public static void AddStaff(DbEntities db, string Username, string Name, string ICNo, string Email, string MobileNo, string Department, string Designation, string StaffId)
        {
            var user = db.User.Local.Where(r => r.ICNo == ICNo && r.UserType == UserType.Staff).FirstOrDefault() ?? db.User.Where(r => r.ICNo == ICNo && r.UserType == UserType.Staff).FirstOrDefault();

            var department = AddDepartment(db, Department);

            var designation = AddDesignation(db, Designation);

            var roles = db.RoleDefault.Local.Where(r => r.DefaultRoleType == DefaultRoleType.DefaultStaff).ToList();

            List<UserRole> staffroles = new List<UserRole>();

            foreach (var role in roles)
            {
                staffroles.Add(new UserRole { Role = role.Role });
            }
            
            if (user == null)
            {

                var useraccount = new UserAccount
                {
                    LoginId = Username,
                    IsEnable = true,
                    LoginAttempt = 0,
                    HashPassword = "",
                    Salt = "",
                    UserRoles = staffroles
                };

                var staff = new StaffProfile
                {
                    StaffId = StaffId,
                    BranchId = null,
                    Department = department,
                    Designation = designation
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
                        StaffProfile = staff,
                        CountryCode = "60"
                    });
            }
            else
            {
                user.Name = Name;
                user.Email = Email;
                user.MobileNo = MobileNo;

                db.User.Attach(user);
                db.Entry(user).Property(x => x.Name).IsModified = true;
                db.Entry(user).Property(x => x.Email).IsModified = true;
                db.Entry(user).Property(x => x.MobileNo).IsModified = true;

                var account = user.UserAccount;
                var staff = user.StaffProfile;

                account.LoginId = Username;

                db.UserAccount.Attach(account);
                db.Entry(account).Property(x => x.LoginId).IsModified = true;

                staff.Department = department;
                staff.Designation = designation;
                staff.StaffId = StaffId;

                db.StaffProfile.Attach(staff);
                db.Entry(staff).Property(x => x.DepartmentId).IsModified = true;
                db.Entry(staff).Property(x => x.DesignationId).IsModified = true;
                db.Entry(staff).Property(x => x.StaffId).IsModified = true;

            }
        }

        public static void DefaultSLAReminder(DbEntities db)
        {

            db.SLAReminder.AddOrUpdate(s => s.NotificationType,
                new SLAReminder { NotificationCategory = NotificationCategory.System, SLAEventType = SLAEventType.ActivateAccount, NotificationType = NotificationType.ActivateAccount, ETCode = "ET001SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.System, SLAEventType = SLAEventType.ResetPassword, NotificationType = NotificationType.ResetPassword, ETCode = "ET002SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.System, SLAEventType = SLAEventType.SystemError, NotificationType = NotificationType.SystemError, ETCode = "ET003SY", SLAResolutionTime = 0, IntervalDuration = 0, SLADurationType = SLADurationType.Days }
            );
        }

        public static void DefaultParameterGroup(DbEntities db)
        {
            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            new ParameterGroup { SLAEventType = SLAEventType.System, TemplateParameterType = TemplateParameterType.UserFullName });

            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = TemplateParameterType.UserFullName });

            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            new ParameterGroup { SLAEventType = SLAEventType.ResetPassword, TemplateParameterType = TemplateParameterType.UserFullName });

            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            new ParameterGroup { SLAEventType = SLAEventType.System, TemplateParameterType = TemplateParameterType.Link });

            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = TemplateParameterType.Link });

            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            new ParameterGroup { SLAEventType = SLAEventType.ResetPassword, TemplateParameterType = TemplateParameterType.Link });

            db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
            new ParameterGroup { SLAEventType = SLAEventType.ActivateAccount, TemplateParameterType = TemplateParameterType.LoginDetail });

        }

        public static void DefaultTemplate(DbEntities db)
        {

            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.ActivateAccount,
                    NotificationCategory = NotificationCategory.System,
                    TemplateName = NotificationType.ActivateAccount.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.ActivateAccount).ToString(),
                    enableEmail = true,
                    TemplateSubject = "New FE Portal Account Created",
                    TemplateMessage = "&lt;p&gt;Dear&amp;nbsp;&lt;span style=&quot;font-size: 1rem;&quot;&gt;[#UserFullName],&lt;/span&gt;&lt;/p&gt;&lt;p&gt;You can activate your account [#Link].&amp;nbsp;&lt;/p&gt;&lt;p&gt;Your login details:&lt;/p&gt;&lt;p&gt;[#LoginDetail]&lt;br&gt;&lt;/p&gt;&lt;p&gt;&lt;span style=&quot;color: rgb(255, 255, 255); font-size: 12px; text-align: center; white-space: nowrap; background-color: rgb(41, 182, 246);&quot;&gt;&lt;br&gt;&lt;/span&gt;&lt;/p&gt;",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType },
                new TemplateParameters { NotificationType = NotificationType.ActivateAccount, TemplateParameterType = "UserFullName" },
                new TemplateParameters { NotificationType = NotificationType.ActivateAccount, TemplateParameterType = "Link" },
                new TemplateParameters { NotificationType = NotificationType.ActivateAccount, TemplateParameterType = "LoginDetail" }
                );

            db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
                new NotificationTemplate
                {
                    NotificationType = NotificationType.ResetPassword,
                    NotificationCategory = NotificationCategory.System,
                    TemplateName = NotificationType.ResetPassword.DisplayName(),
                    TemplateRefNo = "T" + ((int)NotificationType.ResetPassword).ToString(),
                    enableEmail = true,
                    TemplateSubject = "FE Portal Password Reset",
                    TemplateMessage = "&lt;p style=&quot;font-size: 16px;&quot;&gt;Dear&amp;nbsp;&lt;span style=&quot;font-size: 1rem;&quot;&gt;[#UserFullName],&lt;/span&gt;&lt;/p&gt;&lt;p style=&quot;font-size: 16px;&quot;&gt;You can reset your password [#Link].&amp;nbsp;&lt;/p&gt;&lt;p style=&quot;font-size: 16px;&quot;&gt;&lt;br&gt;&lt;/p&gt;",
                    enableSMSMessage = false,
                    SMSMessage = "SMS Message Template",
                    enableWebMessage = false,
                    WebMessage = "Web Message Template",
                    WebNotifyLink = "",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.Id,
                    User = user,
                    Display = true
                });

            db.TemplateParameters.AddOrUpdate(t => new { t.NotificationType, t.TemplateParameterType },
                new TemplateParameters { NotificationType = NotificationType.ResetPassword, TemplateParameterType = "UserFullName" },
                new TemplateParameters { NotificationType = NotificationType.ResetPassword, TemplateParameterType = "Link" }
                );


        }

    }

}
