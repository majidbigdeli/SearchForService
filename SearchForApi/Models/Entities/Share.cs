using System;
using System.Collections.Generic;

namespace SearchForApi.Models.Entities
{
    public class Share: BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public string Keyword { get; set; }
        public int ViewCount { get; set; }
        public DateTime? LastViewOn { get; set; }
        public Guid SceneId { get; set; }
        public string Token { get; set; }

        public ICollection<ShareHistory> ShareHistories { get; set; }

        public Scene Scene { get; set; }
        public User User { get; set; }
    }
}