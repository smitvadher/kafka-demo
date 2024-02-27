using Confluent.Kafka;

namespace Kafka.Core
{
    public class KafkaConsumerConfig<TValue> : ConsumerConfig where TValue : IMessage
    {
        public KafkaConsumerConfig()
        {
            AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        }
    }
}
