using System;

namespace SearchForApi.Models.Exceptions
{
    public class SearchBannedKeywordException : Exception
    {
        public SearchBannedKeywordException() : base()
        {
            this.Data.Add("code", "searchBannedKeyword");
            this.Data.Add("message", "Search banned keywords is not allowed");
        }
    }
}