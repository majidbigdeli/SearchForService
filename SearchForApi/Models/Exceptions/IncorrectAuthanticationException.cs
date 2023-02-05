using System;
namespace SearchForApi.Models.Exceptions
{
    public class IncorrectAuthanticationException : Exception
    {
        public IncorrectAuthanticationException() : base()
        {
            this.Data.Add("code", "incorrectAuthantication");
            this.Data.Add("message", "Username or Password is incorrect");
        }
    }
}

