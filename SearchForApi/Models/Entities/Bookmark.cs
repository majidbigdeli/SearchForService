using System;

namespace SearchForApi.Models.Entities
{
    public class Bookmark : BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public Guid SceneId { get; set; }

        public Scene Scene { get; set; }
        public User User { get; set; }
    }
}