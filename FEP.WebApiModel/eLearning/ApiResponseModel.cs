using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEP.WebApiModel.eLearning
{
    public class TrxResult<T>
    {
        public int CourseId { get; set; }
        public int ObjectId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
