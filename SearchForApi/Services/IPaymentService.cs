using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models.Entities;

namespace SearchForApi.Services
{
    public interface IPaymentService
    {
        Task<ResultDto<RequestTokenDto>> RequestToken(string refId, int amount, string description);
        ResultDto<CallbackResultDto> ParseCallbackResult(PaymentGateway gateway, IFormCollection formData, IQueryCollection queryString);
        Task<ResultDto<VerifyPaymentDto>> VerifyPayment(PaymentGateway gateway, string paymentRefId, int amount);
    }
}
