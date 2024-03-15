using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Kafka.Core
{
    internal sealed class KafkaDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (typeof(T) == typeof(Null))
                return default;

            if (typeof(T) == typeof(Ignore))
                return default;

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
        }
    }
}
