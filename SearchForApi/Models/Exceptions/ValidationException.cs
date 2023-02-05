using System;
namespace SearchForApi.Models.Exceptions
{
    public class ValidationException: Exception
    {
        public ValidationException() : base()
        {
            this.Data.Add("code", "validation");
            this.Data.Add("message", "The parameter(s) is not valid");
        }
    }
}

