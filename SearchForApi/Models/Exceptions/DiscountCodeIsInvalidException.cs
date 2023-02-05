using System;

namespace SearchForApi.Models.Exceptions
{
    public class DiscountCodeIsInvalidException : Exception
    {
        public DiscountCodeIsInvalidException() : base()
        {
            this.Data.Add("code", "discountCodeIsInvalid");
            this.Data.Add("message", "Discount code is invalid");
        }
    }
}