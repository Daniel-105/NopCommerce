using Nop.Core.Configuration;
using System;

namespace Nop.Plugin.Payments.Klarna
{
    public class KlarnaPaymentSettings : ISettings
    {
        public bool UseTestEnvironment { get; set; }

        public string AccountId { get; set; }

        public string ApiKey { get; set; }

        public bool ActivateMBWay { get; set; }

        public bool ActivateMultibanco { get; set; }

        public bool ActivateCreditCard { get; set; }
        public bool ActivateSantanderConsumer { get; set; }
        public int SantanderConsumerMin { get; set; }
        public int SantanderConsumerMax { get; set; }

        public bool AdditionalFeePercentage { get; set; }

        public Decimal AdditionalFee { get; set; }
        public bool EnableFeature { get; internal set; }
        public string Name { get; internal set; }
        public string DescriptionText { get; internal set; }





        // For Klarna
        public string UserName { get; set; }
        public string Password { get; set; }
        public string KlarnaApiUrl { get; set; }


        public string PlaygroundUserName { get; set; }
        public string PaygroundPassword { get; set; }
        public string playgroundKlarnaApiUrl { get; set; }

        public bool UsePlayground { get; set; }
    }
}