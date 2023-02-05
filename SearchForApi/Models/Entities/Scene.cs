using System;
using System.Collections.Generic;
using System.Linq;
using SearchForApi.Utilities;

namespace SearchForApi.Models.Entities
{
    public class Scene : BaseEntity<Guid>
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public Guid MovieId { get; set; }
        public SceneLangaugeType Language { get; set; }
        public string Text { get; set; }
        public string PlainText { get; set; }

        public SceneCheckResultType CheckResultType { get; set; }
        public SceneCheckType? CheckType { get; set; }
        public SceneCheckExcludeType? CheckExcludeType { get; set; }
        public DateTime? CheckedOn { get; set; }
        public Guid? CheckedByUserId { get; set; }
        public Guid? CheckedBySceneId { get; set; }

        public User CheckedByUser { get; set; }
        public Scene CheckedByScene { get; set; }
        public ICollection<Scene> CheckedScenes { get; set; }

        public Movie Movie { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<History> Histories { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<Share> Shares { get; set; }

        public string NormalizedPlainText => PlainText.RemoveNewLines();
        public bool? IsForKids => Movie?.Genres?.Any(p => p.GenreId == GenreType.genre_animation);
    }

    public enum SceneLangaugeType
    {
        En = 1,
        Fa
    }

    public enum SceneCheckResultType
    {
        NotChecked,
        Normal,
        Excluded
    }

    public enum SceneCheckType
    {
        Manual = 1,
        Seen,
        Reported,
        Shared,
        Bookmarked
    }

    public enum SceneCheckExcludeType
    {
        Scene = 1,
        Text,
        VideoContent,
        TextContent,
        NotSync,
        Other,
        NotForKids
    }
}