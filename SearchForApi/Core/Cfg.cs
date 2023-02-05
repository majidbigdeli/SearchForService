using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SearchForApi.Core
{
    public static class Cfg
    {
        public static bool IsDev => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public static string BaseUrl => Environment.GetEnvironmentVariable("BaseUrl");
        public static string SceneBaseUrl => Environment.GetEnvironmentVariable("SceneBaseUrl");

        public static string EmailVerificationResultUrl => Environment.GetEnvironmentVariable("EmailVerificationResultUrl");
        public static string ResetPasswordBaseUrl => Environment.GetEnvironmentVariable("ResetPasswordBaseUrl");

        public static string ElasticHost => Environment.GetEnvironmentVariable("ElasticHost");
        public static string ElasticDefaultIndex => Environment.GetEnvironmentVariable("ElasticDefaultIndex");
        public static string ElasticUser => Environment.GetEnvironmentVariable("ElasticUser");
        public static string ElasticPassword => Environment.GetEnvironmentVariable("ElasticPassword");

        public static string ElasticLogHost => Environment.GetEnvironmentVariable("ElasticLogHost");
        public static string ElasticLogUser => Environment.GetEnvironmentVariable("ElasticLogUser");
        public static string ElasticLogPassword => Environment.GetEnvironmentVariable("ElasticLogPassword");

        public static string S3Endpoint => Environment.GetEnvironmentVariable("S3Endpoint");
        public static string S3AccessKey => Environment.GetEnvironmentVariable("S3AccessKey");
        public static string S3SecretKey => Environment.GetEnvironmentVariable("S3SecretKey");

        public static string ScenesBucketName => Environment.GetEnvironmentVariable("ScenesBucketName");

        public static string EncryptionPassPhrase => Environment.GetEnvironmentVariable("EncryptionPassPhrase");

        public static string SendinBlueApiKey => Environment.GetEnvironmentVariable("SendinBlueApiKey");
        public static string SendinBlueSenderName => Environment.GetEnvironmentVariable("SendinBlueSenderName");
        public static string SendinBlueSenderEmail => Environment.GetEnvironmentVariable("SendinBlueSenderEmail");

        public static string JWTValidAudience => Environment.GetEnvironmentVariable("JWTValidAudience");
        public static string JWTValidIssuer => Environment.GetEnvironmentVariable("JWTValidIssuer");
        public static SymmetricSecurityKey JWTSecretKey =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTSecretKey")));

        public static string PaymentReturnUrl(string refId) => $"{Environment.GetEnvironmentVariable("PaymentReturnUrl")}/{refId}";

        public static int MaxActiveDevicesPerUser => int.Parse(Environment.GetEnvironmentVariable("MaxActiveDevicesPerUser"));

        public static string RedisHost => Environment.GetEnvironmentVariable("RedisHost");

        public static string JaegerHost => Environment.GetEnvironmentVariable("JaegerHost");
        public static int JaegerPort => int.Parse(Environment.GetEnvironmentVariable("JaegerPort"));

        public static string PostgresConnectionString => Environment.GetEnvironmentVariable("PostgresConnectionString");

        public static int UserShouldBeRenewPlanDaysBefore => int.Parse(Environment.GetEnvironmentVariable("UserShouldBeRenewPlanDaysBefore"));

        public static string SharedSceneBaseUrl => Environment.GetEnvironmentVariable("SharedSceneBaseUrl");
        public static string SharedSceneImageBaseUrl => Environment.GetEnvironmentVariable("SharedSceneImageBaseUrl");
        public static string UserAvatarImageBaseUrl => Environment.GetEnvironmentVariable("UserAvatarImageBaseUrl");
        public static string ResourcesBaseUrl => Environment.GetEnvironmentVariable("ResourcesBaseUrl");

        public static string SearchforModuleServiceUrl => Environment.GetEnvironmentVariable("SearchforModuleServiceUrl");

        public static string AppReleaseBaseUrl => Environment.GetEnvironmentVariable("AppReleaseBaseUrl");

        public static string KavehnegarApiKey => Environment.GetEnvironmentVariable("KavehnegarApiKey");

        public static int TrialDays => int.Parse(Environment.GetEnvironmentVariable("TrialDays"));

        public static int MaxFeatureItemCount => int.Parse(Environment.GetEnvironmentVariable("MaxFeatureItemCount"));
        public static int MaxSearchSeggestionItemCount => int.Parse(Environment.GetEnvironmentVariable("MaxSearchSeggestionItemCount"));

        public static int MaxAnonymousQuoteViewPerSearch => int.Parse(Environment.GetEnvironmentVariable("MaxAnonymousQuoteViewPerSearch"));
        public static int MaxSearchPerDay => int.Parse(Environment.GetEnvironmentVariable("MaxSearchPerDay"));
        public static int MaxQuoteViewPerSearch => int.Parse(Environment.GetEnvironmentVariable("MaxQuoteViewPerSearch"));
        public static int MaxShareQuote => int.Parse(Environment.GetEnvironmentVariable("MaxShareQuote"));
        public static int MaxBookmarkedCount => int.Parse(Environment.GetEnvironmentVariable("MaxBookmarkedCount"));
        public static int MaxLastHistoriesDays => int.Parse(Environment.GetEnvironmentVariable("MaxLastHistoriesDays"));

        public static string SymspellBaseUrl => Environment.GetEnvironmentVariable("SymspellBaseUrl");
    }
}

