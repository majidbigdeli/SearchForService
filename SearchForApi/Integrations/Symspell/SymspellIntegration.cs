using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MethodTimer;
using Nest;
using RestSharp;
using SearchForApi.Core;

namespace SearchForApi.Integrations.Symspell
{
	public class SymspellIntegration : ISymspellIntegration
    {
        private readonly RestClient _client;

        public SymspellIntegration()
		{
            _client = new RestClient(Cfg.SymspellBaseUrl);
        }

        [Time("phrase={phrase},distance={distance}")]
        public async Task<string> CorrectPhrase(string phrase, int distance = 2)
        {
            try
            {
                var request = new RestRequest("/compound/json", Method.POST).AddJsonBody(new
                {
                    document = phrase,
                    distance,
                });

                var result = await _client.ExecutePostAsync<List<SymspellResultDto>>(request);
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"StatusCode: {result.StatusCode}, Message: {result.Content}");

                var data = result.Data;

                if (!data.Any())
                    throw new Exception($"Not Found: /{phrase}/");

                return data.First().term;
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Third-Party Exception: {integration}/{api}", "Symspell", "Correect-Phrase");
                return phrase;
            }
        }
    }
}

