using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace GasMon
{
    public class SQSService
    {
        private readonly IAmazonSQS _sqsClient;

        public SQSService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task<string> CreateQueueAsync()
        {
            var res = await _sqsClient.CreateQueueAsync("GasMonQueueAlt");
            return res.QueueUrl;
        }

        public async Task DeleteQueueAsync(string queueUrl)
        {
            await _sqsClient.DeleteQueueAsync(queueUrl);
        }

        public async Task<IEnumerable<Message>> FetchMessagesAsync(string queueUrl)
        {
            var req = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 5,
                MaxNumberOfMessages = 10
            };
            var res = await _sqsClient.ReceiveMessageAsync(req);
            var messages = res.Messages;
            await DeleteSQSMessage(queueUrl, messages);
            return messages;
        }
        
        public async Task DeleteSQSMessage(string queueUrl, IEnumerable<Message> messages)
        {
            var deleteEntries = messages
                .Select(message => new DeleteMessageBatchRequestEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    ReceiptHandle = message.ReceiptHandle
                }).ToList();
            
            var req = new DeleteMessageBatchRequest
            {
                QueueUrl = queueUrl,
                Entries = deleteEntries
            };
            await _sqsClient.DeleteMessageBatchAsync(req);
        }
    }
}
