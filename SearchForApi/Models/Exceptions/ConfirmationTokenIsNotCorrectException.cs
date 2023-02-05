using System;
namespace SearchForApi.Models.Exceptions
{
    public class ConfirmationTokenIsNotCorrectException : Exception
    {
        public ConfirmationTokenIsNotCorrectException() : base()
        {
            this.Data.Add("code", "confirmationTokenIsNotCorrect");
            this.Data.Add("message", "The confirmation token is not correct");
        }
    }
}

