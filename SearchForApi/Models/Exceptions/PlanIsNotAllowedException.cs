using System;

namespace SearchForApi.Models.Exceptions
{
    public class PlanIsNotAllowedException : Exception
    {
        public PlanIsNotAllowedException(string message) : base(message)
        {
            this.Data.Add("code", "planIsNotAllowed");
            this.Data.Add("message", "You'are not allowed to use this action. Please upgrade your plan");
        }
    }
}