using System;

namespace SearchForApi.Models.Dtos
{
    public class MovieAssetDto
    {
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
    }
}