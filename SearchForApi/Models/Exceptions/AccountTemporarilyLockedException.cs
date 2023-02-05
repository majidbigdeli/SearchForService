using System;
namespace SearchForApi.Models.Exceptions
{
    public class AccountTemporarilyLockedException : Exception
    {
        public AccountTemporarilyLockedException() : base()
        {
            this.Data.Add("code", "accountTemporarilyLocked");
            this.Data.Add("message", "Your Account Has Been Temporarily Locked");
        }
    }
}

