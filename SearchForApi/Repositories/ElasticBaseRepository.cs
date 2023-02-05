using System;
using Nest;
using SearchForApi.Core;

namespace SearchForApi.Repositories
{
    public class ElasticBaseRepository
    {
        public ElasticClient Client { get; set; }

        public ElasticBaseRepository()
        {
            var settings = new ConnectionSettings(new Uri(Cfg.ElasticHost))
                    .DefaultIndex(Cfg.ElasticDefaultIndex)
                    .ThrowExceptions(alwaysThrow: true)
                    .EnableDebugMode()
                    .BasicAuthentication(Cfg.ElasticUser, Cfg.ElasticPassword);

            Client = new ElasticClient(settings);
        }
    }
}

