namespace Kafka.Core
{
    public interface IKafkaConsumer<out TValue> where TValue : IMessage
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }
}
