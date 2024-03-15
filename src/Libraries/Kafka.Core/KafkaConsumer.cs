using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kafka.Core
{
    public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue> where TValue : IMessage
    {
        private readonly IKafkaConsumerHandler<TKey, TValue> _consumerHandler;
        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;
        private readonly KafkaConsumerConfig<TValue> _config;

        public KafkaConsumer(IKafkaConsumerHandler<TKey, TValue> consumerHandler,
            IOptions<KafkaConsumerConfig<TValue>> config,
            ILogger<KafkaConsumer<TKey, TValue>> logger)
        {
            _consumerHandler = consumerHandler;
            _logger = logger;
            _config = config.Value;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            var consumerBuilder = new ConsumerBuilder<TKey, TValue>(_config)
                .SetValueDeserializer(new KafkaDeserializer<TValue>());

            using (var consumer = consumerBuilder.Build())
            {
                var topic = _config.Topic ?? typeof(TValue).Name;
                consumer.Subscribe(topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        var message = consumeResult.Message;
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
