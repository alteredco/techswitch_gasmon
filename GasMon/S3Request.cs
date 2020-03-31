using System.Collections.Generic;
using System.IO;
using Amazon.S3;
using Newtonsoft.Json;

namespace GasMon
{
    public class S3Request
    {
        private readonly string BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba";
        private const string FileName = "locations.json";
        private readonly IAmazonS3 s3Client;

        public S3Request(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
        }

        public IEnumerable<Location> FetchLocations()
        {

            var response = s3Client.GetObjectAsync(BucketName, FileName ).Result;
            using var streamReader = new StreamReader(response.ResponseStream); 
            var content = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<IEnumerable<Location>>(content);
        }
    }
}