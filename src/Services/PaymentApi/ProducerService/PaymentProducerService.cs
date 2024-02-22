using Confluent.Kafka;
using Newtonsoft.Json;
using PaymentApi.Messages;

namespace PaymentApi.ProducerService
{
    public class PaymentProducerService
    {
        private readonly IProducer<string, string> _producer;

        public PaymentProducerService(IConfiguration configuration)
        {
            var producerconfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string, string>(producerconfig).Build();
        }

        public async Task ProduceAsync(string topic, PaymentMessage message)
        {
            var kafkaMessage = new Message<string, string>
            {
                Value = JsonConvert.SerializeObject(message)
            };
            await _producer.ProduceAsync(topic, kafkaMessage);
        }
    }
}
