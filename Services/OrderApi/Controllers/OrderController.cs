using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public IActionResult PlaceOrder([FromBody] PlaceOrderRequest orderRequest)
        {
            return Ok("Order placed successfully");
        }
    }
}
