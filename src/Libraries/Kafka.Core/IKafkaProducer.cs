namespace Kafka.Core
{
    public interface IKafkaProducer<in TKey, in TValue> where TValue : IMessage
    {
        Task ProduceAsync(TKey key, TValue message, CancellationToken cancellationToken);
    }
}
