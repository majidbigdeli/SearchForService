using System.Collections.Generic;

namespace SearchForApi.Models
{
    public class SharedResultItemModel
    {
        public string Id { get; set; }
        public IEnumerable<SearchResultItemTextItems> Texts { get; set; }
        public IEnumerable<SearchResultItemTextItems> Translations { get; set; }
        public string SceneUrl { get; set; }
        public string MovieName { get; set; }
        public string Keyword { get; set; }
        public string ImageUrl { get; set; }
    }
}