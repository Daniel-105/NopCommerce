using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.Klarna;
using Nop.Plugin.Payments.Klarna.Models;
using Nop.Plugin.Payments.Klarna.Services;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.ScheduleTasks;
using System;

namespace Nop.Plugin.Payments.Klarna.Services
{
    internal class FetchPaymentsListTask : IScheduleTask
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IPaymentService _paymentService;
        private readonly KlarnaPaymentSettings _klarnaPaymentSettings;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;

        public FetchPaymentsListTask(
          ILocalizationService localizationService,
          ILogger logger,
          IPaymentService paymentService,
          IOrderProcessingService orderProcessingService,
          IOrderService orderService,
          KlarnaPaymentSettings klarnaPaymentSettings)
        {
            this._localizationService = localizationService;
            this._logger = logger;
            this._paymentService = paymentService;
            this._orderProcessingService = orderProcessingService;
            this._orderService = orderService;
            this._klarnaPaymentSettings = klarnaPaymentSettings;
        }

        public async void Execute()
        {
            if (!this._paymentService.IsPaymentMethodActive(await this._paymentService.LoadPaymentMethodBySystemName("Payments.Klarna")))
                return;
            foreach (string g in KlarnaManager.SingleDetailsListRequest(new KlarnaListPaymentRequestModel()
            {
                CreatedAt = DateTime.Now.AddMinutes(-65.0),
                Page = 1,
                RecordsPerPage = 100,
                Type = "sale"
            }, this._klarnaPaymentSettings.AccountId, this._klarnaPaymentSettings.ApiKey, this._klarnaPaymentSettings.UseTestEnvironment))
            {
                Order orderByGuid = await this._orderService.GetOrderByGuidAsync(new Guid(g));
                if (orderByGuid != null && orderByGuid.PaymentStatus != PaymentStatus.Paid)
                {
                    await this._orderProcessingService.MarkOrderAsPaidAsync(orderByGuid);
                    await this._logger.InformationAsync("Found unpaid order with guid " + g);
                }
            }
            await this._logger.InformationAsync(await this._localizationService.GetResourceAsync("Plugins.Payments.Klarna.FetchPaymentsListTask.Success"));
        }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}