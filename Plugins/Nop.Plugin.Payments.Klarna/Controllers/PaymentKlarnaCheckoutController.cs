using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Services.Tax;
using Nop.Web.Controllers;
using Nop.Web.Factories;
namespace Nop.Plugin.Payments.Klarna.Controllers
{
    public class PaymentKlarnaCheckoutController : CheckoutController
    {
        public PaymentKlarnaCheckoutController(AddressSettings addressSettings,
            CaptchaSettings captchaSettings,
            CustomerSettings customerSettings,
            IAddressAttributeParser addressAttributeParser,
            IAddressModelFactory addressModelFactory,
            IAddressService addressService,
            ICheckoutModelFactory checkoutModelFactory,
            ICountryService countryService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            ILogger logger,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IPaymentPluginManager paymentPluginManager,
            IPaymentService paymentService,
            IProductService productService,
            IShippingService shippingService,
            IShoppingCartService shoppingCartService,
            IStoreContext storeContext,
            ITaxService taxService,
            IWebHelper webHelper,
            IWorkContext workContext,
            OrderSettings orderSettings,
            PaymentSettings paymentSettings,
            RewardPointsSettings rewardPointsSettings,
            ShippingSettings shippingSettings,
            TaxSettings taxSettings) :
            base(addressSettings, captchaSettings, customerSettings, addressAttributeParser,
                addressModelFactory, addressService, checkoutModelFactory, countryService,
                customerService, genericAttributeService, localizationService, logger,
                orderProcessingService, orderService, paymentPluginManager, paymentService,
                productService, shippingService, shoppingCartService, storeContext, taxService,
                webHelper, workContext, orderSettings, paymentSettings, rewardPointsSettings,
                shippingSettings, taxSettings)
        {
            _addressSettings = addressSettings;
            _customerSettings = customerSettings;
            _addressAttributeParser = addressAttributeParser;
            _addressModelFactory = addressModelFactory;
            _addressService = addressService;
            _checkoutModelFactory = checkoutModelFactory;
            _countryService = countryService;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _logger = logger;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _paymentPluginManager = paymentPluginManager;
            _paymentService = paymentService;
            _productService = productService;
            _shippingService = shippingService;
            _shoppingCartService = shoppingCartService;
            _storeContext = storeContext;
            _webHelper = webHelper;
            _workContext = workContext;
            _orderSettings = orderSettings;
            _paymentSettings = paymentSettings;
            _rewardPointsSettings = rewardPointsSettings;
            _shippingSettings = shippingSettings;
        }
        [Route("checkout/completed/{orderId:regex(\\d*)}")]
        public async override Task<IActionResult> Completed(int? orderId)
        {
            IActionResult result;
            result = this.View("~/Plugins/Payments.Klarna/Views/CheckoutCompleted.cshtml", new Nop.Plugin.Payments.Klarna.Models.CheckoutCompletedModel()
            {
                OrderId = orderId.Value,
                CustomOrderNumber = "0",
                OnePageCheckoutEnabled = true
            });
            return result;
        }

        private readonly AddressSettings _addressSettings;

        private readonly CaptchaSettings _captchaSettings;
        private readonly ITaxService _taxService;
        private readonly TaxSettings _taxSettings;

        private readonly CustomerSettings _customerSettings;


        private readonly IAddressAttributeParser _addressAttributeParser;

        private readonly IAddressModelFactory _addressModelFactory;


        private readonly IAddressService _addressService;


        private readonly ICheckoutModelFactory _checkoutModelFactory;


        private readonly ICountryService _countryService;


        private readonly ICustomerService _customerService;


        private readonly IGenericAttributeService _genericAttributeService;


        private readonly ILocalizationService _localizationService;


        private readonly ILogger _logger;


        private readonly IOrderProcessingService _orderProcessingService;


        private readonly IOrderService _orderService;


        private readonly IPaymentService _paymentService;


        private readonly IShippingService _shippingService;
        private readonly IPaymentPluginManager _paymentPluginManager;


        private readonly IShoppingCartService _shoppingCartService;


        private readonly IProductService _productService;


        private readonly IStoreContext _storeContext;


        private readonly IWebHelper _webHelper;


        private readonly IWorkContext _workContext;


        private readonly OrderSettings _orderSettings;


        private readonly PaymentSettings _paymentSettings;

        private readonly RewardPointsSettings _rewardPointsSettings;


        private readonly ShippingSettings _shippingSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;


    }
}



