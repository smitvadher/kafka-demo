using Kafka.Core;

namespace PaymentApi.Messages
{
    public record PaymentMessage : IMessage
    {
        public int OrderId { get; set; }

        public bool Paid { get; set; }

        public string ErrorMessage { get; set; }
    }
}
