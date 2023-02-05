using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using SearchForApi.Core;
//using static SearchForApi.GAuth;

namespace SearchForApi.Integrations.SearchForModules
{
    public class GoogleTokenValidator : BaseModule, IGoogleTokenValidator
    {
        //private readonly GAuthClient _client;
        //private readonly RestClient _client;

        public GoogleTokenValidator()
        {
            //_client = new GAuthClient(_channel);
            //_client = new RestClient(Cfg.SearchforModuleServiceUrl);
        }

        //public async Task<ValidateTokenReply> Validate(string idToken)
        //{
        //    var result = await _client.ValidateTokenAsync(new ValidateTokenRequest { IdToken = idToken });
        //    return result;
        //}

        //public async Task<ValidateTokenReply> Validate(string idToken)
        //{
        //    try
        //    {
        //        var request = new RestRequest("api/auth/google/validate", Method.POST).AddJsonBody(new
        //        {
        //            IdToken = idToken
        //        });

        //        var result = await _client.ExecutePostAsync<ValidateTokenReply>(request);
        //        if (result.StatusCode != HttpStatusCode.OK)
        //            throw new Exception($"StatusCode: {result.StatusCode}, Message: {result.Content}");

        //        var data = result.Data;
        //        return data;
        //    }
        //    catch (Exception e)
        //    {
        //        Serilog.Log.Error(e, "Third-Party Exception: {integration}/{api}", "Google Auth", "Validate-Token");
        //        return null;
        //    }
        //}
    }
}

