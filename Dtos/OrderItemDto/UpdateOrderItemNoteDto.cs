using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.OrderItemDto
{
    public class UpdateOrderItemNoteDto
    {
        public int OrderItemId { get; set; }
        public string Note { get; set; }
    }
}
