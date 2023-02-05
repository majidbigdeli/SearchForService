using System;
using System.Net.Http;
using Grpc.Net.Client;
using SearchForApi.Core;

namespace SearchForApi.Integrations.SearchForModules
{
    public class BaseModule
    {
        public readonly GrpcChannel _channel;

        public BaseModule()
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _channel = GrpcChannel.ForAddress(Cfg.SearchforModuleServiceUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
        }
    }
}