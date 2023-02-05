using System;
using Microsoft.AspNetCore.Http;
using SearchForApi.Models.Entities;
using SearchForApi.Models.Exceptions;

namespace SearchForApi.Utilities
{
    public static class HttpRequestExtensions
    {
        public static string Ip(this HttpRequest httpRequest)
        {
            var ipAddress = httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
            return ipAddress;
        }

        public static PlatformType Platform(this HttpRequest httpRequest)
        {
            var platform = httpRequest.HttpContext.Request.Headers["platform"].ToString();
            var result = platform.ParseEnum<PlatformType>();

            if (result == PlatformType.None)
                throw new FakeRequestException();

            return result;
        }

        public static Version AppVersion(this HttpRequest httpRequest)
        {
            var appVersion = httpRequest.HttpContext.Request.Headers["appVersion"].ToString();
            _ = Version.TryParse(appVersion, out Version result);

            if (result == null)
                throw new FakeRequestException();

            return result;
        }
    }
}