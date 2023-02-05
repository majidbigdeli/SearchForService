using System.Collections.Generic;

namespace SearchForApi.Models.Entities
{
    public class Language: BaseEntity<int>
    {
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
        public ICollection<MovieLanguage> MovieLanguages { get; set; }
    }
}