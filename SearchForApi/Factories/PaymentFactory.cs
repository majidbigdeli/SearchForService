using System;
using SearchForApi.Integrations.Payment;
using SearchForApi.Integrations.Payment.PayPing;
using SearchForApi.Integrations.Payment.ZarinPal;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly IPlanFactory _planFactory;

        public PaymentFactory(IDateTimeFactory dateTimeFactory, IPlanFactory planFactory)
        {
            _dateTimeFactory = dateTimeFactory;
            _planFactory = planFactory;
        }

        public Payment CreateNewPaymentInstance(Guid userId, Plan plan, bool is3Months, string redirectUrl, Discount discount = null)
        {
            var finalAmount = _planFactory.CalculatePlanFinalAmount(plan, is3Months, discount);

            var newPaymentInstance = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = finalAmount,
                UserId = userId,
                CreatedOn = _dateTimeFactory.UtcNow,
                Status = PaymentStatus.New,
                StatusChangedOn = _dateTimeFactory.UtcNow,
                Description = string.Format("Plan '{0}' Subscription for '{1}' month(s)", plan.Id, is3Months ? 3 : 1),
                RedirectUrl = redirectUrl,

                PlanId = plan.Id,
                PlanIs3Months = is3Months,
                PlanDiscountPrecentFor3Months = plan.DiscountPrecentFor3Months,
                PlanPrice = plan.Price,
                PlanDiscount = plan.Discount,
                Price3Months = plan.Price3Months,
            };

            if (discount != null)
                newPaymentInstance.DiscountCode = discount.Id;

            return newPaymentInstance;
        }

        public void UpdateConfirmResult(Payment payment, ResultDto<CallbackResultDto> result)
        {
            if (!result.Succeeded)
            {
                payment.Status = PaymentStatus.FailedByUser;
                payment.StatusChangedOn = _dateTimeFactory.UtcNow;
                payment.StatusMessage = result.ErrorMessage;
                payment.CallbackStatusCode = result.Result.PaymentRefId;
            }
            else
            {
                payment.Status = PaymentStatus.Paid;
                payment.StatusChangedOn = _dateTimeFactory.UtcNow;
                payment.CallbackStatusCode = result.Result.PaymentRefId;
            }
        }

        public void UpdateTokenResult(Payment payment, ResultDto<RequestTokenDto> result)
        {
            payment.GatewayType = result.Result.GatewayType;

            if (!result.Succeeded)
            {
                payment.Status = PaymentStatus.FailedByToken;
                payment.StatusChangedOn = _dateTimeFactory.UtcNow;
                payment.StatusMessage = result.ErrorMessage;
            }
            else
            {
                payment.Status = PaymentStatus.Token;
                payment.StatusChangedOn = _dateTimeFactory.UtcNow;
                payment.Token = result.Result.Code;
                payment.PayUrl = result.Result.PayUrl;
            }
        }

        public void UpdateVerifyResult(Payment payment, ResultDto<VerifyPaymentDto> result)
        {
            if (!result.Succeeded)
            {
                payment.Status = PaymentStatus.FailedByVerify;
                payment.StatusChangedOn = _dateTimeFactory.UtcNow;
                payment.StatusMessage = result.ErrorMessage;
            }
            else
            {
                payment.Status = PaymentStatus.Verified;
                payment.StatusChangedOn = _dateTimeFactory.UtcNow;
                payment.CardNumber = result.Result.CardNumber;
                payment.CardHashPan = result.Result.CardHashPan;
            }
        }

        public IPaymentIntegration GetPaymentGatewayInstance(PaymentGateway gateway)
        {
            object paymentIntegration = null;
            switch (gateway.Id)
            {
                case PaymentGatewayType.PayPing:
                    paymentIntegration = new PayPingIntegration(gateway.AccessToken);
                    break;
                case PaymentGatewayType.ZarinPal:
                    paymentIntegration = new ZarinPalIntegration(gateway.AccessToken);
                    break;
            }

            return (IPaymentIntegration)paymentIntegration;
        }
    }
}