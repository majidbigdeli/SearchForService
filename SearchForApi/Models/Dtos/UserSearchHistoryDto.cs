using System;

namespace SearchForApi.Models.Dtos
{
    public class UserSearchHistoryDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Keyword { get; set; }
    }
}