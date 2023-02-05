using System;
namespace SearchForApi.Models.Dtos
{
	public class FeatureItemDto
	{
        public Guid Id { get; set; }
        public string Keyword { get; set; }
        public string Translation { get; set; }
        public string Sample { get; set; }
        public string CoverUrl { get; set; }
        public string ParameterId { get; set; }
    }
}

