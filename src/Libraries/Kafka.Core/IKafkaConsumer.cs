namespace Kafka.Core
{
    public interface IKafkaConsumer<out TKey, out TValue> where TValue : IMessage
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }
}
