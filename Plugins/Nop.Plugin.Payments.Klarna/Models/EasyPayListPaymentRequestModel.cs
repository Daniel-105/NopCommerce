using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class KlarnaListPaymentRequestModel
    {
        public int Page { get; set; }

        public int RecordsPerPage { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
