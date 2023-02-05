using System;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.AspNetCore.Http;
using SearchForApi.Core;
using SearchForApi.Factories;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;
using SearchForApi.Utilities;
using SearchForApi.Utilities.LockManager;

namespace SearchForApi.Services
{
    public class PlanService : IPlanService
    {
        private readonly PlanRepository _planRepository;
        private readonly IPaymentFactory _paymentFactory;
        private readonly PaymentRepository _paymentRepository;
        private readonly IUserService _userService;
        private readonly IDiscountService _discountService;
        private readonly IPaymentService _paymentService;
        private readonly IRequestPlanService _requestPlanService;

        public PlanService(PlanRepository planRepository, IPaymentFactory paymentFactory, PaymentRepository paymentRepository, IUserService userService, IDiscountService discountService, IPaymentService paymentService, IRequestPlanService requestPlanService)
        {
            _planRepository = planRepository;
            _paymentFactory = paymentFactory;
            _paymentRepository = paymentRepository;
            _userService = userService;
            _discountService = discountService;
            _paymentService = paymentService;
            _requestPlanService = requestPlanService;
        }

        [Time("userId={userId},sceneIndex={sceneIndex}")]
        public async Task IsAllowedViewPredefinedItemScene(Guid? userId, int sceneIndex)
        {
            var isAllowedViewScene = sceneIndex + 1 <= Cfg.MaxAnonymousQuoteViewPerSearch;

            if (!isAllowedViewScene && userId != null)
            {
                var userPlan = await _userService.GetUserCurrentPlanStatus((Guid)userId);
                if (userPlan.Type != PlanType.Basic)
                    isAllowedViewScene = sceneIndex + 1 <= userPlan.QuoteViewPerSearch;
            }

            if (!isAllowedViewScene)
                throw new PlanIsNotAllowedException("View predefined item scene has been limited!");
        }

        [Time("userId={userId},keyword={keyword},sceneIndex={sceneIndex}")]
        public async Task IsAllowedToSearch(Guid userId, string keyword, int sceneIndex)
        {
            var userPlan = await _userService.GetUserCurrentPlanStatus(userId);

            if (userPlan.Type == PlanType.Basic)
                throw new PlanIsNotAllowedException("Basic users limitations!");

            var normalizedKeyword = keyword.NormalizeKeyword();
            var isDuplicateKeyword = userPlan.SearchedItems.Any(p => p == normalizedKeyword);

            var isAllowedSearch = userPlan.SearchPerDayRemain > 0;
            var isAllowedViewScene = sceneIndex + 1 <= userPlan.QuoteViewPerSearch;

            if (!isAllowedSearch && !isDuplicateKeyword)
                throw new PlanIsNotAllowedException("Search has been limited!");

            if (!isAllowedViewScene)
                throw new PlanIsNotAllowedException("View scene has been limited!");
        }

        [Time("userId={userId}")]
        public async Task IsAllowedToBookmark(Guid userId)
        {
            var userPlan = await _userService.GetUserCurrentPlanStatus(userId);

            var isAllowed = userPlan.BookmarkedRemain > 0;
            if (!isAllowed)
                throw new PlanIsNotAllowedException("Bookmark scene has been limited!");
        }

        [Time("userId={userId}")]
        public async Task IsAllowedToShare(Guid userId)
        {
            var userPlan = await _userService.GetUserCurrentPlanStatus(userId);

            var isAllowed = userPlan.ShareQuoteRemain > 0;
            if (!isAllowed)
                throw new PlanIsNotAllowedException("Share scene has been limited!");
        }

        [Time("userId={userId},type={type}")]
        public async Task UpdateUserStatus(Guid userId, UserPlanStatusType type, bool incremental, string keyword = null)
        {
            var userPlan = await _userService.GetUserCurrentPlanStatus(userId);

            var incrementalStep = incremental ? 1 : -1;
            switch (type)
            {
                case UserPlanStatusType.Search:
                    var normalizedKeyword = keyword.NormalizeKeyword();
                    var isDuplicateKeyword = userPlan.SearchedItems.Any(p => p == normalizedKeyword);
                    if (!isDuplicateKeyword)
                    {
                        userPlan.SearchPerDayRemain += incrementalStep;
                        userPlan.SearchedItems.Add(normalizedKeyword);
                    }
                    break;
                case UserPlanStatusType.Bookmark:
                    userPlan.BookmarkedRemain += incrementalStep;
                    break;
                case UserPlanStatusType.Share:
                    userPlan.ShareQuoteRemain += incrementalStep;
                    break;
            }

            await _userService.SetUserCurrentPlanStatus(userId, userPlan);
        }

        [Time("discountCode={discountCode}")]
        public async Task<Discount> GetDiscount(Guid userId, string discountCode)
        {
            var existDiscount = await _discountService.GetDisountCode(userId, discountCode);
            if (existDiscount == null)
                throw new DiscountCodeIsExpiredException();

            return existDiscount;
        }

        [Time("userId={userId},planId={planId},is3Months={is3Months},redirectUrl={redirectUrl},discountCode={discountCode}")]
        public async Task<(RequestTokenDto requestToken, bool needToPay)> RequestPlan(Guid userId, PlanType planId, bool is3Months, string redirectUrl, string discountCode)
        {
            if (planId == PlanType.Basic)
                throw new ValidationException();

            var existPlan = await _planRepository.Get(planId);
            if (existPlan == null)
                throw new ItemNotFoundException();

            var existDiscount = await _discountService.GetDisountCode(userId, discountCode);
            if (existDiscount?.IsFree == true)
            {
                await _requestPlanService.RequestFreePlan(userId, planId, is3Months, existDiscount);
                return (RequestTokenDto.Empty, false);
            }
            else
            {
                var result = await _requestPlanService.RequestPaidPlan(userId, existPlan, is3Months, redirectUrl, existDiscount);
                return (result, true);
            }
        }

        [Time("refIdString={refIdString}")]
        public async Task<(bool succeeded, string redirectUrl)> VerifyPlanPayment(string refIdString, IFormCollection formData, IQueryCollection queryString)
        {
            if (!Guid.TryParse(refIdString, out var refId))
                throw new ValidationException();

            var existPayment = await _paymentRepository.GetWithGateway(refId);
            if (existPayment == null)
                throw new ItemNotFoundException();

            using (var lockManager = new VerifyPlanPaymentLockManager())
            {
                await lockManager.AcquireLock(refIdString);

                if (existPayment.Status != PaymentStatus.Token)
                    throw new ValidationException();

                var result = _paymentService.ParseCallbackResult(existPayment.Gateway, formData, queryString);
                _paymentFactory.UpdateConfirmResult(existPayment, result);
                await _paymentRepository.SaveChanges();
                if (!result.Succeeded) return (false, existPayment.RedirectUrl);

                var (user, oldPlanType, oldPlanChangedOn, oldPlanExpireDate) = await _userService.SetUserCurrentPlan(existPayment.UserId, existPayment.PlanId, existPayment.PlanIs3Months);
                await _userService.RemoveUserCurrentPlanStatus(existPayment.UserId);

                var verifyResult = await _paymentService.VerifyPayment(existPayment.Gateway, existPayment.CallbackStatusCode, existPayment.Amount);
                _paymentFactory.UpdateVerifyResult(existPayment, verifyResult);
                await _paymentRepository.SaveChanges();
                if (!verifyResult.Succeeded)
                {
                    await _userService.SetUserCurrentPlan(user, oldPlanType, oldPlanChangedOn, oldPlanExpireDate);
                    await _userService.RemoveUserCurrentPlanStatus(existPayment.UserId);

                    return (false, existPayment.RedirectUrl);
                }

                await _discountService.AddNewUserDiscount(existPayment.UserId, existPayment.DiscountCode);

                //_ = _userService.AddOrUpdateUserContact(existPayment.UserId, new
                //{
                //    Plan = (int)user.PlanId,
                //    ExpireDate = user.PlanExpireDate,
                //    SMS = user.PhoneNumber?.NationalizePhoneNumber()
                //});

                return (true, existPayment.RedirectUrl);
            }
        }
    }

    public class VerifyPlanPaymentLockManager : LockManager<string>
    {
        public VerifyPlanPaymentLockManager() : base("VerifyPlanPayment") { }
    }
}