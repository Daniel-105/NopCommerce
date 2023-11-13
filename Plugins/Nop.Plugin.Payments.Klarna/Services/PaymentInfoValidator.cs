using FluentValidation;
using Nop.Plugin.Payments.Klarna.Interface;
using Nop.Plugin.Payments.Klarna.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Services
{
    public class PaymentInfoValidator : BaseNopValidator<PaymentInfoModel> 
    {
        //public Task Initialization { get; private set; }

        //public PaymentInfoValidator(ILocalizationService localizationService)
        //{
        //    Initialization = GetMessage(localizationService);
        //}

        //public async Task GetMessage(ILocalizationService localizationService)
        //{
        //    DefaultValidatorOptions.WithMessage<PaymentInfoModel, string>(DefaultValidatorExtensions.NotEmpty<PaymentInfoModel, string>(base.RuleFor<string>((PaymentInfoModel model) => model.Name)), await localizationService.GetResourceAsync("Plugins.Payments.Klarna.Customer.Name.Required"));
        //    //DefaultValidatorOptions.WithMessage<PaymentInfoModel, string>(DefaultValidatorExtensions.EmailAddress<PaymentInfoModel>(base.RuleFor<string>((PaymentInfoModel model) => model.Email)), await localizationService.GetResourceAsync("Plugins.Payments.Klarna.Customer.Email.Wrong"));
        //    //DefaultValidatorOptions.When<PaymentInfoModel, string>(DefaultValidatorOptions.WithMessage<PaymentInfoModel, string>(DefaultValidatorExtensions.Matches<PaymentInfoModel>(base.RuleFor<string>((PaymentInfoModel model) => model.Phone), "[0-9]{1,15}$"), await localizationService.GetResourceAsync("Plugins.Payments.Klarna.Customer.Phone.Wrong")), (PaymentInfoModel model) => model.SelectedType == "mbw", 0);
        //    //DefaultValidatorOptions.When<PaymentInfoModel, string>(DefaultValidatorOptions.WithMessage<PaymentInfoModel, string>(DefaultValidatorExtensions.Matches<PaymentInfoModel>(base.RuleFor<string>((PaymentInfoModel model) => model.PhoneIndicative), "^(\\+?\\d{1,3}|\\d{1,4})$"), await localizationService.GetResourceAsync("Plugins.Payments.Klarna.Customer.PhoneIndicative.Wrong")), (PaymentInfoModel model) => model.SelectedType == "mbw", 0);
        //    //DefaultValidatorOptions.WithMessage<PaymentInfoModel, string>(DefaultValidatorExtensions.Matches<PaymentInfoModel>(base.RuleFor<string>((PaymentInfoModel model) => model.FiscalNumber), "^(?=([A-Za-z]{2,4}))\\1(?![\\W_]+$)(?=.{2,12}$)[-_ 0-9]*(?:[a-zA-Z][-_ 0-9]*){0,2}$"), await localizationService.GetResourceAsync("Plugins.Payments.Klarna.Customer.FiscalNumber.Wrong"));
        //}
    }
}