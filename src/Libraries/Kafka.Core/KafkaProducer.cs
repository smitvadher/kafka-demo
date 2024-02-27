using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Kafka.Core
{
    public class KafkaProducer<TValue> : IKafkaProducer<TValue> where TValue : IMessage
    {
        private readonly KafkaProducerConfig<TValue> _kafkaConfigurations;
        private readonly IProducer<string, string> _producer;

        public KafkaProducer(IOptions<KafkaProducerConfig<TValue>> kafkaConfigurations)
        {
            _kafkaConfigurations = kafkaConfigurations.Value;
            var producerBuilder = new ProducerBuilder<string, string>(kafkaConfigurations.Value);

            _producer = producerBuilder.Build();
        }

        public async Task ProduceAsync(string key, TValue message, CancellationToken cancellationToken)
        {
            var topic = _kafkaConfigurations.Topic ?? message.GetType().Name;

            var producedMessage = new Message<string, string>
            {
                Key = key,
                Value = JsonConvert.SerializeObject(message)
            };

            await _producer.ProduceAsync(topic, producedMessage, cancellationToken);
        }
    }
}
