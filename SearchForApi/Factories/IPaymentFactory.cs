using System;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface IPaymentFactory
    {
        Payment CreateNewPaymentInstance(Guid userId, Plan plan, bool is3Months, string redirectUrl, Discount discount = null);
        void UpdateTokenResult(Payment payment, ResultDto<RequestTokenDto> result);
        void UpdateConfirmResult(Payment payment, ResultDto<CallbackResultDto> result);
        void UpdateVerifyResult(Payment payment, ResultDto<VerifyPaymentDto> result);
        IPaymentIntegration GetPaymentGatewayInstance(PaymentGateway gateway);
    }
}