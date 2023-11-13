using Nop.Web.Framework.Models;
using System;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public record CheckoutCompletedModel : BaseNopModel
    {
        public int OrderId { get; set; }

        public string CustomOrderNumber { get; set; }

        public bool OnePageCheckoutEnabled { get; set; }

        public string MBEntity { get; set; }

        public string MBReference { get; set; }

        public Decimal OrderTotal { get; set; }
        public string UrlSC { get; set; }

    }
}
