using System;

namespace SearchForApi.Models.Entities
{
    public class BannedKeyword : BaseEntity<Guid>
    {
        public string Keyword { get; set; }
        public SceneLangaugeType Language { get; set; }
    }
}