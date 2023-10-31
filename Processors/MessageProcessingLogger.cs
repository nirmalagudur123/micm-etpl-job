using Amazon.SQS.Model;
using Amazon.SQS;
using micm_etpl_job.Interfaces;
using Microsoft.Extensions.Logging;

namespace micm_etpl_job.Processors
{
    public class MessageProcessingLogger : IMessageProcessingLogger
    {
        private readonly ILogger<MessageProcessingLogger> _logger;


        public MessageProcessingLogger(ILogger<MessageProcessingLogger> logger)
        {
            _logger = logger;
        }

        public async void LogProcessStep(string message)
        {
            //log to SignalFX
            _logger.LogInformation(message);

            await LogToSQS(message);
        }

        private async Task LogToSQS(string message)
        {
            try
            {
                //using (var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1))
                //{
                //    await SendMessageToSQS(sqsClient, message);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing.");
            }
        }

        private async Task SendMessageToSQS(AmazonSQSClient sqsClient, string messageBody)
        {
            var sqsQueueUrl = "";

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SQS_MESSAGE_PROCESSING_URL")))
            {
                sqsQueueUrl = Environment.GetEnvironmentVariable("SQS_MESSAGE_PROCESSING_URL");
            }

            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = sqsQueueUrl,
                MessageBody = $"{DateTime.UtcNow:G}: " + messageBody
            };

            await sqsClient.SendMessageAsync(sendMessageRequest);
        }
    }
}
