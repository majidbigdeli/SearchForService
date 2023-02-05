using Nest;

namespace SearchForApi.Models.Entities
{
    [ElasticsearchType(RelationName = "SceneType")]
    public class ElasticSubtitleEntity
    {
        [Text]
        public string MovieId { get; set; }

        [Text]
        public string SceneId { get; set; }

        [Number]
        public int Season { get; set; }

        [Number]
        public int Episode { get; set; }

        [Text]
        public string Language { get; set; }

        [Text]
        public string MovieLanguage { get; set; }

        [Text]
        public string MovieName { get; set; }

        [Text]
        public string Mpaa { get; set; }

        [Text]
        public string Excluded { get; set; }

        [Number]
        public int StartTime { get; set; }

        [Number]
        public int EndTime { get; set; }

        [Text(Norms = false)]
        public string Text { get; set; }

        [Text]
        public string Genres { get; set; }

        [Text]
        public string GenresTranslation { get; set; }

        [Number]
        public int ReleaseYear { get; set; }

        [Number]
        public float MovieScore { get; set; }

        [Number]
        public int RandomSeed { get; set; }
    }
}

