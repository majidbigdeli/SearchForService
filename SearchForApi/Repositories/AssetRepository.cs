using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using MethodTimer;

namespace SearchForApi.Repositories
{
    public class AssetRepository : S3BaseRepository
    {
        public AssetRepository()
        {
        }

        [Time("objectName={objectName},filePath={filePath},contentType={contentType},publicRead={publicRead}")]
        public async Task UploadFile(string objectName, string filePath, string contentType, bool publicRead = false)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = "users-assets",
                    Key = objectName,
                    FilePath = filePath,
                    ContentType = contentType,
                    CannedACL = publicRead ? S3CannedACL.PublicRead : S3CannedACL.NoACL
                };

                var response = await Client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        [Time("objectName={objectName},url={url},contentType={contentType},publicRead={publicRead}")]
        public async Task UploadUrl(string objectName, string url, string contentType, bool publicRead = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var stream = await client.GetByteArrayAsync(url);

                    var putRequest = new PutObjectRequest
                    {
                        BucketName = "users-assets",
                        Key = objectName,
                        InputStream = new MemoryStream(stream),
                        ContentType = contentType,
                        CannedACL = publicRead ? S3CannedACL.PublicRead : S3CannedACL.NoACL
                    };

                    var response = await Client.PutObjectAsync(putRequest);
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}

