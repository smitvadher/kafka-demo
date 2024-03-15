using Kafka.Core;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Messages;

namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IKafkaProducer<string, PaymentMessage> _kafkaProducer;

        public PaymentController(IKafkaProducer<string, PaymentMessage> kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(CancellationToken cancellationToken)
        {
            var random = new Random();
            var message = new PaymentMessage
            {
                TransactionId = Guid.NewGuid(),
                OrderId = random.Next(100),
                Paid = random.Next(100) % 2 == 0,
            };

            if (!message.Paid)
                message.ErrorMessage = "Invalid transaction details.";

            await _kafkaProducer.ProduceAsync("PaymentTransaction" + message.OrderId, message, cancellationToken);

            if (!message.Paid)
                return BadRequest(message.ErrorMessage);

            return Ok();
        }
    }
}
