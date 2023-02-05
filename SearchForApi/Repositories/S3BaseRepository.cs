using Amazon.S3;
using SearchForApi.Core;

namespace SearchForApi.Repositories
{
    public class S3BaseRepository
    {
        public IAmazonS3 Client { get; set; }

        public S3BaseRepository()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(Cfg.S3AccessKey, Cfg.S3SecretKey);
            var config = new AmazonS3Config { ServiceURL = Cfg.S3Endpoint };
            Client = new AmazonS3Client(awsCredentials, config);
        }
    }
}

