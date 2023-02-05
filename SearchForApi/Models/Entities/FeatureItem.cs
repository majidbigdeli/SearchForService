using System;
namespace SearchForApi.Models.Entities
{
	public class FeatureItem : BaseEntity<Guid>
	{
        public string Keyword { get; set; }
        public string Translation { get; set; }
        public string Sample { get; set; }
        public string Cover { get; set; }
        public int Order { get; set; }
        public string ParameterId { get; set; }
        public Guid FeatureId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ChangedOn { get; set; }
        public Guid ChangedByUserId { get; set; }
        public bool IsEnable { get; set; }

        public Feature Feature { get; set; }
        public User CheckedByUser { get; set; }
    }

    public enum FeatureItemOwnerType
    {
        System = 1,
        User
    }
}

