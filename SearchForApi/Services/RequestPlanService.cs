using System;
using MethodTimer;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;
using System.Threading.Tasks;
using SearchForApi.Factories;

namespace SearchForApi.Services
{
    public class RequestPlanService : IRequestPlanService
    {
        private readonly IPaymentFactory _paymentFactory;
        private readonly PaymentRepository _paymentRepository;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IDiscountService _discountService;

        public RequestPlanService(IPaymentFactory paymentFactory, PaymentRepository paymentRepository, IUserService userService, IPaymentService paymentService, IDiscountService discountService)
        {
            _paymentFactory = paymentFactory;
            _paymentRepository = paymentRepository;
            _userService = userService;
            _paymentService = paymentService;
            _discountService = discountService;
        }

        [Time("userId={userId},planId={planId},is3Months={is3Months}")]
        public async Task RequestFreePlan(Guid userId, PlanType planId, bool is3Months, Discount discount)
        {
            await _userService.SetUserCurrentPlan(userId, planId, is3Months);
            await _userService.RemoveUserCurrentPlanStatus(userId);

            await _discountService.AddNewUserDiscount(userId, discount);
        }

        [Time("userId={userId},is3Months={is3Months},redirectUrl={redirectUrl}")]
        public async Task<RequestTokenDto> RequestPaidPlan(Guid userId, Plan plan, bool is3Months, string redirectUrl, Discount discount)
        {
            var newPayment = _paymentFactory.CreateNewPaymentInstance(userId, plan, is3Months, redirectUrl, discount);
            await _paymentRepository.Insert(newPayment);

            var result = await _paymentService.RequestToken(newPayment.Id.ToString(), newPayment.Amount, newPayment.Description);
            _paymentFactory.UpdateTokenResult(newPayment, result);
            await _paymentRepository.SaveChanges();
            if (!result.Succeeded)
                throw new ThirdPartyException();

            return result.Result;
        }
    }
}

