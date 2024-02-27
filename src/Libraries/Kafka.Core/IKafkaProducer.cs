namespace Kafka.Core
{
    public interface IKafkaProducer<TValue> where TValue : IMessage
    {
        Task ProduceAsync(string key, TValue message, CancellationToken cancellationToken);
    }
}
