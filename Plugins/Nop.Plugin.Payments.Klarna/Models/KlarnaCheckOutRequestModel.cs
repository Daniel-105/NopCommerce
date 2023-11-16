using System.Text.Json.Serialization;
using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class KlarnaCheckOutRequestModel
    {
        [JsonProperty("acquiring_channel", NullValueHandling = NullValueHandling.Ignore)]
        public string AcquiringChannel { get; set; }

        [JsonProperty("attachment", NullValueHandling = NullValueHandling.Ignore)]
        public Attachment Attachment { get; set; }

        [JsonProperty("billing_address", NullValueHandling = NullValueHandling.Ignore)]
        public IngAddress BillingAddress { get; set; }

        [JsonProperty("custom_payment_method_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> CustomPaymentMethodIds { get; set; }

        [JsonProperty("customer", NullValueHandling = NullValueHandling.Ignore)]
        public CustomerCheckout Customer { get; set; }

        [JsonProperty("design", NullValueHandling = NullValueHandling.Ignore)]
        public string Design { get; set; }

        [JsonPropertyName("locale")] 
        [JsonProperty("locale", NullValueHandling = NullValueHandling.Ignore)]
        public string Locale { get; set; }

        [JsonProperty("merchant_data", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchantData { get; set; }

        [JsonProperty("merchant_reference1", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchantReference1 { get; set; }

        [JsonProperty("merchant_reference2", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchantReference2 { get; set; }

        [JsonProperty("merchant_urls", NullValueHandling = NullValueHandling.Ignore)]
        public MerchantUrls MerchantUrls { get; set; }

        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public Options Options { get; set; }

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

        [JsonProperty("shipping_address", NullValueHandling = NullValueHandling.Ignore)]
        public IngAddress ShippingAddress { get; set; }

        [JsonProperty("intent", NullValueHandling = NullValueHandling.Ignore)]
        public string Intent { get; set; }
    }

    public class Attachment
    {
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string Body { get; set; }

        [JsonProperty("content_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }
    }

    public class IngAddress
    {
        [JsonProperty("attention", NullValueHandling = NullValueHandling.Ignore)]
        public string Attention { get; set; }

        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("family_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FamilyName { get; set; }

        [JsonProperty("given_name", NullValueHandling = NullValueHandling.Ignore)]
        public string GivenName { get; set; }

        [JsonProperty("organization_name", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationName { get; set; }

        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty("postal_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty("street_address", NullValueHandling = NullValueHandling.Ignore)]
        public string StreetAddress { get; set; }

        [JsonProperty("street_address2", NullValueHandling = NullValueHandling.Ignore)]
        public string StreetAddress2 { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public class CustomerCheckout
    {
        [JsonProperty("date_of_birth", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DateOfBirth { get; set; }

        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }

        [JsonProperty("last_four_ssn", NullValueHandling = NullValueHandling.Ignore)]
        public string LastFourSsn { get; set; }

        [JsonProperty("national_identification_number", NullValueHandling = NullValueHandling.Ignore)]
        public string NationalIdentificationNumber { get; set; }

        [JsonProperty("organization_entity_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationEntityType { get; set; }

        [JsonProperty("organization_registration_id", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationRegistrationId { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("vat_id", NullValueHandling = NullValueHandling.Ignore)]
        public string VatId { get; set; }
    }

    public class MerchantUrls
    {
        [JsonProperty("confirmation", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Confirmation { get; set; }

        [JsonProperty("notification", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Notification { get; set; }

        [JsonProperty("push", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Push { get; set; }

        [JsonProperty("authorization", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Authorization { get; set; }
    }

    public class Options
    {
        [JsonProperty("color_border", NullValueHandling = NullValueHandling.Ignore)]
        public string ColorBorder { get; set; }

        [JsonProperty("color_border_selected", NullValueHandling = NullValueHandling.Ignore)]
        public string ColorBorderSelected { get; set; }

        [JsonProperty("color_details", NullValueHandling = NullValueHandling.Ignore)]
        public string ColorDetails { get; set; }

        [JsonProperty("color_text", NullValueHandling = NullValueHandling.Ignore)]
        public string ColorText { get; set; }

        [JsonProperty("radius_border", NullValueHandling = NullValueHandling.Ignore)]
        public string RadiusBorder { get; set; }
    }

    public class OrderLine
    {
        [JsonProperty("image_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageUrl { get; set; }

        [JsonProperty("merchant_data", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchantData { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("product_identifiers", NullValueHandling = NullValueHandling.Ignore)]
        public ProductIdentifiers ProductIdentifiers { get; set; }

        [JsonProperty("product_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ProductUrl { get; set; }

        [JsonPropertyName("quantity")] 
        [JsonProperty("quantity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Quantity { get; set; }

        [JsonProperty("quantity_unit", NullValueHandling = NullValueHandling.Ignore)]
        public string QuantityUnit { get; set; }

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

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        
        [JsonPropertyName("unit_price")]
        [JsonProperty("unit_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? UnitPrice { get; set; }

        [JsonProperty("subscription", NullValueHandling = NullValueHandling.Ignore)]
        public Subscription Subscription { get; set; }
    }

    public class ProductIdentifiers
    {
        [JsonProperty("brand", NullValueHandling = NullValueHandling.Ignore)]
        public string Brand { get; set; }

        [JsonProperty("category_path", NullValueHandling = NullValueHandling.Ignore)]
        public string CategoryPath { get; set; }

        [JsonProperty("global_trade_item_number", NullValueHandling = NullValueHandling.Ignore)]
        public string GlobalTradeItemNumber { get; set; }

        [JsonProperty("manufacturer_part_number", NullValueHandling = NullValueHandling.Ignore)]
        public string ManufacturerPartNumber { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }
    }

    public class Subscription
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public string Interval { get; set; }

        [JsonProperty("interval_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? IntervalCount { get; set; }
    }

}
