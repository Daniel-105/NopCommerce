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

        public bool AdditionalFeePercentage { get; set; }

        public Decimal AdditionalFee { get; set; }




        // Check if you want to use playground
        public bool UsePlayground { get; set; }


        // For Klarna Real Api
        public string UserName { get; set; }
        public string Password { get; set; }
        public string KlarnaApiUrl { get; set; }


        // For Klarna Playground Api
        public string PlaygroundUserName { get; set; }
        public string PaygroundPassword { get; set; }
        public string playgroundKlarnaApiUrl { get; set; }

    }
}