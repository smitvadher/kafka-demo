using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kafka.Core
{
    public class KafkaConsumerManager : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly Dictionary<Type, bool> MessageTypes = new();
        private static readonly List<Thread> ConsumerThreads = new();

        public KafkaConsumerManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        internal static void AddConsumerMessage<TValue>() where TValue : IMessage
        {
            MessageTypes.Add(typeof(IKafkaConsumer<TValue>), false);
        }

        private static void StartConsumers(IServiceProvider servicesProvider, CancellationToken stoppingToken)
        {
            foreach (var kafkaConsumer in MessageTypes.Where(kc => !kc.Value))
            {
                using (var scope = servicesProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService(kafkaConsumer.Key);
                    if (service is not IKafkaConsumer<IMessage> consumer) continue;

                    var thread = new Thread(() => consumer.ConsumeAsync(stoppingToken));
                    thread.Start();

                    ConsumerThreads.Add(thread);

                    MessageTypes[kafkaConsumer.Key] = true;
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                StartConsumers(_serviceProvider, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var thread in ConsumerThreads)
                thread.Join(TimeSpan.FromSeconds(5));

            await base.StopAsync(cancellationToken);
        }
    }
}
