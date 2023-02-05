using System;
using System.Collections.Generic;

namespace SearchForApi.Models.Entities
{
    public class Movie : BaseEntity<Guid>
    {
        public int InternalId { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public string PersianTitle { get; set; }
        public string PersianPlot { get; set; }
        public MovieType Type { get; set; }
        public string ImdbId { get; set; }
        public float? ImdbScore { get; set; }
        public string StorageId { get; set; }
        public string BucketName { get; set; }
        public MpaaType Mpaa { get; set; }
        public string MpaaDescription { get; set; }
        public int LanguageId { get; set; }
        public int ReleaseYear { get; set; }
        public float ViewScore { get; set; }
        public int SegmentCount { get; set; }
        public TimeSpan? IntroStart { get; set; }
        public TimeSpan? IntroEnd { get; set; }

        public AssetsCheckStatus AssetsCheckedStatus { get; set; }
        public DateTime? AssetsCheckedOn { get; set; }
        public Guid? AssetsCheckedByUserId { get; set; }

        public Language Language { get; set; }

        public User AssetsCheckedByUser { get; set; }
        public ICollection<MovieLanguage> Languages { get; set; }
        public ICollection<MovieGenre> Genres { get; set; }
        public ICollection<Scene> Scenes { get; set; }
    }

    public enum MovieType
    {
        Movie = 1,
        Series
    }

    public enum AssetsCheckStatus
    {
        None,
        Verified,
        CoverIssue,
        ThumbnailIssue,
        CoverAndThumbnailIssue
    }

    public enum MpaaType
    {
        Unknown,
        Normal,
        StrongSexual,
        Nudity,
        Sexual,
        Sex,
        StrongViolence,
        Violence,
        R,
        Limitation
    }
}