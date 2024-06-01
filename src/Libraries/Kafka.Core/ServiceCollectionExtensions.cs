using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kafka.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducerWithDefaultKey<TValue>(this IServiceCollection services,
            IConfiguration configuration,
            Action<KafkaProducerConfig<TValue>> configAction = null) where TValue : class, IMessage
        {
            return AddKafkaProducer<string, TValue>(services, configuration, configAction);
        }

        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services,
            IConfiguration configuration,
            Action<KafkaProducerConfig<TValue>> configAction = null) where TValue : class, IMessage
        {
            services.AddSingleton<IKafkaProducer<TKey, TValue>, KafkaProducer<TKey, TValue>>();
            services.Configure<KafkaProducerConfig<TValue>>(options =>
            {
                options.BootstrapServers = configuration["Kafka:BootstrapServers"];
                configAction?.Invoke(options);
            });

            return services;
        }

        public static IServiceCollection AddKafkaConsumerWithDefaultKey<TValue, THandler>(this IServiceCollection services,
            IConfiguration configuration,
            Action<KafkaConsumerConfig<TValue>> configAction = null) where TValue : IMessage
            where THandler : class, IKafkaConsumerHandler<string, TValue>
        {
            return AddKafkaConsumer<string, TValue, THandler>(services, configuration, configAction);
        }

        public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(this IServiceCollection services,
            IConfiguration configuration,
            Action<KafkaConsumerConfig<TValue>> configAction = null) where TValue : IMessage
            where THandler : class, IKafkaConsumerHandler<TKey, TValue>
        {
            RegisterKafkaConsumerServices<TKey, TValue, THandler>(services);
            ConfigureKafkaConsumer(services, configuration, configAction);
            AddKafkaConsumerManagerIfNotRegistered(services);

            return services;
        }

        private static void RegisterKafkaConsumerServices<TKey, TValue, THandler>(IServiceCollection services)
            where TValue : IMessage
            where THandler : class, IKafkaConsumerHandler<TKey, TValue>
        {
            services.AddScoped<IKafkaConsumer<TKey, TValue>, KafkaConsumer<TKey, TValue>>();
            services.AddScoped<IKafkaConsumerHandler<TKey, TValue>, THandler>();
            KafkaConsumerManager.AddConsumerMessage<TKey, TValue>();
        }

        private static void AddKafkaConsumerManagerIfNotRegistered(IServiceCollection services)
        {
            if (!services.Any(service => service.ServiceType == typeof(KafkaConsumerManager)))
                services.AddHostedService<KafkaConsumerManager>();
        }

        private static void ConfigureKafkaConsumer<TValue>(
            IServiceCollection services,
            IConfiguration configuration,
            Action<KafkaConsumerConfig<TValue>> configAction)
            where TValue : IMessage
        {
            services.Configure<KafkaConsumerConfig<TValue>>(options =>
            {
                options.BootstrapServers = configuration["Kafka:BootstrapServers"];
                options.GroupId = "PaymentConsumerGroup";

                configAction?.Invoke(options);
            });
        }
    }
}
