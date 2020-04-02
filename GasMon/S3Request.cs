using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace GasMon
{
    public class S3Request
    {
        private static readonly string BucketName = "gasmonitoring-locationss3bucket-pgef0qqmgwba";
        private const string FileName = "locations.json";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.EUWest2;
        private readonly IAmazonS3 _s3Client;

        public S3Request(IAmazonS3 s3Client)
        {
            this._s3Client = s3Client;
        }

        public async Task<string> FetchLocations(IAmazonS3 s3Client)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = FileName
            };

            using GetObjectResponse response = await s3Client.GetObjectAsync(request);
            using var streamReader = new StreamReader(response.ResponseStream); 
            var content = streamReader.ReadToEnd();
            return content;
        }

        public List<Location> CreateLocationsList()
        {
            var s3Client = new AmazonS3Client(BucketRegion);
            var content = FetchLocations(s3Client).Result;
            return JsonConvert.DeserializeObject<List<Location>>(content);
        }
    }
}