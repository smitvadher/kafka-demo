using Microsoft.AspNetCore.Mvc;
using PaymentApi.Messages;
using PaymentApi.ProducerService;

namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentProducerService _paymentProducerService;

        public PaymentController(PaymentProducerService paymentProducerService)
        {
            _paymentProducerService = paymentProducerService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment()
        {
            var random = new Random();
            var message = new PaymentMessage()
            {
                OrderId = random.Next(100),
                Paid = random.Next(100) % 2 == 0,
            };

            if (!message.Paid)
                message.ErrorMessage = "Invalid transaction details.";

            await _paymentProducerService.ProduceAsync("PaymentTransaction", message);

            if (!message.Paid)
                return BadRequest(message.ErrorMessage);

            return Ok();
        }
    }
}
