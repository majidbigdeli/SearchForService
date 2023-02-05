using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.Dtos
{
	public class FeatureDto
	{
        public Guid Id { get; set; }
        public string Title { get; set; }
        public FeatureType Type { get; set; }
        public string CoverUrl { get; set; }
    }
}

