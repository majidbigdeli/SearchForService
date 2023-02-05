using System;

namespace SearchForApi.Models.Entities
{
    public class Report: BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public Guid SceneId { get; set; }
        public ReportType Type { get; set; }

        public Scene Scene { get; set; }
        public User User { get; set; }
    }

    public enum ReportType
    {
        Scene = 1,
        Text,
        VideoContent,
        TextContent,
        NotSync,
        Other,
        NotForKids
    }
}