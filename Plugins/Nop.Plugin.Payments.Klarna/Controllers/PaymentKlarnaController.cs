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

namespace Nop.Plugin.Payments.Klarna.Controllers
{
    [AuthorizeAdmin(false)]
    [Area("Admin")]
    public class PaymentKlarnaController : BasePaymentController
    {

        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        public PaymentKlarnaController(ILocalizationService localizationService, IPermissionService permissionService, ISettingService settingService, IStoreContext storeContext, IHttpClientFactory httpClientFactory, IPaymentService paymentService)
        {
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            _httpClient = httpClientFactory.CreateClient();
            this._paymentService = paymentService;
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
                ConfigurationModel configurationModel = new ConfigurationModel
                {
                    AdditionalFee = klarnaPaymentSettings.AdditionalFee,
                    AdditionalFeePercentage = klarnaPaymentSettings.AdditionalFeePercentage,
                    ActiveStoreScopeConfiguration = activeStoreScopeConfiguration,
                    AccountId = klarnaPaymentSettings.AccountId,
                    ApiKey = klarnaPaymentSettings.ApiKey,
                    UseTestEnvironment = klarnaPaymentSettings.UseTestEnvironment,
                    ActivateCreditCard = klarnaPaymentSettings.ActivateCreditCard,
                    ActivateMBWay = klarnaPaymentSettings.ActivateMBWay,
                    ActivateMultibanco = klarnaPaymentSettings.ActivateMultibanco,
                    ActivateSantanderConsumer = klarnaPaymentSettings.ActivateSantanderConsumer,
                    SantanderConsumerMin = klarnaPaymentSettings.SantanderConsumerMin,
                    SantanderConsumerMax = klarnaPaymentSettings.SantanderConsumerMax
                };
                bool flag2 = activeStoreScopeConfiguration > 0;
                if (flag2)
                {
                    configurationModel.AdditionalFee_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, decimal>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.AdditionalFee, activeStoreScopeConfiguration);
                    configurationModel.AdditionalFeePercentage_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.AdditionalFeePercentage, activeStoreScopeConfiguration);
                    configurationModel.AccountId_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.AccountId, activeStoreScopeConfiguration);
                    configurationModel.ApiKey_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ApiKey, activeStoreScopeConfiguration);
                    configurationModel.UseTestEnvironment_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.UseTestEnvironment, activeStoreScopeConfiguration);
                    configurationModel.ActivateCreditCard_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateCreditCard, activeStoreScopeConfiguration);
                    configurationModel.ActivateMBWay_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateMBWay, activeStoreScopeConfiguration);
                    configurationModel.ActivateMultibanco_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateMultibanco, activeStoreScopeConfiguration);
                    configurationModel.ActivateSantanderConsumer_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateSantanderConsumer, activeStoreScopeConfiguration);
                    configurationModel.SantanderConsumerMin_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, int>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.SantanderConsumerMin, activeStoreScopeConfiguration);
                    configurationModel.SantanderConsumerMax_OverrideForStore = await this._settingService.SettingExistsAsync<KlarnaPaymentSettings, int>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.SantanderConsumerMax, activeStoreScopeConfiguration);
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
                bool flag2 = !base.ModelState.IsValid;
                if (flag2)
                {
                    result = await this.Configure();
                }
                else
                {
                    int activeStoreScopeConfiguration = await this._storeContext.GetActiveStoreScopeConfigurationAsync();
                    KlarnaPaymentSettings klarnaPaymentSettings = await this._settingService.LoadSettingAsync<KlarnaPaymentSettings>(activeStoreScopeConfiguration);
                    klarnaPaymentSettings.AdditionalFee = model.AdditionalFee;
                    klarnaPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
                    klarnaPaymentSettings.AccountId = model.AccountId;
                    klarnaPaymentSettings.ApiKey = model.ApiKey;
                    klarnaPaymentSettings.UseTestEnvironment = model.UseTestEnvironment;
                    klarnaPaymentSettings.ActivateCreditCard = model.ActivateCreditCard;
                    klarnaPaymentSettings.ActivateMBWay = model.ActivateMBWay;
                    klarnaPaymentSettings.ActivateMultibanco = model.ActivateMultibanco;
                    klarnaPaymentSettings.ActivateSantanderConsumer = model.ActivateSantanderConsumer;
                    klarnaPaymentSettings.SantanderConsumerMin = model.SantanderConsumerMin;
                    klarnaPaymentSettings.SantanderConsumerMax = model.SantanderConsumerMax;
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, decimal>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.AdditionalFee, model.AdditionalFee_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.AccountId, model.AccountId_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, string>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ApiKey, model.ApiKey_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.UseTestEnvironment, model.UseTestEnvironment_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateCreditCard, model.ActivateCreditCard_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateMBWay, model.ActivateMBWay_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateMultibanco, model.ActivateMultibanco_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, bool>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.ActivateSantanderConsumer, model.ActivateSantanderConsumer_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, int>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.SantanderConsumerMin, model.SantanderConsumerMin_OverrideForStore, activeStoreScopeConfiguration, false);
                    await this._settingService.SaveSettingOverridablePerStoreAsync<KlarnaPaymentSettings, int>(klarnaPaymentSettings, (KlarnaPaymentSettings x) => x.SantanderConsumerMax, model.SantanderConsumerMax_OverrideForStore, activeStoreScopeConfiguration, false);
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
            // Replace with your actual API endpoint URL
            string apiUrl = "https://api.playground.klarna.com/payments/v1/sessions";

            // Replace with your Basic Authentication credentials
            string username = "PK131523_f3731dbad121";
            string password = "qSNhaFgn6Ls3bj1P";
            OrderDetailsModel orderDetailsModel = new OrderDetailsModel();
            PaymentInfoModel paymentInfoModel = new PaymentInfoModel();
            WishlistModel wishlistModel = new WishlistModel();
            MiniShoppingCartModel miniShoppingCartModel = new MiniShoppingCartModel();
            var subtotal = miniShoppingCartModel.SubTotal;
            Console.WriteLine(subtotal);


            // Create JSON data to send in the request
            string jsonData = @"
        {
            ""purchase_country"": ""PT"",
            ""purchase_currency"": ""EUR"",
            ""locale"": ""pt-PT"",
            ""order_amount"": 20000,
            ""order_tax_amount"": 0,
            ""order_lines"": [
                {
                    ""type"": ""physical"",
                    ""reference"": ""19-402"",
                    ""name"": ""black T-Shirt"",
                    ""quantity"": 2,
                    ""unit_price"": 5000,
                    ""tax_rate"": 0,
                    ""total_amount"": 10000,
                    ""total_discount_amount"": 0,
                    ""total_tax_amount"": 0
                },
                {
                    ""type"": ""physical"",
                    ""reference"": ""123123"",
                    ""name"": ""red trousers"",
                    ""quantity"": 1,
                    ""unit_price"": 10000,
                    ""tax_rate"": 0,
                    ""total_amount"": 10000,
                    ""total_discount_amount"": 0,
                    ""total_tax_amount"": 0
                }
            ]
        }";

            try
            {
                // Set Basic Authentication credentials
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));

                // Set the Content-Type header to application/json
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send a POST request with JSON content
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, new StringContent(jsonData, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    // Read the response body as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    //JObject json = JObject.Parse(responseContent);

                    // Return the JSON data as a JsonResult
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
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }


        [Route("/PlaceOrder")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string authorizationToken)
        {
            // Replace with your actual API endpoint URL
            string apiUrl = "https://api.playground.klarna.com/payments/v1/authorizations/" + authorizationToken + "/order";

            // Replace with your Basic Authentication credentials
            string username = "PK131523_f3731dbad121";
            string password = "qSNhaFgn6Ls3bj1P";

            // Create JSON data to send in the request
            string jsonData = @"
        {
            ""purchase_country"": ""PT"",
            ""purchase_currency"": ""EUR"",
            ""locale"": ""pt-PT"",
            ""order_amount"": 20000,
            ""order_tax_amount"": 0,
            ""order_lines"": [
                {
                    ""type"": ""physical"",
                    ""reference"": ""19-402"",
                    ""name"": ""black T-Shirt"",
                    ""quantity"": 2,
                    ""unit_price"": 5000,
                    ""tax_rate"": 0,
                    ""total_amount"": 10000,
                    ""total_discount_amount"": 0,
                    ""total_tax_amount"": 0
                },
                {
                    ""type"": ""physical"",
                    ""reference"": ""123123"",
                    ""name"": ""red trousers"",
                    ""quantity"": 1,
                    ""unit_price"": 10000,
                    ""tax_rate"": 0,
                    ""total_amount"": 10000,
                    ""total_discount_amount"": 0,
                    ""total_tax_amount"": 0
                }
            ]
        }";

            try
            {
                // Set Basic Authentication credentials
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));

                // Set the Content-Type header to application/json
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send a POST request with JSON content
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, new StringContent(jsonData, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    // Read the response body as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseContent);

                    var typeJson = json.GetType();
                    //var jsonResponse = JsonSerializer.Deserialize<JsonObject>(responseContent);

                    // Return the JSON data as a JsonResult
                    //return Json(responseContent);
                    //return RedirectToAction("ProcessPaymentAsync", "PaymentKlarna");

                    //I'm changing this variable.
                    //trying to find why don't I have access to ProcessPaymentRequest


                    //TODO criar variavel de sessão HttpContext.Session.Get<ProcessPaymentRequest>("OrderPaymentInfo"); e popular com os dados
                    HttpContext.Session.Set<ProcessPaymentRequest>("OrderPaymentInfo", new ProcessPaymentRequest());

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
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }
       
        private readonly ILocalizationService _localizationService;


        private readonly IPermissionService _permissionService;


        private readonly ISettingService _settingService;


        private readonly IStoreContext _storeContext;
    }
}
