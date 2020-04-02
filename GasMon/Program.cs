using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace GasMon
{
    class Program
    {
        static void Main(string[] args)
        {
            var s3Client = new AmazonS3Client();
            var sqsClient = new AmazonSQSClient();
            var snsClient = new AmazonSimpleNotificationServiceClient();
            var s3Request = new S3Request(s3Client);
            var sqsService = new SQSService(sqsClient);
            var snsService = new SNSService(snsClient, sqsClient);
            
            var locations = s3Request.FetchLocations();
            
            var processor = new MessageProcessor(sqsService);

            using (var queue = new SQSQueue(sqsService, snsService))
            {
                var endTime = DateTime.Now.AddMinutes(1);
                while (DateTime.Now < endTime)
                {
                    processor.ProcessMessages(queue.QueueUrl);
                }
            };

            Console.WriteLine("monitoring complete...");
        }
    }
}