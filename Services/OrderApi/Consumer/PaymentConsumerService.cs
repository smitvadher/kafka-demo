using Confluent.Kafka;
using Newtonsoft.Json;
using OrderApi.Messages;

namespace OrderApi.Consumer
{
    public class PaymentConsumerService : BackgroundService
    {
        private readonly ILogger<PaymentConsumerService> _logger;
        private readonly IConsumer<string, string> _consumer;

        public PaymentConsumerService(IConfiguration configuration, ILogger<PaymentConsumerService> logger)
        {
            _logger = logger;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "PaymentConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig)
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("PaymentTransaction");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    var message = JsonConvert.DeserializeObject<PaymentMessage>(consumeResult.Message.Value);

                    // logic to mark order as paid
                    // adjust inventory - (can also produce adjust inventory message if inventory is managed in other service)
                    _logger.LogInformation(message.Paid
                        ? $"Received order#{message.OrderId} payment successfully."
                        : $"Order#{message.OrderId} payment failed due to {message.ErrorMessage}.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing Kafka message: {ex.Message}");
                }
                await Task.Delay(1000, cancellationToken);
            }

            _consumer.Close();
        }
    }
}
