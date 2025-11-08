using AutoMapper;
using Common.ViewModels;
using Dtos.OrderDto;
using Microsoft.EntityFrameworkCore;
using PD.BL.Helpers.OrderHelper;
using PD.DAL.Entitites.AppEntitites;
using PD.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.OrderService
{
    public class OrderService : IOrderService
    {
        
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<OrderItem> _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IBaseRepository<Order> orderRepository,
            IBaseRepository<OrderItem> orderItemRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<OrderDto>> CreateAnonymousOrderAsync()
        {
            var newOrder = new Order
            {
                OrderNumber = OrderNumberHelper.GenerateOrderNumber(),
                Status = "Pending",
                OrderTime = DateTime.UtcNow,
                Notes = null,
                TotalPrice = 0
            };

            await _orderRepository.AddAsync(newOrder);

            var data = _mapper.Map<OrderDto>(newOrder);

            return ResultViewModel<OrderDto>.Success(data, "Anonymous order created", 201);
        }

        public async Task<ResultViewModel<OrderDto>> AssignTableToOrderAsync(int orderId, string tableNumber)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return ResultViewModel<OrderDto>.NotFound("Order not found", 404);
            }

            order.TableNumber = tableNumber;
            order.Status = "Confirmed";

            await _orderRepository.UpdateAsync(order);

            var data = _mapper.Map<OrderDto>(order);
            return ResultViewModel<OrderDto>.Success(data, "Order finalized with table", 200);
        }

        public async Task<ResultViewModel<OrderDto>> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetAsync(
                predicate: o => o.Id == orderId,
                asNoTracking: true,
                includeFunc: q => q.Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.MenuItem)
            );

            if (order == null)
            {
                return ResultViewModel<OrderDto>.NotFound("Order not found", 404);
            }

            var data = _mapper.Map<OrderDto>(order);
            return ResultViewModel<OrderDto>.Success(data, "Order retrieved", 200);
        }

        public async Task<ResultViewModel<List<OrderDto>>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetListAsync(
                asNoTracking: true, // ← false yerine true
                includeFunc: x => x.Include(o => o.OrderItems)
                                   .ThenInclude(xo => xo.MenuItem)
            );

            var data = _mapper.Map<List<OrderDto>>(orders);
            return ResultViewModel<List<OrderDto>>.Success(data, "Orders retrieved", 200);
        }

        public async Task<ResultViewModel<OrderDto>> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return ResultViewModel<OrderDto>.NotFound("Order not found", 404);
            }

            order.Status = newStatus;
            await _orderRepository.UpdateAsync(order);

            var data = _mapper.Map<OrderDto>(order);
            return ResultViewModel<OrderDto>.Success(data, "Order status updated", 200);
        }

        public async Task<ResultViewModel<bool>> DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return ResultViewModel<bool>.NotFound("Order not found", 404);
            }
            
            await _orderRepository.DeleteAsync(order);
            return ResultViewModel<bool>.Success(true, "Order deleted successfully", 200);
        }
    }
}
