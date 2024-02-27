namespace Kafka.Core
{
    public interface IKafkaConsumerHandler<TValue> where TValue : IMessage
    {
        Task HandleAsync(TValue message);
    }
}
