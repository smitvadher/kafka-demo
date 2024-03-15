using Confluent.Kafka;
namespace Kafka.Core
{
    public interface IKafkaConsumerHandler<TKey, TValue> where TValue : IMessage
    {
        Task HandleAsync(Message<TKey, TValue> message);
    }
}
