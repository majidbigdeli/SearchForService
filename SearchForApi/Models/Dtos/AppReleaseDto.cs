using System;
namespace SearchForApi.Models.Dtos
{
    public class AppReleaseDto
    {
        public string Version { get; set; }
        public DateTime ReleasedOn { get; set; }
        public string ReleaseNote { get; set; }
        public string ReleaseUrl { get; set; }
        public string DownloadUrl { get; set; }
        public bool ForceUpdate { get; set; }
    }
}
