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