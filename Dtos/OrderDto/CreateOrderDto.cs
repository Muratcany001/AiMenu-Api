using Dtos.OrderItemDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.OrderDto
{
    public class CreateOrderDto
    {
        public string TableNumber { get; set; }
        public string Notes { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}
