using SearchForApi.Models.Entities;

namespace SearchForApi.Integrations.Payment
{
    public class RequestTokenDto
    {
        public string Code { get; set; }
        public string PayUrl { get; set; }
        public PaymentGatewayType GatewayType { get; set; }

        public static RequestTokenDto Empty => new RequestTokenDto();
    }
}

