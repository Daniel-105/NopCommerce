using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.Klarna.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Newtonsoft.Json.Linq;
using Nop.Services.Payments;
using Nop.Plugin.Payments.Klarna;
using Nop.Services.Payments;
using Nop.Core.Http.Extensions;
using Nop.Web.Models.Order;
using Nop.Web.Models.ShoppingCart;
using Nop.Core.Domain.Orders;
using Nop.Services.Orders;
using Nop.Web.Factories;
using System.Text.Json;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.Extensions.Options;
using RestSharp;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nop.Core.Domain.Discounts;

namespace Nop.Plugin.Payments.Klarna.Controllers
{
    [AuthorizeAdmin(false)]
    [Area("Admin")]
    public class PaymentKlarnaController : BasePaymentController
    {

        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IShoppingCartService _cart;
        private readonly IWorkContext _workContext;
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly KlarnaPaymentSettings _klarnaPaymentSettings;
        private readonly ILocalizationService _localizationService;


        private readonly IPermissionService _permissionService;


        private readonly ISettingService _settingService;


        private readonly IStoreContext _storeContext;

        public PaymentKlarnaController(ILocalizationService localizationService,
            IPermissionService permissionService,
            KlarnaPaymentSettings klarnaPaymentSettings,
            ISettingService settingService,
            IStoreContext storeContext,
            IHttpClientFactory httpClientFactory,
            IWorkContext workContext,
            IShoppingCartModelFactory shoppingCartModelFactory,
            IPaymentService paymentService,
            IShoppingCartService cart
            )
        {
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            _httpClient = httpClientFactory.CreateClient();
            this._paymentService = paymentService;
            _workContext = workContext;
            _shoppingCartModelFactory = shoppingCartModelFactory;
            _klarnaPaymentSettings = klarnaPaymentSettings;
            _cart = cart;

        }
        public async Task<IActionResult> Configure()
        {
            bool flag = !await this._permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods);
            IActionResult result;
            if (flag)
            {
                result = this.AccessDeniedView();
            }
            else
            {
                int activeStoreScopeConfiguration = await this._storeContext.GetActiveStoreScopeConfigurationAsync();
                KlarnaPaymentSettings klarnaPaymentSettings = await this._settingService.LoadSettingAsync<KlarnaPaymentSettings>(activeStoreScopeConfiguration);

                // Populating the model
                ConfigurationModel configurationModel = new ConfigurationModel
                {
                    //using the klarnaPaymentSettings model and binding it with the UsePlayground, UserName and Password 
                    //of the ConfigurationModel
                    UsePlayground = klarnaPaymentSettings.UsePlayground,

                    // for klarna
                    UserName = klarnaPaymentSettings.UserName,
                    Password = klarnaPaymentSettings.Password,
                    KlarnaApiUrl = klarnaPaymentSettings.KlarnaApiUrl,

                    //For Klarna Playground
                    PlaygroundUserName = klarnaPaymentSettings.PlaygroundUserName,
                    PaygroundPassword = klarnaPaymentSettings.PaygroundPassword,
                    playgroundKlarnaApiUrl = klarnaPaymentSettings.playgroundKlarnaApiUrl

                };
                bool flag2 = activeStoreScopeConfiguration > 0;
                if (flag2)
                {
                    configurationModel.Playground_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.UsePlayground, activeStoreScopeConfiguration);

                    // For Klarna
                    configurationModel.KlarnaApiUrl_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.KlarnaApiUrl, activeStoreScopeConfiguration);
                    configurationModel.UserName_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.UserName, activeStoreScopeConfiguration);
                    configurationModel.Password_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.Password, activeStoreScopeConfiguration);

                    // For Klarna playground
                    configurationModel.PlaygroundKlarnaApiUrl_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.playgroundKlarnaApiUrl, activeStoreScopeConfiguration);
                    configurationModel.PlaygroundUserName_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.PlaygroundUserName, activeStoreScopeConfiguration);
                    configurationModel.PlaygroundPassword_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.PaygroundPassword, activeStoreScopeConfiguration);
                }
                result = this.View("~/Plugins/Payments.Klarna/Views/Configure.cshtml", configurationModel);
            }
            return result;
        }


        [HttpPost]
        //[AdminAntiForgery(false)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            bool flag = !await this._permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods);
            IActionResult result;
            if (flag)
            {
                result = this.AccessDeniedView();
            }
            else
            {
                // In case the credentials aren't been saved in the database, the commented line was that line that was there before
                //bool flag2 = !base.ModelState.IsValid;
                bool flag2 = model == null;
                if (flag2)
                {
                    result = await this.Configure();
                }
                else
                {
                    int activeStoreScopeConfiguration = await this._storeContext.GetActiveStoreScopeConfigurationAsync();

                    KlarnaPaymentSettings klarnaPaymentSettings = await this._settingService.LoadSettingAsync<KlarnaPaymentSettings>(activeStoreScopeConfiguration);


                    klarnaPaymentSettings.UsePlayground= model.UsePlayground;

                    // for klarna
                    klarnaPaymentSettings.UserName = model.UserName;
                    klarnaPaymentSettings.Password = model.Password;
                    klarnaPaymentSettings.KlarnaApiUrl = model.KlarnaApiUrl;

                    //For Klarna Playground
                    klarnaPaymentSettings.PlaygroundUserName = model.PlaygroundUserName;
                    klarnaPaymentSettings.PaygroundPassword = model.PaygroundPassword;
                    klarnaPaymentSettings.playgroundKlarnaApiUrl = model.playgroundKlarnaApiUrl;


                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.UsePlayground, model.Playground_OverrideForStore, activeStoreScopeConfiguration, false);

                    //For Klarna
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.KlarnaApiUrl, model.KlarnaApiUrl_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.UserName, model.UserName_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.Password, model.Password_OverrideForStore, activeStoreScopeConfiguration, false);


                    //For Klarna
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.playgroundKlarnaApiUrl, model.PlaygroundKlarnaApiUrl_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.PlaygroundUserName, model.PlaygroundUserName_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.PaygroundPassword, model.PlaygroundPassword_OverrideForStore, activeStoreScopeConfiguration, false);

                    await this._settingService.ClearCacheAsync();


                    //this.SuccessNotification(await this._localizationService.GetResourceAsync("Admin.Plugins.Saved"), true);
                    result = await this.Configure();
                }
            }
            return result;
        }

        [Route("/GetClientToken")]
        [HttpPost]
        public async Task<JsonResult> GetClientToken()
        {
            try
            {
                // Initializing the variables
                string apiUrl;
                string username;
                string password;

                int activeStoreScopeConfiguration = await this._storeContext.GetActiveStoreScopeConfigurationAsync();
                KlarnaPaymentSettings klarnaPaymentSettings = await this._settingService.LoadSettingAsync<KlarnaPaymentSettings>(activeStoreScopeConfiguration);


                // For the playground
                if (klarnaPaymentSettings.UsePlayground == true)
                {
                    //// Replace with your Basic Authentication credentials
                    //string apiUrl = "https://api.playground.klarna.com/payments/v1/sessions";
                    apiUrl = klarnaPaymentSettings.playgroundKlarnaApiUrl;

                    //string username = "PK131523_f3731dbad121";
                    username = klarnaPaymentSettings.PlaygroundUserName;

                    //string password = "qSNhaFgn6Ls3bj1P";
                    password = klarnaPaymentSettings.PaygroundPassword;
                }
                else
                {
                    // Replace with your Basic Authentication credentials

                    //string apiUrl = "https://api.playground.klarna.com/payments/v1/sessions";
                    apiUrl = klarnaPaymentSettings.KlarnaApiUrl;

                    //string username = "PK131523_f3731dbad121";
                    username = klarnaPaymentSettings.UserName;


                    //string password = "qSNhaFgn6Ls3bj1P";
                    password = klarnaPaymentSettings.Password;

                }


                var store = await _storeContext.GetCurrentStoreAsync();
                var cart = await _cart.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
                var modelShopping = new ShoppingCartModel();
                var model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(modelShopping, cart, false);

                var amountOfItems = model.Items.Count();

                var modelCheckout = new KlarnaCheckOutRequestModel();

                modelCheckout.PurchaseCountry = "PT";
                modelCheckout.PurchaseCurrency = "EUR";
                modelCheckout.Locale = "pt-PT";
                modelCheckout.OrderTaxAmount = 0;
                modelCheckout.OrderAmount = 0;

                var orderLines = new List<OrderLine>();

                Dictionary<int, List<object>> modelValue = new Dictionary<int, List<object>>();

                foreach (var item in model.Items)
                {
                    var order = new OrderLine();
                    order.Type = "physical";
                    order.Reference = item.ProductId.ToString();
                    order.Name = item.ProductName;
                    order.Quantity = item.Quantity;
                    order.UnitPrice = Convert.ToDecimal(item.UnitPrice.Replace("$", "").Replace("€", "").Replace(".", ","));
                    order.TaxRate = 0;
                    order.TotalAmount = Convert.ToDecimal(item.SubTotal.Replace("$", "").Replace("€", "").Replace(".", ","));
                    order.TotalDiscountAmount = 0;
                    order.TotalTaxAmount = 0;
                    modelCheckout.OrderAmount = modelCheckout.OrderAmount + order.TotalAmount;

                    orderLines.Add(order);
                }

                modelCheckout.OrderLines = orderLines;


                var jsonDataC = JsonSerializer.Serialize(modelCheckout);
                var client = new RestClient(apiUrl);
                var request = new RestRequest("", Method.POST);
                request.AddHeader("Content-Type", "application/json");

                var basicAuth = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                               .GetBytes(username + ":" + password));

                request.AddHeader("Authorization", "Basic " + basicAuth);

                request.AddJsonBody(jsonDataC);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    // Read the response body as a string
                    string responseContent = response.Content.ToString();

                    //JObject json = JObject.Parse(responseContent);
                    var klarnaRequestDataVM = new KlarnaRequestDataVM
                    {
                        ResponseContent = responseContent,
                        jsonData = jsonDataC
                    };
                    return Json(klarnaRequestDataVM);
                }
                else
                {
                    // Handle the case where the request was not successful
                    return Json(new { error = "API request failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }


        [Route("/PlaceOrder")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string authorizationToken)
        {
            try
            {

                // Replace with your actual API endpoint URL
                string apiUrl = "https://api.playground.klarna.com/payments/v1/authorizations/" + authorizationToken + "/order";

                // Replace with your Basic Authentication credentials
                string username = "PK131523_f3731dbad121";
                string password = "qSNhaFgn6Ls3bj1P";


                var store = await _storeContext.GetCurrentStoreAsync();
                var cart = await _cart.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
                var modelShopping = new ShoppingCartModel();
                var model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(modelShopping, cart, false);

                var modelCheckout = new KlarnaCheckOutRequestModel();

                modelCheckout.PurchaseCountry = "PT";
                modelCheckout.PurchaseCurrency = "EUR";
                modelCheckout.Locale = "pt-PT";
                modelCheckout.OrderTaxAmount = 0;
                modelCheckout.OrderAmount = 0;

                var orderLines = new List<OrderLine>();

                Dictionary<int, List<object>> modelValue = new Dictionary<int, List<object>>();

                foreach (var item in model.Items)
                {
                    var order = new OrderLine();
                    order.Type = "physical";
                    order.Reference = item.ProductId.ToString();
                    order.Name = item.ProductName;
                    order.Quantity = item.Quantity;
                    order.UnitPrice = Convert.ToDecimal(item.UnitPrice.Replace("$", "").Replace("€", "").Replace(".", ","));
                    order.TaxRate = 0;
                    order.TotalAmount = Convert.ToDecimal(item.SubTotal.Replace("$", "").Replace("€", "").Replace(".", ","));
                    order.TotalDiscountAmount = 0;
                    order.TotalTaxAmount = 0;
                    modelCheckout.OrderAmount = modelCheckout.OrderAmount + order.TotalAmount;

                    orderLines.Add(order);
                }

                modelCheckout.OrderLines = orderLines;


                var jsonDataC = JsonSerializer.Serialize(modelCheckout);
                var client = new RestClient(apiUrl);
                var request = new RestRequest("", Method.POST);
                request.AddHeader("Content-Type", "application/json");

                var basicAuth = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                               .GetBytes(username + ":" + password));

                request.AddHeader("Authorization", "Basic " + basicAuth);

                request.AddJsonBody(jsonDataC);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    // Read the response body as a string
                    string responseContent = response.Content.ToString();

                    //TODO criar variavel de sessão HttpContext.Session.Get<ProcessPaymentRequest>("OrderPaymentInfo"); e popular com os dados
                    HttpContext.Session.Set<ProcessPaymentRequest>("OrderPaymentInfo", new ProcessPaymentRequest());


                    //JObject json = JObject.Parse(responseContent);
                    return Json(responseContent);
                }
                else
                {
                    // Handle the case where the request was not successful
                    return Json(new { error = "API request failed" });
                }
            }


            catch (Exception ex)
            {
                //return Json(new { error = "An error occurred: " + ex.Message });
                return null;
            }
        }

        //public string KlarnaAPI { get; private set; }
        //public string KlarnaUserName { get; private set; }
        //public string KlarnaPassword { get; private set; }
        //public readonly ConfigurationModel _configurationModel;
    }
}