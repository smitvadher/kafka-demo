using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Kafka.Core
{
    internal sealed class Serializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            if (typeof(T) == typeof(Null))
                return null;

            if (typeof(T) == typeof(Ignore))
                throw new NotSupportedException();

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}
