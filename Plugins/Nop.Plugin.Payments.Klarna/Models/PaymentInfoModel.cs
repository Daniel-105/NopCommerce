using Nop.Web.Areas.Admin.Models.ShoppingCart;
using Nop.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public record PaymentInfoModel : BaseNopModel
    {
        public PaymentInfoModel() => this.AvailableTypes = new List<string>();

        public IList<string> AvailableTypes { get; set; }

        public string PaymentExtraFieldsShowOn { get; set; }

        public string SelectedType { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string PhoneIndicative { get; set; }

        public string FiscalNumber { get; set; }
        public string DescriptionText { get; internal set; }

        public ShoppingCartModel ShoppingCartModel { get; set; }
    }
}
