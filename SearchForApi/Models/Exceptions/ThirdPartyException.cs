using System;
namespace SearchForApi.Models.Exceptions
{
    public class ThirdPartyException : Exception
    {
        public ThirdPartyException() : base()
        {
            this.Data.Add("code", "thirdParty");
            this.Data.Add("message", "An error occured on Third-Party services");
        }
    }
}

