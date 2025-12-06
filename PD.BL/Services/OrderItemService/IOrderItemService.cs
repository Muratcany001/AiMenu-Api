using Common.ViewModels;
using Dtos.OrderItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.OrderItemService
{
    public interface IOrderItemService
    {
        Task<ResultViewModel<OrderItemDto>> AddOrderItemAsync(int? OrderId,string? tableNumber, CreateOrderItemDto createOrderItemDto);
        Task<ResultViewModel<OrderItemDto>> SetQuantityAsync(SetQuantityDto setQuantityDto);
        Task<ResultViewModel<bool>> DeleteOrderItemAsync(int orderItemId);
        Task<ResultViewModel<List<OrderItemDto>>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<ResultViewModel<OrderItemDto>> UpdateOrderItemNoteById(int orderItemId, UpdateOrderItemNoteDto updateOrderItemNoteDto);
    }
}
