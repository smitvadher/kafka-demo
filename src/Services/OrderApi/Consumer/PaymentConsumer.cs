using Kafka.Core;
using OrderApi.Messages;

namespace OrderApi.Consumer
{
    public class PaymentConsumer : IKafkaConsumerHandler<PaymentMessage>
    {
        private readonly ILogger<PaymentConsumer> _logger;

        public PaymentConsumer(ILogger<PaymentConsumer> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(PaymentMessage message)
        {
            // logic to mark order as paid
            // adjust inventory - (can also produce adjust inventory message if inventory is managed in other service)
            _logger.LogInformation(message.Paid
                ? $"Received order#{message.OrderId} payment successfully."
                : $"Order#{message.OrderId} payment failed due to {message.ErrorMessage}.");

            return Task.CompletedTask;
        }
    }
}
