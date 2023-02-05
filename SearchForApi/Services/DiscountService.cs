using System;
using System.Threading.Tasks;
using MethodTimer;
using SearchForApi.Factories;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;
using SearchForApi.Repositories;

namespace SearchForApi.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly DiscountRepository _discountRepository;
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly IDiscountFactory _discountFactory;
        private readonly UserDiscountRepository _userDiscountRepository;
        private readonly IUserDiscountFactory _userDiscountFactory;

        public DiscountService(DiscountRepository discountRepository, IDateTimeFactory dateTimeFactory, IDiscountFactory discountFactory, UserDiscountRepository userDiscountRepository, IUserDiscountFactory userDiscountFactory)
        {
            _discountRepository = discountRepository;
            _dateTimeFactory = dateTimeFactory;
            _discountFactory = discountFactory;
            _userDiscountRepository = userDiscountRepository;
            _userDiscountFactory = userDiscountFactory;
        }

        [Time("userId={userId},discountCode={discountCode}")]
        public async Task<Discount> GetDisountCode(Guid userId, string discountCode)
        {
            if (string.IsNullOrEmpty(discountCode)) return null;

            var normalizedDiscountCode = discountCode.Trim().ToLower();
            var existDiscount = await _discountRepository.Get(normalizedDiscountCode);

            if (!_discountFactory.DiscountIsValid(existDiscount))
                return null;

            var usedUserDiscounts = await _userDiscountRepository.GetUsedUserDiscountsByCode(userId, existDiscount.Id);
            if (usedUserDiscounts >= existDiscount.CountPerUser)
                return null;

            return existDiscount;
        }

        [Time("userId={userId}")]
        public async Task<UserDiscount> AddNewUserDiscount(Guid userId, Discount discount)
        {
            var newUserDiscount = _userDiscountFactory.CreateNewUserDiscountInstance(userId, discount);
            await _userDiscountRepository.Insert(newUserDiscount);

            return newUserDiscount;
        }

        [Time("userId={userId},discountCode={discountCode}")]
        public async Task<UserDiscount> AddNewUserDiscount(Guid userId, string discountCode)
        {
            var existDiscount = await _discountRepository.Get(discountCode);

            var newUserDiscount = _userDiscountFactory.CreateNewUserDiscountInstance(userId, existDiscount);
            await _userDiscountRepository.Insert(newUserDiscount);

            return newUserDiscount;
        }
    }
}