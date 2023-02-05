using System;

namespace SearchForApi.Models
{
    public class UserBookmarkModel
    {
        public string Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Text { get; set; }
        public string MovieName { get; set; }
    }
}