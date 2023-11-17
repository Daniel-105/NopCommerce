using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Areas.Admin.Models.ShoppingCart;
using Nop.Web.Models.Order;

namespace Nop.Plugin.Payments.Klarna.Models
{
    public class KlarnaRequestDataVM
    {
        public string jsonData { get; set; }
        public string ResponseContent { get; set; }
        //public ShoppingCartModel ShoppingCartModel { get; set; }
    }
}
