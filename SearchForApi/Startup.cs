using System;
using System.Text.Json.Serialization;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using SearchForApi.Core;
using SearchForApi.Factories;
using SearchForApi.Integrations.Kavehnegar;
using SearchForApi.Integrations.SearchForModules;
using SearchForApi.Integrations.Sendinblue;
using SearchForApi.Integrations.Symspell;
using SearchForApi.Mappers;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;
using SearchForApi.Repositories;
using SearchForApi.Services;
using SearchForApi.Utilities;

namespace SearchForApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthFactory, AuthFactory>();
            services.AddScoped<ElasticSubtitleRepository>();
            services.AddScoped<MovieRepository>();
            services.AddScoped<PlanRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<PaymentRepository>();
            services.AddScoped<PaymentGatewayRepository>();
            services.AddScoped<SceneRepository>();
            services.AddScoped<SceneFileRepository>();
            services.AddScoped<AssetRepository>();
            services.AddScoped<HistoryRepository>();
            services.AddScoped<BookmarkRepository>();
            services.AddScoped<AppReleaseRepository>();
            services.AddScoped<BannedKeywordRepository>();
            services.AddScoped<DiscountRepository>();
            services.AddScoped<ShareRepository>();
            services.AddScoped<ShareHistoryRepository>();
            services.AddScoped<ReportRepository>();
            services.AddScoped<UserDeviceRepository>();
            services.AddScoped<FeatureRepository>();
            services.AddScoped<FeatureItemRepository>();
            services.AddScoped<UserDiscountRepository>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<ISceneService, SceneService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IBannedKeywordFactory, BannedKeywordFactory>();
            services.AddScoped<ISceneFactory, SceneFactory>();
            services.AddScoped<IMetricService, MetricService>();
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentFactory, PaymentFactory>();
            services.AddScoped<ILimitationService, LimitationService>();
            services.AddScoped<IFeatureService, FeatureService>();
            services.AddScoped<IRequestPlanService, RequestPlanService>();

            services.AddSingleton<IKavehnegarIntegration, KavehnegarIntegration>();
            services.AddSingleton<ISendinblueIntegration, SendinblueIntegration>();
            services.AddSingleton<IHistoryMetricFactory, HistoryMetricFactory>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IMovieFactory, MovieFactory>();
            services.AddSingleton<IPlanFactory, PlanFactory>();
            services.AddSingleton<IDateTimeFactory, DateTimeFactory>();
            services.AddSingleton<ILinkFactory, LinkFactory>();
            services.AddSingleton<IDiscountFactory, DiscountFactory>();
            services.AddSingleton<IUserDiscountFactory, UserDiscountFactory>();
            services.AddSingleton<IFeatureFactory, FeatureFactory>();
            services.AddSingleton<IHistoryFactory, HistoryFactory>();
            services.AddSingleton<IShareFactory, ShareFactory>();
            services.AddSingleton<IShareHistoryFactory, ShareHistoryFactory>();
            services.AddSingleton<IGoogleTokenValidator, GoogleTokenValidator>();
            services.AddSingleton<ISymspellIntegration, SymspellIntegration>();

            services.AddOpenTracing();

            services.AddSingleton<ITracer>(sp =>
            {
                var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender(Cfg.JaegerHost, Cfg.JaegerPort, 0))
                    .Build();
                var tracer = new Tracer.Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .WithReporter(reporter)
                    .Build();
                return tracer;
            });

            services.Configure<HttpHandlerDiagnosticOptions>(options =>
                options.OperationNameResolver =
                    request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");

            services.AddDbContext<ApiContext>(options => options.UseNpgsql(
                Cfg.PostgresConnectionString,
                options => options.SetPostgresVersion(new Version(9, 6))));

            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApiContext>()
            .AddUserStore<ApiUserStore>()
            .AddUserManager<ApiUserManager>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = !Cfg.IsDev;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Cfg.JWTValidAudience,
                    ValidIssuer = Cfg.JWTValidIssuer,
                    IssuerSigningKey = Cfg.JWTSecretKey
                };
            });

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Cfg.RedisHost;
                option.InstanceName = "master";
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("development", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyHeader();
                });

                opt.AddDefaultPolicy(builder =>
                {
                    builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(
                            "https://search4.ir",
                            "https://*.search4.ir",
                            "https://searchfor.ir",
                            "https://*.searchfor.ir")
                        .WithHeaders(
                            "platform",
                            "appVersion",
                            "authorization",
                            "api-version",
                            "content-type")
                        .AllowAnyMethod();
                });
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<DefaultProfile>();
            });

            services.AddControllers(options =>
                options.Filters.Add(new HttpResponseExceptionFilter())
            )
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerDoc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            if (env.IsDevelopment())
                app.UseCors("development");
            else
                app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDoc(provider);
        }
    }
}

