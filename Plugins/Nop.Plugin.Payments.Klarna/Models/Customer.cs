using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class Customer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("phone_indicative")]
        public string PhoneIndicative { get; set; }

        [JsonProperty("fiscal_number")]
        public string FiscalNumber { get; set; }

        [JsonProperty("key")]
        public string CustomerKey { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }
    }
}
