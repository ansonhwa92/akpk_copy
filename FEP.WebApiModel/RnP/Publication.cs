using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEP.Model;

namespace FEP.WebApiModel
{
    public class PublicationApiModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Author { get; set; }
        public string Coauthor { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ISBN { get; set; }
        public bool Free { get; set; }
        public bool Hardcopy { get; set; }
        public bool Digitalcopy { get; set; }
        public bool HDcopy { get; set; }
        public float HPrice { get; set; }
        public float DPrice { get; set; }
        public float HDPrice { get; set; }
        public string ProofOfApproval { get; set; }
        public int StockBalance { get; set; }
        public string WithdrawalReason { get; set; }
        public string ProofOfWithdrawal { get; set; }
        public DateTime? DateAdded { get; set; }
        public PublicationStatus Status { get; set; }
        public PublicationWithdrawalStatus WStatus { get; set; }
        public int ViewCount { get; set; }
        public int PurchaseCount { get; set; }
        public List<PublicationApproval> Approvals { get; set; }
        public List<PublicationWithdrawal> Withdrawals { get; set; }
    }

    public class PublicationCategoryApiModel
    {

    }
}
