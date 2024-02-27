using Microsoft.Extensions.DependencyInjection;

namespace Kafka.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer<TValue>(this IServiceCollection services,
            Action<KafkaProducerConfig<TValue>> configAction) where TValue : class, IMessage
        {
            services.AddSingleton<IKafkaProducer<TValue>, KafkaProducer<TValue>>();
            services.Configure(configAction);

            return services;
        }

        public static IServiceCollection AddKafkaConsumer<TValue, THandler>(this IServiceCollection services,
            Action<KafkaConsumerConfig<TValue>> configAction) where TValue : IMessage
            where THandler : class, IKafkaConsumerHandler<TValue>
        {
            services.AddScoped<IKafkaConsumer<TValue>, KafkaConsumer<TValue>>();
            services.AddScoped<IKafkaConsumerHandler<TValue>, THandler>();
            KafkaConsumerManager.AddConsumerMessage<TValue>();
            services.Configure(configAction);

            if (services.All(x => x.ServiceType != typeof(KafkaConsumerManager)))
                services.AddHostedService<KafkaConsumerManager>();

            return services;
        }
    }
}
