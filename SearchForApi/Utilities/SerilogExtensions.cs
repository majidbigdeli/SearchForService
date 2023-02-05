using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace SearchForApi.Utilities
{
    public static class HttpHeadersLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithHttpHeaders(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With<HttpHeaderEnricher>();
        }
    }

    public class HttpHeaderEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpHeaderEnricher() : this(new HttpContextAccessor())
        {
        }

        public HttpHeaderEnricher(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var platform = _contextAccessor.HttpContext?.Request?.Headers?["platform"];
            var platformProperty = propertyFactory.CreateProperty("Platform", platform);
            logEvent.AddOrUpdateProperty(platformProperty);

            var appVersion = _contextAccessor.HttpContext?.Request?.Headers?["appVersion"];
            var appVersionProperty = propertyFactory.CreateProperty("AppVersion", appVersion);
            logEvent.AddOrUpdateProperty(appVersionProperty);
        }
    }
}