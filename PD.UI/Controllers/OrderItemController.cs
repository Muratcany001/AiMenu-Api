using Dtos.OrderItemDto;
using Microsoft.AspNetCore.Mvc;
using PD.BL.Services.OrderItemService;

namespace PD.UI.Controllers
{
    [ApiController]
    public class OrderItemController : Controller
    {
        private readonly IOrderItemService _orderItemService;
        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpPost("api/orderItems/addOrderItem")]
        public async Task<IActionResult> AddOrderItem(int? orderId, string? tableNumber, CreateOrderItemDto createOrderItemDto)
        {
            var result = await _orderItemService.AddOrderItemAsync(orderId,tableNumber, createOrderItemDto);
            return Ok(result);
        }
        [HttpDelete("api/orderItems/deleteOrderItem/{orderItemId}")]
        public async Task<IActionResult> DeleteOrderitem(int orderItemId)
        {
            var result = await _orderItemService.DeleteOrderItemAsync(orderItemId);
            return Ok(result);
        }

        [HttpGet("api/orderItems/getOrderItemsByOrderId/{orderId}")]
        public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
        {
            var result = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);
            return Ok(result);
        }
        [HttpPut("api/orderItems/setQuantity")]
        public async Task<IActionResult> SetQuantity(SetQuantityDto setQuantityDto)
        {
        
            var result = await _orderItemService.SetQuantityAsync(setQuantityDto);
            return Ok(result);
        
        }
    }
}
