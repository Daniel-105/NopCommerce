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
    }
}