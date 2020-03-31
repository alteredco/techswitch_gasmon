using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GasMon
{
    public class S3Request
    {
        private readonly string bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");
        private readonly string region = Environment.GetEnvironmentVariable("AWS_REGION");
        private readonly string accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        private readonly string secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        private readonly string fileName = "locations.json";
        
        private IAmazonS3 s3Client = 
            new AmazonS3Client(awsAccessKeyId: Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
                awsSecretAccessKey: Environment.GetEnvironmentVariable("AWS_SECRET_KEY"),
                RegionEndpoint.EUWest2
                );

        public void LocationsFetcher(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
        }
        
        public async Task<string> FetchLocations()
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };
            
            using GetObjectResponse response = await s3Client.GetObjectAsync(request);
            using var streamReader = new StreamReader(response.ResponseStream); 
            var content = streamReader.ReadToEnd();

            return JsonSerializer.Deserialize<string>(content);
        }
    }
}