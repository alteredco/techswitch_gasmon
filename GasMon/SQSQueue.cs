using System.ComponentModel.DataAnnotations;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace GasMon
{
    public class SQSQueue
    {
        public static string CreateSQSQueue()
        {
            AmazonSQSClient sqsClient = new AmazonSQSClient();
            CreateQueueRequest sqsRequest = new CreateQueueRequest();
            sqsRequest.QueueName = "GasMonQueue";
            var response = sqsClient.CreateQueueAsync(sqsRequest);
        
            return response.QueueUrl;
        }

        public async void ReadSQSQueue(string queueUrl, string messageBody)
        {
            AmazonSQSClient sqsClient = new AmazonSQSClient();
            ReceiveMessageRequest messageRequest =  
                new ReceiveMessageRequest(queueUrl);
            ReceiveMessageResponse messageResponse = await sqsClient.ReceiveMessageAsync(messageRequest);
            bool noMoreMessages = false;

            if (messageResponse.Messages.Count != 0)
            {
                for(int i = 0; i < messageResponse.Messages.Count; i++)
                {
                    if (messageResponse.Messages[i].Body == messageBody)
                    {
                        var receiptHandle = messageResponse.Messages[i].ReceiptHandle;
                    } 
                }
            }
            
        }

        public async void DeleteSQSMessage(string queueUrl, string receiptHandle, AmazonSQSClient client)
        {
            DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
            deleteMessageRequest.QueueUrl = queueUrl;
            deleteMessageRequest.ReceiptHandle = receiptHandle;

            DeleteMessageResponse deleteMessageResponse = await client.DeleteMessageAsync(deleteMessageRequest);
        }
    }
}