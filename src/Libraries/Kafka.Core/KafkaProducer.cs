using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Kafka.Core
{
    public class KafkaProducer<TValue> : IKafkaProducer<TValue> where TValue : IMessage
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaProducerConfig<TValue> _config;

        public KafkaProducer(IOptions<KafkaProducerConfig<TValue>> config)
        {

            _config = config.Value;
            var producerBuilder = new ProducerBuilder<string, string>(config.Value);
            _producer = producerBuilder.Build();
        }

        public async Task ProduceAsync(string key, TValue message, CancellationToken cancellationToken)
        {
            var topic = _config.Topic ?? message.GetType().Name;

            var producedMessage = new Message<string, string>
            {
                Key = key,
                Value = JsonConvert.SerializeObject(message)
            };

            await _producer.ProduceAsync(topic, producedMessage, cancellationToken);
        }
    }
}
