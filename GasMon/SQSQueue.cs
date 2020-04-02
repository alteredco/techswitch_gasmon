using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Amazon.S3;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace GasMon
{
    public class SQSQueue : IDisposable
    {
        private readonly SQSService _sqsService;
        private readonly SNSService _snsService;

        public string QueueUrl { get; }
        private readonly string _subscriptionArn;
        public SQSQueue(SQSService sqsService, SNSService snsService)
        {
            _sqsService = sqsService;
            _snsService = snsService;

            QueueUrl = sqsService.CreateQueueAsync().Result;
            _subscriptionArn = _snsService.SubscribeQueueAsync(QueueUrl).Result;

        }

        
        public void Dispose()
        {
            _snsService.UnsubscribeQueueAsync(_subscriptionArn).Wait();
            _sqsService.DeleteQueueAsync(QueueUrl).Wait();
        }
    }
}