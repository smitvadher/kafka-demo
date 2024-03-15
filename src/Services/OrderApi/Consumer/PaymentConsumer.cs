using Confluent.Kafka;
using Kafka.Core;
using OrderApi.Messages;

namespace OrderApi.Consumer
{
    public class PaymentConsumer : IKafkaConsumerHandler<string, PaymentMessage>
    {
        private readonly ILogger<PaymentConsumer> _logger;

        public PaymentConsumer(ILogger<PaymentConsumer> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(Message<string, PaymentMessage> message)
        {
            var data = message.Value;

            // logic to mark order as paid
            // adjust inventory - (can also produce adjust inventory message if inventory is managed in other service)
            _logger.LogInformation(data.Paid
                ? $"Received order#{data.OrderId} payment successfully."
                : $"Order#{data.OrderId} payment failed due to {data.ErrorMessage}.");

            return Task.CompletedTask;
        }
    }
}
