using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SearchForApi.Factories;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models.Entities;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFactory _paymentFactory;
        private readonly PaymentGatewayRepository _paymentGatewayRepository;

        public PaymentService(IPaymentFactory paymentFactory, PaymentGatewayRepository paymentGatewayRepository)
        {
            _paymentFactory = paymentFactory;
            _paymentGatewayRepository = paymentGatewayRepository;
        }

        public async Task<ResultDto<RequestTokenDto>> RequestToken(string refId, int amount, string description)
        {
            ResultDto<RequestTokenDto> result = null;

            var activeGateways = await _paymentGatewayRepository.GetActives();
            foreach (var activeGateway in activeGateways)
            {
                var paymentIntegration = _paymentFactory.GetPaymentGatewayInstance(activeGateway);
                result = await paymentIntegration.RequestToken(refId, amount, description);
                if (result.Succeeded)
                    break;
            }

            return result;
        }

        public ResultDto<CallbackResultDto> ParseCallbackResult(PaymentGateway gateway, IFormCollection formData, IQueryCollection queryString)
        {
            var paymentIntegration = _paymentFactory.GetPaymentGatewayInstance(gateway);
            return paymentIntegration.ParseCallbackResult(formData, queryString);
        }

        public Task<ResultDto<VerifyPaymentDto>> VerifyPayment(PaymentGateway gateway, string paymentRefId, int amount)
        {
            var paymentIntegration = _paymentFactory.GetPaymentGatewayInstance(gateway);
            return paymentIntegration.VerifyPayment(paymentRefId, amount);
        }
    }
}
