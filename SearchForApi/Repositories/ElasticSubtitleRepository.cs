using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Nest;
using SearchForApi.Models.Entities;

namespace SearchForApi.Repositories
{
    public class ElasticSubtitleRepository : ElasticBaseRepository
    {
        public ElasticSubtitleRepository()
        {
        }

        [Time("phrase={phrase},skip={skip},language={language}")]
        public async Task<ISearchResponse<ElasticSubtitleEntity>> LookUpPhrase(string phrase, int skip, bool kidsMode, SceneLangaugeType language)
        {
            try
            {
                var result = await Client.SearchAsync<ElasticSubtitleEntity>(s =>
                s.Query(p =>
                    p.MatchPhrase(q => q.Field(f => f.Text).Query(phrase)) &&
                    (kidsMode ? p.Match(q => q.Field(f => f.Genres).Query("animation")) : new QueryContainer()) &&
                    p.Bool(k => k.Filter(
                        fq => fq.Term(p => p.MovieLanguage, "english"),
                        fq => fq.Term(p => p.Language, language.ToString().ToLower()),
                        fq => fq.Term(p => p.Mpaa, "normal"),
                        fq => !fq.Term(p => p.Excluded, "yes")
                        // fq => !fq.Terms(p => p.Field(f => f.baneType).Terms("strong_sexual", "nudity", "sexual", "sex", "R", "unknown"))
                    )))
                .Skip(skip)
                .Take(1)
                .Sort(s => s
                   .Field(f => f
                       .Field(c => c.MovieScore)
                       .Order(SortOrder.Descending)
                   )
                   .Field(f => f
                       .Field(c => c.RandomSeed)
                       .Order(SortOrder.Ascending)
                   )
                ));

                return result;
            }
            catch
            {
                return null;
            }
        }

        [Time("sceneId={sceneId}")]
        public async Task<ElasticSubtitleEntity> GetDocBySceneId(string sceneId)
        {
            try
            {
                var result = await Client.SearchAsync<ElasticSubtitleEntity>(s =>
                s.Query(p =>
                    p.Bool(k => k.Filter(
                        fq => fq.MatchPhrase(q => q.Field(f => f.SceneId).Query(sceneId))
                    )))
                );

                return result.Hits.FirstOrDefault()?.Source;
            }
            catch
            {
                return null;
            }
        }

        [Time("sceneIds={sceneIds}")]
        public async Task<ISearchResponse<ElasticSubtitleEntity>> GetDocsBySceneId(List<string> sceneIds)
        {
            try
            {
                var filters = sceneIds.Select(p =>
                    new Func<QueryContainerDescriptor<ElasticSubtitleEntity>, QueryContainer>
                        (fq => fq.MatchPhrase(q => q.Field(f => f.SceneId).Query(p))));

                var result = await Client.SearchAsync<ElasticSubtitleEntity>(u => u
                    .Query(q => q.Bool(v => v.Should(filters)))
                );

                return result;
            }
            catch
            {
                return null;
            }
        }

        [Time("sceneIds={sceneIds}")]
        public async Task ExcludeDocsBySceneId(List<string> sceneIds)
        {
            try
            {
                var filters = sceneIds.Select(p =>
                    new Func<QueryContainerDescriptor<ElasticSubtitleEntity>, QueryContainer>
                        (fq => fq.MatchPhrase(q => q.Field(f => f.SceneId).Query(p))));

                var result = await Client.UpdateByQueryAsync<ElasticSubtitleEntity>(u => u
                    .Query(q => q.Bool(v => v.Should(filters)))
                    .Script($"ctx._source.excluded = 'Yes'")
                );

                if (!result.IsValid || result.Updated != sceneIds.Count)
                    throw new Exception("Scene Exclude Operation was not valid.");
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Elastic Exception: {method}/{sceneId}", "ExcludeDocById", sceneIds[0]);
            }
        }
    }
}

