using System;
using Amazon.S3;
using Amazon.S3.Model;
using MethodTimer;

namespace SearchForApi.Repositories
{
    public class SceneFileRepository : S3BaseRepository
    {
        public SceneFileRepository()
        {
        }

        [Time("bucketName={bucketName},objectName={objectName},durationMinute={durationMinute}")]
        public string GeneratePreSignedURL(string bucketName, string objectName, double durationMinute = 5)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    Expires = DateTime.Now.AddMinutes(durationMinute)
                };
                return Client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error: '{e.Message}'");
                return null;
            }
        }
    }
}

