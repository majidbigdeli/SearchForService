using System;
using SearchForApi.Core;
using SearchForApi.Models.Entities;
using SearchForApi.Utilities;

namespace SearchForApi.Factories
{
    public class LinkFactory : ILinkFactory
    {
        public LinkFactory()
        {
        }

        public string GetStreamUrl(Guid? userId, string id, int startTime, int endTime)
        {
            var urlQueries = StringCipher.Encrypt($"{userId}/{id}/{startTime}/{endTime}");
            return $"{Cfg.SceneBaseUrl}/scene/file/{urlQueries}.m3u8";
        }

        public (Guid userId, string id, int startSegment, int endSegment) ParseStreamUrl(string queries)
        {
            var urlQueries = StringCipher.Decrypt(queries);
            var urlQueriesItems = urlQueries.Split('/');

            _ = Guid.TryParse(urlQueriesItems[0], out Guid userId);

            return (userId, urlQueriesItems[1], int.Parse(urlQueriesItems[2]), int.Parse(urlQueriesItems[3]));
        }

        public string GetSegmentUrl(string bucketName, string segmentS3Url)
        {
            var urlPath = new Uri(segmentS3Url).PathAndQuery;
            return $"../{bucketName}{urlPath}";
        }

        public (string bucket, string id) ParseSegmentUrl(string queries)
        {
            var urlQueries = StringCipher.Decrypt(queries);
            var urlQueriesItems = urlQueries.Split('/');
            return (urlQueriesItems[0], urlQueriesItems[1]);
        }

        public string GetConfirmEmailUrl(string email, string token)
        {
            return $"{Cfg.BaseUrl}/auth/confirm-email/{email}/{token.UrlEncoded()}";
        }

        public string GetRestPasswordUrl(string email, string token)
        {
            return $"{Cfg.ResetPasswordBaseUrl}/{email}/{token.UrlEncoded()}";
        }

        public string GetSceneShareUrl(string keyword, string sharedToken)
        {
            return $"{Cfg.SharedSceneBaseUrl}/{keyword.ReplaceSpaceWithDash()}/{sharedToken}";
        }

        public string GetUserAvatarImageUrl(string storageId)
        {
            var result = $"{Cfg.UserAvatarImageBaseUrl}/user_avatar_{storageId}";

            return result;
        }

        public string GetAppReleaseDownload(string releaseId)
        {
            var result = $"{Cfg.AppReleaseBaseUrl}/search4_v{releaseId}.apk";

            return result;
        }

        public string GetFeatureCoverUrl(string cover)
        {
            var result = $"{Cfg.ResourcesBaseUrl}/{cover}";

            return result;
        }

        public string GetSceneAssetImageUrl(Movie movie, SceneAssetType type)
        {
            var storageId = movie.StorageId;
            var imageStorageId = string.Empty;

            switch (type)
            {
                case SceneAssetType.PosterMedium:
                    imageStorageId = $"{storageId}_poster_m.jpg";
                    break;
                case SceneAssetType.PosterLarge:
                    imageStorageId = $"{storageId}_poster_l.jpg";
                    break;
                case SceneAssetType.PosterXLarge:
                    imageStorageId = $"{storageId}_poster_xl.jpg";
                    break;
                case SceneAssetType.Cover:
                    imageStorageId = $"{storageId}_cover.jpg";
                    break;
            }

            var coverHasIssue =
                (movie.AssetsCheckedStatus == AssetsCheckStatus.CoverAndThumbnailIssue ||
                movie.AssetsCheckedStatus == AssetsCheckStatus.CoverIssue) &&
                type == SceneAssetType.Cover;

            var posterHasIssue =
                (movie.AssetsCheckedStatus == AssetsCheckStatus.CoverAndThumbnailIssue ||
                movie.AssetsCheckedStatus == AssetsCheckStatus.ThumbnailIssue) &&
                type != SceneAssetType.Cover;

            if (coverHasIssue)
                imageStorageId = ""; // fixme: set the placeholder
            else if (posterHasIssue)
                imageStorageId = ""; // fixme: set the placeholder

            return $"{Cfg.SharedSceneImageBaseUrl}/{imageStorageId}";
        }
    }

    public enum SceneAssetType
    {
        PosterMedium = 1,
        PosterLarge,
        PosterXLarge,
        Cover
    }
}

