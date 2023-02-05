using System;
namespace SearchForApi.Integrations.Payment.PayPing
{
    public class PayPingVerifyPaymentDto
    {
        public int amount { get; set; }
        public string cardNumber { get; set; }
        public string cardHashPan { get; set; }
    }
}
