using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SearchForApi.Core;
using SearchForApi.Utilities;
using Serilog;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using Serilog.Sinks.Elasticsearch;

namespace SearchForApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog(ConfigureLogger);


        public static Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> ConfigureLogger => (context, provider, configuration) =>
        {
            var env = context.HostingEnvironment;

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .Enrich.WithHttpHeaders();

            configuration.WriteTo.Console().MinimumLevel.Information();

            configuration
                .WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(Cfg.ElasticLogHost))
                {
                    ModifyConnectionSettings = x => x.BasicAuthentication(Cfg.ElasticLogUser, Cfg.ElasticLogPassword),
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat = "searchforapi-{0:yyyy.MM.dd}",
                });
        };
    }
}

