using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class EasyPayPaymentDetailsResponseModel
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("customer")]
        public Customer Customer { get; set; }
        [JsonProperty("payment_status")]
        public string PaymentStatus { get; set; }
    }

    public class EasyPayFrequentPaymentDetailsResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("customer")]
        public Customer Customer { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
