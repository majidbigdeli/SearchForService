using System;

namespace SearchForApi.Models.Entities
{
    public class MovieGenre: BaseEntity<Guid>
    {
        public Guid MovieId { get; set; }
        public GenreType GenreId { get; set; }

        public Movie Movie { get; set; }
        public Genre Genre { get; set; }
    }
}