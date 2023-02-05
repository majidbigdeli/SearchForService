using System;

namespace SearchForApi.Models.Exceptions
{
    public class FakeRequestException : Exception
    {
        public FakeRequestException() : base()
        {
            this.Data.Add("code", "fakeRequest");
            this.Data.Add("message", "The request is not valid");
        }
    }
}