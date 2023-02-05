using System;
namespace SearchForApi.Models.Entities
{
    public class AppRelease : BaseEntity<string>
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public string ReleaseNote { get; set; }
        public bool IsEnable { get; set; }
        public PlatformType Platform { get; set; }
        public bool ForceUpdate { get; set; }

        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public int VersionBuild { get; set; }
        public int VersionRevision { get; set; }

        public Version Version
        {
            get => new(Id);
            set => Id = value.ToString();
        }
    }
}
