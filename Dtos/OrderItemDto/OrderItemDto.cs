using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.OrderItemDto
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string Quanitty { get; set; }
        public string Note { get; set; }

    }
}
