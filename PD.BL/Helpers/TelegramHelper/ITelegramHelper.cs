using Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Helpers.TelegramHelper
{
    public interface ITelegramHelper
    {
        Task SendOrderCompletedAsync(OrderDto orderDto);
    }
}
