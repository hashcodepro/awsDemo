using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace awsDemos.Queues
{
    public class DemoQueue
    {
        string serviceUrl, queueUrl;
        AmazonSQSConfig config;
        AmazonSQSClient client;
        public DemoQueue()
        {
            serviceUrl = "service url";
            queueUrl = "queue url";
            config = new AmazonSQSConfig
            {
                ServiceURL = serviceUrl
            };
            client = new AmazonSQSClient(config);
        }

        public async Task<SendMessageResponse> publish(string data)
        {
            var sendRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = data
            };

            return await client.SendMessageAsync(sendRequest);
        }
    }
}