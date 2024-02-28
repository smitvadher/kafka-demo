using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Kafka.Core
{
    public class KafkaConsumer<TValue> : IKafkaConsumer<TValue> where TValue : IMessage
    {
        private readonly IKafkaConsumerHandler<TValue> _consumerHandler;
        private readonly ILogger<KafkaConsumer<TValue>> _logger;
        private readonly KafkaConsumerConfig<TValue> _config;

        public KafkaConsumer(IKafkaConsumerHandler<TValue> consumerHandler,
            IOptions<KafkaConsumerConfig<TValue>> config,
            ILogger<KafkaConsumer<TValue>> logger)
        {
            _consumerHandler = consumerHandler;
            _logger = logger;
            _config = config.Value;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            var consumerBuilder = new ConsumerBuilder<string, string>(_config);

            using (var consumer = consumerBuilder.Build())
            {
                consumer.Subscribe(typeof(TValue).Name);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        var message = JsonConvert.DeserializeObject<TValue>(consumeResult.Message.Value);
                        await _consumerHandler.HandleAsync(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing Kafka message: {ex.Message}");
                    }

                    await Task.Delay(1000, cancellationToken);
                }
            }
        }
    }
}
