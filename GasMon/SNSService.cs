using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SQS;

namespace GasMon
{
    public class SNSService
    {
        private const string TopicArn = "arn:aws:sns:eu-west-2:099421490492:GasMonitoring-snsTopicSensorDataPart1-1YOM46HA51FB";
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IAmazonSQS _sqsClient;

        public SNSService(IAmazonSimpleNotificationService snsClient, IAmazonSQS sqsClient)
        {
            _snsClient = snsClient;
            _sqsClient = sqsClient;
        }

        public async Task<string> SubscribeQueueAsync(string queueUrl)
        {
            return await _snsClient.SubscribeQueueAsync(TopicArn, _sqsClient, queueUrl);
        }

        public async Task UnsubscribeQueueAsync(string subscriptionArn)
        {
            await _snsClient.UnsubscribeAsync(subscriptionArn);
        }
        
    }
}