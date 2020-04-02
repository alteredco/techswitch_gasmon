using System;
using System.Linq;

namespace GasMon
{
    public class MessageProcessor
    {
        private readonly SQSService _sqsService;

        public MessageProcessor(SQSService sqsService)
        {
            _sqsService = sqsService;
        }
        public void ProcessMessages(string queueUrl)
        {
            var messages = _sqsService
                .FetchMessagesAsync(queueUrl).Result;
            
            foreach (var message in messages)
            {
                Console.WriteLine(message.MessageId);
            }
        }
    }
}