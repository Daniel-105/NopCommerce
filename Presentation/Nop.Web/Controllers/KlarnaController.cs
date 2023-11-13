using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Customers;
using Nop.Services.Orders;
using Nop.Services.Payments;

namespace Nop.Web.Controllers
{
    public class KlarnaController : BasePublicController
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentPluginManager _paymentPluginManager;
        private readonly ICustomerService _customerService;
        private readonly IOrderProcessingService _orderProcessingService;
        public KlarnaController(ICustomerService _customerService,
            IPaymentPluginManager _paymentPluginManager,
            IPaymentService _paymentService,
             IOrderProcessingService _orderProcessingService)
        {
            this._customerService = _customerService;
            this._paymentPluginManager = _paymentPluginManager;
            this._paymentService = _paymentService;
            this._orderProcessingService = _orderProcessingService;

        }

        /// <summary>
        /// customerId
        /// storeId
        /// paymentInfo - ProcessPaymentRequest
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NopException"></exception>
        public async Task<ProcessPaymentResult> Test()
        {
            var customer = await _customerService.GetCustomerByIdAsync(1);
            var paymentMethod = await _paymentPluginManager
                .LoadPluginBySystemNameAsync("Payments.Klarna", customer, 1)
                ?? throw new NopException("Payment method couldn't be loaded");

            var cValues = new Dictionary<string, object>();
            //paymentInfo.Status
            cValues.Add("Status", true);

            return await paymentMethod.ProcessPaymentAsync(new ProcessPaymentRequest()
            {
                CustomValues = cValues
            });

        }
    }
}
