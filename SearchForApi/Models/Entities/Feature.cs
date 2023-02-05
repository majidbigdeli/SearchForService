using System;
using System.Collections.Generic;

namespace SearchForApi.Models.Entities
{
	public class Feature : BaseEntity<Guid>
	{
        public FeatureType Type { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public int Order { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsEnable { get; set; }

        public ICollection<FeatureItem> Items { get; set; }
    }

	public enum FeatureType
    {
		DailyWord = 1,
		List,
		DailyScene,
		Category
    }
}

