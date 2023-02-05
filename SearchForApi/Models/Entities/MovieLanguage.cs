using System;

namespace SearchForApi.Models.Entities
{
    public class MovieLanguage: BaseEntity<Guid>
    {
        public Guid MovieId { get; set; }
        public int LanguageId { get; set; }

        public Movie Movie { get; set; }
        public Language Language { get; set; }
    }
}