using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;
using Nop.Plugin.Payments.Klarna.Models;

namespace Nop.Plugin.Payments.Klarna.Components
{

    [ViewComponent(Name = "PaymentKlarna")]
    public class PaymentKlarnaViewComponent : NopViewComponent
    {
        private readonly KlarnaPaymentSettings _klarnaPaymentSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        public PaymentKlarnaViewComponent(KlarnaPaymentSettings klarnaPaymentSettings,
            ILocalizationService localizationService,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            _klarnaPaymentSettings = klarnaPaymentSettings;
            _localizationService = localizationService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var store = await _storeContext.GetCurrentStoreAsync();

            var model = new PaymentInfoModel
            {
                DescriptionText = await _localizationService.GetLocalizedSettingAsync(_klarnaPaymentSettings,
                    x => x.DescriptionText, (await _workContext.GetWorkingLanguageAsync()).Id, store.Id)
            };

            return View("~/Plugins/Payments.Klarna/Views/PaymentInfo.cshtml", model);
        }
    }
}