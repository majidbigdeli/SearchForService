using System;
namespace SearchForApi.Models.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException() : base()
        {
            this.Data.Add("code", "duplicateUser");
            this.Data.Add("message", "Username is already token");
        }
    }
}

