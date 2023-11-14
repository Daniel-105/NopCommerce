using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;
using Nop.Plugin.Payments.Klarna.Models;
using Nop.Web.Models.Order;
using Nop.Web.Models.ShoppingCart;
using Nop.Core.Domain.Orders;
using Nop.Services.Orders;
using Nop.Web.Factories;
 
namespace Nop.Plugin.Payments.Klarna.Components
{
 
    [ViewComponent(Name = "PaymentKlarna")]
    public class PaymentKlarnaViewComponent : NopViewComponent
    {
        private readonly KlarnaPaymentSettings _klarnaPaymentSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IShoppingCartService _cart;
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
 
        public PaymentKlarnaViewComponent(
            KlarnaPaymentSettings klarnaPaymentSettings,
            ILocalizationService localizationService,
            IStoreContext storeContext,
            IShoppingCartService cart,
            IWorkContext workContext,
            IShoppingCartModelFactory shoppingCartModelFactory)
        {
            _klarnaPaymentSettings = klarnaPaymentSettings;
            _localizationService = localizationService;
            _storeContext = storeContext;
            _workContext = workContext;
            _cart = cart;
            _shoppingCartModelFactory = shoppingCartModelFactory;
        }
 
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var store = await _storeContext.GetCurrentStoreAsync();
            var cart = await _cart.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
            var modelShopping = new ShoppingCartModel();
            var model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(modelShopping, cart, false);

 
            return View("~/Plugins/Payments.Klarna/Views/PaymentInfo.cshtml", model);
        }
    }
}