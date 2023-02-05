using System;
namespace SearchForApi.Models.Exceptions
{
    public class EmailIsNotConfirmedException : Exception
    {
        public EmailIsNotConfirmedException() : base()
        {
            this.Data.Add("code", "emailIsNotConfirmed");
            this.Data.Add("message", "The email is not confirmed");
        }
    }
}

