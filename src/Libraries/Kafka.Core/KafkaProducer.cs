using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Kafka.Core
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue> where TValue : class, IMessage
    {
        private readonly KafkaProducerConfig<TValue> _config;
        private readonly IProducer<TKey, TValue> _producer;

        public KafkaProducer(IOptions<KafkaProducerConfig<TValue>> config)
        {
            _config = config.Value;
            var producerBuilder = new ProducerBuilder<TKey, TValue>(config.Value)
                .SetValueSerializer(new Serializer<TValue>());
            _producer = producerBuilder.Build();
        }

        public async Task ProduceAsync(TKey key, TValue message, CancellationToken cancellationToken)
        {
            var topic = _config.Topic ?? message.GetType().Name;

            var producedMessage = new Message<TKey, TValue>
            {
                Key = key,
                Value = message
            };

            await _producer.ProduceAsync(topic, producedMessage, cancellationToken);
        }
    }
}
