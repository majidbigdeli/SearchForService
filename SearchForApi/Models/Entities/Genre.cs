using System.Collections.Generic;

namespace SearchForApi.Models.Entities
{
    public class Genre : BaseEntity<GenreType>
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }

    public enum GenreType
    {
        genre_biography = 1,
        genre_documentary = 2,
        genre_sport = 3,
        genre_drama = 4,
        genre_adventure = 5,
        genre_thriller = 6,
        genre_mystery = 7,
        genre_romance = 8,
        genre_comedy = 9,
        genre_action = 10,
        genre_crime = 11,
        genre_reality_tv = 12,
        genre_horror = 13,
        genre_fantasy = 14,
        genre_animation = 15,
        genre_sci_fi = 16,
        genre_music = 17,
        genre_history = 18,
        genre_family = 19,
        genre_short = 20,
        genre_musical = 21,
        genre_war = 22,
        genre_western = 23,
        genre_theatre = 24,
        genre_stand_up_comedy = 25,
        genre_live_action = 26,
        genre_superhero = 27,
        genre_magic = 28,
        genre_news = 29,
        genre_film_noir = 30
    }
}