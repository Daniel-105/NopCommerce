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




        // For Klarna Real Api
        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.UserName")]
        public string? UserName { get; set; }
        public bool UserName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.Password")]
        public string? Password { get; set; }
        public bool Password_OverrideForStore { get; set; }
        
        [NopResourceDisplayName("Plugins.Payments.Klarna.Fields.KlarnaApiUrl")]
        public string? KlarnaApiUrl { get; set; }
        public bool KlarnaApiUrl_OverrideForStore { get; set; }


        // For Klarna Playground Api
        public string? PlaygroundUserName { get; set; }
        public bool PlaygroundUserName_OverrideForStore { get; set; }

        public string? PaygroundPassword { get; set; }
        public bool PlaygroundPassword_OverrideForStore { get; set; }

        public string? playgroundKlarnaApiUrl { get; set; }
        public bool PlaygroundKlarnaApiUrl_OverrideForStore { get; set; }

        // Check if you want to use playground
        public bool UsePlayground { get; set; }
        public bool Playground_OverrideForStore { get; set; }
    }
}