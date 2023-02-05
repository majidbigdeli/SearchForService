using System;
namespace SearchForApi.Models.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : base()
        {
            this.Data.Add("code", "itemNotFound");
            this.Data.Add("message", "The item(s) is not found");
        }
    }
}

