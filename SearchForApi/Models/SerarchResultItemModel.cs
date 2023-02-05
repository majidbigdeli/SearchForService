using System.Collections.Generic;

namespace SearchForApi.Models
{
    public class SerarchResultItemModel
    {
        public string Id { get; set; }
        public IEnumerable<SearchResultItemTextItems> Texts { get; set; }
        public IEnumerable<SearchResultItemTextItems> Translations { get; set; }
        public string SceneUrl { get; set; }
        public string MovieName { get; set; }
        public bool IsBookmarked { get; set; }
        public bool? IsForKids { get; set; }
    }

    public class SearchResultItemTextItems
    {
        public string Text { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}

