using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Core;
using System.Text;
using Nop.Services.Plugins;
using Nop.Services.Payments;
using Nop.Services.Localization;
using Nop.Services.Configuration;
using Nop.Services.ScheduleTasks;
using Nop.Plugin.Payments.Klarna.Models;
using Nop.Plugin.Payments.Klarna.Services;
using FluentValidation.Results;
using Nop.Plugin.Payments.Klarna.Components;
using Microsoft.AspNetCore.Components;
using StackExchange.Redis.Profiling;

namespace Nop.Plugin.Payments.Klarna
{
    internal class KlarnaPaymentProcessor : BasePlugin, IPaymentMethod, IPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPaymentService _paymentService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly KlarnaPaymentSettings _klarnaPaymentSettings;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly IStoreContext _storeContext;

        public bool SupportCapture => false;

        public bool SupportPartiallyRefund => false;

        public bool SupportRefund => false;

        public bool SupportVoid => false;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

        public bool SkipPaymentInfo => false;

        public Task<string> PaymentMethodDescription => this._localizationService.GetResourceAsync("Pay in 3 invokations with Klarna Payments");

        public bool HideInWidgetList => true;

        public KlarnaPaymentProcessor(
          ILocalizationService localizationService,
          IPaymentService paymentService,
          ISettingService settingService,
          IWebHelper webHelper,
          IHttpContextAccessor httpContextAccessor,
          IScheduleTaskService scheduleTaskService,
          KlarnaPaymentSettings klarnaPaymentSettings,
          IStoreContext storeContext)
        {
            this._localizationService = localizationService;
            this._httpContextAccessor = httpContextAccessor;
            this._paymentService = paymentService;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._scheduleTaskService = scheduleTaskService;
            this._klarnaPaymentSettings = klarnaPaymentSettings;
            this._storeContext = storeContext;
        }
        //TODO passar a not implemented
        public async Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {

            ProcessPaymentResult processPaymentResult = new ProcessPaymentResult()
            {
                NewPaymentStatus = PaymentStatus.Pending
            };

           

            return processPaymentResult;
        }

        public async Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(this._httpContextAccessor.HttpContext.Session.GetString("customValues"));
            this._httpContextAccessor.HttpContext.Session.Remove("customValues");
            string selectedType = await this.GetSelectedType(dictionary["Payment Type"].ToString());
            if (!(selectedType == "cc"))
                return;
            Models.Customer customer = new Models.Customer()
            {
                CustomerKey = Guid.NewGuid().ToString(),
                Email = dictionary["Email"].ToString(),
                FiscalNumber = dictionary["Fiscal Number"].ToString(),
                Language = "PT",
                Name = dictionary["Name"].ToString()
            };
            var currentStore = await this._storeContext.GetCurrentStoreAsync();
            Nop.Plugin.Payments.Klarna.Models.Capture capture = new Nop.Plugin.Payments.Klarna.Models.Capture()
            {
                Descriptive = "Loja online " + currentStore.Name,
                TransactionKey = postProcessPaymentRequest.Order.OrderGuid.ToString()
            };
            this._httpContextAccessor.HttpContext.Response.Redirect(KlarnaManager.SinglePaymentCreditCardRequest(new KlarnaRequestModel()
            {
                Currency = "EUR",
                ExpirationTime = DateTime.Now.AddDays(1.0),
                MerchantKey = postProcessPaymentRequest.Order.OrderGuid.ToString(),
                Method = selectedType,
                Type = "sale",
                Value = postProcessPaymentRequest.Order.OrderTotal,
                Customer = customer,
                Capture = capture
            }, this._klarnaPaymentSettings.AccountId, this._klarnaPaymentSettings.ApiKey, this._klarnaPaymentSettings.UseTestEnvironment));
        }

        public async Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart) => false;

        public async Task<Decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart) => await this._paymentService.CalculateAdditionalFee(cart, this._klarnaPaymentSettings.AdditionalFee, this._klarnaPaymentSettings.AdditionalFeePercentage);

        public async Task<CapturePaymentResult> CaptureAsync(
          CapturePaymentRequest capturePaymentRequest)
        {
            return new CapturePaymentResult()
            {
                Errors = (IList<string>)new string[1]
              {
          "Capture method not supported"
              }
            };
        }

        public async Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest) => new RefundPaymentResult()
        {
            Errors = (IList<string>)new string[1]
          {
        "Refund method not supported"
          }
        };

        public async Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest) => new VoidPaymentResult()
        {
            Errors = (IList<string>)new string[1]
          {
        "Void method not supported"
          }
        };

        public async Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(
          ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        public async Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(
          CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return new CancelRecurringPaymentResult();
        }

        public async Task<bool> CanRePostProcessPaymentAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            return false;
        }

        public async Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            List<string> stringList = new List<string>();
            var selectedType = await this.GetSelectedType(form["SelectedType"].ToString());
            //var validationResult = new Services.PaymentInfoValidator(this._localizationService).Validate(new PaymentInfoModel()
            //{
            //    SelectedType = selectedType,
            //    Name = (string)form["Name"],
            //    Email = (string)form["Email"],
            //    Phone = (string)form["Phone"],
            //    PhoneIndicative = (string)form["PhoneIndicative"],
            //    FiscalNumber = (string)form["FiscalNumber"]
            //});
            //if (!validationResult.IsValid)
            //    stringList.AddRange(validationResult.Errors.Select<ValidationFailure, string>((Func<ValidationFailure, string>)(error => error.ErrorMessage)));
            return (IList<string>)stringList;
        }

        public async Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            ProcessPaymentRequest paymentInfo = new ProcessPaymentRequest()
            {
                CustomValues = new Dictionary<string, object>()
        {
          {
            "Payment Type",
            (object) form["SelectedType"].FirstOrDefault<string>()
          },
          {
            "Name",
            (object) form["Name"].FirstOrDefault<string>()
          },
          {
            "Email",
            (object) form["Email"].FirstOrDefault<string>()
          },
          {
            "Fiscal Number",
            (object) form["FiscalNumber"].FirstOrDefault<string>()
          }
        }
            };
            if (!string.IsNullOrEmpty(form["PhoneIndicative"].FirstOrDefault<string>()))
                paymentInfo.CustomValues.Add("Phone Indicative", (object)form["PhoneIndicative"].FirstOrDefault<string>());
            if (!string.IsNullOrEmpty(form["Phone"].FirstOrDefault<string>()))
                paymentInfo.CustomValues.Add("Phone", (object)form["Phone"].FirstOrDefault<string>());
            return paymentInfo;
        }

        public override string GetConfigurationPageUrl() => this._webHelper.GetStoreLocation() + "Admin/PaymentKlarna/Configure";

        public string GetPublicViewComponentName() => "PaymentKlarna";

        public override async Task InstallAsync()
        {
            await this._settingService.SaveSettingAsync<KlarnaPaymentSettings>(new KlarnaPaymentSettings()
            {
                UseTestEnvironment = true,
                ActivateMBWay = true,
                ActivateMultibanco = true,
                ActivateCreditCard = true
            });
            if (await this._scheduleTaskService.GetTaskByTypeAsync("Nop.Plugin.Payments.Klarna.Services.FetchPaymentsListTask") == null)
                await this._scheduleTaskService.InsertTaskAsync(new ScheduleTask()
                {
                    Enabled = true,
                    Seconds = 3600,
                    Name = "Fetch completed payments (Klarna)",
                    Type = "Nop.Plugin.Payments.Klarna.Services.FetchPaymentsListTask"
                });
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Instructions", "\r\n                <br>Para usar o plugin siga os seguintes passos:\r\n                <ul>\r\n                <li>Iniciar sessão na sua conta EasyPay (<a href='https://backoffice.test.Klarna.pt/'>Versão de teste</a> / <a href='https://backoffice.Klarna.pt/'>Versão de pagamentos reais</a>)</li>\r\n                <li>Selecionar a opção 'Web Services > Configuration API 2.0' e depois clicar em 'Keys'. Crie uma nova Key ou use uma já existente e extraia os parametros 'ID' (Account ID) e Key (API Key) para usar na configuração abaixo</li>\r\n                <li>Selecionar a opção 'Web Services > Configuration API 2.0' e depois clicar em 'Notifications'. Introduzir o seguint url em 'Genric - URL': <br> {0}</li>\r\n                <li>Selecionar a opção 'Web Services > URL Configuration' e depois clicar em 'Notifications'. Introduzir o seguint url em 'VISA:Forward': <br> {1}</li>\r\n                <li>No menu do NOP Commerce ir a 'Gestão de conteúdo > Modelos de mensagem' e editar a mensagem com o nome 'OrderPlaced.CustomerNotification'. Inserir as seguintes linhas antes de 'Billing Address':\r\n                <i><br>%if (!string.IsNullOrEmpty(%Payment.MBEntity%))\r\n                <br>\r\n                <br>Entidade Multibanco: %Payment.MBEntity%\r\n                <br>Referencia Multibanco: %Payment.MBReference%\r\n                <br>\r\n                <br>endif%</i></li>\r\n                <li>Inserir a 'Account ID' e 'API Key' nas opções abaixo</li>\r\n                <li>Selecionar os metodos de pagamento a suportar nas opções abaixo</li>\r\n                <li>Gravar</li>\r\n                </ul>\r\n            ");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("plugins.payments.Klarna.paymentmethoddescription", "Pagar com MBWay / Refêrencia multibanco / Cartão de Crédito / Santander Consumer");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.UseTestEnvironment", "Usar modo de teste");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.AccountId", "Account ID");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ApiKey", "API Key");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ActivateMBWay", "Activar MBWay");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ActivateMultibanco", "Activar Referência Multibanco");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ActivateCreditCard", "Activar Cartão de Crédito");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ActivateSantanderConsumer", "Activar Santander Consumer");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.AdditionalFeePercentage", "Taxa Adicional Percentagem");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.AdditionalFee", "Taxa Adicional");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Customer.Name", "Nome");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("plugins.payments.Klarna.customer.name.required", "Nome é obrigatório");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Customer.Email", "Email");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("plugins.payments.Klarna.customer.email.wrong", "Email não foi introduzida no formato correto");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Customer.PhoneIndicative", "Indicativo pais (ex: +351)");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("plugins.payments.Klarna.customer.phoneindicative.wrong", "Indicativo não foi introduzida no formato correto");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Customer.Phone", "Telemóvel");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("plugins.payments.Klarna.customer.phone.wrong", "Telemóvel não foi introduzida no formato correto");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Customer.FiscalNumber", "Identificação fiscal (ex: PT123456789)");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("plugins.payments.Klarna.customer.fiscalnumber.wrong", "Identificação fiscal não foi introduzida no formato correto (tem de começar com as inicias do país)");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.MBWay", "MBWay");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.Multibanco", "Referência Multibanco");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.CreditCard", "Cartão de Crédito");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.SantanderConsumer", "Santander Consumer");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.MultibancoPaymentTitle", "Dados para pagamento por multibanco");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.Entity", "Entidade");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.Reference", "Referência");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.Total", "Valor");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Errors.Internal", "Erro interno");
            await this._localizationService.AddOrUpdateLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.ProcessingPayment", "Ocorreu um erro a processar o pagamento, por favor verifique os dados introduzidos e tente novamente");

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await this._settingService.DeleteSettingAsync<KlarnaPaymentSettings>();
            ScheduleTask taskByType = await this._scheduleTaskService.GetTaskByTypeAsync("Nop.Plugin.Payments.Klarna.Services.FetchPaymentsListTask");
            if (taskByType != null)
                await this._scheduleTaskService.DeleteTaskAsync(taskByType);
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Instructions");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.UseTestEnvironment");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.AccountId");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ApiKey");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ActivateMBWay");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ActivateMultibanco");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.ActivateCreditCard");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.AdditionalFeePercentage");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.AdditionalFee");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Customer.Name");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Customer.Email");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Customer.PhoneIndicative");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Customer.Phone");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Customer.FiscalNumber");
            await this._localizationService.DeleteLocaleResourceAsync("plugins.payments.Klarna.customer.name.required");
            await this._localizationService.DeleteLocaleResourceAsync("plugins.payments.Klarna.customer.email.wrong");
            await this._localizationService.DeleteLocaleResourceAsync("plugins.payments.Klarna.customer.phoneindicative.wrong");
            await this._localizationService.DeleteLocaleResourceAsync("plugins.payments.Klarna.customer.phone.wrong");
            await this._localizationService.DeleteLocaleResourceAsync("plugins.payments.Klarna.customer.fiscalnumber.wrong");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.MBWay");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.Multibanco");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.CreditCard");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.MultibancoPaymentTitle");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.Entity");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.Reference");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.Total");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Errors.Internal");
            await this._localizationService.DeleteLocaleResourceAsync("Plugins.Payments.Klarna.Checkout.ProcessingPayment");

            await base.UninstallAsync();
        }

        private async Task<string> GetSelectedType(string selectedTypeFull)
        {
            string selectedType = (string)null;
            if (selectedTypeFull == await this._localizationService.GetResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.MBWay"))
                selectedType = "mbw";
            else if (selectedTypeFull == await this._localizationService.GetResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.Multibanco"))
                selectedType = "mb";
            else if (selectedTypeFull == await this._localizationService.GetResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.CreditCard"))
                selectedType = "cc";
            else if (selectedTypeFull == await this._localizationService.GetResourceAsync("Plugins.Payments.Klarna.Fields.PaymentMethods.SantanderConsumer"))
                selectedType = "sc";
            return selectedType;
        }

        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return await PaymentMethodDescription;
        }

        public Type GetPublicViewComponent()
        {
            return typeof(PaymentKlarnaViewComponent);
        }
    }
}