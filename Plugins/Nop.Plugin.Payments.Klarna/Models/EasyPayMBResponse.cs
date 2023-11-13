using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class EasyPayMBResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public List<string> Message { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("method")]
        public MethodEasyPay Method { get; set; }

        [JsonProperty("customer")]
        public Capture Customer { get; set; }

        [JsonProperty("capture")]
        public Capture Capture { get; set; }
    }
    public class MethodEasyPay
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("entity")]        
        public long Entity { get; set; }

        [JsonProperty("reference")]        
        public long Reference { get; set; }
    }
}
