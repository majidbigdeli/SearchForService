using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SearchForApi.Integrations.Payment
{
    public abstract class PaymentBaseIntegration: IPaymentIntegration
    {
        public PaymentBaseIntegration(string _accessToken)
        {
            AccessToken = _accessToken;
        }

        public string AccessToken { get; }
        public abstract string Name { get; }

        public abstract string GetPayUrl(string code);
        public abstract string GetStatusMessage(int code);
        public abstract ResultDto<CallbackResultDto> ParseCallbackResult(IFormCollection formData, IQueryCollection queryString);
        public abstract Task<ResultDto<RequestTokenDto>> RequestToken(string refId, int amount, string description);
        public abstract Task<ResultDto<VerifyPaymentDto>> VerifyPayment(string paymentRefId, int amount);
    }
}
