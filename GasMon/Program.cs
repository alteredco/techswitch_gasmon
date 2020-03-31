using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace GasMon
{
    class Program
    {
        static void Main(string[] args)
        {
            var s3Client = new AmazonS3Client();
            var s3Request = new S3Request(s3Client);
            var locations = s3Request.FetchLocations();
            
            Console.WriteLine(locations);
        }
    }
}