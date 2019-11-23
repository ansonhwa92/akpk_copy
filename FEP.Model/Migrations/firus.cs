using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

            //publication settings
            if (!db.PublicationSettings.Any())
            {
                db.PublicationSettings.Add(new PublicationSettings { HardcopyReturnPeriod = 30, MinimumPublishedYear = 1900 });
            }

            //banks
            if (!db.BankInformation.Any())
            {
                db.BankInformation.Add(new BankInformation { ShortName = "Maybank" });
                db.BankInformation.Add(new BankInformation { ShortName = "CIMB Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Public Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "RHB Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Hong Leong Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Ambank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Affin Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Alliance Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Bank Islam" });
                db.BankInformation.Add(new BankInformation { ShortName = "Bank Muamalat" });
                db.BankInformation.Add(new BankInformation { ShortName = "Bank Rakyat" });
                db.BankInformation.Add(new BankInformation { ShortName = "BSN" });
                db.BankInformation.Add(new BankInformation { ShortName = "HSBC Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Standard Chartered" });
                db.BankInformation.Add(new BankInformation { ShortName = "UOB Bank" });
                db.BankInformation.Add(new BankInformation { ShortName = "Maybank2E" });
            }

            //promotion codes
            if (!db.PromotionCode.Any())
            {
                db.PromotionCode.Add(new PromotionCode {
                    Code = "MFM1234",
                    MoneyValue = 10,
                    PercentageValue = 0,
                    ExpiryDate = DateTime.Parse("31-12-2019 23:59:59"),
                    Used = false
                });
                db.PromotionCode.Add(new PromotionCode
                {
                    Code = "MFM5678",
                    MoneyValue = 20,
                    PercentageValue = 0,
                    ExpiryDate = DateTime.Parse("30-09-2019 23:59:59"),
                    Used = false
                });
            }

            //targeted group cities
            if (!db.TargetedGroupCities.Any())
            {
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 0, Name = "Any" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 5826, Name = "Kuala Nerus" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7327, Name = "Ayer Baloi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7328, Name = "Ayer Hitam (JHR)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7329, Name = "Ayer Tawar 2" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7330, Name = "Ayer Tawar 3" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7331, Name = "Ayer Tawar 4" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7332, Name = "Ayer Tawar 5" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7333, Name = "Permaisuri" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7334, Name = "Sungai Tong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7335, Name = "Kijal" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7336, Name = "Kuala Berang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7337, Name = "Kuala Besut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7338, Name = "Kuala Terengganu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7339, Name = "Marang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7340, Name = "Paka" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7341, Name = "Dungun" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7342, Name = "Jerteh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7343, Name = "Kampung Raja" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7344, Name = "Kemasek" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7345, Name = "Kerteh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7346, Name = "Ketengah Jaya" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7347, Name = "Ayer Puteh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7348, Name = "Bukit Besi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7349, Name = "Bukit Payong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7350, Name = "Ceneh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7351, Name = "Chalok" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7352, Name = "Cukai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7353, Name = "Sungai Pelek" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7354, Name = "Tanjong Karang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7355, Name = "Tanjong Sepat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7356, Name = "Telok Panglima Garang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7357, Name = "Ajil" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7358, Name = "Al Muktatfi Billah Shah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7359, Name = "Shah Alam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7360, Name = "Subang Airport" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7361, Name = "Subang Jaya" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7362, Name = "Sungai Ayer Tawar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7363, Name = "Sungai Besar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7364, Name = "Sungai Buloh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7365, Name = "Sekinchan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7366, Name = "Semenyih" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7367, Name = "Sepang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7368, Name = "Serdang (SL)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7369, Name = "Serendah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7370, Name = "Seri Kembangan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7371, Name = "Pulau Carey" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7372, Name = "Pulau Indah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7373, Name = "Pulau Ketam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7374, Name = "Rasa" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7375, Name = "Rawang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7376, Name = "Sabak Bernam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7377, Name = "Kuala Kubu Baru" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7378, Name = "Kuala Selangor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7379, Name = "Pandan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7380, Name = "Pelabuhan Klang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7381, Name = "Petaling Jaya" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7382, Name = "Puchong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7383, Name = "Jeram (SL)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7384, Name = "Kajang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7385, Name = "Kapar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7386, Name = "Kerling" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7387, Name = "Klang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7388, Name = "Klia" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7389, Name = "Cheras (SL)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7390, Name = "Cyberjaya" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7391, Name = "Dengkil" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7392, Name = "Gombak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7393, Name = "Hulu Langat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7394, Name = "Jenjarom" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7395, Name = "Batang Berjuntai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7396, Name = "Batang Kali" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7397, Name = "Batu Arang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7398, Name = "Batu Caves (SL)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7399, Name = "Beranang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7400, Name = "Bukit Rotan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7401, Name = "Tatau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7402, Name = "Ampang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7403, Name = "Bandar Baru Bangi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7404, Name = "Bandar Puncak Alam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7405, Name = "Bangi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7406, Name = "Banting" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7407, Name = "Siburan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7408, Name = "Simunjan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7409, Name = "Song" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7410, Name = "Spaoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7411, Name = "Sri Aman" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7412, Name = "Sundar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7413, Name = "Saratok" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7414, Name = "Sarikei" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7415, Name = "Sebauh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7416, Name = "Sebuyau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7417, Name = "Serian" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7418, Name = "Sibu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7419, Name = "Miri" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7420, Name = "Mukah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7421, Name = "Nanga Medamit" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7422, Name = "Niah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7423, Name = "Pusa" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7424, Name = "Roban" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7425, Name = "Lingga" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7426, Name = "Long Lama" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7427, Name = "Lubok Antu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7428, Name = "Lundu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7429, Name = "Lutong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7430, Name = "Matu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7431, Name = "Kanowit" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7432, Name = "Kapit" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7433, Name = "Kota Samarahan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7434, Name = "Kuching" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7435, Name = "Lawas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7436, Name = "Limbang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7437, Name = "Dalat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7438, Name = "Daro" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7439, Name = "Debak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7440, Name = "Engkilili" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7441, Name = "Julau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7442, Name = "Kabong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7443, Name = "Bekenu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7444, Name = "Belaga" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7445, Name = "Belawai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7446, Name = "Betong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7447, Name = "Bintangor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7448, Name = "Bintulu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7449, Name = "Tenom" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7450, Name = "Tuaran" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7451, Name = "Asajaya" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7452, Name = "Balingian" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7453, Name = "Baram" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7454, Name = "Bau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7455, Name = "Sipitang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7456, Name = "Tambunan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7457, Name = "Tamparuli" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7458, Name = "Tanjung Aru" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7459, Name = "Tawau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7460, Name = "Tenghilan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7461, Name = "Papar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7462, Name = "Penampang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7463, Name = "Putatan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7464, Name = "Ranau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7465, Name = "Sandakan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7466, Name = "Semporna" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7467, Name = "Lahad Datu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7468, Name = "Likas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7469, Name = "Membakut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7470, Name = "Menumbok" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7471, Name = "Nabawan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7472, Name = "Pamol" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7473, Name = "Kota Kinabalu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7474, Name = "Kota Kinabatangan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7475, Name = "Kota Marudu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7476, Name = "Kuala Penyu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7477, Name = "Kudat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7478, Name = "Kunak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7479, Name = "Beluran" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7480, Name = "Beverly" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7481, Name = "Bongawan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7482, Name = "Inanam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7483, Name = "Keningau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7484, Name = "Kota Belud" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7485, Name = "Tanjong Bungah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7486, Name = "Tanjung Bungah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7487, Name = "Tasek Gelugor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7488, Name = "Tasek Gelugur" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7489, Name = "Usm Pulau Pinang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7490, Name = "Beaufort" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7491, Name = "Penang Hill" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7492, Name = "Perai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7493, Name = "Permatang Pauh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7494, Name = "Pulau Pinang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7495, Name = "Simpang Ampat (PG)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7496, Name = "Sungai Jawi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7497, Name = "Gelugor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7498, Name = "Jelutong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7499, Name = "Kepala Batas (PG)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7500, Name = "Kubang Semang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7501, Name = "Nibong Tebal" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7502, Name = "Penaga" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7503, Name = "Balik Pulau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7504, Name = "Batu Ferringhi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7505, Name = "Batu Maung" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7506, Name = "Bayan Lepas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7507, Name = "Bukit Mertajam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7508, Name = "Butterworth" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7509, Name = "Kaki Bukit" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7510, Name = "Kangar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7511, Name = "Kuala Perlis" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7512, Name = "Padang Besar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7513, Name = "Simpang Ampat (PL)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7514, Name = "Ayer Itam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7515, Name = "Trolak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7516, Name = "Trong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7517, Name = "Tronoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7518, Name = "Ulu Bernam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7519, Name = "Ulu Kinta" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7520, Name = "Arau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7521, Name = "Tanjong Tualang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7522, Name = "Tapah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7523, Name = "Tapah Road" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7524, Name = "Teluk Intan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7525, Name = "Temoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7526, Name = "Tldm Lumut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7527, Name = "Sungkai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7528, Name = "Taiping" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7529, Name = "Tanah Rata (PK)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7530, Name = "Tanjong Malim" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7531, Name = "Tanjong Piandang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7532, Name = "Tanjong Rambutan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7533, Name = "Simpang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7534, Name = "Simpang Ampat Semanggol" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7535, Name = "Sitiawan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7536, Name = "Slim River" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7537, Name = "Sungai Siput" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7538, Name = "Sungai Sumun" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7539, Name = "Rantau Panjang (PK" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7540, Name = "Sauk" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7541, Name = "Selama" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7542, Name = "Selekoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7543, Name = "Seri Manjong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7544, Name = "Seri Manjung" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7545, Name = "Pangkor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7546, Name = "Pantai Remis" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7547, Name = "Parit" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7548, Name = "Parit Buntar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7549, Name = "Pengkalan Hulu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7550, Name = "Pusing" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7551, Name = "Lumut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7552, Name = "Malim Nawar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7553, Name = "Mambang Di Awan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7554, Name = "Manong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7555, Name = "Matang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7556, Name = "Padang Rengas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7557, Name = "Kuala Kangsar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7558, Name = "Kuala Kurau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7559, Name = "Kuala Sepetang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7560, Name = "Lambor Kanan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7561, Name = "Langkap" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7562, Name = "Lenggong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7563, Name = "Ipoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7564, Name = "Jeram (PK)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7565, Name = "Kampar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7566, Name = "Kampung Gajah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7567, Name = "Kampung Kepayang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7568, Name = "Kamunting" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7569, Name = "Chikus" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7570, Name = "Enggor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7571, Name = "Gerik" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7572, Name = "Gopeng" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7573, Name = "Hutan Melintang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7574, Name = "Intan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7575, Name = "Bruas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7576, Name = "Changkat Jering" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7577, Name = "Changkat Keruing" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7578, Name = "Chemor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7579, Name = "Chenderiang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7580, Name = "Chenderong Balai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7581, Name = "Bandar Seri Iskandar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7582, Name = "Batu Gajah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7583, Name = "Batu Kurau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7584, Name = "Behrang Stesen" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7585, Name = "Bidor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7586, Name = "Bota" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7587, Name = "Tanah Rata (PH)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7588, Name = "Temerloh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7589, Name = "Triang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7590, Name = "Ayer Tawar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7591, Name = "Bagan Datoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7592, Name = "Bagan Serai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7593, Name = "Raub" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7594, Name = "Ringlet" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7595, Name = "Sega" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7596, Name = "Sungai Koyan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7597, Name = "Sungai Lembing" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7598, Name = "Sungai Ruan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7599, Name = "Lurah Bilut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7600, Name = "Maran" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7601, Name = "Mentakab" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7602, Name = "Muadzam Shah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7603, Name = "Padang Tengku" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7604, Name = "Pekan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7605, Name = "Kemayan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7606, Name = "Kuala Krau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7607, Name = "Kuala Lipis" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7608, Name = "Kuala Rompin" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7609, Name = "Kuantan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7610, Name = "Lanchang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7611, Name = "Dong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7612, Name = "Gambang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7613, Name = "Genting Highlands" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7614, Name = "Jaya Gading" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7615, Name = "Jerantut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7616, Name = "Karak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7617, Name = "Brinchang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7618, Name = "Bukit Fraser" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7619, Name = "Bukit Goh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7620, Name = "Chenor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7621, Name = "Chini" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7622, Name = "Damak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7623, Name = "Balok" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7624, Name = "Bandar Bera" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7625, Name = "Bandar Pusat Jengka" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7626, Name = "Bandar Tun Abdul Razak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7627, Name = "Benta" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7628, Name = "Bentong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7629, Name = "Seremban" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7630, Name = "Si Rusa" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7631, Name = "Simpang Durian" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7632, Name = "Simpang Pertang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7633, Name = "Tampin" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7634, Name = "Tanjong Ipoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7635, Name = "Nilai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7636, Name = "Port Dickson" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7637, Name = "Pusat Bandar Palong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7638, Name = "Rantau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7639, Name = "Rembau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7640, Name = "Rompin" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7641, Name = "Kota" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7642, Name = "Kuala Klawang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7643, Name = "Kuala Pilah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7644, Name = "Labu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7645, Name = "Linggi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7646, Name = "Mantin" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7647, Name = "Bandar Baru Enstek" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7648, Name = "Bandar Seri Jempol" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7649, Name = "Batu Kikir" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7650, Name = "Gemas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7651, Name = "Gemencheh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7652, Name = "Johol" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7653, Name = "Merlimau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7654, Name = "Selandar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7655, Name = "Sungai Rambai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7656, Name = "Sungai Udang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7657, Name = "Tanjong Kling" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7658, Name = "Bahau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7659, Name = "Jasin" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7660, Name = "Kem Trendak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7661, Name = "Kuala Sungai Baru" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7662, Name = "Lubok China" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7663, Name = "Masjid Tanah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7664, Name = "Melaka" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7665, Name = "Air Keroh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7666, Name = "Alor Gajah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7667, Name = "Asahan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7668, Name = "Ayer Keroh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7669, Name = "Bemban" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7670, Name = "Durian Tunggal" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7671, Name = "Batu Caves (KL)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7672, Name = "Cheras (KL)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7673, Name = "Kuala Lumpur" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7674, Name = "Setapak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7675, Name = "Labuan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7676, Name = "Putrajaya" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7677, Name = "Rantau Panjang (KN)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7678, Name = "Selising" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7679, Name = "Tanah Merah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7680, Name = "Temangan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7681, Name = "Tumpat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7682, Name = "Wakaf Bharu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7683, Name = "Kuala Krai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7684, Name = "Machang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7685, Name = "Melor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7686, Name = "Pasir Mas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7687, Name = "Pasir Puteh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7688, Name = "Pulai Chondong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7689, Name = "Gua Musang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7690, Name = "Jeli" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7691, Name = "Kem Desa Pahlawan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7692, Name = "Ketereh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7693, Name = "Kota Bharu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7694, Name = "Kuala Balah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7695, Name = "Universiti Utara Malaysia" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7696, Name = "Yan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7697, Name = "Ayer Lanas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7698, Name = "Bachok" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7699, Name = "Cherang Ruku" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7700, Name = "Dabong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7701, Name = "Pendang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7702, Name = "Pokok Sena" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7703, Name = "Serdang (KH)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7704, Name = "Sik" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7705, Name = "Simpang Empat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7706, Name = "Sungai Petani" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7707, Name = "Kupang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7708, Name = "Langgar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7709, Name = "Langkawi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7710, Name = "Lunas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7711, Name = "Merbok" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7712, Name = "Padang Serai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7713, Name = "Kota Sarang Semut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7714, Name = "Kuala Kedah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7715, Name = "Kuala Ketil" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7716, Name = "Kuala Nerang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7717, Name = "Kuala Pegang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7718, Name = "Kulim" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7719, Name = "Jeniang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7720, Name = "Jitra" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7721, Name = "Karangan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7722, Name = "Kepala Batas (KH)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7723, Name = "Kodiang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7724, Name = "Kota Kuala Muda" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7725, Name = "Bandar Baharu" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7726, Name = "Bandar Bahru" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7727, Name = "Bedong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7728, Name = "Bukit Kayu Hitam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7729, Name = "Changloon" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7730, Name = "Gurun" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7731, Name = "Tangkak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7732, Name = "Ulu Tiram" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7733, Name = "Yong Peng" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7734, Name = "Alor Setar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7735, Name = "Ayer Hitam (KH)" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7736, Name = "Baling" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7737, Name = "Seri Gading" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7738, Name = "Seri Medan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7739, Name = "Simpang Rengam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7740, Name = "Sri Gading" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7741, Name = "Sri Medan" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7742, Name = "Sungai Mati" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7743, Name = "Rengam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7744, Name = "Rengit" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7745, Name = "Segamat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7746, Name = "Semerah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7747, Name = "Senai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7748, Name = "Senggarang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7749, Name = "Parit Raja" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7750, Name = "Parit Sulong" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7751, Name = "Pasir Gudang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7752, Name = "Pekan Nenas" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7753, Name = "Pengerang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7754, Name = "Pontian" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7755, Name = "Muar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7756, Name = "Nusajaya" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7757, Name = "Pagoh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7758, Name = "Paloh" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7759, Name = "Panchor" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7760, Name = "Parit Jawa" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7761, Name = "Kukup" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7762, Name = "Kulai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7763, Name = "Labis" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7764, Name = "Layang-Layang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7765, Name = "Masai" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7766, Name = "Mersing" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7767, Name = "Gugusan Taib Andak" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7768, Name = "Jementah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7769, Name = "Johor Bahru" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7770, Name = "Kahang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7771, Name = "Kluang" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7772, Name = "Kota Tinggi" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7773, Name = "Bukit Gambir" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7774, Name = "Bukit Pasir" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7775, Name = "Chaah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7776, Name = "Endau" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7777, Name = "Gelang Patah" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7778, Name = "Gerisek" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7779, Name = "Bandar Penawar" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7780, Name = "Bandar Tenggara" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7781, Name = "Batu Anam" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7782, Name = "Batu Pahat" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7783, Name = "Bekok" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7784, Name = "Benut" });
                db.TargetedGroupCities.Add(new TargetedGroupCities { StateID = 0, Code = 7789, Name = "Pendang (KH)" });
            }

            //DefaultSLAReminder(db);
            //DefaultParameterGroup(db);
            //DefaultTemplate(db);
        }

        public static void DefaultSLAReminder(DbEntities db)
        {
            db.SLAReminder.AddOrUpdate(s => s.NotificationType,

                // survey
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.SubmitSurvey, NotificationType = NotificationType.Submit_Survey_Creation, ETCode = "ET101RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelSurvey, NotificationType = NotificationType.Submit_Survey_Cancellation, ETCode = "ET102RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.PublishSurvey, NotificationType = NotificationType.Submit_Survey_Publication, ETCode = "ET103RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifySurvey, NotificationType = NotificationType.Verify_Survey_Creation, ETCode = "ET111RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_1, ETCode = "ET121RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_2, ETCode = "ET122RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_3, ETCode = "ET123RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApproveSurvey, NotificationType = NotificationType.Approve_Survey_Creation_Final, ETCode = "ET124RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.DistributeSurvey, NotificationType = NotificationType.Submit_Survey_Distribution, ETCode = "ET131RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.AnswerSurvey, NotificationType = NotificationType.Submit_Survey_Response, ETCode = "ET132RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                // publication
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.SubmitPublication, NotificationType = NotificationType.Submit_Publication_Creation, ETCode = "ET201RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelPublication, NotificationType = NotificationType.Submit_Publication_Cancellation, ETCode = "ET202RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.PublishPublication, NotificationType = NotificationType.Submit_Publication_Publication, ETCode = "ET203RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification, ETCode = "ET204RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.WithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal, ETCode = "ET205RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelModifyPublication, NotificationType = NotificationType.Submit_Publication_Modification_Cancellation, ETCode = "ET206RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.CancelWithdrawPublication, NotificationType = NotificationType.Submit_Publication_Withdrawal_Cancellation, ETCode = "ET207RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublication, NotificationType = NotificationType.Verify_Publication_Creation, ETCode = "ET211RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublicationModification, NotificationType = NotificationType.Verify_Publication_Modification, ETCode = "ET212RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.VerifyPublicationWithdrawal, NotificationType = NotificationType.Verify_Publication_Withdrawal, ETCode = "ET213RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_1, ETCode = "ET221RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_2, ETCode = "ET222RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_3, ETCode = "ET223RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublication, NotificationType = NotificationType.Approve_Publication_Creation_Final, ETCode = "ET224RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_1, ETCode = "ET225RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_2, ETCode = "ET226RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_3, ETCode = "ET227RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationModification, NotificationType = NotificationType.Approve_Publication_Modification_Final, ETCode = "ET228RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_1, ETCode = "ET229RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_2, ETCode = "ET230RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_3, ETCode = "ET231RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.ApprovePublicationWithdrawal, NotificationType = NotificationType.Approve_Publication_Withdrawal_Final, ETCode = "ET232RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                // refunds (will be superseded by payment below in future)
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.RefundPublication, NotificationType = NotificationType.Submit_Publication_Refund, ETCode = "ET241RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.RefundPublication, NotificationType = NotificationType.Approve_Publication_Refund_Incomplete, ETCode = "ET242RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.ResearchAndPublication, SLAEventType = SLAEventType.RefundPublication, NotificationType = NotificationType.Approve_Publication_Refund_Complete, ETCode = "ET243RP", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },

                // payment
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_GL, ETCode = "ET001PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_GL, ETCode = "ET002PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Payment, ETCode = "ET011PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Payment, ETCode = "ET012PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Submit_Refund_Request, ETCode = "ET021PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Verify_Refund_Request, ETCode = "ET022PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Approve_Refund_Request, ETCode = "ET023PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Pending_Refund, ETCode = "ET024PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Refund_Incomplete, ETCode = "ET025PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days },
                new SLAReminder { NotificationCategory = NotificationCategory.Payment, SLAEventType = SLAEventType.Payment, NotificationType = NotificationType.Payment_Refund_Complete, ETCode = "ET026PY", SLAResolutionTime = 3, IntervalDuration = 1, SLADurationType = SLADurationType.Days }

            );
        }

        public static void DefaultParameterGroup(DbEntities db)
        {

            foreach (TemplateParameterType paramType in Enum.GetValues(typeof(TemplateParameterType)))
            {
                SLAEventType EventType;

                int pType = (int)paramType;

                if (pType >= 101 && pType <= 120)
                {
                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.SubmitSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.PublishSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifySurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApproveSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.DistributeSurvey, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.AnswerSurvey, TemplateParameterType = paramType });

                    continue;

                }

                if (pType >= 121 && pType <= 130)
                {
                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.SubmitPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.PublishPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ModifyPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.WithdrawPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelModifyPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.CancelWithdrawPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyPublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationModification, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.VerifyPublicationWithdrawal, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApprovePublication, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationModification, TemplateParameterType = paramType });

                    db.ParameterGroup.AddOrUpdate(p => new { p.TemplateParameterType, p.SLAEventType },
                    new ParameterGroup { SLAEventType = SLAEventType.ApprovePublicationWithdrawal, TemplateParameterType = paramType });

                    continue;

                }

                if (pType >= 131 && pType <= 140)
                {
                    // will be superseded by below
                    db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                    new ParameterGroup { SLAEventType = SLAEventType.RefundPublication, TemplateParameterType = paramType });

                    // other refunds can add here
                    db.ParameterGroup.AddOrUpdate(p => p.TemplateParameterType,
                    new ParameterGroup { SLAEventType = SLAEventType.Refund, TemplateParameterType = paramType });

                    continue;
                }
            }


        }

        public static void DefaultTemplate(DbEntities db)
        {

            var user = db.User.Local.Where(r => r.Name.Contains("System Admin")).FirstOrDefault() ?? db.User.Where(r => r.Name.Contains("System Admin")).FirstOrDefault();

            //db.NotificationTemplates.AddOrUpdate(t => t.NotificationType,
            //    new NotificationTemplate
            //    {
            //        NotificationType = NotificationType.ResetPassword,
            //        NotificationCategory = NotificationCategory.System,
            //        TemplateName = NotificationType.ResetPassword.DisplayName(),
            //        TemplateRefNo = "T" + ((int)NotificationType.ResetPassword).ToString(),
            //        enableEmail = true,
            //        TemplateSubject = "New FE Portal Account Created",
            //        TemplateMessage = "&lt;p&gt;Dear&amp;nbsp;&lt;span style=&quot;font-size: 1rem;&quot;&gt;[#UserFullName],&lt;/span&gt;&lt;/p&gt;&lt;p&gt;You can activate your account [#Link].&amp;nbsp;&lt;/p&gt;&lt;p&gt;Your login details:&lt;/p&gt;&lt;p&gt;[#LoginDetail]&lt;br&gt;&lt;/p&gt;&lt;p&gt;&lt;span style=&quot;color: rgb(255, 255, 255); font-size: 12px; text-align: center; white-space: nowrap; background-color: rgb(41, 182, 246);&quot;&gt;&lt;br&gt;&lt;/span&gt;&lt;/p&gt;",
            //        enableSMSMessage = false,
            //        SMSMessage = "SMS Message Template",
            //        enableWebMessage = false,
            //        WebMessage = "Web Message Template",
            //        CreatedDate = DateTime.Now,
            //        CreatedBy = user.Id,
            //        User = user,
            //        Display = true
            //    });

        }

    }

}
