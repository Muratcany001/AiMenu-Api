using AutoMapper;
using Common.ViewModels;
using Dtos.OrderDto;
using Dtos.OrderItemDto;
using PD.DAL.Entitites.AppEntitites;
using PD.DAL.Interface;
using System;
using PD.BL.Helpers.OrderHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PD.BL.Services.MenuItemService;
using Dtos.MenuItemDto;
using Microsoft.EntityFrameworkCore;

namespace PD.BL.Services.OrderItemService
{
    public class OrderItemService : IOrderItemService
    {  
        private readonly OrderNumberHelper _orderNumberHelper;
        private readonly IBaseRepository<OrderItem> _orderItemRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<MenuItem> _menuItemRepository;

        public OrderItemService(IBaseRepository<OrderItem> orderItemRepository, IMapper mapper, IBaseRepository<Order> orderRepository, OrderNumberHelper orderNumberHelper, IBaseRepository<MenuItem> menuItemService)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderNumberHelper = orderNumberHelper;
            _menuItemRepository = menuItemService;
        }

        public async Task<ResultViewModel<OrderItemDto>> AddOrderItemAsync(int? OrderId, string? tableNumber,CreateOrderItemDto createOrderItemDto)
        {
            

            var order= await _orderRepository.GetAsync(x => x.Id == OrderId);
            if(order == null)
            {
                order = new Order
                {
                    OrderNumber = OrderNumberHelper.GenerateOrderNumber(),
                    Status = "Pending",
                    Notes = null,
                    TableNumber = tableNumber,
                    TotalPrice = 0,
                    OrderTime = DateTime.Now
                };
                await _orderRepository.AddAsync(order);
            }
            
            var existedItem = await _orderItemRepository.GetAsync(x => x.OrderId == order.Id && x.MenuItemId == createOrderItemDto.MenuItemId);
            var menuItem = await _menuItemRepository.GetByIdAsync(createOrderItemDto.MenuItemId);
            OrderItem orderItemEntity;
            if (existedItem != null)
            {
                existedItem.Quantity += createOrderItemDto.Quantity;
                await _orderItemRepository.UpdateAsync(existedItem);
                orderItemEntity = existedItem;
            }
            else
            {
                createOrderItemDto.OrderId = order.Id;
                orderItemEntity = _mapper.Map<OrderItem>(createOrderItemDto);
                await _orderItemRepository.AddAsync(orderItemEntity);
            }
            order = await _orderRepository.GetAsync(x => x.Id == order.Id);
            order.TotalPrice += (int)(createOrderItemDto.Quantity * menuItem.Price);
            await _orderRepository.UpdateAsync(order);

            var data = _mapper.Map<OrderItemDto>(orderItemEntity);
            return ResultViewModel<OrderItemDto>.Success(data, "order item added successfully", 201);
        }

        public async Task<ResultViewModel<bool>> DeleteOrderItemAsync(int orderItemId)
        {
            var existedItem = await _orderItemRepository.GetByIdAsync(orderItemId);
            if (existedItem == null)
            {
                return ResultViewModel<bool>.NotFound("Order item not found", 404);
            }
            await _orderItemRepository.DeleteAsync(existedItem);
            return ResultViewModel<bool>.Success(true, "Order item deleted successfully", 200);
        }

        public async Task<ResultViewModel<List<OrderItemDto>>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            var orderItems = await _orderItemRepository.GetListAsync(x => x.OrderId == orderId
                , asNoTracking: true
                , includeFunc: x => x.Include(q => q.MenuItem)
                );
            if (!orderItems.Any())
            {
                return ResultViewModel<List<OrderItemDto>>.NotFound("No order items found", 404);
            }
            var list = _mapper.Map<List<OrderItemDto>>(orderItems);
            return ResultViewModel<List<OrderItemDto>>.Success(list, "Order list found", 200);
        }

        public async Task<ResultViewModel<OrderItemDto>> SetQuantityAsync(SetQuantityDto setQuantityDto)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(setQuantityDto.OrderItemId);
            if (orderItem == null)
            {
                return ResultViewModel<OrderItemDto>.NotFound("Order item not found", 404);
            }
            
            if(setQuantityDto.Quantity < 1)
            {
                return ResultViewModel<OrderItemDto>.Failure("Quantity must be at least 1", null, 400);
            }
            orderItem.Quantity = setQuantityDto.Quantity;
            
            await _orderItemRepository.UpdateAsync(orderItem);
            var data = _mapper.Map<OrderItemDto>(orderItem);
            return ResultViewModel<OrderItemDto>.Success(data, "Quantity updated successfully", 200);
        }

        public async Task<ResultViewModel<OrderItemDto>> UpdateOrderItemNoteById(int orderItemId, UpdateOrderItemNoteDto updateOrderItemNoteDto)
        {
            var orderItem =  await _orderItemRepository.GetByIdAsync(orderItemId);
            if (orderItem == null)
            {
                ResultViewModel<OrderItemDto>.NotFound("Order item not found", 404);
            }
            if(updateOrderItemNoteDto.Note.Any())
            {
                return ResultViewModel<OrderItemDto>.Failure("Note cannot be empty", null, 400);
            }
            orderItem.Note = updateOrderItemNoteDto.Note;
            
            await _orderItemRepository.UpdateAsync(orderItem);
            var data = _mapper.Map<OrderItemDto>(orderItem);
            return ResultViewModel<OrderItemDto>.Success(data, "Order item note updated successfully", 200);
        }
    }
}
