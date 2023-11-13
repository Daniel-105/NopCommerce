using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.UseTestEnvironment")]
        public bool UseTestEnvironment { get; set; }

        public bool UseTestEnvironment_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.AccountId")]
        public string AccountId { get; set; }

        public bool AccountId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.ApiKey")]
        public string ApiKey { get; set; }

        public bool ApiKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.ActivateMBWay")]
        public bool ActivateMBWay { get; set; }

        public bool ActivateMBWay_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.ActivateMultibanco")]
        public bool ActivateMultibanco { get; set; }
        public bool ActivateMultibanco_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.ActivateSantanderConsumer")]
        public bool ActivateSantanderConsumer { get; set; }
        public bool ActivateSantanderConsumer_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.SantanderConsumerMin")]
        public int SantanderConsumerMin { get; set; }
        public bool SantanderConsumerMin_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.SantanderConsumerMax")]
        public int SantanderConsumerMax { get; set; }
        public bool SantanderConsumerMax_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.ActivateCreditCard")]
        public bool ActivateCreditCard { get; set; }

        public bool ActivateCreditCard_OverrideForStore { get; set; }

        //Name that will show up on the page
        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }

        public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.AdditionalFee")]
        public Decimal AdditionalFee { get; set; }

        public bool AdditionalFee_OverrideForStore { get; set; }
        public bool EnableFeature { get; internal set; }
    }
}