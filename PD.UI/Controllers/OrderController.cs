using Microsoft.AspNetCore.Mvc;
using PD.BL.Services.OrderService;

namespace PD.UI.Controllers
{
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }

        [HttpPost("api/orders/createOrder")]
        public async Task<IActionResult> CreateAnonymousOrder()
        {
            var result = await _orderService.CreateAnonymousOrderAsync();
            return CreatedAtAction(nameof(CreateAnonymousOrder), result);

        }
        [HttpPost("api/orders/{orderId}/assignTable")]
        public async Task<IActionResult> AssignTableToOrder(int orderId, [FromQuery] string tableNumber)
        {
            var result = await _orderService.AssignTableToOrderAsync(orderId, tableNumber);

            return Ok(result);

        }
        [HttpGet("api/orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(result);
        }
        [HttpGet("api/orders/{orderNumber}")]
        public async Task<IActionResult> GetOrderItemsByOrderNumber(string orderNumber)
        {
            var result = await _orderService.GetOrderItemsByOrderNumber(orderNumber);
            return Ok(result);
        }
        [HttpPut("api/orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromQuery] string newStatus)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            return Ok(result);
        }
        [HttpDelete("api/orders/deleteOrder/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            return Ok(result);
        }

    }
}
