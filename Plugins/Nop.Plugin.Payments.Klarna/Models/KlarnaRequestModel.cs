using Newtonsoft.Json;
using Nop.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class KlarnaRequestModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("expiration_time")]
        public DateTime ExpirationTime { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("capture")]
        public Capture Capture { get; set; }

        [JsonProperty("key")]
        public string MerchantKey { get; set; }

        [JsonProperty("value")]
        public Decimal Value { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
        //[JsonProperty("sdd_mandate")]
        //public EasyPaySddMandateModel SddMandate { get; set; } // "cc", "mb", "mbw"
    }
}
