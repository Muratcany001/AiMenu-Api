using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.OrderDto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int TotalPrice { get; set; }
        public string TableNumber { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
