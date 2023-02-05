using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SearchForApi.Integrations.Payment
{
    public interface IPaymentIntegration
    {
        string Name { get; }
        Task<ResultDto<RequestTokenDto>> RequestToken(string refId, int amount, string description);
        string GetPayUrl(string code);
        Task<ResultDto<VerifyPaymentDto>> VerifyPayment(string paymentRefId, int amount);
        ResultDto<CallbackResultDto> ParseCallbackResult(IFormCollection formData, IQueryCollection queryString);
        string GetStatusMessage(int code);
    }
}

