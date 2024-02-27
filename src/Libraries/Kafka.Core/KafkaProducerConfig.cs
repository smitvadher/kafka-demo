using Confluent.Kafka;

namespace Kafka.Core
{
    public class KafkaProducerConfig<TValue> : ProducerConfig where TValue : IMessage
    {
        public string Topic { get; set; }
    }
}
