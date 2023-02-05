using System;

namespace SearchForApi.Models.Exceptions
{
    public class DiscountCodeIsExpiredException : Exception
    {
        public DiscountCodeIsExpiredException() : base()
        {
            this.Data.Add("code", "discountCodeIsExpired");
            this.Data.Add("message", "Discount code is expired");
        }
    }
}