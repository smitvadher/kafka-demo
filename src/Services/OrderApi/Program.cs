using Kafka.Core;
using OrderApi.Consumer;
using OrderApi.Messages;

namespace OrderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddKafkaConsumer<PaymentMessage, PaymentConsumer>(o =>
            {
                o.BootstrapServers = builder.Configuration.GetSection("Kafka:BootstrapServers").Value;
                o.GroupId = "PaymentConsumerGroup";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
