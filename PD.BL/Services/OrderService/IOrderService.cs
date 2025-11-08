using Common.ViewModels;
using Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.OrderService
{
    
        public interface IOrderService
        {            
            Task<ResultViewModel<OrderDto>> CreateAnonymousOrderAsync();
            Task<ResultViewModel<OrderDto>> AssignTableToOrderAsync(int orderId, string tableNumber);
            Task<ResultViewModel<OrderDto>> GetOrderByIdAsync(int orderId);
            Task<ResultViewModel<List<OrderDto>>> GetAllOrdersAsync();
            Task<ResultViewModel<OrderDto>> UpdateOrderStatusAsync(int orderId, string newStatus);
            Task<ResultViewModel<bool>> DeleteOrderAsync(int orderId);
        }
}
