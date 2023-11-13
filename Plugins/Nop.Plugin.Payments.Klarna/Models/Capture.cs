using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class Capture
    {
        [JsonProperty("transaction_key")]
        public string TransactionKey { get; set; }

        [JsonProperty("descriptive")]
        public string Descriptive { get; set; }
    }
}
