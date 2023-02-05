using System;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface ILinkFactory
    {
        string GetStreamUrl(Guid? userId, string id, int startTime, int endTime);
        (Guid userId, string id, int startSegment, int endSegment) ParseStreamUrl(string queries);
        string GetSegmentUrl(string bucketName, string segmentS3Url);
        (string bucket, string id) ParseSegmentUrl(string queries);
        string GetConfirmEmailUrl(string email, string token);
        string GetRestPasswordUrl(string email, string token);
        string GetSceneShareUrl(string keyword, string sharedToken);
        string GetSceneAssetImageUrl(Movie movie, SceneAssetType type);
        string GetUserAvatarImageUrl(string storageId);
        string GetAppReleaseDownload(string releaseId);
        string GetFeatureCoverUrl(string cover);
    }
}

