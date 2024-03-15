using Microsoft.Extensions.DependencyInjection;

namespace Kafka.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services,
            Action<KafkaProducerConfig<TValue>> configAction) where TValue : class, IMessage
        {
            services.AddSingleton<IKafkaProducer<TKey, TValue>, KafkaProducer<TKey, TValue>>();
            services.Configure(configAction);

            return services;
        }

        public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(this IServiceCollection services,
            Action<KafkaConsumerConfig<TValue>> configAction) where TValue : IMessage
            where THandler : class, IKafkaConsumerHandler<TKey, TValue>
        {
            services.AddScoped<IKafkaConsumer<TKey, TValue>, KafkaConsumer<TKey, TValue>>();
            services.AddScoped<IKafkaConsumerHandler<TKey, TValue>, THandler>();
            KafkaConsumerManager.AddConsumerMessage<TKey, TValue>();
            services.Configure(configAction);

            if (services.All(x => x.ServiceType != typeof(KafkaConsumerManager)))
                services.AddHostedService<KafkaConsumerManager>();

            return services;
        }
    }
}
