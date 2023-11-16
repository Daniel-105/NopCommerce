using System.Text.Json.Serialization;
using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class KlarnaCheckOutRequestModel
    {
        [JsonPropertyName("locale")] 
        [JsonProperty("locale", NullValueHandling = NullValueHandling.Ignore)]
        public string Locale { get; set; }

        [JsonPropertyName("order_amount")] 
        [JsonProperty("order_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OrderAmount { get; set; }

        [JsonPropertyName("order_lines")] 
        [JsonProperty("order_lines", NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderLine> OrderLines { get; set; }

        [JsonPropertyName("order_tax_amount")] 
        [JsonProperty("order_tax_amount", NullValueHandling = NullValueHandling.Ignore)]
        public long? OrderTaxAmount { get; set; }

        [JsonPropertyName("purchase_country")]
        [JsonProperty("purchase_country", NullValueHandling = NullValueHandling.Ignore)]
        public string PurchaseCountry { get; set; }

        [JsonPropertyName("purchase_currency")]
        [JsonProperty("purchase_currency", NullValueHandling = NullValueHandling.Ignore)]
        public string PurchaseCurrency { get; set; }
    }

    public class OrderLine
    {
        [JsonPropertyName("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonPropertyName("quantity")] 
        [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Quantity { get; set; }

        [JsonPropertyName("reference")] 
        [JsonProperty("reference", NullValueHandling = NullValueHandling.Ignore)]
        public string Reference { get; set; }

        [JsonPropertyName("tax_rate")] 
        [JsonProperty("tax_rate", NullValueHandling = NullValueHandling.Ignore)]
        public long? TaxRate { get; set; }

        [JsonPropertyName("total_amount")] 
        [JsonProperty("total_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalAmount { get; set; }

        [JsonPropertyName("total_discount_amount")]
        [JsonProperty("total_discount_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalDiscountAmount { get; set; }

        [JsonPropertyName("total_tax_amount")]
        [JsonProperty("total_tax_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalTaxAmount { get; set; }

        [JsonPropertyName("type")]
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        
        [JsonPropertyName("unit_price")]
        [JsonProperty("unit_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? UnitPrice { get; set; }
    }
}
