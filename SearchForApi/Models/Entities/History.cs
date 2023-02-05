using System;

namespace SearchForApi.Models.Entities
{
    public class History : BaseEntity<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public Guid? UserId { get; set; }
        public HistoryType Type { get; set; }
        public HistoryReferType ReferType { get; set; }
        public string SearchKeyword { get; set; }
        public bool SearchIsFound { get; set; }
        public SceneLangaugeType Language { get; set; }
        public Guid? SceneId { get; set; }
        public int? HitsCount { get; set; }
        public int? HitIndex { get; set; }

        public Scene Scene { get; set; }
        public User User { get; set; }
    }

    public enum HistoryType
    {
        Search = 1,
        Scene,
    }

    public enum HistoryReferType
    {
        Manual = 1,
        History,
        Feature
    }
}