using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Services;
using System.Threading.Tasks;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            await _orderService.CreateOrderAsync(request);

            return Accepted();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _orderService.GetOrdersAsync());
        }
    }
}